namespace Infrastructure.HostedServices.Config;

public class XmlSynchronizerConfig : BaseConfig
{
    public string XmlFolderPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "XmlImports");
    public string ArchiveFolderPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "XmlImports", "Archive");
}
