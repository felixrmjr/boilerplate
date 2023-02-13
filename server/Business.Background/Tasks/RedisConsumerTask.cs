using Business.Domain.Interfaces.Repositories;
using Business.Domain.Model;

namespace Business.Background.Tasks
{
    public class RedisConsumerTask
    {
        private readonly IRedisRepository _redisRepository;

        public RedisConsumerTask(IRedisRepository redisRepository)
        {
            _redisRepository = redisRepository;
        }

        public async Task RedisConsumer()
        {
            var keys = await _redisRepository.GetAllKeysWithValue<Image>();

            try
            {
                if (keys != null && keys.Count > 0)
                {
                    var (nsfw, sfw) = GenerateFolders();

                    await Parallel.ForEachAsync(keys, new ParallelOptions { MaxDegreeOfParallelism = 5 }, async (key, cancellationToken) =>
                    {
                        if (key != null)
                        {
                            if (cancellationToken.IsCancellationRequested)
                                throw new TaskCanceledException(nameof(RedisConsumer));

                            if (key.is_nsfw)
                                await DownloadImageAsync(nsfw, key.image_id.ToString(), new Uri(key.url));
                            else
                                await DownloadImageAsync(sfw, key.image_id.ToString(), new Uri(key.url));

                            await _redisRepository.Delete(key.image_id.ToString());
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(keys);
                Console.WriteLine(ex.ToString());
            }

            static (string, string) GenerateFolders()
            {
                string nsfw = @"E:\NSFW";
                string sfw = @"E:\SFW";
                if (!Directory.Exists(nsfw))
                    Directory.CreateDirectory(nsfw);

                if (!Directory.Exists(sfw))
                    Directory.CreateDirectory(sfw);

                return (nsfw, sfw);
            }
        }

        private async Task DownloadImageAsync(string directoryPath, string fileName, Uri uri)
        {
            using (var httpClient = new HttpClient())
            {
                var uriWithoutQuery = uri.GetLeftPart(UriPartial.Path);
                var fileExtension = Path.GetExtension(uriWithoutQuery);

                var path = Path.Combine(directoryPath, $"{fileName}{fileExtension}");

                if (!File.Exists(path))
                {
                    var imageBytes = await httpClient.GetByteArrayAsync(uri);
                    await File.WriteAllBytesAsync(path, imageBytes);
                }
            }
        }
    }
}
