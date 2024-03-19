namespace RexStudios.LanguageDependentNotification
{
    using Microsoft.Xrm.Sdk;
    using System;
    public interface IUserRepo { 
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Entity GetUserDetailsById(Guid? userId);
    }
}
