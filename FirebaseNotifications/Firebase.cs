using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseNotifications
{    
    public static class Firebase
    {
        const string FcmEndpoint= "https://fcm.googleapis.com/v1/projects/your-project-id/messages:send";
        private static async Task<string> GetAccessToken()
        {
            var client = new HttpClient();
            var credentialPath = "path of service json"; // Provide the full path to your service account file
            using (var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
            {
                // Load the credentials from the JSON content
                GoogleCredential googleCredential = GoogleCredential.FromStream(stream)
                    .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");

                // Get the access token
                return await googleCredential.UnderlyingCredential.GetAccessTokenForRequestAsync();
            }           
        }
        public static object GetPayload(PushNotificationSetUpDto pushNotificationSetUpDto)
        {
            return new FcmNotificationPayload()
            {
                message = new Message()
                {
                    token = pushNotificationSetUpDto.FcmToken,  // Send notification to multiple tokens
                    notification = new FCMNotification()
                    {
                        title = !string.IsNullOrWhiteSpace(pushNotificationSetUpDto.Title) ? pushNotificationSetUpDto.Title : "Title",
                        body = pushNotificationSetUpDto.Body,
                        image = pushNotificationSetUpDto.ImageUrl,
                        
                    },
                    data = new Dictionary<string, string>()
                    {
                        { "badge", pushNotificationSetUpDto.Badge.ToString() }, 
                    }
                }
            };           
        }
        public static async Task<bool> PushNotification(PushNotificationSetUpDto pushNotificationSetUpDto)
        {
            try
            {
                var client = new HttpClient();
                string accessToken = await GetAccessToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var payload = GetPayload(pushNotificationSetUpDto);
                var stringContent = new StringContent(JsonConvert.SerializeObject(payload));
                stringContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = await client.PostAsync(FcmEndpoint, stringContent).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
            catch (TaskCanceledException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
