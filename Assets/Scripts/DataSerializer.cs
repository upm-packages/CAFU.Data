using System.Text;
using UnityEngine;

namespace CAFU.Data.DataSerializer
{
    public class JsonUtility<T> : IDataSerializer<T>
    {
        public static JsonUtility<T> Default { get; } = new JsonUtility<T>();

        public byte[] Serialize(T data)
        {
            return Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
        }

        public T Deserialize(byte[] data)
        {
            return JsonUtility.FromJson<T>(Encoding.UTF8.GetString(data));
        }
    }
}