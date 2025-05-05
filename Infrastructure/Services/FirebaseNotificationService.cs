using Application.Common.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class FirebaseNotificationService : INotificationService
    {
        private readonly FirebaseApp _firebaseApp;

        public FirebaseNotificationService(IConfiguration configuration)
        {
            var credentialsPath = configuration["Firebase:CredentialsPath"];
            //_firebaseApp = FirebaseApp.Create(new AppOptions
            //{
            //    Credential = GoogleCredential.FromFile(credentialsPath)
            //});
        }
        public async Task SendNotificationAsync(string title, string body, string userToken)
        {
            var message = new Message
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Token = userToken
            };

            try
            {
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine($"Successfully sent message: {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }
    }
}
