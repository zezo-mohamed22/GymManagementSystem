using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Helper
{
    public sealed record Result(bool Suceess,string? Error = null , ResultKind Kind = ResultKind.OK)
    {
        public static Result OK()
        {
            return new Result(true);
        }
        
        public static Result Fail(string error , ResultKind Kind = ResultKind.Conflict) 
        {
            return new Result(false,error,Kind);
        }
        public static Result NotFound(string error="Not Found") 
        {
            return new Result(false, error, ResultKind.NotFound);
        }
        public static Result Validation(string error)
        {
            return new Result(false, error, ResultKind.ValidationFailed );
        }
    
    }
    public sealed record Result<T>(bool Suceess,T? Value, string? Error = null, ResultKind Kind = ResultKind.OK)
    {
        public static Result<T> OK(T value)
        {
            return new Result<T>(true,value);
        }

        public static Result<T> Fail(string error, ResultKind Kind = ResultKind.Conflict)
        {
            return new Result<T>(false,default, error, Kind);
        }
        public static Result<T> NotFound(string error = "Not Found")
        {
            return new Result<T>(false,default, error, ResultKind.NotFound);
        }


    }
    public enum ResultKind
    {
        OK,
        NotFound,
        Conflict,
        ValidationFailed,
        Forbidden
    }
}
