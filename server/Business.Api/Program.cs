namespace AD.Server
{
    public class Program
    {
        // TODO:
        // Bogus -> https://www.youtube.com/watch?v=dlKFwIg2Ekw&ab_channel=GPCode
        // MassTransit -> https://www.youtube.com/watch?v=HySA0Ij-GWQ&ab_channel=JoseCarlosMacoratti https://www.youtube.com/watch?v=oA4xrvPnPYc&ab_channel=RaphaelAndrade
        // RequestLogginFilter

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
