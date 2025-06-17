using System;
using Microsoft.Extensions.Configuration;

namespace DreamCraftServer0._02.Utils
{
    public static class Config
    {
        private static IConfigurationRoot _config;

        static Config()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _config = builder.Build();
        }

        public static string Db_Host => _config["Database:Host"];
        public static string Db_Name => _config["Database:Name"];
        public static string Db_User => _config["Database:User"];
        public static string Db_Password => _config["Database:Password"];

        public static int TcpPort => int.Parse(_config["Server:TcpPort"]);
        public static int UdpPort => int.Parse(_config["Server:UdpPort"]);
    }
}
