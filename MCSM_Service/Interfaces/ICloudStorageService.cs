namespace MCSM_Service.Interfaces
{
    public interface ICloudStorageService
    {
        Task<string> UploadImage(Guid id, string contentType, Stream stream);
        Task<string> DeleteImage(Guid id);

        Task<string> UploadDocument(Guid id, string contentType, Stream stream);
        Task<string> DeleteDocument(Guid id);
    }
}
