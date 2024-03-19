// C# sourcecode
/**********************************************************************************************************************
Author: Param Varun Reddy
Created: 2024-02-28
Description: Repository pattern Data Layer class UserDetails, that was implemented to get the User Details by Id. 
Entity has fields to get the user Language. we will be retrieving these fields first, if these fields are Null, 
then we will be taking User settings or preferences.
***********************************************************************************************************************/

namespace RexStudios.LanguageDependentNotification
{
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using RexStudios.Extensions;
    using RexStudios.LanguageDependentNotification.Core;
    using System;

    internal class UserDL : EntityRetrieverBase<Entity>, IUserRepo
    {
        private ITracingService tracingService = null;
        public UserDL(IOrganizationService service, ITracingService _tracingService)
            : base(service, _tracingService, "systemusers")
        {
            traceService = _tracingService;
        }

        /// <summary>
        /// Finfd if the user id Exxist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool Exists(Guid id)
        {
            return this.Retrieve(id, new ColumnSet(false)) != null;
        }

        /// <summary>
        /// Get All Attributes in an entity by guid
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="columnSet"></param>
        /// <returns></returns>
        public override Entity Retrieve(Guid entityId, ColumnSet columnSet)
        {
            return base.Retrieve(entityId, columnSet);
        }

        /// <summary>
        /// Get User Details by using Uswer Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Entity GetUserDetailsById(Guid? userId)
        {
            try
            {
                if (orgService == null)
                    throw new InvalidPluginExecutionException($"GetUserDetailsById: Organization service is null, can't process the request");
                if (GuidExtensions.IsNullOrEmpty(userId))
                    throw new InvalidPluginExecutionException($"GetUserDetailsById: Guid is null, can't process the request");

                var userDetails = this.Retrieve(userId.Value, new ColumnSet(false));
                if(userDetails != null)
                {
                    return userDetails;
                }
                return null;
            }
            catch (InvalidPluginExecutionException ex)
            {
                tracingService?.Trace($"Invalid Exception GetUserDetailsById: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                tracingService?.Trace($"Invalid Exception GetUserDetailsById: {ex.Message}");
                throw ex;
            }
        }
    }
}
