using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.EnumFolder
{
    public static class StatusSetter
    {
      
        public static string Statussetter()
        {
            if(Seed.datetime1.Day.Equals(DateTime.Now.Day))
            {
                return EnumBase.StatusReturner.Scheduled.ToString();
            }
            else if(Seed.datetime1.Minute.Equals(DateTime.Now.Minute))
            {
                return EnumBase.StatusReturner.Pending.ToString();
                //if the notification is accepted it is meant to trigger ongoin then after the period trigger completed.
                //Else it should continue pending then after the period and its still on pending it will trigger skipped.
            }
            else if(Seed.datetime1.Equals(DateTime.Now))
            {
                return EnumBase.StatusReturner.Ongoing.ToString(); 
            }
            else if(Seed.datetime2.Equals(DateTime.Now))
            {
                return EnumBase.StatusReturner.Completed.ToString(); 
            }
            return EnumBase.StatusReturner.Unscheduled.ToString();
        }
           
        

    }
    
}
