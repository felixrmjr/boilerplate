using Business.Database;
using Business.Domain.Interfaces.Repositories;
using Business.Domain.Model;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;

namespace Business.Repository.Repositories
{
    public class FileRepository : IFileRepository
    {
        public IDisposable SubscriptionNotifications { get; private set; }
        public IDisposable SubscriptionIncompleteUpoads { get; private set; }
        public IDisposable SubscriptionFilesInBucket { get; private set; }
        public IDisposable SubscriptionObjects { get; private set; }
        // TODO: Trocar por uma collection no mongo ou cache no redis.
        public List<string> Notifications { get; private set; } = new List<string>();
        public List<string> IncompleteUploads { get; private set; } = new List<string>();
        public List<string> FilesInBucket { get; private set; } = new List<string>();
        public List<string> Objects { get; private set; } = new List<string>();


        private readonly AppMinioDbContext _context;

        public FileRepository(IOptions<MinioConnection> settings)
        {
            _context = new AppMinioDbContext(settings);
        }

        public async Task<bool> VerifyIfBucketExists(string bucket)
        {
            var args = new BucketExistsArgs()
                .WithBucket(bucket);
            var found = await _context.Minio.BucketExistsAsync(args);
            if (found)
                return found;
            else
                throw new FileNotFoundException();
        }

        public async Task UpdateFile(string bucket, string filePath, string obj, ServerSideEncryption? sse = null)
        {
            var bs = File.ReadAllBytes(filePath);
            using (var filestream = new MemoryStream(bs))
            {
                var fileInfo = new FileInfo(filePath);
                var metaData = new Dictionary<string, string> { { "Metadata", $"{bucket}_{obj}" } };
                var args = new PutObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(obj)
                    .WithStreamData(filestream)
                    .WithObjectSize(filestream.Length)
                    .WithContentType("application/octet-stream")
                    .WithHeaders(metaData)
                    .WithServerSideEncryption(sse);
                await _context.Minio.PutObjectAsync(args);
            }
        }

        public async Task CopyFile(string fromBucket, string fromObj, string toBucket, string toObj, ServerSideEncryption? sseSrc = null, ServerSideEncryption? sseDest = null)
        {
            var cpSrcArgs = new CopySourceObjectArgs()
                .WithBucket(fromBucket)
                .WithObject(fromObj)
                .WithServerSideEncryption(sseSrc);
            var args = new CopyObjectArgs()
                .WithBucket(toBucket)
                .WithObject(toObj)
                .WithCopyObjectSource(cpSrcArgs)
                .WithServerSideEncryption(sseDest);
            await _context.Minio.CopyObjectAsync(args);
        }

        public async Task CreateBucket(string bucket)
        {
            await _context.Minio.MakeBucketAsync(
                new MakeBucketArgs()
                    .WithBucket(bucket));
        }

        public async Task DeleteBucket(string bucket)
        {
            await _context.Minio.RemoveBucketAsync(
                new RemoveBucketArgs()
                    .WithBucket(bucket));
        }

        public void GetFilesInBucketByPrefix(string bucket, string? prefix, bool? recursive = true, bool? versions = false)
        {
            var listArgs = new ListObjectsArgs()
                .WithBucket(bucket)
                .WithPrefix(prefix)
                .WithRecursive((bool)recursive);

            var observable = _context.Minio.ListObjectsAsync(listArgs);
            SubscriptionFilesInBucket = observable.Subscribe(
                item => FilesInBucket.Add(item.Key),
                ex => throw new Exception(ex.Message));
        }

        public async Task<BucketNotification> GetBucketNotifications(string bucket)
        {
            var args = new GetBucketNotificationsArgs()
                .WithBucket(bucket);
            return await _context.Minio.GetBucketNotificationsAsync(args);
        }

        public async Task<IEnumerable<Bucket>> GetAllBuckets()
        {
            var list = await _context.Minio.ListBucketsAsync();
            return list.Buckets;
        }

        public void ListenIncompleteUploads(string bucket, string? prefix, bool? recursive = true)
        {
            var args = new ListIncompleteUploadsArgs()
                .WithBucket(bucket)
                .WithPrefix(prefix)
                .WithRecursive((bool)recursive);

            var observable = _context.Minio.ListIncompleteUploads(args);
            SubscriptionIncompleteUpoads = observable.Subscribe(
                item => IncompleteUploads.Add(item.Key),
                ex => throw new Exception(ex.Message));
        }

        public void ListenBucketNotifications(string bucket, List<EventType> events, string? prefix = "", string? suffix = "", bool? recursive = true)
        {
            events = events ?? new List<EventType> { EventType.ObjectCreatedAll };
            var args = new ListenBucketNotificationsArgs()
                .WithBucket(bucket)
                .WithPrefix(prefix)
                .WithEvents(events)
                .WithSuffix(suffix);

            var observable = _context.Minio.ListenBucketNotificationsAsync(bucket, events, prefix, suffix);
            SubscriptionNotifications = observable.Subscribe(
                notification => Notifications.Add(notification.json),
                ex => throw new Exception(ex.Message));
        }

        public async Task RemoveAllBucketNotifications(string bucket)
        {
            var args = new RemoveAllBucketNotificationsArgs()
                .WithBucket(bucket);
            await _context.Minio.RemoveAllBucketNotificationsAsync(args);
        }

        public async Task RemoveIncompleteUpload(string bucket, string obj)
        {
            var args = new RemoveIncompleteUploadArgs()
                .WithBucket(bucket)
                .WithObject(obj);
            await _context.Minio.RemoveIncompleteUploadAsync(args);
        }

        public async Task RemoveObject(string bucket, string obj, string? versionId = null)
        {
            var args = new RemoveObjectArgs()
                .WithBucket(bucket)
                .WithObject(obj);

            if (!string.IsNullOrEmpty(versionId))
            {
                args = args.WithVersionId(versionId);
            }

            await _context.Minio.RemoveObjectAsync(args);
        }

        public async Task RemoveObjects(string bucket, List<string> objs)
        {
            var objArgs = new RemoveObjectsArgs()
                .WithBucket(bucket)
                .WithObjects(objs);

            var objectsOservable = await _context.Minio.RemoveObjectsAsync(objArgs).ConfigureAwait(false);
            SubscriptionObjects = objectsOservable.Subscribe(
                objDeleteError => Objects.Add(objDeleteError.Key),
                ex => throw new Exception(ex.Message));
        }
    }
}
