using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.BankSystem.Bases
{
    public class Response<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Meta { get; set; }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
      
        public T Data { get; set; }
        public Response() { }

        public Response(T data,string message=null) {
            Succeeded = true;
            this.Data = data;
            this.Message = message;
        }
        public Response(string message)
        {
            this.Message = message;
            Succeeded=false;
        }
        public Response(string message, bool succeeded)
        {
            Succeeded = succeeded;
            Message = message;
        }
    }
}
