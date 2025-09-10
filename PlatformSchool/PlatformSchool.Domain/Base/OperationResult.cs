using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformSchool.Domain.Base
{
    public class OperationResult<TModel>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public TModel Data { get; set; }
        public OperationResult()
        {

        }
        public OperationResult(bool isSuccess, string message, dynamic? data = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }
        public static OperationResult<TModel> Success(string message, dynamic? data = null)
        {
            return new OperationResult<TModel>(true, message, data);
        }
        public static OperationResult<TModel> Failure(string message)
        {
            return new OperationResult<TModel>(false, message);
        }
    }
}
