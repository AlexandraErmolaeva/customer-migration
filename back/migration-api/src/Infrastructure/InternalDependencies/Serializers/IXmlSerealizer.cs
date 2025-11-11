namespace Infrastructure.InternalDependencies.Serializers;

public interface IXmlSerealizer
{
    Task<T> DeserializeObjectAsync<T>(string xml);
    Task<string> SerializeObjectAsync<T>(T obj);
}
