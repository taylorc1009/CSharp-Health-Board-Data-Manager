using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    //Decorates visit type 2
    class bathDecorator : visitDecorator
    {
        public override void validateType(Dictionary<int, Staff> members, int[] staff, int type)
        {
            foreach (int id in staff)
            {
                Staff m = new Staff();
                m = members[id];
                //Detects if there are enough staff members for this visit type
                if (staff.Length != 2)
                    throw new Exception("\nINVALID amount of staff members for visit type: " + type + "%");
                //Detects if the staff members are of the right category
                else if (m.category != "Care Worker")
                    throw new Exception("\nINVALID staff member for visit type: " + type + ", " + m.category + "%");
            }
        }
        public override void validateTime(List<Visit> visits, int[] staff, int patient, DateTime dateTime)
        {
            //Iterates over every visit stored in "visits"
            foreach (Visit visit in visits)
            {
                //Stores the time span of the current visit based on its type, for "bath" this is between the start time + 30 minutes
                DateTime vStart = DateTime.Parse(visit.dateTime.ToString());
                DateTime vEnd = vStart.AddMinutes(30);
                //Checks if the staff member we're trying to add to this visit is present at another during this time
                foreach (int id in visit.staffID)
                    foreach (int findID in staff)
                        if (id == findID)
                            //Detects if the visit time we're trying to add starts within the duration of another
                            if (dateTime >= vStart && dateTime <= vEnd)
                                throw new Exception("\nINVALID schedule, at least 1 staff member occupied at this time%");
                //Checks if the client is already due another visit at the time we're trying to add
                if (visit.clientID == patient)
                    if (dateTime >= vStart && dateTime <= vEnd)
                        throw new Exception("\nINVALID schedule, client occupied at this time%");
            }
        }
    }
}