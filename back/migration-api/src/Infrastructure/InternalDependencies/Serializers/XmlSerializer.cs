using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Infrastructure.InternalDependencies.Serializers;

public class XmlSerializer : IXmlSerealizer
{
    public async Task<T> DeserializeObjectAsync<T>(string xml)
    {
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        var bytes = Encoding.UTF8.GetBytes(xml);

        await using var memoryStream = new MemoryStream(bytes);
        using var reader = XmlReader.Create(memoryStream);
        return (T)serializer.Deserialize(reader);
    }

    public async Task<string> SerializeObjectAsync<T>(T obj)
    {
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        var namespaces = new XmlSerializerNamespaces();
        namespaces.Add("", "");

        await using var memoryStream = new MemoryStream();
        var settings = new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(false),
            Indent = true,
            OmitXmlDeclaration = false
        };

        using (var writer = XmlWriter.Create(memoryStream, settings))
        {
            serializer.Serialize(writer, obj, namespaces);
        }

        memoryStream.Position = 0;
        using var reader = new StreamReader(memoryStream, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
}