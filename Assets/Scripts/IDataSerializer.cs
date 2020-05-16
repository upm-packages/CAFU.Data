namespace CAFU.Data
{
    public interface IDataSerializer<T>
    {
        T Serialize(byte[] data);
        byte[] Deserialize(T data);
    }
}