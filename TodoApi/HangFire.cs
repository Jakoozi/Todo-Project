using Hangfire;
using Hangfire.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;
using static TodoApi.EnumBase;

namespace TodoApi
{

    public class BackgroundTask
    {

        TodoContext context { get; }

        public BackgroundTask(TodoContext todoContext)
        {
            context = todoContext;
            RecurringJob.AddOrUpdate("Schedule Daily Task", () => ScheduleDailyTask(), Cron.DayInterval(1));
            RecurringJob.AddOrUpdate("Pending Daily Task", () => PendingDailyTask(), Cron.MinuteInterval(1));
            //RecurringJob.AddOrUpdate("Ongoing Daily Task", () => OngoingDailyTask(), Cron.MinuteInterval(1));
            RecurringJob.AddOrUpdate("Completed Daily Task", () => CompletedDailyTask(), Cron.MinuteInterval(1));
            RecurringJob.AddOrUpdate("Skipped Daily Task", () => SkippedDailyTask(), Cron.MinuteInterval(1));

        }

        public void SkippedDailyTask()
        {
            var pendingtasks = GetPendingTasks();
            if (pendingtasks.Count() == 0)
            {
                Console.WriteLine("Empty List");
            }
            else
            {
                foreach (var item in pendingtasks)
                {
                    if (item.EndTime.Hour <= DateTime.Now.Hour)
                    {
                        if (item.EndTime.Minute <= DateTime.Now.Minute)
                        {
                            item.StatusReturner = StatusReturner.Skipped;
                        }
                        else
                        {
                            Console.WriteLine("Empty List");
                        }
                        context.SaveChanges();
                    }

                }

            }
        }

        //i am testing this one out
        public void CompletedDailyTask()
        {
            var completedtasks = GetOngoingTasks();
            if (completedtasks.Count() == 0)
            {
                Console.WriteLine("Empty List");
            }
            else
            {
                foreach (var item in completedtasks)
                {
                    if (item.EndTime.Hour <= DateTime.Now.Hour)
                    {
                        if (item.EndTime.Minute <= DateTime.Now.Minute)
                        {
                            item.StatusReturner = StatusReturner.Completed;
                        }
                        else
                        {
                            Console.WriteLine("Empty List");
                        }
                        context.SaveChanges();
                    }

                }

            }


        }

        public void PendingDailyTask()
        {

            var scheduledtasks = GetScheduledTasks();
            if (scheduledtasks.Count() == 0)
            {
                Console.WriteLine("Empty List");
            }
            else
            {
                foreach (var item in scheduledtasks)
                {

                    if (item.StartTime.Hour <= DateTime.Now.Hour)
                    {
                        if (item.StartTime.Minute <= DateTime.Now.Minute)
                        {
                            item.StatusReturner = StatusReturner.Pending;
                        }
                        else
                        {
                            Console.WriteLine("Empty List");
                        }
                        context.SaveChanges();
                    }
                }

            }
        }

        public void ScheduleDailyTask()
        {

            var unscheduledTask = GetUnscheduledTasks();
            if (unscheduledTask.Count() == 0)
            {
                Console.WriteLine("Empty List");
            }
            else
            {
                foreach (var item in unscheduledTask)
                {
                    if (item.StartTime.Day <= DateTime.Now.Day)
                    {
                        item.StatusReturner = StatusReturner.Scheduled;
                    }
                    else
                    {
                        Console.WriteLine("Empty List");
                    }
                    context.SaveChanges();
                }
            }
        }



        public IEnumerable<TodoItem> GetUnscheduledTasks()
        {
            var unscheduleditems = context.TodoItems.Where(x => x.StatusReturner == StatusReturner.Unscheduled).ToList();
            return unscheduleditems;
        }
        public IEnumerable<TodoItem> GetScheduledTasks()
        {
            var scheduledItem = context.TodoItems.Where(x => x.StatusReturner == StatusReturner.Scheduled).ToList();
            return scheduledItem;


        }
        public IEnumerable<TodoItem> GetPendingTasks()
        {
            var pendingtasks = context.TodoItems.Where(x => x.StatusReturner == StatusReturner.Pending).ToList();
            return pendingtasks;
        }
        public IEnumerable<TodoItem> GetOngoingTasks()
        {
            var ongoingItem = context.TodoItems.Where(x => x.StatusReturner == StatusReturner.Ongoing).ToList();
            return ongoingItem;
        }
    }



    // public class HangFire
    // {
    //
    // }



}



//public void OngoingDailyTask()
//{
//    TimeSpan startTime = new TimeSpan(hours: 0, minutes: 0, seconds: 0); //will be changed to (0,0,0)
//    TimeSpan endTime = new TimeSpan(hours: 13, minutes: 0, seconds: 0);
//    TimeSpan currentTime = DateTime.Now.TimeOfDay;

//    if (startTime >= currentTime && currentTime <= endTime)
//    {
//        var pendingtasks = GetPendingTask();
//        //this depends on if the user accepts the pending request.
//        //it eigther goes to ongoing or remains pending.
//        //after the EndTime Value, if it was accepted(Ongoing) it moves to completed,
//        //if it was notacceptd(pending), it moves to Skipped.
//        if (pendingtasks != null)
//        {

//            foreach (var item in pendingtasks)
//            {
//                //if the task is accepted in my client side. That shuold be what will be here.
//                if (item.StartTime.Minute.Equals(DateTime.Now.Minute))
//                {
//                    item.StatusReturner = StatusReturner.Ongoing;
//                }
//            }
//            context.SaveChanges();
//        }
//    }
//}
