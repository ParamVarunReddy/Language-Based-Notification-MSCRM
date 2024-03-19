namespace RexStudios.Notifications
{
    using System;
    using System.Threading.Tasks;

    public interface IWebRequestAction
    {
        Task<(string, bool)> WebResponseMessage();
    }
}
