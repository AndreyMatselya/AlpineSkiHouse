using System.IO;
using Microsoft.Extensions.Configuration;

namespace AlpineSkiHouse.Web.Configuration
{
    public class CsrInformationConfigurationProvider : FileConfigurationProvider
    {
        public CsrInformationConfigurationProvider(FileConfigurationSource source) : base(source) { }

        public override void Load(Stream stream)
        {
            var parser = new CsrInformationParser();
            Data = parser.Parse(stream);
        }
    }
}
