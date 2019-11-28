using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.EnumFolder;
using TodoApi.Models;
using TodoApi.Models.ViewModels;
using TodoApi.Extensions;

namespace TodoApi.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private readonly TodoContext _context;
        public AdminController(TodoContext context)
        {
            _context = context;
        }
        [HttpPost("registeruser")]
        public IActionResult createadmin([FromBody]AdminViewModel adminModel)
        {

            ResponseModel responseModel = new ResponseModel();
            if (adminModel == null)
            {
                return BadRequest(responseModel.error);
            }
            try
            {
                Admin admin2 = _context.Admins.FirstOrDefault(p => p.Email == adminModel.Email);
                if (admin2 != null)
                {
                    responseModel.userExists = true;
                    return Ok(responseModel.userExists);
                }
                else
                {
                    Admin newAdmin = new Admin
                    {
                        Email = adminModel.Email,
                        Password = adminModel.Password
                    };
                    _context.Admins.Add(newAdmin);
                    _context.SaveChanges();
                    //i have to send back the users info here including his id
                    //that id will now be colleceted by my front end and used to fecth the users tasks
                    return Ok(responseModel.userExists);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody]AdminLogin admin)
        {
            try
            {
                ResponseModel response = new ResponseModel();
                Admin emailAdmin = _context.Admins.FirstOrDefault(p => p.Email == admin.Email);
                if (emailAdmin == null)
                {
                    return Ok(3);
                }
                else
                {
                    if (emailAdmin.Password == admin.Password)
                    {

                        response.isSuccessful = true;
                        return Ok(1);
                    }
                    else
                    {
                        response.isSuccessful = false;
                        return Ok(2);
                    }
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
    //[Route("api/admin")]
    ////[ApiController]
    //public class AdminController : Controller
    //{
    //    private readonly TodoContext _context;
    //    AdminControllerExtension _controllerExtension;



    //    public AdminController(TodoContext context, AdminControllerExtension controllerExtension)
    //    {
    //        _context = context;
    //        _controllerExtension = controllerExtension;
    //        if (_context.Admins.Count() == 0)
    //        {
    //            _context.Admins.Add(new Admin { Email = "Item1", Password = "123" });
    //            _context.SaveChanges();
    //        }
    //    }


    //    [HttpPost("registeruser")]
    //    public IActionResult createadmin([FromBody]AdminViewModel adminModel)
    //    {

    //            ResponseModel responseModel = new ResponseModel();
    //            if (adminModel == null)
    //            {
    //                return BadRequest(responseModel.error);
    //            }
    //        try
    //        {
    //            bool doesUserExist = _controllerExtension.DoesUserExist(adminModel.Email);
    //            if(doesUserExist)
    //            {
    //                responseModel.userExists = true;

    //                return Ok(responseModel.userExists);
    //            }
    //            else
    //            {
    //                bool userAdded = _controllerExtension.addUser(adminModel);

    //                return Ok(responseModel.userExists);
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(responseModel.error);
    //        }

    //    }

    //    [HttpPost("login")]
    //    public IActionResult LoginUser([FromBody]AdminLogin admin)
    //    {
    //        try
    //        {
    //            ResponseModel responseModel = new ResponseModel();
    //            Admin admin2 = _context.Admins.Single(p => p.Email == admin.Email && p.Password == admin.Password);
    //            if (admin == null)
    //            {

    //                return BadRequest(responseModel.error);
    //            }

    //            responseModel.isSuccessful = true;
    //            return Ok(responseModel.isSuccessful);
    //        }
    //        catch (Exception ex)
    //        {
    //            ResponseModel responseModel = new ResponseModel();
    //            responseModel.message = ex.Message;
    //            return BadRequest(responseModel.message);
    //        }
    //    }
    //}
}
