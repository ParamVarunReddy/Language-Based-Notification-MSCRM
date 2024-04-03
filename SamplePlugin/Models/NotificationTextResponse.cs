using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace RexStudios.LDN.Notifications.Models
{
    internal class NotificationTextResponse
    {
        public List<Entity> Entities { get; set; }
        public string Message { get; set; }
        public bool isError { get; set; }
        public  string NotificationText { get; set; }
        public  string ErrorText { get; set; }
    }
}
