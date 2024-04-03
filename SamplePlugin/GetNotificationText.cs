// C# sourcecode
/**********************************************************************************************************************
//Author: Param Varun Reddy
//Created: 2024-02-27
//Description: Get Notification Action method is used to get the notification text based on the NotificationType, Language code and User preferences
//Trying to Implement the Strategy pattern to get the data from the Dynamics CRM\
***********************************************************************************************************************/

namespace RexStudios.LDN.Notifications
{
    using Microsoft.Xrm.Sdk;
    using RexStudios.PluginDevelopment;
    using RexStudios.LDN.Notifications.Models;
    using RexStudios.LDN.Notifications.Interfaces;
    using RexStudios.Extensions;
    using System;
    using System.Collections.Generic;

    public class GetNotificationText : PluginBase
    {
        private NotificationTextRequest notificationTextRequest;
        private NotificationTextResponse notificationTextResponse;
        private Dictionary<string, Func<INotificationStrategy>> _strategyMap;

        public GetNotificationText() : base(typeof(GetNotificationText))
        {
            notificationTextRequest = new NotificationTextRequest();
            notificationTextResponse = new NotificationTextResponse();
        }

        private static Dictionary<string, Func<INotificationStrategy>> CreateCommandDictionary(IOrganizationService organizationService, ITracingService tracingService)
        {
            return new Dictionary<string, Func<INotificationStrategy>>
            {
                {"1001", ()=>new GetNotificationByEntityNameStrategy(organizationService,tracingService)},
                {"1111", () => new GetNotificationByCommandandNotificationTypeStrategy(organizationService,tracingService)},
                {"1101", () => new GetNotificationByCommandTextStrategy(organizationService,tracingService)},
            };
        }

        protected override void ExecuteCrmPlugin(LocalPluginContext localContext)
        {
            try
            {
                if (localContext.TracingService == null)
                {
                    throw new InvalidPluginExecutionException("Failed to get Tracing service");
                }
                localContext.Trace("Executing Get Notification Text Action ..");
                _strategyMap = CreateCommandDictionary(localContext.OrganizationService, localContext.TracingService);
                notificationTextRequest = localContext.PluginExecutionContext.InputParameters["request"] != null ?
                    JsonSerializerExtension.Deserialize<NotificationTextRequest>(localContext.PluginExecutionContext.InputParameters["request"].ToString()) : null;
                if (notificationTextRequest == null)
                    throw new InvalidPluginExecutionException("The request object is null, can not process the request");
                notificationTextRequest.languageId = localContext.CustomUserLanguage;
                string key = $"{(!string.IsNullOrEmpty(notificationTextRequest.EntityLogicalName) ? "1" : "0")}" +
                    $"{(!string.IsNullOrEmpty(notificationTextRequest.CommandText) ? "1" : "0")}" +
                    $"{(!string.IsNullOrEmpty(notificationTextRequest.NotificationType) ? "1" : "0")}" +
                    $"1";
                if (_strategyMap.TryGetValue(key, out var strategyFactory))
                {
                    var strategy = strategyFactory();
                    NotificationTextResponse notificationresponse = strategy.GetNotification(notificationTextRequest);
                    localContext.PluginExecutionContext.OutputParameters["response"] = JsonSerializerExtension.Serialize(notificationTextResponse);
                }
                else
                {
                    throw new InvalidPluginExecutionException($"there is a mismatch with the parametrs sent, plese check and resend");
                }
            }
            catch (InvalidPluginExecutionException ex)
            {
                localContext.PluginExecutionContext.OutputParameters["ErrorText"] = notificationTextResponse.ErrorText = ex.Message;
                localContext.PluginExecutionContext.OutputParameters["isError"] = notificationTextResponse.isError = true;
                throw ex;
            }
            catch (TimeoutException ex)
            {
                localContext.PluginExecutionContext.OutputParameters["ErrorText"] = notificationTextResponse.ErrorText = ex.Message;
                localContext.PluginExecutionContext.OutputParameters["isError"] = notificationTextResponse.isError = true;
                throw ex;
            }
            catch (Exception ex)
            {
                localContext.PluginExecutionContext.OutputParameters["ErrorText"] = notificationTextResponse.ErrorText = ex.Message;
                localContext.PluginExecutionContext.OutputParameters["isError"] = notificationTextResponse.isError = true;
                throw ex;
            }
        }


    }
}
