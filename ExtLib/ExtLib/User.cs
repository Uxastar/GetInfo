using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

namespace ExtLib
{
    public interface IUser
    {
        string Name { get; set; }
        string PC { get; set; }
        string IP { get; set; }
        string Version { get; set; }
        byte[] Screen { get; set; }

        byte[] GetBinary();
        byte[] GetXML();
        byte[] GetJSON();
    }

    public interface IUser<T>
    {

        string Name { get; set; }
        string PC { get; set; }
        string IP { get; set; }
        string Version { get; set; }
        T Screen { get; set; }

        byte[] GetBinary();
        byte[] GetXML();
        byte[] GetJSON();
    }


    [Serializable, DataContract]
    public class User
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string PC { get; set; }
        [DataMember]
        public string IP { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public byte[] Screen { get; set; }


        public User()
        {

        }

        public User(string name, string pc, string ip, string version)
        {
            this.Name = name;
            this.PC = pc;
            this.IP = ip;
            this.Version = version;
        }

        public User(string name, string pc, string ip, string version, byte[] screen)
        {
            this.Name = name;
            this.PC = pc;
            this.IP = ip;
            this.Version = version;
            this.Screen = screen;
        }

        public byte[] GetBinary()
        {

            BinaryFormatter formatter = new BinaryFormatter();


            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, this);
                return stream.ToArray();
            }

        }

        public byte[] GetXML()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(User));

            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, this);
                return stream.ToArray();
            }
        }

        public byte[] GetJSON()
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(User));

            using (MemoryStream stream = new MemoryStream())
            {
                jsonFormatter.WriteObject(stream, this);
                return stream.ToArray();
            }
        }

    }
}
