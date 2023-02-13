using Minio.DataModel;

namespace Business.Domain.Interfaces.Repositories
{
    public interface IFileRepository
    {
        Task<bool> VerifyIfBucketExists(string bucket);
        Task UpdateFile(string bucket, string filePath, string obj, ServerSideEncryption? sse = null);
        Task CopyFile(string fromBucket, string fromObj, string toBucket, string toObj, ServerSideEncryption? sseSrc = null, ServerSideEncryption? sseDest = null);
        Task CreateBucket(string bucket);
        Task DeleteBucket(string bucket);
        void GetFilesInBucketByPrefix(string bucket, string? prefix, bool? recursive = true, bool? versions = false);
        Task<BucketNotification> GetBucketNotifications(string bucket);
        Task<IEnumerable<Bucket>> GetAllBuckets();
        void ListenIncompleteUploads(string bucket, string? prefix, bool? recursive = true);
        void ListenBucketNotifications(string bucket, List<EventType> events, string? prefix = "", string? suffix = "", bool? recursive = true);
        Task RemoveAllBucketNotifications(string bucket);
        Task RemoveIncompleteUpload(string bucket, string obj);
        Task RemoveObject(string bucket, string obj, string? versionId = null);
        Task RemoveObjects(string bucket, List<string> objs);
    }
}
