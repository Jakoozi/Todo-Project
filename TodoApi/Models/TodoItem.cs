using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class TodoItem
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public EnumBase.Category Category { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public EnumBase.StatusReturner StatusReturner { get; set; }
        public long UserId { get; set; }
        
    }

   
}
