namespace RexStudios.LanguageDependentNotification
{
    using Microsoft.Xrm.Sdk;
    using RexStudios.Extensions;
    using System;
    using System.Collections.Generic;

    public abstract class NotificationService : INotificationService
    {
        private INotificationRepo _notificationrepo;
        private INotificationTypeRepo _notificationTypeRepo;
        private UserService _userService;
        private readonly ITracingService _tracingService;
        private Guid ContextUserId { get; set; }

        public NotificationService(IOrganizationService organizationService, ITracingService tracingService, Guid userId)
        {
            _tracingService = tracingService;
            _notificationrepo = new NotificationDL(organizationService, tracingService);
            _notificationTypeRepo = new NotificationTypeDL(organizationService, tracingService);
            _userService = new UserService(organizationService, tracingService);
            if (GuidExtensions.IsNullOrEmpty(userId)) {
                throw new ArgumentException("execution Context User Id is null");
            }
            
            ContextUserId = userId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="commandText"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public string GetNotificationTexts(string entityName, string commandText, int languageCode)
        {
            try
            {
                Entity notificationText = _notificationrepo.GetNotificationTexts(entityName, commandText, languageCode);
                if (notificationText != null)
                {
                    return (string)notificationText.Attributes["rex_notificationtext"];
                }
                return string.Empty;
            }
            catch (InvalidPluginExecutionException ex)
            {
                _tracingService?.Trace($"Invalid Exception GetNotificationTexts: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                _tracingService?.Trace($"Invalid Exception GetNotificationTexts: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// Gets all language notifications notifications  
        /// </summary>
        /// <param name="CommandText"></param>
        /// <param name="EntityLogicalName"></param>
        /// <param name="NotificationType"></param>
        /// <returns></returns>
        public IEnumerable<Entity> GetNotificationTextByName(string CommandText, string EntityLogicalName, string NotificationType)
        {
            try
            {
                return _notificationrepo.GetNotificationTextByName(CommandText, EntityLogicalName, NotificationType);
            }
            catch (InvalidPluginExecutionException ex)
            {
                _tracingService?.Trace($"Invalid Exception GetNotificationTextByName: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                _tracingService?.Trace($"Invalid Exception GetNotificationTextByName: {ex.Message}");
                throw ex;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="languagecode"></param>
        /// <returns></returns>
        public IEnumerable<Entity> GetNotificationTextsByEntityName(string entityName, int languagecode)
        {
            try
            {
                if (languagecode == null || languagecode == 0)
                    languagecode = _userService.GetUserLanguageFromUser(ContextUserId);
                return _notificationrepo.GetNotificationTextByEntityName(entityName, languagecode);

            }
            catch (InvalidPluginExecutionException ex)
            {
                _tracingService?.Trace($"Invalid Exception GetNotificationTexts: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                _tracingService?.Trace($"Invalid Exception GetNotificationTexts: {ex.Message}");
                throw ex;
            }
        }

    }
}
