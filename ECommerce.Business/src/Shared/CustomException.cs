using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceBusiness;
public class CustomException : Exception
{
    public int StatusCode { get; set; }

    public CustomException(int statusCode, string msg) : base(msg)
    {
        StatusCode = statusCode;
    }
    public static CustomException NotFoundException(string msg = "Not found")
    {
        return new CustomException(404, msg);
    }
    public static CustomException DuplicateEmailException(string msg = "Email already exist")
    {
        return new CustomException(409, msg);
    }
    public static CustomException ProductNotAvailableException(string msg = "Product's inventory is not available")
    {
        return new CustomException(503, msg);
    }
    public static CustomException InvalidLoginCredentialsException(string msg = "Invalid Login Credentials")
    {
        return new CustomException(401, msg);
    }
    public static CustomException UnknownErrorException(string msg = "Failed to retrieve information")
    {
         return new CustomException(500, msg);
    }
    public static CustomException BadRequestException(string msg = "Empty request data body or parameter.")
    {
         return new CustomException(400, msg);
    }
}
   

