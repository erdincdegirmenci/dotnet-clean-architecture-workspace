using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Managers;

namespace Template.Infrastructure.Managers
{
    public class ConfigManager : IConfigManager
    {
        static readonly Dictionary<string, string> configurations = new Dictionary<string, string>();


        public static void LoadConfig(IConfiguration configuration)
        {
            // Connection string
            configurations["DBConnection"] = configuration.GetConnectionString("DefaultConnection");

            // AppSettings
            configurations["AppSettings:PasswordHashKey"] = configuration["AppSettings:PasswordHashKey"];
            configurations["AppSettings:LocalCacheKey"] = configuration["AppSettings:LocalCacheKey"];
            configurations["AppSettings:MailExpireDay"] = configuration["AppSettings:MailExpireDay"];
            configurations["AppSettings:LoginFailedTryLimit"] = configuration["AppSettings:LoginFailedTryLimit"];
            configurations["AppSettings:LoginFailedTimeLimit"] = configuration["AppSettings:LoginFailedTimeLimit"];
            configurations["AppSettings:FrontEndUrl"] = configuration["AppSettings:FrontEndUrl"];

            // FeatureManagement
            configurations["FeatureManagement:UseKafka"] = configuration["FeatureManagement:UseKafka"];
            configurations["FeatureManagement:UseElastic"] = configuration["FeatureManagement:UseElastic"];
            configurations["FeatureManagement:UseRedis"] = configuration["FeatureManagement:UseRedis"];
            configurations["FeatureManagement:UseOpenTelemetry"] = configuration["FeatureManagement:UseOpenTelemetry"];
            configurations["FeatureManagement:UseSwagger"] = configuration["FeatureManagement:UseSwagger"];

            // JwtOptions
            configurations["JwtOptions:Issuer"] = configuration["JwtOptions:Issuer"];
            configurations["JwtOptions:Audience"] = configuration["JwtOptions:Audience"];
            configurations["JwtOptions:SecurityKey"] = configuration["JwtOptions:SecurityKey"];
            configurations["JwtOptions:TokenExpiration"] = configuration["JwtOptions:TokenExpiration"];
            configurations["JwtOptions:RefreshTokenExpiration"] = configuration["JwtOptions:RefreshTokenExpiration"];

            // Redis
            configurations["Redis:ConnectionString"] = configuration["Redis:ConnectionString"];

            // Kafka
            configurations["Kafka:BootstrapServers"] = configuration["Kafka:BootstrapServers"];
            configurations["Kafka:Topic"] = configuration["Kafka:Topic"];

            // ElasticSearch
            configurations["ElasticSearchConfiguration:Uri"] = configuration["ElasticSearchConfiguration:Uri"];
            configurations["ElasticSearchConfiguration:Index"] = configuration["ElasticSearchConfiguration:Index"];

            //configurations.Add("DBConnection", configuration.GetConnectionString("DBConnection"));

            ////Veritabanından çekilecek
            //configurations.Add("AppSettings:PasswordHashKey", configuration.GetValue<string>("AppSettings:PasswordHashKey"));
            //configurations.Add("AppSettings:LocalCacheKey", configuration.GetValue<string>("AppSettings:LocalCacheKey"));
            //configurations.Add("AppSettings:MailExpireDay", configuration.GetValue<string>("AppSettings:MailExpireDay"));
            //configurations.Add("AppSettings:LoginFailedTryLimit", configuration.GetValue<string>("AppSettings:LoginFailedTryLimit"));
            //configurations.Add("AppSettings:LoginFailedTimeLimit", configuration.GetValue<string>("AppSettings:LoginFailedTimeLimit"));
            //configurations.Add("AppSettings:FrontEndUrl", configuration.GetValue<string>("AppSettings:FrontEndUrl"));



            //configurations.Add("JwtOptions:Audience", configuration.GetValue<string>("JwtOptions:Audience"));
            //configurations.Add("JwtOptions:Issuer", configuration.GetValue<string>("JwtOptions:Issuer"));
            //configurations.Add("JwtOptions:TokenExpiration", configuration.GetValue<string>("JwtOptions:TokenExpiration"));
            //configurations.Add("JwtOptions:SecurityKey", configuration.GetValue<string>("JwtOptions:SecurityKey"));
            //configurations.Add("JwtOptions:RefreshTokenExpiration", configuration.GetValue<string>("JwtOptions:RefreshTokenExpiration"));

            //configurations.Add("Grcp:Host", configuration.GetValue<string>("Grcp:Host"));

        }

        public string GetConfig(string configName)
        {
            if (configurations.ContainsKey(configName))
                return configurations[configName];
            return null;
        }
    }
}
