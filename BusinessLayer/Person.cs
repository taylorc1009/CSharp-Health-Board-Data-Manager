namespace BusinessLayer
{
    // I have made a person class as both Staff and Client need the following attributes and methods, though it's abstract as we don't want a direct
    // instance of it
    public abstract class Person
    {
        //Initialises attributes
        private string firstName = "";
        private string lastName = "";
        private string Address1 = "";
        private string Address2 = "";
        private double locLon = 0;
        private double locLat = 0;
        //Setters and getters
        public string forename { get => firstName; set => firstName = value; }
        public string surname { get => lastName; set => lastName = value; }
        public string address1 { get => Address1; set => Address1 = value; }
        public string address2 { get => Address2; set => Address2 = value; }
        public double longitude { get => locLon; set => locLon = value; }
        public double latitude { get => locLat; set => locLat = value; }
    }
}