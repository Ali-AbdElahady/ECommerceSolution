using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public static class FirebaseInitializer
    {
        public static void Initialize(IConfiguration configuration)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                // Retrieve the credentials path from appsettings.json
                var credentialsPath = configuration["Firebase:CredentialsPath"];
                if (string.IsNullOrEmpty(credentialsPath))
                {
                    throw new InvalidOperationException("Firebase credentials path is not configured.");
                }

                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(credentialsPath)
                });
            }
        }
    }
}
