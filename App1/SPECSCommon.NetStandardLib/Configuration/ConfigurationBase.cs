using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SPECSCommon.NetStandardLib.Configuration
{
    public abstract class ConfigurationBase<ConfigType> where ConfigType : class
    {
        #region SubTypes
        #endregion SubTypes

        #region Static Fields
        #endregion Static Fields

        #region Static Properties
        #endregion Static Properties

        #region Static Methods
        #endregion Static Methods

        #region Fields
        #endregion Fields

        #region Properties
        protected abstract string FileName { get; }
        protected virtual string DefaultFileName { get; }

        public ConfigType ConfigData { get; protected set; }
        #endregion Properties

        #region Events
        #endregion Events

        #region CTOR
        public ConfigurationBase(params string[] args)
        {
            if (string.IsNullOrEmpty(FileName))
            {
                throw new ArgumentNullException("FileName cannot be null.");
            }

            string fileNameDirectory = Path.GetDirectoryName(FileName);
            if (!Directory.Exists(fileNameDirectory))
            {
                Directory.CreateDirectory(fileNameDirectory);
            }

            // Start by trying to read the XML data
            ReadXMLData();
            
            // Put any command line arguments in a dictionary
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (args != null)
            {
                for (int i = 0; i < args.Length; i += 2)
                {
                    string arg = args[i];
                    if (arg == null)
                    {
                        continue;
                    }

                    dictionary.Add(args[i].ToLower(), args[i + 1]);
                }
            }
            // Now override any settings that were set in the command line or appconfig
            ProcessesConfiguration(dictionary);
        }
        #endregion CTOR

        #region Public Methods
        public void Save()
        {
            using (Stream s = new FileStream(FileName, FileMode.Create))
            {
                GetSerializer().SerializeConfigData(s, ConfigData);
            }
        }
        public void Load()
        {
            ReadXMLData();
        }
        #endregion Public Methods

        #region Protected Methods
        protected abstract void ProcessesConfiguration(Dictionary<string, string> argDictionary);
        protected virtual IConfigurationSerializer GetSerializer()
        {
            return new XMLConfigurationSerializer();
        }

        protected virtual object GetAppSettingValue(string key)
        {
            return null;
        }
        protected T GetConfigurationValue<T>(string key, T defaultValue, Dictionary<string, string> commandLineArgs)
        {
            object value = null;
            string cmdValue = string.Empty;
            if (commandLineArgs.TryGetValue($"-{key.ToLower()}", out cmdValue))
            {
                value = cmdValue;
            }
            else
            {
                value = GetAppSettingValue(key);
            }

            if (value != null)
            {
                T converted = defaultValue;
                if (typeof(T).GetTypeInfo().IsEnum)
                {
                    converted = (T)Enum.Parse(typeof(T), value.ToString());
                }
                else
                {
                    converted = (T)Convert.ChangeType(value, typeof(T));
                }
                
                return converted;
            }

            return defaultValue;
        }
        #endregion Protected Methods

        #region Private Methods
        private void ReadXMLData()
        {
            string path = GetConfigFilePath();
            if (String.IsNullOrEmpty(path))
            {
                return;
            }

            using (Stream s = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                ConfigData = GetSerializer().DeserializeConfigData<ConfigType>(s);
            }
        }
        private string GetConfigFilePath()
        {
            string path = FileName;
            if (!File.Exists(path))
            {
                if (!String.IsNullOrEmpty(DefaultFileName))
                {
                    path = DefaultFileName;
                    if (!File.Exists(path))
                    {
                        return null;
                    }
                }
                else
                {
                    return null;

                }
            }
            return path;
        }
        #endregion Private Methods
    }
}