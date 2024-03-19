namespace RexStudios.LanguageDependentNotification
{
    using Microsoft.Xrm.Sdk;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface INotificationTypeRepo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NotificationTypeId"></param>
        /// <returns></returns>
        Entity GetNotificationTypeById(Guid? NotificationTypeId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NotificationTypeText"></param>
        /// <returns></returns>
        Entity GetNotificationTypeByText(string NotificationTypeText);

    }
}
