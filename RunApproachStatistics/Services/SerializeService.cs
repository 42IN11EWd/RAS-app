using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace RunApproachStatistics.Services
{
    public static class SerializeService
    {

        #region Serialize
        /// <summary>
        /// Serialize an object to an XmlDocument
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toSerialize"></param>
        /// <returns>XmlDocument</returns>
        public static XmlDocument SerializeObjectToXml<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            XmlDocument xd = null;
            using (MemoryStream memStm = new MemoryStream())
            {
                xmlSerializer.Serialize(memStm, toSerialize);
                memStm.Position = 0;
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;


                using (var xtr = XmlReader.Create(memStm))
                {
                    xd = new XmlDocument();
                    xd.Load(xtr);
                }
            }
            return xd;
        }


        /// <summary>
        /// Serialize an object to a string
        /// (To be able to pass it to the next activity)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toSerialize"></param>
        /// <returns>String</returns>
        public static string SerializeObjectToString<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            String str = null;
            using (System.IO.StringWriter textWriter = new System.IO.StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                str = textWriter.ToString();
            }
            return str;
        }


        /// <summary>
        /// Deserialize an XmlDocument to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(XmlDocument xmlDoc)
        {
            String xml = xmlDoc.OuterXml;
            return DeserializeFromString<T>(xml);
        }


        /// <summary> 
        /// Deserialize a string to an object
        /// </summary>
        /// <param name="objectData"></param>
        /// <param name="type">Object type that will be returned</param>
        /// <returns>Deserialized object from type T</returns>
        public static T DeserializeFromString<T>(string objectData)
        {
            var serializer = new XmlSerializer(typeof(T));
            T result;
            using (TextReader reader = new StringReader(objectData))
            {
                result = (T)serializer.Deserialize(reader);
            }
            return result;
        }
        #endregion

    }
}
