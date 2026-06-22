using BiblioSystem.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Exceptions
{
    public class NotFoundException : ErrorServiceException
    {
        public NotFoundException(string message)
            : base(message, controller => controller.NotFound(new { message }))
        {
        }
    }
}