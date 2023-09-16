using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SPECSCommon.NetStandardLib.Configuration
{
    public class XMLConfigurationSerializer : IConfigurationSerializer
    {
        public T DeserializeConfigData<T>(Stream s)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(s);
        }

        public void SerializeConfigData<T>(Stream s, T data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(s, data);
        }
    }
}
