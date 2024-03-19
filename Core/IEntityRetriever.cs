namespace RexStudios.LanguageDependentNotification.Core
{
    using System;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    internal interface IEntityRetriever<TEntity> where TEntity : Entity
    {
        TEntity RetrieveEntity(Guid entityId, ColumnSet columnSet);
        EntityCollection RetrieveMultipleEntity(QueryBase query);
    }
}
