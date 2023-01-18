using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SalonCS.Model;

namespace SalonCS.Filters
{
    public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if(context.Exception is null)
            {
                return;
            }

            context.Result = new ObjectResult(new ServiceResponse<string>
            {
                Message = "Unexpected error occured",
                Success = false,
            });
        }
    }
}
