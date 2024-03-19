namespace RexStudios.LanguageDependentNotification.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    internal abstract class EntityRetrieverBase<TEntity> : IEntityRetriever<TEntity> where TEntity : Entity
    {
        protected IOrganizationService orgService;
        protected ITracingService traceService;
        /// <summary>
        /// Gets the name of the entity logical.
        /// </summary>
        /// <value>
        /// The name of the entity logical.
        /// </value>
        protected string EntityLogicalName { get; private set; }

        protected EntityRetrieverBase(IOrganizationService service, ITracingService tracingService, string entityLogicalName)
        {
            this.orgService = service;
            this.EntityLogicalName = entityLogicalName.ToLower();
            this.traceService = tracingService;
        }

        public TEntity RetrieveEntity(Guid entityId, ColumnSet columnSet = null)
        {
            return orgService.Retrieve(this.EntityLogicalName, entityId, columnSet).ToEntity<TEntity>();
        }

        public EntityCollection RetrieveMultipleEntity(QueryBase query)
        {
            return orgService.RetrieveMultiple(query);
        }
        public EntityCollection RetrieveMultipleEntity(FetchExpression fetch)
        {
            return orgService.RetrieveMultiple(fetch);
        }

        /// <summary>
        /// Retrieves the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="columnSet">The column set.</param>
        /// <returns></returns>
        ///<exception cref="System.ArgumentNullException">entityLogicalName</exception>
        public virtual TEntity Retrieve(Guid entityId, ColumnSet columnSet = null)
        {
            if (null == this.EntityLogicalName)
            {
                throw new ArgumentNullException("entityLogicalName");
            }
            return this.RetrieveEntity(entityId, columnSet);
        }

        /// <summary>
        /// Retrieves the specified entity reference.
        /// </summary>
        /// <param name="entityReference">The entity reference.</param>
        /// <param name="columnSet">The column set.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">entityReference</exception>
        public virtual TEntity Retrieve(EntityReference entityReference, ColumnSet columnSet = null)
        {
            if (null == entityReference)
            {
                throw new ArgumentNullException("entityReference");
            }
            return orgService.Retrieve(entityReference.Name, entityReference.Id, columnSet).ToEntity<TEntity>();
        }

        /// <summary>
        /// Existses the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public abstract bool Exists(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> RetrieveMultipleEntities(QueryBase query)
        {
            if (query is QueryExpression queryExpression)
            {
                if (!queryExpression.ColumnSet.Columns.Contains("statecode"))
                {
                    queryExpression.ColumnSet.AddColumn("statecode");
                }

                if (!queryExpression.ColumnSet.Columns.Contains("statuscode"))
                {
                    queryExpression.ColumnSet.AddColumn("statuscode");
                }
            }
            EntityCollection entityCollection = RetrieveMultipleEntity(query);
            if (entityCollection == null)
                return null;
            else if (entityCollection != null && entityCollection.Entities != null && entityCollection.Entities.Count() > 0)
            {
                return entityCollection.Entities.Cast<TEntity>().Where(x => x.GetAttributeValue<OptionSetValue>("statecode").Value == 0 &&
                x.GetAttributeValue<OptionSetValue>("statuscode").Value == 1);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fetchExpression"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> RetrieveMultipleEntities(FetchExpression fetchExpression)
        {
            EntityCollection entityCollection = RetrieveMultipleEntity(fetchExpression);
            if (entityCollection == null)
                return null;
            else if (entityCollection != null && entityCollection.Entities != null && entityCollection.Entities.Count() > 0)
            {
                return entityCollection.Entities.Cast<TEntity>();
            }
            return null;
        }
    }

}
