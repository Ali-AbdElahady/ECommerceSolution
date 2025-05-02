using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Infrastructure.Services
{
    public static class FirebaseInitializer
    {
        public static void Initialize()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("firebase-config.json")
                });
            }
        }
    }
}
