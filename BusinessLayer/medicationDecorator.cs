using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    //Decorates visit type 1
    class medicationDecorator : visitDecorator
    {
        public override void validateType(Dictionary<int, Staff> members, int[] staff, int type)
        {
            //Detects if there are enough staff members for this visit type
            if (staff.Length != 1)
                throw new Exception("\nINVALID amount of staff members for visit type: " + type + "%");
            //Detects if the staff members are of the right category
            else if (members[staff[0]].category != "Community Nurse")
                throw new Exception("\nINVALID staff member for visit type: " + type + ", " + members[staff[0]].category + "%");
        }
        public override void validateTime(List<Visit> visits, int[] staff, int patient, DateTime dateTime)
        {
            //Iterates over every visit stored in "visits"
            foreach (Visit visit in visits)
            {
                //Stores the time span of the current visit based on its type, for "medication" this is between the start time + 20 minutes
                DateTime vStart = DateTime.Parse(visit.dateTime.ToString());
                DateTime vEnd = vStart.AddMinutes(20);
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