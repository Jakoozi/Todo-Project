using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.EnumFolder;
using TodoApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers
{
    [Route("api/todo")]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;
        public TodoController(TodoContext context)
        {
            _context = context;
            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _context.TodoItems.ToList();
        }
        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            if (item.StartTime.Day.Equals(DateTime.Now.Day))
            {
                item.StatusReturner = EnumBase.StatusReturner.Scheduled;
            }
            else if (item.StartTime.Minute.Equals(DateTime.Now.Minute))
            {
                item.StatusReturner = EnumBase.StatusReturner.Pending;
                //if the notification is accepted it is meant to trigger ongoin then after the period trigger completed.
                //Else it should continue pending then after the period and its still on pending it will trigger skipped.
            }
            else if (item.StartTime.Equals(DateTime.Now))
            {
                item.StatusReturner = EnumBase.StatusReturner.Ongoing;
            }
            else if (item.EndTime.Equals(DateTime.Now))
            {
                item.StatusReturner = EnumBase.StatusReturner.Completed;
            }
            item.StatusReturner = EnumBase.StatusReturner.Unscheduled;
            //else if (item.StartTime.Day != DateTime.Now.Day)
            //{
               
            //}

            //this is how to cast a string to an enum
            //item.Status = (EnumBase.StatusReturner)Enum.Parse(typeof(EnumBase.StatusReturner), StatusSetter.Statussetter(), true);
            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("UpdateTodo/{id}")]
        public IActionResult Update(long id, [FromBody] TodoItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            item.StatusReturner = (EnumBase.StatusReturner)Enum.Parse(typeof(EnumBase.StatusReturner), StatusSetter.Statussetter(), true);
            todo.Name = item.Name;
            todo.StartTime = item.StartTime;
            todo.Category = item.Category;
            todo.EndTime = item.EndTime;
            todo.StatusReturner = item.StatusReturner;


            _context.TodoItems.Update(todo);
            _context.SaveChanges();
            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
            //return new NoContentResult();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if(todo == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }
        // GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
