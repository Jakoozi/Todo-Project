using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class ResponseModel
    {
        public string message = "Successful";
        public string error = "Error Occured";
        public string userIsNull = "No User";
        public bool isSuccessful { get; set; } = false;
        public bool userExists { get; set; } = false;

    }
}
