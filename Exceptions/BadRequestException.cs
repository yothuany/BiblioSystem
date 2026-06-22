using BiblioSystem.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Exceptions
{
    public class BadRequestException : ErrorServiceException
    {
        public BadRequestException(string message)
            : base(message, controller => controller.BadRequest(new { message }))
        {
        }
    }
}