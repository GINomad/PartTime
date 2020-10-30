using Microsoft.Extensions.Configuration;
using PT.Infrastructure.Abstractions;

namespace PT.Infrastructure
{
    public class Settings :ISettings
    {
        public Settings(IConfiguration configuration)
        {
            ConnectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public string ConnectionString { get; private set; }
        public static Settings Default { get; set; }
    }
}
