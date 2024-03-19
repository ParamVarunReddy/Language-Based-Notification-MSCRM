namespace RexStudios.LanguageDependentNotification
{
    using System;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using RexStudios.LanguageDependentNotification.Core;
    using RexStudios.Extensions;
    using System.Linq;
    using System.Collections.Generic;

    internal class NotificationDL: EntityRetrieverBase<Entity>, INotificationRepo
    {
        private ITracingService tracingService = null;
        public  NotificationDL(IOrganizationService service, ITracingService tracingService)
            : base(service, tracingService, "rex_notification")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool Exists(Guid id)
        {
            return this.Retrieve(id, new ColumnSet(false)) != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="columnSet"></param>
        /// <returns></returns>
        public override Entity Retrieve(Guid entityId, ColumnSet columnSet)
        {
            return base.Retrieve(entityId, columnSet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EntityLogicalName"></param>
        /// <returns></returns>
        public IEnumerable<Entity> GetNotificationTextByEntityName(string EntityLogicalName, int languagecode)
        {
            try
            {
                if (orgService == null)
                    throw new InvalidPluginExecutionException($"GetNotificationTextByEntityName: Organization service is null, can't process the request");
                if (StringExtensions.IsNullOrEmpty(EntityLogicalName))
                    throw new InvalidPluginExecutionException($"GetNotificationTextByEntityName: NotificationType is null, can't process the request");

                QueryExpression query = new QueryExpression(base.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(true),
                    Criteria = new FilterExpression()
                    {
                        Conditions = {
                        new ConditionExpression()
                        {
                            AttributeName = "rex",
                            Operator = ConditionOperator.Equal,
                            Values = { EntityLogicalName }
                        },
                        new ConditionExpression()
                        {
                            AttributeName = "rex",
                            Operator = ConditionOperator.Equal,
                            Values = { languagecode }
                        }
                    }
                    }
                };
                tracingService?.Trace($"1--> GetNotificationTextByEntityName: Query expression is completed ");
                return base.RetrieveMultipleEntities(query);
            }
            catch (InvalidPluginExecutionException ex)
            {
                tracingService?.Trace($"Invalid Exception GetNotificationTextByEntityName: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                tracingService?.Trace($"Invalid Exception GetNotificationTextByEntityName: {ex.Message}");
                throw ex;
            }
        }


        /// <summary>
        /// Get Notification Message Text by sending the GUId
        /// </summary>
        /// <param name="NotificationId"></param>
        /// <returns></returns>
        public Entity GetNotificationTextByID(Guid? NotificationId)
        {
            try
            {
                if (orgService == null)
                    throw new InvalidPluginExecutionException($"GetNotificationTextByID: Organization service is null, can't process the request");
                if (GuidExtensions.IsNullOrEmpty(NotificationId))
                    throw new InvalidPluginExecutionException($"GetNotificationTextByID: NotificationTypeId is null or empty, can't process the request");
                return this.Retrieve(NotificationId.Value, new ColumnSet(true));
            }
            catch (InvalidPluginExecutionException ex)
            {
                tracingService?.Trace($"Invalid Exception GetNotificationTextByID: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                tracingService?.Trace($"Invalid Exception GetNotificationTextByID: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// Get NotificationText by Sending the 
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        public Entity GetNotificationTextByName(string CommandText)
        {
            try
            {
                if (orgService == null)
                    throw new InvalidPluginExecutionException($"GetNotificationTextByName: Organization service is null, can't process the request");
                if (StringExtensions.IsNullOrEmpty(CommandText))
                    throw new InvalidPluginExecutionException($"GetNotificationTextByName: NotificationType is null, can't process the request");

                QueryExpression query = new QueryExpression(base.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(true),
                    Criteria = new FilterExpression()
                    {
                        Conditions = {
                        new ConditionExpression()
                        {
                            AttributeName = "rex_",
                            Operator = ConditionOperator.Equal,
                            Values = { CommandText }
                        }
                    }
                    }
                };
                tracingService?.Trace($"1--> GetNotificationTextByName: Query expression is completed ");
                return base.RetrieveMultipleEntities(query).FirstOrDefault();
            }
            catch (InvalidPluginExecutionException ex)
            {
                tracingService?.Trace($"Invalid Exception GetNotificationTextByName: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                tracingService?.Trace($"Invalid Exception GetNotificationTextByName: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// Get Notification Text based on the logical name and notificationType
        /// </summary>
        /// <param name="CommandText"></param>
        /// <param name="EntityLogicalName"></param>
        /// <param name="NotificationType"></param>
        /// <returns></returns>
        public IEnumerable<Entity> GetNotificationTextByName(string CommandText, string EntityLogicalName, string NotificationType)
        {
            try
            {
                if (orgService == null)
                    throw new InvalidPluginExecutionException($"GetNotificationTextByEntityName: Organization service is null, can't process the request");
                if (StringExtensions.IsNullOrEmpty(EntityLogicalName))
                    throw new InvalidPluginExecutionException($"GetNotificationTextByEntityName: NotificationType is null, can't process the request");

                QueryExpression query = new QueryExpression(base.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(true),
                    Criteria = new FilterExpression()
                    {
                        Conditions = {
                        new ConditionExpression()
                        {
                            AttributeName = "rex",
                            Operator = ConditionOperator.Equal,
                            Values = { EntityLogicalName }
                        },
                        new ConditionExpression()
                        {
                            AttributeName = "rex",
                            Operator = ConditionOperator.Equal,
                            Values = { EntityLogicalName }
                        },
                        new ConditionExpression()
                        {
                            AttributeName = "rex",
                            Operator = ConditionOperator.Equal,
                            Values = { EntityLogicalName }
                        }
                    },
                        FilterOperator = LogicalOperator.And
                    }
                };
                tracingService?.Trace($"1--> GetNotificationTextByEntityName: Query expression is completed ");
                return base.RetrieveMultipleEntities(query).ToList();
            }
            catch (InvalidPluginExecutionException ex)
            {
                tracingService?.Trace($"Invalid Exception GetNotificationTextByEntityName: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                tracingService?.Trace($"Invalid Exception GetNotificationTextByEntityName: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="commandText"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public Entity GetNotificationTexts(string entityName, string commandText, int languageCode)
        {
            try
            {
                if (orgService == null)
                    throw new InvalidPluginExecutionException($"GetNotificationTexts: Organization service is null, can't process the request");
                if (StringExtensions.IsNullOrEmpty(EntityLogicalName))
                    throw new InvalidPluginExecutionException($"GetNotificationTexts: NotificationType is null, can't process the request");

                QueryExpression query = new QueryExpression(base.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(true),
                    Criteria = new FilterExpression()
                    {
                        Conditions = {
                        new ConditionExpression()
                        {
                            AttributeName = "rex",
                            Operator = ConditionOperator.Equal,
                            Values = { EntityLogicalName }
                        },
                        new ConditionExpression()
                        {
                            AttributeName = "rex",
                            Operator = ConditionOperator.Equal,
                            Values = { EntityLogicalName }
                        },
                        new ConditionExpression()
                        {
                            AttributeName = "rex",
                            Operator = ConditionOperator.Equal,
                            Values = { EntityLogicalName }
                        }
                    },
                        FilterOperator = LogicalOperator.And
                    }
                };
                tracingService?.Trace($"1--> GetNotificationTexts: Query expression is completed ");
                return base.RetrieveMultipleEntities(query).FirstOrDefault();
            }
            catch (InvalidPluginExecutionException ex)
            {
                tracingService?.Trace($"Invalid Exception GetNotificationTexts: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                tracingService?.Trace($"Invalid Exception GetNotificationTexts: {ex.Message}");
                throw ex;
            }
        }
    }
}
