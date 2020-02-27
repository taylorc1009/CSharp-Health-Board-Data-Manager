namespace BusinessLayer
{
    public class PersonFactory
    {
        //dynamic return type as we're either returning a Staff or Client object
        public dynamic createPerson(string type)
        {
            if (type == "staff")
                return new Staff();
            else if (type == "client")
                return new Client();
            else
                return null;
        }
    }
}