using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Autofac;
using FluentValidation;
using FluentValidation.Results;
using FoxTales.Infrastructure.DependencyInjection;


namespace FoxTales.Infrastructure.CommandFramework
{
    public abstract class AbstractCommand
    {
      

        internal bool IsChained { get; set; }
        public ICollection<ValidationResult> ValidationResults { get; private set; }

        public bool IsValid
        {
            get { return ValidationResults.All(x => x.IsValid); }
        }

        private readonly ILifetimeScope _lifetimeScope;
        private IsolationLevel _isolationLevel;

        internal AbstractCommand()
        {
            _lifetimeScope = AutofacBootstrapper.BeginLifetimeScopeByApplicationType();
            ValidationResults = new List<ValidationResult>();
        }

        protected void AddPostValidationError<T, TProperty>(Expression<Func<T, TProperty>> exp, string errorMessage, string errorCode)
        {
            var validationResult = new ValidationResult();
            var validationFailure = new ValidationFailure(GetPropertyName(exp), errorMessage);
            validationFailure.ErrorCode = errorCode;
            validationResult.Errors.Add(validationFailure);
            ValidationResults.Add(validationResult);
        }

        private string GetPropertyName<TObject, TResult>(Expression<Func<TObject, TResult>> exp)
        {
            // extract property name
            return ((MemberExpression) exp.Body).Member.Name;
        }

        internal void Run(IsolationLevel isolationLevel)
        {
            Run(isolationLevel, _lifetimeScope);
        }

        internal void Run(IsolationLevel isolationLevel, ILifetimeScope lifetimeScope)
        {
           
                _isolationLevel = isolationLevel;

                TransactionScope scope = null;
                if (!IsChained)
                {
                    scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = isolationLevel });
                }

                try
                {
                    PerformValidation(lifetimeScope);
                    if (!IsValid)
                    {
                        return;
                    }
                    PerformCommand(lifetimeScope);
                    scope?.Complete();
                }
                catch (Exception ex)
                {
                   // Log.Logger.Here().Error(ex, "Unhandled exception in command");
                    throw;
                }
                finally
                {
                    scope?.Dispose();
                }
        }

        public virtual TimeSpan? WarnIfExecutionExceeds => null;

        private void PerformValidation(ILifetimeScope lifetimeScope)
        {
            ValidationResults = GetValidators(lifetimeScope).ToList().Select(v => v.Validate(this)).ToList();
        }
        
        protected void StartChildCommand(CommandBase cmd)
        {
            cmd.PerformValidation(_lifetimeScope);

            if (!cmd.IsValid)
            {
                return;
            }
            cmd.IsChained = true;
            cmd.Run(_isolationLevel);
        }

        protected TOutput StartChildCommand<TOutput>(CommandBase<TOutput> cmd)
        {
            cmd.PerformValidation(_lifetimeScope);

            if (cmd.IsValid)
            {
                cmd.IsChained = true;
                return cmd.Execute(_isolationLevel);
            }

            return default(TOutput);
        }

        public string[] GetErrorMessages()
        {
            if (IsValid)
            {
                return new string[0];
            }

            return ValidationResults.SelectMany(e => e.Errors.Select(d => (string.IsNullOrWhiteSpace(d.ErrorCode) ? "" : $"{d.ErrorCode}: ") + d.ErrorMessage)).ToArray();
        }

        protected abstract void PerformCommand(ILifetimeScope lifetimeScope);

        protected abstract IEnumerable<IValidator> GetValidators(ILifetimeScope lifetimeScope);

        internal abstract void DoExecute(IsolationLevel isolationLevel);
    }

    public abstract class CommandBase : AbstractCommand
    {
        public event CommandFinishedDelegate Finished;
        public delegate void CommandFinishedDelegate(AbstractCommand cmd);

        public void Execute(IsolationLevel isolationLevel)
        {
            Run(isolationLevel);
            Finished?.Invoke(this);
        }

        internal override void DoExecute(IsolationLevel isolationLevel)
        {
            Execute(isolationLevel);
        }

        public CommandTask ToCommandTask(IsolationLevel isolationLevel)
        {
            return CommandTask.Create(isolationLevel, this);
        }

        protected override IEnumerable<IValidator> GetValidators(ILifetimeScope lifetimeScope)
        {
            return lifetimeScope.Resolve<IEnumerable<IValidator>>().Where(v => v.CanValidateInstancesOfType(GetType()));
        }

        protected abstract void OnExecuting(ILifetimeScope lifetimeScope);

        protected override void PerformCommand(ILifetimeScope lifetimeScope)
        {
            OnExecuting(lifetimeScope);
        }

        public CommandBase DisableAutoLogging()
        {
         
            return this;
        }
    }

    public abstract class CommandBase<TOutput> : AbstractCommand
    {
        public event CommandFinishedDelegate Finished;
        public delegate void CommandFinishedDelegate(CommandBase<TOutput> cmd, TOutput result);

        private TOutput _result;

        public TOutput Execute(IsolationLevel isolationLevel)
        {
            Run(isolationLevel);
            Finished?.Invoke(this, _result);
            return _result;
        }

        internal override void DoExecute(IsolationLevel isolationLevel)
        {
            Execute(isolationLevel);
        }

        protected abstract TOutput OnExecuting(ILifetimeScope lifetimeScope);

        protected override void PerformCommand(ILifetimeScope lifetimeScope)
        {
            _result = OnExecuting(lifetimeScope);
        }

        protected override IEnumerable<IValidator> GetValidators(ILifetimeScope lifetimeScope)
        {
            return lifetimeScope.Resolve<IEnumerable<IValidator>>().Where(v => v.CanValidateInstancesOfType(GetType()));
        }

        public CommandBase<TOutput> DisableAutoLogging()
        {
          
            return this;
        }
    }
}