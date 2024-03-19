
namespace RexStudios.LanguageDependentNotification
{
    using Microsoft.Xrm.Sdk;
    using RexStudios.Extensions;
    using System;

    public class UserService : IUserService
    {
        private IUserSettingsRepo _userSettingsRepo;
        private IUserRepo _userRepo;
        private ILanguageCodeRepo _languageCodeRepo;
        private readonly ITracingService _tracingService;

        public UserService(IOrganizationService organizationService, ITracingService tracingService)
        {
            _tracingService = tracingService;
            _userSettingsRepo = new UserSettingsDL(organizationService,tracingService);
            _userRepo = new UserDL(organizationService,tracingService);
            _languageCodeRepo = new LanguageCodeDL(organizationService,tracingService);
           
        }

        public int GetUserLanguageFromUser(Guid userId)
        {
            return this.GetLangauageEntity(userId);
        }

        public int GetLangauageEntity(Guid userId)
        {
            try
            {
                var userDetail = _userRepo.GetUserDetailsById(userId);
                if (userDetail != null)
                {
                    EntityReference languageId = (EntityReference)userDetail.Attributes["rex_languageId"];
                    if (languageId != null &&
                    !GuidExtensions.IsNullOrEmpty(languageId.Id))
                    {
                        Entity languageCode = _languageCodeRepo.GetLanguageByID(languageId.Id);
                        if (languageCode != null)
                        {
                            return (int)languageCode.Attributes["rex_languagecode"];
                        }
                    }
                }
                else
                {
                    return this.GetUserLanguageFromUserPreferences(userId);
                }
            }
            catch (InvalidPluginExecutionException ex)
            {
                _tracingService?.Trace($"Invalid Exception GetNotificationTextByEntityName: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                _tracingService?.Trace($"Invalid Exception GetNotificationTextByEntityName: {ex.Message}");
                throw ex;
            }
            return 0;
        }

        public int GetUserLanguageFromUserPreferences(Guid UserId)
        {
            return _userSettingsRepo.GetUserLanguageId(UserId);
        }
    }
}

