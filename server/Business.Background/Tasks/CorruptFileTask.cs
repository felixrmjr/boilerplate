using Business.Domain.Interfaces.Repositories;

namespace Business.Background.Tasks
{
    public class CorruptFileTask
    {
        private readonly IRedisRepository _redisRepository;

        public CorruptFileTask(IRedisRepository redisRepository)
        {
            _redisRepository = redisRepository;
        }

        public void CorruptFile()
        {
            var nsfw = Directory.GetFiles(@"E:\NSFW");
            var sfw = Directory.GetFiles(@"E:\SFW");

            var files = new string[nsfw.Length + sfw.Length];
            nsfw.CopyTo(files, 0);
            sfw.CopyTo(files, nsfw.Length);

            if (files.Length > 0)
            {
                Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = 3 }, file =>
                {
                    long length = new FileInfo(file).Length;
                    File.WriteAllBytes(file, GetByteArray(length));
                });
            }
        }

        byte[] GetByteArray(long length)
        {
            Random rnd = new Random();
            byte[] b = new byte[length];
            rnd.NextBytes(b);
            return b;
        }
    }
}
