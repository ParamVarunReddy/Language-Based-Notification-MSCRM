namespace RexStudios.LDN.Notifications.Models
{
    internal class NotificationTextRequest
    {
        public string EntityLogicalName { get; set; }
        public string CommandText { get; set; }
        public string NotificationType { get; set; }
        public int languageId { get; set; }
    }
}
