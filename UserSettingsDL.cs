// C# sourcecode
/**********************************************************************************************************************
Author: Param Varun Reddy
Created: 2024-02-28
Description: Repository pattern Data Layer class UserDetails, where we will get the user Preferences by using the logged in User Id
***********************************************************************************************************************/

namespace RexStudios.LanguageDependentNotification
{
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using RexStudios.LanguageDependentNotification.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class UserSettingsDL : EntityRetrieverBase<Entity>, IUserSettingsRepo
    {
        private ITracingService tracingService = null;
        public UserSettingsDL(IOrganizationService service, ITracingService _tracingService)
            : base(service, _tracingService, "usersettings")
        {
            traceService = _tracingService;
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
        /// Gets User settings by UserId
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Entity> GetUserSettingsbyUserId(Guid? UserId)
        {
            try
            {
                if (orgService == null)
                    throw new InvalidPluginExecutionException($"GetUserSettingsbyUserId: Organization service is null, can't process the request");
                QueryExpression query = new QueryExpression(base.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(true),
                    Criteria = new FilterExpression()
                    {
                        Conditions = {
                        new ConditionExpression()
                        {
                            AttributeName = "systemuserid",
                            Operator = ConditionOperator.Equal,
                            Values = {UserId}
                        }
                    }
                    }
                };
                tracingService?.Trace($"1--> GetUserSettingsbyUserId: Query expression is completed ");
                return base.RetrieveMultipleEntities(query);
            }
            catch (InvalidPluginExecutionException ex)
            {
                tracingService?.Trace($"Invalid Exception GetUserSettingsbyUserId: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                tracingService?.Trace($"Invalid Exception GetUserSettingsbyUserId: {ex.Message}");
                throw ex;
            }

        }

        /// <summary>
        /// Get UserLanguage Id by User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserLanguageId(Guid? userId)
        {
            try
            {
               var userSettings = this.GetUserSettingsbyUserId(userId).FirstOrDefault();
                if (userSettings!=null)
                {
                    return (int)userSettings["uilanguageid"];
                }
                return 0;
            }
            catch (InvalidPluginExecutionException ex)
            {
                tracingService?.Trace($"Invalid Exception GetUserLanguageId: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                tracingService?.Trace($"Invalid Exception GetUserLanguageId: {ex.Message}");
                throw ex;
            }
        }
    }
}
