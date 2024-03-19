
namespace RexStudios.LanguageDependentNotification
{
    using Microsoft.Xrm.Sdk;
    using System;

    public interface ILanguageCodeRepo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        Entity GetLanguageByLanguageCode(int? languageCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Entity GetLanguageByID(Guid? Id);

        /// <summary>
        /// Gat language by
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        Entity GeyLanguagebyCulture(string culture); 
    }
}
