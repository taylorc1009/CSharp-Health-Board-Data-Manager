using System;
using System.Runtime.Serialization;

namespace BusinessLayer
{
    //This declares the class as serializable to the program
    [Serializable()]
    //Inherits from Person
    public class Staff : Person, ISerializable
    {
        //Category attribute is only present for staff members to specify their job title
        private string Category = "";
        public string category { get => Category; set => Category = value; }
        //This allows us to create a blank instance of Staff
        public Staff() { }
        public Staff Construct(string forename, string surname, string address1, string address2, string category, double longitude, double latitude)//int id, 
        {
            this.forename = forename;
            this.surname = surname;
            this.address1 = address1;
            this.address2 = address2;
            this.Category = category;
            this.longitude = longitude;
            this.latitude = latitude;
            return this;
        }
        //This method is required by the .Serialize method of the "BinaryFormatter" class (check the IOSystem class)
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //Takes the values from the class attributes and adds them to "info" with the name (in quotes) we want them serialized under
            info.AddValue("forename", forename);
            info.AddValue("surname", surname);
            info.AddValue("address1", address1);
            info.AddValue("address2", address2);
            info.AddValue("category", category);
            info.AddValue("longitude", longitude);
            info.AddValue("latitude", latitude);
        }
        //And this is required by .Deserialize to determine which attributes to store the serialized data in
        public Staff(SerializationInfo info, StreamingContext context)
        {
            //Takes the values from the serialized names and stores them back in their corresponding attributes
            this.forename = (string)info.GetValue("forename", typeof(string));
            this.surname = (string)info.GetValue("surname", typeof(string));
            this.address1 = (string)info.GetValue("address1", typeof(string));
            this.address2 = (string)info.GetValue("address2", typeof(string));
            this.category = (string)info.GetValue("category", typeof(string));
            this.longitude = (double)info.GetValue("longitude", typeof(double));
            this.latitude = (double)info.GetValue("latitude", typeof(double));
        }
    }
}