// C# sourcecode
/**********************************************************************************************************************
Author: Param Varun Reddy
Created: 2024-02-28
Description: Repository pattern Data Layer class LanguageCode,
***********************************************************************************************************************/
namespace RexStudios.LanguageDependentNotification
{
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using RexStudios.Extensions;
    using RexStudios.LanguageDependentNotification.Core;
    using System;
    using System.Linq;

    internal class LanguageCodeDL : EntityRetrieverBase<Entity>, ILanguageCodeRepo
    {
        private ITracingService tracingService;
        public LanguageCodeDL(IOrganizationService service, ITracingService _tracingService)
            : base(service, _tracingService, "rex_languagecode")
        {
            traceService = _tracingService;
        }

        /// <summary>
        /// Check if Entity Exist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool Exists(Guid id)
        {
            return this.Retrieve(id, new ColumnSet(false)) != null;
        }

        /// <summary>
        /// Retrieve Entity by ID
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="columnSet"></param>
        /// <returns></returns>
        public override Entity Retrieve(Guid entityId, ColumnSet columnSet)
        {
            return base.Retrieve(entityId, columnSet);
        }

        /// <summary>
        /// Get Language by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Entity GetLanguageByID(Guid? Id)
        {
            try
            {
                if (orgService == null)
                    throw new InvalidPluginExecutionException($"GetLanguageByID: Organization service is null, can't process the request");
                if (GuidExtensions.IsNullOrEmpty(Id))
                    throw new InvalidPluginExecutionException($"GetLanguageByID: Guid is null, can't process the request");
                return this.Retrieve(Id.Value, new ColumnSet(true));
            }
            catch (InvalidPluginExecutionException ex)
            {
                tracingService?.Trace($"Invalid Exception GetLanguageByID: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                tracingService?.Trace($"Invalid Exception GetLanguageByID: {ex.Message}");
                throw ex;
            }
        }

        // <summary>
        // Get Language Entity By Language Code 
        // </summary>
        // <param name="languageCode"></param>
        // <returns>Language Entity</returns>
        public Entity GetLanguageByLanguageCode(int? languageCode)
        {
            try
            {
                if (orgService == null)
                    throw new InvalidPluginExecutionException($"GetLanguageByLanguageCode: Organization service is null, can't process the request");
                if (languageCode== null || languageCode==0)
                    throw new InvalidPluginExecutionException($"GetLanguageByLanguageCode: languagecode is null, can't process the request");
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
                            Values = {languageCode}
                        }
                    }
                    }
                };
                return base.RetrieveMultipleEntities(query).FirstOrDefault(); 
            }
            catch (InvalidPluginExecutionException ex)
            {
                tracingService?.Trace($"Invalid Exception GetLanguageByLanguageCode: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                tracingService?.Trace($"Invalid Exception GetLanguageByLanguageCode: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// Get Lanaguage by langaugeCode
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public Entity GeyLanguagebyCulture(string culture)
        {
            try
            {
                if (orgService == null)
                    throw new InvalidPluginExecutionException($"GetLanguageByID: Organization service is null, can't process the request");
                if (StringExtensions.IsNullOrEmpty(culture))
                    throw new InvalidPluginExecutionException($"GetLanguageByID: Guid is null, can't process the request");
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
                            Values = {culture}
                        }
                    }
                    }
                };
                return base.RetrieveMultipleEntities(query).FirstOrDefault();
            }
            catch (InvalidPluginExecutionException ex)
            {
                tracingService?.Trace($"Invalid Exception GeyLanguagebyCulture: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                tracingService?.Trace($"Invalid Exception GeyLanguagebyCulture: {ex.Message}");
                throw ex;
            }
        }
    }
}
