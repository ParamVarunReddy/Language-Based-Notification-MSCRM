using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace PCPPlugins.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class CRMExtensions
    {
        public static T Clone<T>(this T entity) where T : Entity
        {
            Entity clone = new Entity(entity.LogicalName);
            foreach (KeyValuePair<string, object> attr in entity.Attributes)
            {
                if (attr.Key.ToLower() == entity.LogicalName.ToLower() + "id")
                    continue;
                clone[attr.Key] = attr.Value;
            }
            return clone.ToEntity<T>();
        }

        public static Entity Retrieve(this IOrganizationService service, string entityName, Guid id,
            params string[] columns)
        {
            return service.Retrieve(entityName, id,
                columns != null && columns.Length > 0
                    ? new Microsoft.Xrm.Sdk.Query.ColumnSet(columns)
                    : new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
        }

        public static Entity Retrieve<T>(this IOrganizationService service, string entityName, Guid id,
            params string[] columns) where T : Entity
        {
            return service.Retrieve(entityName, id, columns).ToEntity<T>();
        }
        public static Entity RetrieveEntity(this IPluginExecutionContext context)
        {
            return (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity) ? (Entity)context.InputParameters["Target"] : null;
        }

        public static EntityReference RetrieveEntityReference(this IPluginExecutionContext context)
        {
            return (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is EntityReference) ? (EntityReference)context.InputParameters["Target"] : null;
        }

        public static Entity RetrievePreImageEntity(this IPluginExecutionContext context, string preImageName)
        {
            return (context.PreEntityImages.Contains(preImageName) && context.PreEntityImages[preImageName] is Entity) ? (Entity)context.PreEntityImages[preImageName] : null;
        }

        public static Relationship RetrieveRelationship(this IPluginExecutionContext context)
        {
            return (context.InputParameters.Contains("Relationship") && context.InputParameters["Relationship"] is Relationship) ? (Relationship)context.InputParameters["Relationship"] : null;
        }

        public static EntityReferenceCollection RetrieveRelatedEntities(this IPluginExecutionContext context)
        {
            return (context.InputParameters.Contains("RelatedEntities") && context.InputParameters["RelatedEntities"] is EntityReferenceCollection) ? (EntityReferenceCollection)context.InputParameters["RelatedEntities"] : null;
        }

        public static ExecuteMultipleResponse ExecuteMultiple(this ExecuteMultipleRequest request, IOrganizationService service, ITracingService tracingService)
        {
            try
            {
                var response = (ExecuteMultipleResponse)service.Execute(request);

                // Check the responses for any errors
                foreach (var responseItem in response.Responses)
                {
                    if (responseItem.Fault != null)
                    {
                        // Throw an exception to handle the error
                        throw new Exception(responseItem.Fault.Message);
                    }
                }
                return response;
            }

            catch (FaultException<OrganizationServiceFault> ex)
            {
                tracingService?.Trace($"Error thrown at Execute Multiple Resposne, {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                tracingService?.Trace($"Error thrown at Execute Multiple Resposne, {ex.Message}");
                throw ex;
            }
        }

        public static ExecuteMultipleRequest AddRequests(this ExecuteMultipleRequest request, IEnumerable<OrganizationRequest> requests)
        {
            var orgReqCollection = new OrganizationRequestCollection();
            orgReqCollection.AddRange(requests);
            request.Requests = orgReqCollection;
            return request;
        }

        public static List<Entity> ToEntityList(this EntityCollection entityCollection)
        {
            if (entityCollection == null)
            {
                return new List<Entity>();
            }
            return entityCollection.Entities.ToList();
        }

        public static T ExecuteAction<T>(this IOrganizationService service, string actionName, IDictionary<string, object> parameters)
        {
            // Create an OrganizationRequest object for the custom action
            var request = new OrganizationRequest(actionName);

            // Set the parameters for the custom action
            foreach (var kvp in parameters)
            {
                request[kvp.Key] = kvp.Value;
            }

            // Execute the custom action
            var response = (OrganizationResponse)service.Execute(request);

            if (response == null || !response.Results.Contains("result"))
            {
                return default(T);
            }

            // Get the result of the custom action
            var result = (T)response.Results["result"];

            return result;
        }

        public static Dictionary<int, T> GetOptionSetEnum<T>(this IOrganizationService service, string attributeName) where T : struct
        {
            RetrieveOptionSetRequest retrieveOptionSetRequest = new RetrieveOptionSetRequest
            {
                Name = attributeName
            };
            RetrieveOptionSetResponse retrieveOptionSetResponse = (RetrieveOptionSetResponse)service.Execute(retrieveOptionSetRequest);
            OptionSetMetadata retrievedOptionSetMetadata = (OptionSetMetadata)retrieveOptionSetResponse.OptionSetMetadata;

            if (!typeof(T).IsEnum)
                throw new InvalidOperationException("T must be an enumerated type");

            var optionSetEnum = Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(x => (int)(object)x, x => x);
            foreach (var option in retrievedOptionSetMetadata.Options)
            {
                if (!optionSetEnum.ContainsKey(option.Value.Value))
                    optionSetEnum.Add(option.Value.Value, (T)Enum.ToObject(typeof(T), option.Value.Value));
            }
            return optionSetEnum;
        }

        public static void CalculateAndUpdateRollupField(this Entity entity, IOrganizationService service, string rollupFieldName)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (string.IsNullOrEmpty(rollupFieldName))
            {
                throw new ArgumentNullException(nameof(rollupFieldName));
            }

            var rollupRequest = new CalculateRollupFieldRequest
            {
                Target = entity.ToEntityReference(),
                FieldName = rollupFieldName
            };

            var response = (CalculateRollupFieldResponse)service.Execute(rollupRequest);

            entity = response.Entity;

            service.Update(entity);
        }

        public static string GetOptionSetText(this IOrganizationService service, string entityName, string fieldName, int optionSetValue)
        {
            // Retrieve the option set metadata
            var attributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = fieldName,
                RetrieveAsIfPublished = true
            };
            var attributeResponse = (RetrieveAttributeResponse)service.Execute(attributeRequest);
            var optionSetMetadata = (EnumAttributeMetadata)attributeResponse.AttributeMetadata;

            // Map the option set value to its corresponding label
            var optionSetValueObject = new OptionSetValue(optionSetValue);
            var optionSetLabel = optionSetMetadata.OptionSet
                .Options.FirstOrDefault(o => o.Value == optionSetValueObject.Value)?.Label.UserLocalizedLabel.Label;

            // Return the option set text value
            return optionSetLabel;
        }
    }
}
