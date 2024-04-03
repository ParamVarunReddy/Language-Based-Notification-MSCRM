// C# sourcecode
/**********************************************************************************************************************
//Author: Param Varun Reddy
//Created: 2024-02-27
//Description: Implemented Strategy pattern to get the notifications based on language, Command, entity name and notification type
//
***********************************************************************************************************************/

namespace RexStudios.LDN.Notifications
{
    using Microsoft.Xrm.Sdk;
    using RexStudios.LanguageDependentNotification;
    using RexStudios.LDN.Notifications.Interfaces;
    using RexStudios.LDN.Notifications.Models;
    using System;
    using System.Linq;

    /// <summary>
    /// Get Notification Command by Notification type
    /// </summary>
    public class GetNotificationByCommandandNotificationTypeStrategy : NotificationService, INotificationStrategy
    {
        private IOrganizationService organizationService;
        private ITracingService tracingService;

        public GetNotificationByCommandandNotificationTypeStrategy(IOrganizationService _organizationService, ITracingService _tracingService)
            : base(_organizationService, _tracingService)
        {
            organizationService = _organizationService;
            tracingService = _tracingService;
        }

        /// <summary>
        /// Get notification by Notification type
        /// </summary>
        /// <param name="notificationTextRequest"></param>
        /// <returns></returns>
        NotificationTextResponse INotificationStrategy.GetNotification(NotificationTextRequest notificationTextRequest)
        {
            NotificationTextResponse notificationTextResponse = new NotificationTextResponse();
            notificationTextResponse.Entities = base.GetNotificationTextByName(notificationTextRequest.CommandText, notificationTextRequest.EntityLogicalName, notificationTextRequest.NotificationType).ToList();
            return notificationTextResponse;
        }
    }

    public class GetNotificationByCommandTextStrategy : NotificationService, INotificationStrategy
    {
        private IOrganizationService organizationService;
        private ITracingService tracingService;

        public GetNotificationByCommandTextStrategy(IOrganizationService _organizationService, ITracingService _tracingService)
            : base(_organizationService, _tracingService)
        {
            organizationService = _organizationService;
            tracingService = _tracingService;
        }
        NotificationTextResponse INotificationStrategy.GetNotification(NotificationTextRequest notificationTextRequest)
        {
            NotificationTextResponse notificationTextResponse = new NotificationTextResponse();
            notificationTextResponse.Message = base.GetNotificationTexts(notificationTextRequest.EntityLogicalName, notificationTextRequest.CommandText, notificationTextRequest.languageId);
            return notificationTextResponse;
        }
    }

    public class GetNotificationByEntityNameStrategy : NotificationService, INotificationStrategy
    {
        private IOrganizationService organizationService;
        private ITracingService tracingService;

        public GetNotificationByEntityNameStrategy(IOrganizationService _organizationService, ITracingService _tracingService)
            : base(_organizationService, _tracingService)
        {
            organizationService = _organizationService;
            tracingService = _tracingService;
        }
        NotificationTextResponse INotificationStrategy.GetNotification(NotificationTextRequest notificationTextRequest)
        {
            NotificationTextResponse notificationTextResponse = new NotificationTextResponse();
            notificationTextResponse.Entities = base.GetNotificationTextsByEntityName(notificationTextRequest.EntityLogicalName, notificationTextRequest.languageId).ToList();
            return notificationTextResponse;
        }
    }
}
