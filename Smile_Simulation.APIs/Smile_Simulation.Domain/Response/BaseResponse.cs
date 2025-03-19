using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Response
{
    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T? Data { get; set; }

        [JsonIgnore] // Ignore exceptions during serialization
        public Exception? Exception { get; set; }

        public BaseResponse(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public BaseResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public BaseResponse(bool success, string message, Exception exception)
        {
            Success = success;
            Message = message;
            Exception = exception;
        }
    }
}
