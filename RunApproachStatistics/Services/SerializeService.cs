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

        public static void Serialize<T>(T toSerialize, String fName)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream stream = new FileStream(fName, FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, toSerialize);
            }
        }
        
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