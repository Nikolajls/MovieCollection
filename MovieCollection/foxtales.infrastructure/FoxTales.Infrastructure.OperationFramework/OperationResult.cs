// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoxTales.Infrastructure.OperationFramework.Interfaces;

namespace FoxTales.Infrastructure.OperationFramework
{
    public abstract class OperationResult : IOperationResult
    {
        public delegate void OperationResultDelegate(OperationLogBase log);

        public delegate void OperationResultCreatedDelegate(OperationResult result);

        public static event OperationResultDelegate ErrorRaised;
        public static event OperationResultDelegate WarningRaised;
        public static event OperationResultCreatedDelegate Created;

        private readonly List<OperationLogBase> _logs = new List<OperationLogBase>();

        protected OperationResult()
        {
            if (Created != null) Created(this);
        }

        public IReadOnlyCollection<OperationLogBase> Errors
        {
            get { return _logs.Where(l => l.Type == OperationLogType.Error).ToList().AsReadOnly(); }
        }

        public IReadOnlyCollection<OperationLogBase> Warnings
        {
            get { return _logs.Where(l => l.Type == OperationLogType.Warning).ToList().AsReadOnly(); }
        }
        
        public bool Succeeded()
        {
            return !Errors.Any();
        }

        public void CombineErrors(params IOperationResult[] others)
        {
            foreach (var operationResult in others)
            {
                _logs.AddRange(operationResult.Errors);
            }
        }

        protected void AddLog(OperationLogBase error)
        {
            switch (error.Type)
            {
                case OperationLogType.Warning:
                    if (WarningRaised != null) WarningRaised(error);
                    break;
                default:
                    if (ErrorRaised != null) ErrorRaised(error);
                    break;
            }
            _logs.Add(error);
        }

        public static implicit operator bool(OperationResult me)
        {
            return me.Succeeded();
        }

        public string GetErrors()
        {
            var errors = new StringBuilder();
            foreach (var error in Errors)
            {
                errors.AppendLine(error.Description);
            }
            return errors.ToString();
        }

        public enum EmptyErrorCode
        {

        }
    }

    public class OperationResult<TErrorCode> : OperationResult where TErrorCode : struct, IConvertible, IComparable, IFormattable
    {
        private readonly List<OperationLog<TErrorCode>> _logs = new List<OperationLog<TErrorCode>>();

        public new IReadOnlyCollection<OperationLog<TErrorCode>> Errors
        {
            get { return _logs.Where(l => l.Type == OperationLogType.Error).ToList().AsReadOnly(); }
        }

        public new IReadOnlyCollection<OperationLog<TErrorCode>> Warnings
        {
            get { return _logs.Where(l => l.Type == OperationLogType.Warning).ToList().AsReadOnly(); }
        }

        public OperationResult<TErrorCode> AddError(TErrorCode errorCode, string message = null)
        {
            var log = new OperationLog<TErrorCode>(OperationLogType.Error, errorCode, message);
            _logs.Add(log);
            AddLog(log);
            return this;
        }

        [Obsolete("This becomes an anti-pattern if used in conjuction with code coverage.")]
        public OperationResult<TErrorCode> AddErrorIf(bool condition, TErrorCode errorCode, string message = null)
        {
            if (condition)
            {
                AddError(errorCode, message);
            }
            return this;
        }

        public OperationResult<TErrorCode> AddWarning(TErrorCode errorCode, string message = null)
        {
            var log = new OperationLog<TErrorCode>(OperationLogType.Warning, errorCode, message);
            _logs.Add(log);
            AddLog(log);
            return this;
        }

        [Obsolete("This becomes an anti-pattern if used in conjuction with code coverage.")]
        public OperationResult<TErrorCode> AddWarningIf(bool condition, TErrorCode errorCode, string message = null)
        {
            if (condition)
            {
                AddWarning(errorCode, message);
            }
            return this;
        }

        public bool HasError(TErrorCode errorCode)
        {
            return _logs.Any(l => l.Code.Equals(errorCode));
        }

        public static implicit operator bool(OperationResult<TErrorCode> me)
        {
            return me.Succeeded();
        }
    }

    public class OperationResult<TErrorCode, TResult> : OperationResult<TErrorCode>, IReturnOperationResult<TResult> where TErrorCode : struct, IConvertible, IComparable, IFormattable
    {
        public TResult Result { get; private set; }

        public OperationResult(TResult result = default(TResult))
        {
            Complete(result);
        }

        public void Complete(TResult result)
        {
            Result = result;
        }

        public new OperationResult<TErrorCode, TResult> AddError(TErrorCode errorCode, string message = null)
        {
            base.AddError(errorCode, message);
            return this;
        }

        [Obsolete("This becomes an anti-pattern if used in conjuction with code coverage.")]
        public new OperationResult<TErrorCode, TResult> AddErrorIf(bool condition, TErrorCode errorCode, string message = null)
        {
            base.AddErrorIf(condition, errorCode, message);
            return this;
        }

        public new OperationResult<TErrorCode, TResult> AddWarning(TErrorCode errorCode, string message = null)
        {
            base.AddWarning(errorCode, message);
            return this;
        }

        [Obsolete("This becomes an anti-pattern if used in conjuction with code coverage.")]
        public new OperationResult<TErrorCode, TResult> AddWarningIf(bool condition, TErrorCode errorCode, string message = null)
        {
            base.AddWarningIf(condition, errorCode, message);
            return this;
        }

        public static implicit operator TResult(OperationResult<TErrorCode, TResult> me)
        {
            return me.Result;
        }

        public static implicit operator bool(OperationResult<TErrorCode, TResult> me)
        {
            return me.Succeeded();
        }
    }
}
