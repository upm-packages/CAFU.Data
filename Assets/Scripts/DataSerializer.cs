using System.Text;
using UnityEngine;

namespace CAFU.Data.DataSerializer
{
    public class JsonUtility<T> : IDataSerializer<T>
    {
        public static JsonUtility<T> Default { get; } = new JsonUtility<T>();

        public T Serialize(byte[] data)
        {
            return JsonUtility.FromJson<T>(Encoding.UTF8.GetString(data));
        }

        public byte[] Deserialize(T data)
        {
            return Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
        }
    }
}