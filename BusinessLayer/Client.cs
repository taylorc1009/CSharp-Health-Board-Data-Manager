using System;
using System.Runtime.Serialization;

namespace BusinessLayer
{
    //See the Staff class for serialization comments
    [Serializable()]
    //Inherits from Person
    public class Client : Person, ISerializable
    {
        //This allows us to create a blank instance of Client
        public Client() { }
        //Client constructor
        public Client Construct(string forename, string surname, string address1, string address2, double longitude, double latitude)
        {
            this.forename = forename;
            this.surname = surname;
            this.address1 = address1;
            this.address2 = address2;
            this.longitude = longitude;
            this.latitude = latitude;
            return this;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("forename", forename);
            info.AddValue("surname", surname);
            info.AddValue("address1", address1);
            info.AddValue("address2", address2);
            info.AddValue("longitude", longitude);
            info.AddValue("latitude", latitude);
        }
        public Client(SerializationInfo info, StreamingContext context)
        {
            this.forename = (string)info.GetValue("forename", typeof(string));
            this.surname = (string)info.GetValue("surname", typeof(string));
            this.address1 = (string)info.GetValue("address1", typeof(string));
            this.address2 = (string)info.GetValue("address2", typeof(string));
            this.longitude = (double)info.GetValue("longitude", typeof(double));
            this.latitude = (double)info.GetValue("latitude", typeof(double));
        }
    }
}