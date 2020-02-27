using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    //This is the class that is used to decorate the component, this is where we will override the methods specified with the decorators
    public abstract class visitDecorator : visitComponent
    {
        //Component determines which decorator we will use to override these methods, it is specified by the "component" parameter of "setComponent"
        protected visitComponent component;
        public void setComponent(visitComponent component)
        {
            this.component = component;
        }
        public override void validateType(Dictionary<int, Staff> members, int[] staff, int type)
        {
            //We call the methods of the decorator specified as the component, as long as the component has been set
            if (component != null)
                component.validateType(members, staff, type);
        }
        public override void validateTime(List<Visit> visits, int[] staff, int patient, DateTime dateTime)
        {
            if (component != null)
                component.validateTime(visits, staff, patient, dateTime);
        }
        //validateIDs is constant, we do not change the way we validate the IDs depending on the visit type
        public override void validateIDs(Dictionary<int, Staff> members, Dictionary<int, Client> clients, int[] staff, int patient)
        {
            //Validates whether any of the keys of the Staff and Client Dictionaries = the IDs we're trying to add, if not then the ID doesn't exist
            for (int i = 0; i < members.Count; i++)
                foreach (int id in staff)
                    if (!members.ContainsKey(id))
                        throw new Exception("\nINVALID staff ID: " + id + "%");
            if (!clients.ContainsKey(patient))
                throw new Exception("\nINVALID client ID: " + patient + "%");
        }
    }
}