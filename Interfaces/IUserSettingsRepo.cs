namespace RexStudios.LanguageDependentNotification
{
    using Microsoft.Xrm.Sdk;
    using System;
    using System.Collections.Generic;

    public interface IUserSettingsRepo
    {
        IEnumerable<Entity> GetUserSettingsbyUserId(Guid? UserId);
        int GetUserLanguageId(Guid? UserId);
    }

    public interface IUserService
    {
        int GetUserLanguageFromUser(Guid userId);
        int GetUserLanguageFromUserPreferences(Guid UserId);
    }
}
