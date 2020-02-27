using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    //Decorates visit type 0
    class assessmentDecorator : visitDecorator
    {
        public override void validateType(Dictionary<int, Staff> members, int[] staff, int type)
        {
            //Detects if there are enough staff members for this visit type
            if (staff.Length != 2)
                throw new Exception("\nINVALID amount of staff members for visit type: " + type + "%");
            //Detects if the staff members are of the right category
            // For assessment we need to check if both staff members are the kind we need simultaneously, otherwise we could pass through two staff
            // members of type "Social Worker" or two of type "General Practitioner".
            else if (!((members[staff[0]].category == "Social Worker" && members[staff[1]].category == "General Practitioner") || (members[staff[1]].category == "Social Worker" && members[staff[0]].category == "General Practitioner")))
                throw new Exception("\nINVALID staff member for visit type: " + type + "%");
        }
        public override void validateTime(List<Visit> visits, int[] staff, int patient, DateTime dateTime)
        {
            //Iterates over every visit stored in "visits"
            foreach(Visit visit in visits)
            {
                //Stores the time span of the current visit based on its type, for "assessment" this is between the start time + 60 minutes
                DateTime vStart = DateTime.Parse(visit.dateTime.ToString());
                DateTime vEnd = vStart.AddMinutes(60);
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