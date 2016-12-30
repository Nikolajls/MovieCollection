// Copyright (C) 2014 FoxTales
// Released under the MIT License

using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;
using FoxTales.Infrastructure.DomainFramework.Generics;
using FoxTales.Infrastructure.OperationFramework;
using FoxTales.Infrastructure.UnitOfWorkFramework.Interfaces;

namespace FoxTales.Infrastructure.UnitOfWorkFramework
{
    public class UnitOfWork<TIdentity> : IUnitOfWork<TIdentity> where TIdentity : struct
    {
        private readonly Dictionary<EntityBase<TIdentity>, IUnitOfWorkRepository<TIdentity>> _addedEntities = new Dictionary<EntityBase<TIdentity>, IUnitOfWorkRepository<TIdentity>>();
        private readonly Dictionary<EntityBase<TIdentity>, IUnitOfWorkRepository<TIdentity>> _changedEntities = new Dictionary<EntityBase<TIdentity>, IUnitOfWorkRepository<TIdentity>>();
        private readonly Dictionary<EntityBase<TIdentity>, IUnitOfWorkRepository<TIdentity>> _deletedEntities = new Dictionary<EntityBase<TIdentity>, IUnitOfWorkRepository<TIdentity>>();

        public event CommitSucceededHandler CommitSucceeded;
        public event CommitFailedHandler CommitFailed;
        public event CommittingHandler Committing;

        public UnitOfWork(CommitFailedHandler failedHandler = null, CommitSucceededHandler successHandler = null)
        {
            if (failedHandler != null) CommitFailed += failedHandler;
            if (successHandler != null) CommitSucceeded += successHandler;

            if (UnitOfWork.OperationResultIntegration == OperationResultIntegrationStatus.Enabled)
            {
                OperationResult.Created += result =>
                {
                    Committing += () =>
                    {
                        if (!result.Succeeded() && CommitFailed != null)
                        {
                            CommitFailed(new OperationResultFailedException(result));
                        }
                    };
                };
            }
        }

        public void RegisterAdded(EntityBase<TIdentity> entity, IUnitOfWorkRepository<TIdentity> repository)
        {
            entity.CreatedDate = DateTime.Now;
            if (!_addedEntities.ContainsKey(entity)) _addedEntities.Add(entity, repository);
        }

        public void RegisterChanged(EntityBase<TIdentity> entity, IUnitOfWorkRepository<TIdentity> repository)
        {
            entity.ModifiedDate = DateTime.Now;
            if (!_changedEntities.ContainsKey(entity)) _changedEntities.Add(entity, repository);
        }

        public void RegisterRemoved(EntityBase<TIdentity> entity, IUnitOfWorkRepository<TIdentity> repository)
        {
            if (!_deletedEntities.ContainsKey(entity)) _deletedEntities.Add(entity, repository);
        }

        public OperationResult<UnitOfWorkErrorCode> Commit()
        {
            var result = new OperationResult<UnitOfWorkErrorCode>();

            try
            {
                using (var transaction = new TransactionScope())
                {
                    bool doCommit = false;
                    foreach (var entity in _deletedEntities.Keys)
                    {
                        doCommit = true;
                        _deletedEntities[entity].PersistDeletedItem(entity);
                    }
                    foreach (var entity in _addedEntities.Keys)
                    {
                        doCommit = true;
                        _addedEntities[entity].PersistNewItem(entity);
                    }
                    foreach (var entity in _changedEntities.Keys)
                    {
                        doCommit = true;
                        _changedEntities[entity].PersistUpdatedItem(entity);
                    }
                    if (Committing != null && doCommit)
                    {
                        Committing();
                    }
                    if (CommitSucceeded != null) CommitSucceeded();
                    transaction.Complete();
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                result.AddError(UnitOfWorkErrorCode.InvalidEntity, exceptionMessage);
            }
            catch (Exception ex)
            {
                if (CommitFailed != null) CommitFailed(ex);
                var message = "";
                while (ex!=null)
                {
                    message += ex.Message + Environment.NewLine;
                    ex = ex.InnerException;
                }
                result.AddError(UnitOfWorkErrorCode.Unknown, message);
            }

            if (result.Succeeded())
            {
                _deletedEntities.Clear();
                _addedEntities.Clear();
                _changedEntities.Clear();
            }
            

            return result;
        }

        public IEnumerable<T> GetAddedQueue<T>() where T : EntityBase<TIdentity>
        {
            return _addedEntities.Keys.OfType<T>();
        }

        [Serializable]
        private class OperationResultFailedException : Exception
        {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public OperationResultFailedException(OperationResult result) : this(result.GetErrors())
            {
            }

            private OperationResultFailedException(string message) : base(message)
            {
            }
        }
    }

    public class UnitOfWork : UnitOfWork<Guid>
    {
        public static OperationResultIntegrationStatus OperationResultIntegration = OperationResultIntegrationStatus.Disabled;
    }

    public enum OperationResultIntegrationStatus
    {
        Disabled = 0,
        Enabled = 1
    }
}
