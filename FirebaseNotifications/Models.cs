using System.ComponentModel.DataAnnotations.Schema;

namespace FirebaseNotifications
{
    public class FcmNotificationPayload
    {
        public Message message { get; set; }
    }

    public class Message
    {
        public string token { get; set; }
        public FCMNotification notification { get; set; }
        public Dictionary<string, string> data { get; set; }
    }
    public class PushNotificationSetUpDto
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        public string Body { get; set; }
        public string FcmToken { get; set; }
        [NotMapped]
        public bool IsChat { get; set; } = false;
        public int Badge { get; set; }
        public bool IsAndroid { get; set; }
    }
    public class FCMNotification
    {
        public string image { get; set; }
        public string body { get; set; }
        public string title { get; set; }
    }
}
