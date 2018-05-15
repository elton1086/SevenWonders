using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SevenWonder.Helper
{
    public static class Serializer
    {
        public static string Serialize<T>(T objectClass)
        {
            string result = string.Empty;
            using (var stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, objectClass);
                stream.Seek(0, SeekOrigin.Begin);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            }
            return result;
        }

        public static T Deserialize<T>(string xml)
        {
            object result;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                result = serializer.Deserialize(stream);
            }
            return (T)result;
        }
    }
}
