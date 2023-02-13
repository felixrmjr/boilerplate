﻿using Business.Domain.Model;
using Microsoft.Extensions.Options;
using Minio;

namespace Business.Database
{
    public class AppMinioDbContext
    {
        public MinioClient Minio;

        public AppMinioDbContext(IOptions<MinioConnection> settings)
        {
            Minio = new MinioClient()
                            .WithEndpoint(settings.Value.Endpoint)
                            .WithCredentials(settings.Value.AccessKey, settings.Value.SecretKey)
                            .Build();
        }
    }
}