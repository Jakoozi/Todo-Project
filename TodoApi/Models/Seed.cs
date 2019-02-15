using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public static class Seed
    {

        //public string datetime1 = "10/2/2019 10:30:00";
        //public string datetime2 = "10/30/2019 12:30:00";
        public static DateTime datetimeA = new DateTime(2019, 2, 10, 10, 30 , 00);
        public static DateTime datetimeB = new DateTime(2019, 2, 10, 12, 30, 00);

        public static DateTime datetime1 = datetimeA.AddDays(1);
        public static DateTime datetime2 = datetimeB.AddDays(2);

        public static void initialize(IServiceProvider serviceProvider)
        {
           

            using (var context = new TodoContext(
                 serviceProvider.GetRequiredService<DbContextOptions<TodoContext>>()))
            {
                //Chgecks to see if there is any item in the Database
                if (context.TodoItems.Any())
                    return; //DB has been seeded
                {

                }
                context.TodoItems.AddRange(
                    new TodoItem
                    {
                        Name = " Do My Assignment",
                        Category = EnumBase.Category.School,
                        StartTime = datetime1,
                        EndTime = datetime2,
                        StatusReturner = (EnumBase.StatusReturner)Enum.Parse(typeof(EnumBase.StatusReturner), EnumFolder.StatusSetter.Statussetter(), true),
                    }
                    );
                context.SaveChanges();
            }
        }
    }
}
