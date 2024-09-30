using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace MCSM_Utility.Helpers
{
    public class CloudStorageHelper
    {
        private static readonly StorageClient Storage;
        private static readonly UrlSigner UrlSigner;

        static CloudStorageHelper()
        {
            GoogleCredential credential;

            var credentialJson = Environment.GetEnvironmentVariable("GoogleCloudCredential");
            if (string.IsNullOrWhiteSpace(credentialJson))
            {

                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var projectRoot = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", ".."));
                string credentialPath = Path.Combine(projectRoot, "MCSM_Utility", "Helpers", "CloudStorage", "mcsm-fa24se115-firebase-adminsdk-9hr0w-9c32ab4d6f.json");
                credential = GoogleCredential.FromFile(credentialPath);
            }
            else
            {
                credential = GoogleCredential.FromJson(credentialJson);
            }
            // Storage
            Storage = StorageClient.Create(credential);

            // Url Signer
            UrlSigner = UrlSigner.FromCredential(credential);

        }

        public static StorageClient GetStorage()
        {
            return Storage;
        }

        // Generate signed cloud storage object url 
        public static string GenerateV4UploadSignedUrl(string bucketName, string objectName)
        {
            var options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(24));

            var template = UrlSigner.RequestTemplate
                .FromBucket(bucketName)
                .WithObjectName(objectName)
                .WithHttpMethod(HttpMethod.Get);

            return UrlSigner.Sign(template, options);
        }

    }
}
