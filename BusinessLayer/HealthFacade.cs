/* Object Oriented Software Development Coursework 2: Edinburgh Health Board
 * 40398643
 * Taylor Courtney
 */

using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    public static class visitTypes
    {
        public const int assessment = 0;
        public const int medication = 1;
        public const int bath = 2;
        public const int meal = 3;
    }
    public class HealthFacade
    {
        //Initialises the HealthFacade attributes we need to store the data we need
        private Dictionary<int, Staff> members = new Dictionary<int, Staff>();
        private Dictionary<int, Client> clients = new Dictionary<int, Client>();
        // Visits are stored in a list as the only unique attribute is the date and this is not an ideal key (we don't really need one for visits
        // anyway)
        private List<Visit> visits = new List<Visit>();
        //Person factory to create a new Staff/Client object when we need to later
        private PersonFactory factory = new PersonFactory();
        public Boolean addStaff(int id, string firstName, string surname, string address1, string address2, string category, double baseLocLat, double baseLocLon)
        {
            //try/catch to interrupt the process of adding a staff member whenever it fails
            try
            {
                //Detects whether the current staff ID already exists
                if (members.ContainsKey(id))
                    throw new Exception("Staff member ALREADY EXISTS: ID " + id);
                //Factory creates a new object of type Staff
                Staff member = factory.createPerson("staff");
                member.Construct(firstName, surname, address1, address2, category, baseLocLon, baseLocLat);
                members.Add(id, member);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean addClient(int id, string firstName, string surname, string address1, string address2, double locLat, double locLon)
        {
            try
            {
                if (clients.ContainsKey(id))
                    throw new Exception("Client ALREADY EXISTS: ID " + id);
                //Factory creates a new object of type Client
                Client client = factory.createPerson("client");
                client.Construct(firstName, surname, address1, address2, locLon, locLat);
                clients.Add(id, client);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean addVisit(int[] staff, int patient, int type, string dateTime)
        {
            try
            {
                //Constructs a new object of type Visit, converts the dateTime to an actual DateTime data type
                Visit visit = new Visit(staff, patient, type, Convert.ToDateTime(dateTime));
                //Methods used validate the visit we're attempting to add (these are decorated depending on the visit type, look at the Visit class)
                visit.validateIDs(members, clients, visit.staffID, visit.clientID);
                visit.validateType(members, visit.staffID, visit.type);
                visit.validateTime(visits, visit.staffID, visit.clientID, visit.dateTime);
                visits.Add(visit);
                return true;
            }
            catch (Exception e)
            {
                //The exception string has been tokenized using the char '%' to remove any unnecessary text and make it a bit more readable
                string eStr = e.ToString();
                string[] eCut = eStr.Split('%');
                throw new Exception(eCut[0]);
            }
        }
        public String getStaffList()
        {
            String result = "";
            //Iterates through the Staff objects in the members dictionary, adding them to an output string ("result")
            foreach (KeyValuePair<int, Staff> m in members)
                //First, we add the key (staff member ID), then each attribute of the object identified by this key
                result += "\n" + m.Key.ToString() + ", " + m.Value.forename + " " + m.Value.surname + ", " + m.Value.address1 + ", " + m.Value.address2 + ", " + m.Value.category + ", " + m.Value.latitude.ToString() + ", " + m.Value.longitude.ToString();
            return result;
        }
        public String getClientList()
        {
            String result = "";
            foreach (KeyValuePair<int, Client> c in clients)
                result += "\n" + c.Key.ToString() + ", " + c.Value.forename + " " + c.Value.surname + ", " + c.Value.address1 + ", " + c.Value.address2 + ", " + c.Value.latitude.ToString() + ", " + c.Value.longitude.ToString();
            return result;
        }
        public String getVisitList()
        {
            String result = "";
            foreach (Visit v in visits)
                //string.Join converts the int array "staffID" of the "Visit" type to a string, each value is separated by ", "
                result += "\nStaff: " + string.Join(", ", v.staffID) + ", Client: " + v.clientID.ToString() + ", " + visitTypeToText(v.type) + ", " + v.dateTime;
                //Scroll down for the "visitTypeToText" method
            return result;
        }
        public void clear()
        {
            //Simply clears the attributes of their stored values
            members.Clear();
            clients.Clear();
            visits.Clear();
        }
        public Boolean load()
        {
            //Creates an IOSystem object to load the external data into their corresponding attributes
            IOSystem input = new IOSystem();
            //Runs the load methods of IOSystem
            members = input.loadStaff();
            clients = input.loadClients();
            visits = input.loadVisits();
            /* SQL save and load
             * members = input.load("staff", members, null, null);
             * clients = input.load("clients", null, clients, null);
             * visits = input.load("visits", members, clients, visits);
             */
            return true;
        }
        public Boolean save()
        {
            IOSystem output = new IOSystem();
            //Outputs (saves) the data in the 3 data attributes to external files
            output.save(members, clients, visits);
            return true;
        }
        // A private method which simply converts the visit type from a number to text (this is only used in "getVisitList" when displaying the visits,
        // it does not alter the way the visit types are stored)
        private string visitTypeToText(int type)
        {
            switch (type)
            {
                case 0:
                    return "Assessment";
                case 1:
                    return "Medication";
                case 2:
                    return "Bath";
                case 3:
                    return "Meal";
                default:
                    return null;
            }
        }
    }
}