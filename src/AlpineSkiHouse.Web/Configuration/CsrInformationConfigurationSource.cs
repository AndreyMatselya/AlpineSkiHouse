using Microsoft.Extensions.Configuration;

namespace AlpineSkiHouse.Web.Configuration
{
    public class CsrInformationConfigurationSource : FileConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            FileProvider = FileProvider ?? builder.GetFileProvider();
            return new CsrInformationConfigurationProvider(this);
        }
    }
}
