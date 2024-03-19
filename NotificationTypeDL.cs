namespace RexStudios.LanguageDependentNotification
{
    using System;
    using System.Linq;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using RexStudios.Extensions;
    using RexStudios.LanguageDependentNotification.Core;

    internal class NotificationTypeDL : EntityRetrieverBase<Entity>, INotificationTypeRepo
    {
        private ITracingService tracingService = null;
        public NotificationTypeDL(IOrganizationService service, ITracingService _tracingService)
            : base(service, _tracingService, "rex_notificationtype")
        {
            tracingService = _tracingService;
        }

        public override bool Exists(Guid id)
        {
            return this.Retrieve(id, new ColumnSet(false)) != null;
        }

        /// <summary>
        /// Gets the usersettings entity by Guid 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="columnSet"></param>
        /// <returns></returns>
        public override Entity Retrieve(Guid entityId, ColumnSet columnSet)
        {
            return base.Retrieve(entityId, columnSet);
        }

        /// <summary>
        /// Get NotificationType by Text
        /// </summary>
        /// <param name="NotificationTypeText"></param>
        /// <returns> returns notification type Entity</returns>
        public Entity GetNotificationTypeByText(string NotificationTypeText)
        {
            try
            {
                if (orgService == null)
                    throw new InvalidPluginExecutionException($"GetNotificationTypeByText: Organization service is null, can't process the request");
                if (StringExtensions.IsNullOrEmpty(NotificationTypeText))
                    throw new InvalidPluginExecutionException($"GetNotificationTypeByText: NotificationType is null, can't process the request");

                QueryExpression query = new QueryExpression(base.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(true),
                    Criteria = new FilterExpression()
                    {
                        Conditions = {
                        new ConditionExpression()
                        {
                            AttributeName = "name",
                            Operator = ConditionOperator.Equal,
                            Values = { NotificationTypeText }
                        }
                    }
                    }
                };
                tracingService?.Trace($"1--> GetNotificationTypeByText: Query expression is completed ");
                return base.RetrieveMultipleEntities(query).FirstOrDefault();
            }
            catch (InvalidPluginExecutionException ex)
            {
                tracingService?.Trace($"Invalid Exception GetNotificationTypeByText: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                tracingService?.Trace($"Invalid Exception GetNotificationTypeByText: {ex.Message}");
                throw ex;
            }
        }

        public Entity GetNotificationTypeById(Guid? NotificationTypeId)
        {
            try
            {
                if (orgService == null)
                    throw new InvalidPluginExecutionException($"GetNotificationTypeById: Organization service is null, can't process the request");
                if (GuidExtensions.IsNullOrEmpty(NotificationTypeId))
                    throw new InvalidPluginExecutionException($"GetNotificationTypeById: NotificationTypeId is null or empty, can't process the request");
                return this.Retrieve(NotificationTypeId.Value, new ColumnSet(true));
            }
            catch (InvalidPluginExecutionException ex)
            {
                tracingService?.Trace($"Invalid Exception GetNotificationTypeById: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                tracingService?.Trace($"Invalid Exception GetNotificationTypeById: {ex.Message}");
                throw ex;
            }
        }
    }
}
