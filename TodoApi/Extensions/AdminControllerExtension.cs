using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Models.ViewModels;

namespace TodoApi.Extensions
{
    public class AdminControllerExtension
    {
        Admin _admin = new Admin();
        TodoContext _context;
        public AdminControllerExtension(TodoContext context)
        {
          
            _context = context;
        }

        public  bool DoesUserExist(string email )
        {
            
            Admin admin2 = _context.Admins.Single(p => p.Email == _admin.Email);
            if(admin2 != null)
            { return true; }
            else
            {
                return false;
            }
            
        }
        public bool addUser(AdminViewModel adminview)
        {
            if(adminview.Email != null && adminview.Password != null)
            {
                Admin newAdmin = new Admin
                {
                    Email = adminview.Email,
                    Password = adminview.Password
                };
                _context.Admins.Add(_admin);
                _context.SaveChanges();
                return true;
            }
            else { return false; }

        }
    }
}
