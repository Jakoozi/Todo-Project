using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi
{
     public class EnumBase
    {
        public enum StatusReturner
        {
            Scheduled = 1,
            Unscheduled = 2,
            Pending = 3,
            Ongoing = 4,
            Completed = 5,
            Skipped = 6
            
        }

        public enum Category
        {
            School = 1,
            Chilling = 2,
            Chores = 3
        }
    }
}
