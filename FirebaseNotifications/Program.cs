// See https://aka.ms/new-console-template for more information
using FirebaseNotifications;

Console.WriteLine("Hello, World!");
await Firebase.PushNotification(
                            new PushNotificationSetUpDto
                            {
                                ImageUrl = "",
                                Title = "Test",
                                Body = "Hello world",
                                FcmToken = "",//Add fcm token here
                                Badge = 2,
                                IsAndroid = true,
                            });
