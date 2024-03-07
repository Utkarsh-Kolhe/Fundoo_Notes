using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_Layer.Models
{
    public class ResponseModel<T>
    {
        public int StatusCode { get; set; }

        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
        public T? Data { get; set; }
    }
}
