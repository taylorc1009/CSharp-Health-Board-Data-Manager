using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    //Decorator component; initialises the methods of the class we want to decorate
    public abstract class visitComponent
    {
        public abstract void validateIDs(Dictionary<int, Staff> members, Dictionary<int, Client> clients, int[] staff, int patient);
        public abstract void validateType(Dictionary<int, Staff> members, int[] staff, int type);
        public abstract void validateTime(List<Visit> visits, int[] staff, int patient, DateTime dateTime);
    }
}