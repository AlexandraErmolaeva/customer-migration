namespace Infrastructure.HostedServices.Config;

public class XmlGeneratorConfig : BaseConfig
{
    public string XmlFolderPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "XmlImports");
}
