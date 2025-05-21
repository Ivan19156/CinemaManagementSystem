using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.WebAPI.Helpers
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        
        public static ServiceResult<T> Success(T data)
        {
            return new ServiceResult<T> { IsSuccess = true, Data = data, Message = string.Empty };
        }

        
        public static ServiceResult<T> Failure(string message)
        {
            return new ServiceResult<T> { IsSuccess = false, Data = default, Message = message };
        }
    }

}
