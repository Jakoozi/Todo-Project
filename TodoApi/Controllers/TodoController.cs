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

        [HttpPut("{id}")]
        public IActionResult UpdateById(long id)
        {
            ResponseModel responseModel = new ResponseModel();

            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);     
            if (item == null)
            {
                responseModel.error = "Erro Occured";
                return BadRequest(responseModel);
            }
            item.StatusReturner = EnumBase.StatusReturner.Ongoing;
            _context.TodoItems.Update(item);
            _context.SaveChanges();
            responseModel.message = "Created Successfully";
            return Ok(responseModel.message);

        }
       

        [HttpPost]
        public IActionResult create([FromBody] TodoItem item)
        {
            ResponseModel responseModel = new ResponseModel();
            
            if (item == null)
            {
                responseModel.error = "Erro Occured";
                return BadRequest(responseModel.error);
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
                    return BadRequest(responseModel.error);
                }

                //this is how to cast a string to an enum
                //item.Status = (EnumBase.StatusReturner)Enum.Parse(typeof(EnumBase.StatusReturner), StatusSetter.Statussetter(), true);
                _context.TodoItems.Add(item);
                _context.SaveChanges();

                /*  returnCreatedAtRoute("GetTodo", new { id = item.Id }, item);*/
               
                responseModel.message = "Created Successfully";
                return Ok(responseModel.message);
            }
            catch(Exception ex)
            {
                responseModel.error = "Erro Occured";
                return BadRequest(responseModel.error);
            }
            
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            ResponseModel responseModel = new ResponseModel();

            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                responseModel.error = "Erro Occured";
                return BadRequest(responseModel); 
            }

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            responseModel.message = "Created Successfully";
            return Ok(responseModel);
        }
    }
}
        