using System;
using System.Runtime.Serialization;

namespace BusinessLayer
{
    //See the staff class for serialization comments
    [Serializable()]
    public class Visit : visitDecorator, ISerializable
    {
        //Initialises attributes
        private int[] StaffID = new int[0];
        private int ClientID = 0;
        private int Type = 0;
        private DateTime DateTime;
        //Setters and getters
        public int[] staffID { get => StaffID; set => StaffID = value; }
        public int clientID { get => ClientID; set => ClientID = value; }
        public int type { get => Type; set => Type = value; }
        public DateTime dateTime { get => DateTime; set => DateTime = value; }
        //Allows us to create a blank instance of Visit
        public Visit() { }
        //Visit constructor
        public Visit(int[] staffID, int clientID, int type, DateTime dateTime)
        {
            this.ClientID = clientID;
            this.StaffID = staffID;
            this.Type = type;
            this.DateTime = dateTime;
            this.decorate();
        }
        //Called by the constructor as every instance of the visit class must be decorated
        private void decorate()
        {
            //Decorates according to the visit type
            switch (this.type)
            {
                case 0: //0 = assessment
                    this.setComponent(new assessmentDecorator());
                    break;
                case 1: //1 = medication
                    this.setComponent(new medicationDecorator());
                    break;
                case 2: //2 = bath
                    this.setComponent(new bathDecorator());
                    break;
                case 3: //3 = meal
                    this.setComponent(new mealDecorator());
                    break;
                default: //if type isn't any of these, declare visit type invalid
                    throw new Exception("\nINVALID visit type: " + this.type + "%");
            }
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("StaffID", StaffID);
            info.AddValue("ClientID", ClientID);
            info.AddValue("Type", Type);
            info.AddValue("dateTime", dateTime);
        }
        public Visit(SerializationInfo info, StreamingContext context)
        {
            this.staffID = (int[])info.GetValue("StaffID", typeof(int[]));
            this.clientID = (int)info.GetValue("ClientID", typeof(int));
            this.type = (int)info.GetValue("Type", typeof(int));
            this.dateTime = (DateTime)info.GetValue("dateTime", typeof(DateTime));
        }
    }
}