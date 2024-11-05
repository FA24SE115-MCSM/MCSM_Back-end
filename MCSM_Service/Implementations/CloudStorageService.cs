using Google.Cloud.Storage.V1;
using Google;
using MCSM_Service.Interfaces;
using MCSM_Utility.Helpers;
using MCSM_Utility.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MCSM_Service.Implementations
{
    public class CloudStorageService : ICloudStorageService
    {
        private static readonly StorageClient Storage;
        private readonly AppSetting _settings;

        static CloudStorageService()
        {
            Storage = CloudStorageHelper.GetStorage();
        }

        public CloudStorageService(IOptions<AppSetting> settings)
        {
            _settings = settings.Value;
        }

        public async Task<string> UploadImage(Guid id, string contentType, Stream stream)
        {
            try
            {
                await Storage.UploadObjectAsync(
                    _settings.StorageBucket,
                    $"{_settings.ImageFolder}/{id}",
                    contentType,
                    stream,
                    null,
                    CancellationToken.None);
                var baseURL = "https://firebasestorage.googleapis.com/v0/b";
                var filePath = $"{_settings.ImageFolder}%2F{id}";
                var url = $"{baseURL}/{_settings.StorageBucket}/o/{filePath}?alt=media";
                return url;
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> DeleteImage(Guid id)
        {
            try
            {
                await Storage.DeleteObjectAsync(
                    _settings.StorageBucket,
                    $"{_settings.ImageFolder}/{id}",
                    null,
                    CancellationToken.None
                    );
                return "Delete success";
            }
            catch (GoogleApiException ex)
            {
                return ex.HttpStatusCode.ToString();
            }
        }


        public async Task<string> UploadDocument(Guid id, string contentType, Stream stream)
        {
            try
            {
                await Storage.UploadObjectAsync(
                    _settings.StorageBucket,
                    $"{_settings.DocumentFolder}/{id}",
                    contentType,
                    stream,
                    null,
                    CancellationToken.None);
                var baseURL = "https://firebasestorage.googleapis.com/v0/b";
                var filePath = $"{_settings.DocumentFolder}%2F{id}";
                var url = $"{baseURL}/{_settings.StorageBucket}/o/{filePath}?alt=media";
                return url;
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> DeleteDocument(Guid id)
        {
            try
            {
                await Storage.DeleteObjectAsync(
                    _settings.StorageBucket,
                    $"{_settings.DocumentFolder}/{id}",
                    null,
                    CancellationToken.None
                    );
                return "Delete success";
            }
            catch (GoogleApiException ex)
            {
                return ex.HttpStatusCode.ToString();
            }
        }




        // Object url
        public string GetMediaLink(Guid id)
        {
            return CloudStorageHelper.GenerateV4UploadSignedUrl(
                HttpUtility.UrlEncode(_settings.StorageBucket),
                _settings.ImageFolder + '/' + id);
        }
    }
}
