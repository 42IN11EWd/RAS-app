using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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


        public static void Serialize<T>(T toSerialize, String fName)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream stream = new FileStream(fName, FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, toSerialize);
            }
        }

        //public static void Serialize<T>(T toSerialize, String fName)
        //{
        //    // Create a hashtable of values that will eventually be serialized.
            

        //    // To serialize the hashtable and its key/value pairs,   
        //    // you must first open a stream for writing.  
        //    // In this case, use a file stream.
        //    FileStream fs = new FileStream(fName, FileMode.Create);

        //    // Construct a BinaryFormatter and use it to serialize the data to the stream.
        //    BinaryFormatter formatter = new BinaryFormatter();
        //    try
        //    {
        //        formatter.Serialize(fs, toSerialize);
        //    }
        //    catch (SerializationException e)
        //    {
        //        Console.WriteLine("Failed to serialize. Reason: " + e.Message);
        //        throw;
        //    }
        //    finally
        //    {
        //        fs.Close();
        //    }
        //}


        public static T Deserialize<T>(String fName)
        {
            BinaryFormatter bf = new BinaryFormatter();
            T deserialized;
            using (FileStream stream = new FileStream(fName, FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();
                deserialized = (T) bin.Deserialize(stream);
            }
            return deserialized;
        }
    }
}
