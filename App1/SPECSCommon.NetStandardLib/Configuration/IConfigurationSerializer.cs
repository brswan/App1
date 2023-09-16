using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPECSCommon.NetStandardLib.Configuration
{
    public interface IConfigurationSerializer
    {
        void SerializeConfigData<T>(Stream s, T data);
        T DeserializeConfigData<T>(Stream s);
    }
}
