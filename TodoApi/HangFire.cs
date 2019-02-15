using Hangfire;
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
            RecurringJob.AddOrUpdate("Schedule Daily Task", () => ScheduleDailyTask(), Cron.Daily);
            RecurringJob.AddOrUpdate("Pending Daily Task", () => PendingDailyTask(), Cron.MinuteInterval(1));
            RecurringJob.AddOrUpdate("Ongoing Daily Task", () => OngoingDailyTask(), Cron.MinuteInterval(1));
            RecurringJob.AddOrUpdate("Completed Daily Task", () => CompletedDailyTask(), Cron.MinuteInterval(1));
            RecurringJob.AddOrUpdate("Skipped Daily Task", () => SkippedDailyTask(), Cron.MinuteInterval(1));

        }

        public void SkippedDailyTask()
        {
            //this method will later be implemented.;
        }

        public void CompletedDailyTask()
        {
            TimeSpan startTime = new TimeSpan(hours: 0, minutes: 0, seconds: 0); //will be changed to (0,0,0)
            TimeSpan endTime = new TimeSpan(hours: 13, minutes: 0, seconds: 0);
            TimeSpan currentTime = DateTime.Now.TimeOfDay;


            if (startTime >= currentTime && currentTime <= endTime)
            {
                var ongoingtasks = GetOngoingTask();
                foreach (var item in ongoingtasks)
                {
                    if (item.EndTime.Minute.Equals(DateTime.Now.Minute))
                    {
                        item.StatusReturner = StatusReturner.Completed;
                    }
                }
                context.SaveChanges();
            }
        }

        public void OngoingDailyTask()
        {
            TimeSpan startTime = new TimeSpan(hours: 0, minutes: 0, seconds: 0); //will be changed to (0,0,0)
            TimeSpan endTime = new TimeSpan(hours: 13, minutes: 0, seconds: 0);
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            if (startTime >= currentTime && currentTime <= endTime)
            {
                var pendingtasks = GetPendingTask();
                //this depends on if the user accepts the pending request.
                //it eigther goes to ongoing or remains pending.
                //after the EndTime Value, if it was accepted(Ongoing) it moves to completed,
                //if it was notacceptd(pending), it moves to Skipped.
                if (pendingtasks != null)
                {

                    foreach (var item in pendingtasks)
                    {
                        //if the task is accepted in my client side. That shuold be what will be here.
                        if (item.StartTime.Minute.Equals(DateTime.Now.Minute))
                        {
                            item.StatusReturner = StatusReturner.Ongoing;
                        }
                    }
                    context.SaveChanges();
                }
            }
        }

        public void PendingDailyTask()
        {
            TimeSpan startTime = new TimeSpan(hours: 2, minutes: 0, seconds: 0); //will be changed to (0,0,0)
            TimeSpan endTime = new TimeSpan(hours: 13, minutes: 0, seconds: 0);
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            
            if(startTime <= currentTime && currentTime <= endTime)
            { 
            var scheduledtasks = GetScheduledTasks();
                if (scheduledtasks != null)
                {
                    foreach (var item in scheduledtasks)
                    {
                        if (item.StartTime.Minute.Equals(DateTime.Now.Minute))
                        {
                            item.StatusReturner = StatusReturner.Pending;
                        }
                    }
                    context.SaveChanges();
                }
            }
        }

        public void ScheduleDailyTask()
        {
            StatusChecker statusmodel = new StatusChecker();
            TimeSpan startTime = new TimeSpan(hours: 0, minutes: 0, seconds: 0); //will be changed to (0,0,0)
            TimeSpan endTime = new TimeSpan(hours: 13, minutes: 0, seconds: 0);
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            if (startTime >= currentTime && currentTime <= endTime)
            {

                var hasTaskBeenScheduled = HasTaskBeenScheduled();
                if (!hasTaskBeenScheduled)// could also be used hasTaskBeenScheduled == false.
                {
                    var unscheduledTask = GetUnscheduledTasks();
                    foreach (var item in unscheduledTask)
                    {
                        item.StatusReturner = StatusReturner.Scheduled;
                        //will need to kmnow if the statuschecker table adds all records(items) that changed to scheduled. 
                    }
                    statusmodel.DateChecked = DateTime.Now;

                    context.StatusCheckers.Add(statusmodel);

                }

                context.SaveChanges();
            }
        }

        public  bool HasTaskBeenScheduled()
        {
            StatusChecker statusmodel = new StatusChecker();
            //checks our statuschecker to see if there is an item that contains todays date and returns true, if not it returns false.
            var item = (from u in context.StatusCheckers
                        where u.DateChecked.Date == DateTime.Now.Date
                        select u);
            if(item!=null)
            {
                return true;
            }
            return false;
        }

        public IEnumerable<TodoItem> GetUnscheduledTasks()
        {
            return context.TodoItems.Where(x => x.StatusReturner == StatusReturner.Unscheduled);
        }
        public IEnumerable<TodoItem> GetScheduledTasks()
        {
            return context.TodoItems.Where(x => x.StatusReturner == StatusReturner.Scheduled);
        }
        public IEnumerable<TodoItem> GetPendingTask()
        {
            return context.TodoItems.Where(x => x.StatusReturner == StatusReturner.Pending);
        }
        public IEnumerable<TodoItem> GetOngoingTask()
        {
            return context.TodoItems.Where(x => x.StatusReturner == StatusReturner.Ongoing);
        }
    }



    public class HangFire
    {

    }



}
