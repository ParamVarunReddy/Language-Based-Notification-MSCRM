namespace RexStudios.LanguageDependentNotification
{
    using Microsoft.Xrm.Sdk;
    using System;
    using System.Collections.Generic;

    public interface INotificationRepo
    {
        /// <summary>
        /// Get Notification Message Text by sending the GUId
        /// </summary>
        /// <param name="NotificationId"></param>
        /// <returns></returns>
        Entity GetNotificationTextByID(Guid? NotificationId);

        /// <summary>
        /// Get NotificationText by Sending the 
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        Entity GetNotificationTextByName(string CommandText);

        /// <summary>
        /// Get Notification Text based on the logical name and notificationType
        /// </summary>
        /// <param name="CommandText"></param>
        /// <param name="EntityLogicalName"></param>
        /// <param name="NotificationType"></param>
        /// <returns></returns>
        IEnumerable<Entity> GetNotificationTextByName(string CommandText, string EntityLogicalName, string NotificationType);

        /// <summary>
        /// Gets all notification Messages related to the 
        /// </summary>
        /// <param name="EntityLogicalName"></param>
        /// <returns></returns>
        IEnumerable<Entity> GetNotificationTextByEntityName(string EntityLogicalName, int languagecode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="commandText"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        Entity GetNotificationTexts(string entityName, string commandText, int languageCode);
    }

    public interface INotificationService
    {
        string GetNotificationTexts(string entityName, string commandText, int languageCode);

        IEnumerable<Entity> GetNotificationTextByName(string CommandText, string EntityLogicalName, string NotificationType);

        IEnumerable<Entity> GetNotificationTextsByEntityName(string entityName, int languagecode);
    }
}
