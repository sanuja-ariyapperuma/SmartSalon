using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SalonCS.DTO;
using SalonCS.Model;

namespace SalonCS.Filters
{
    public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {
        private ILogger<ErrorHandlingFilterAttribute> _logger { get; set; }
        public ErrorHandlingFilterAttribute(ILogger<ErrorHandlingFilterAttribute> logger)
        {
            _logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            if(context.Exception is null)
            {
                return;
            }

            _logger.LogError(context.Exception,"Unexpected Error Occured");

            context.Result = new ObjectResult(new ServiceResponse<string>
            {
                Message = "Unexpected error occured",
                Success = false,
            });
        }
    }
}
