namespace CAFU.Data
{
    public interface IDataSerializer<T>
    {
        byte[] Serialize(T data);
        T Deserialize(byte[] data);
    }
}