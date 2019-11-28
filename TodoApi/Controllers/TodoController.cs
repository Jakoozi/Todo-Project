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


        [HttpGet("{id}")]
        public IEnumerable<TodoItem> GetByStatus(long id)
        {

            var item = _context.TodoItems.Where(t => (int)t.StatusReturner == id).ToList();
            if (item == null)
            {
                return null; //NotFound();
            }
            return item;
        }

        [HttpPut("accepttask/{id}")]
        public IActionResult AcceptTaskById(long id)
        {
            ResponseModel responseModel = new ResponseModel();

            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);     
            if (item == null)
            {
                var error = responseModel.error;
                return NotFound(error);
            }
            item.StatusReturner = EnumBase.StatusReturner.Ongoing;
            _context.TodoItems.Update(item);
            _context.SaveChanges();
            return Ok(_context.TodoItems.ToList());

        }
        [HttpPut("declinetask/{id}")]
        public IActionResult DeclineTaskById(long id)
        {
            ResponseModel responseModel = new ResponseModel();

            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                var error = responseModel.error;
                return NotFound(error);
            }
            item.StatusReturner = EnumBase.StatusReturner.Skipped;
            _context.TodoItems.Update(item);
            _context.SaveChanges();
            return Ok(_context.TodoItems.ToList());

        }


        [HttpPost]
        public IActionResult create([FromBody] TodoItem item)
        {
            ResponseModel responseModel = new ResponseModel();
            
            if (item == null)
            {
                return BadRequest(1);
            }

            try
            { 
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

                else if (item.StartTime.Day > DateTime.Now.Day)
                {
                    item.StatusReturner = EnumBase.StatusReturner.Unscheduled;
                }
                else if (item.StartTime.Day < DateTime.Now.Day)
                {
                    return BadRequest(2);
                }

                //this is how to cast a string to an enum
                //item.Status = (EnumBase.StatusReturner)Enum.Parse(typeof(EnumBase.StatusReturner), StatusSetter.Statussetter(), true);
                _context.TodoItems.Add(item);
                _context.SaveChanges();

                /*  returnCreatedAtRoute("GetTodo", new { id = item.Id }, item);*/

                var message = responseModel.message;
                return Ok(20);
            }
            catch(Exception ex)
            {
                var error = responseModel.error;
                return BadRequest(ex);
            }
            
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            ResponseModel responseModel = new ResponseModel();
            var error = responseModel.error;

            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return BadRequest(error); 
            }

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return Ok(1);
        }
    }
}
        