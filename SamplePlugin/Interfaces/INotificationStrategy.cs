
namespace RexStudios.LDN.Notifications.Interfaces
{
    using RexStudios.LDN.Notifications.Models;

    internal interface INotificationStrategy
    {
        NotificationTextResponse GetNotification(NotificationTextRequest request);
    }
}
