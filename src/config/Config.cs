namespace src.config;

static class ConfigurationExtensions
{
    extension(IConfigurationBuilder builder)
    {
        public IConfigurationBuilder AddAppSettings()
        {
            builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.AddEnvironmentVariables();
            return builder;
        }
    }

    extension(IConfiguration configuration)
    {
        public string GetPostgresConnectionString()
        {
            var cs = configuration.GetConnectionString("Postgres")
                ?? configuration["POSTGRES_CONNECTION_STRING"];
            if (string.IsNullOrWhiteSpace(cs))
                throw new InvalidOperationException("Postgres connection string is not configured");
            return cs;
        }
    }
}
