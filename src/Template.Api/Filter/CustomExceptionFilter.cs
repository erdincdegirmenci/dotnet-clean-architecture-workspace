using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Template.Api.Common;
using Template.Application.Managers;
using Template.Infrastructure.Logging;

namespace Template.Api.Middlewares
{
    public class CustomExceptionFilter : ExceptionFilterAttribute, IExceptionFilter
    {
        private readonly ILogManager<CustomExceptionFilter> _logManager;
        private readonly ITransactionContextManager _transactionContextManager;
        public CustomExceptionFilter(ILogManager<CustomExceptionFilter> logManager, ITransactionContextManager transactionContextManager)
        {
            _logManager = logManager;
            _transactionContextManager = transactionContextManager;

        }
        public override void OnException(ExceptionContext context)
        {

            ControllerActionDescriptor controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            int statusCode;

            /*if (context.Exception is HttpResponseException)
            {
                // Internal Server  = 500, Unauthorized = 401
                statusCode = context.HttpContext.Response.StatusCode;
            }*/

            if (context.Exception is ArgumentNullException) statusCode = (int)HttpStatusCode.BadRequest;
            else if (context.Exception is ArgumentException) statusCode = (int)HttpStatusCode.BadRequest;
            else if (context.Exception is UnauthorizedAccessException) statusCode = (int)HttpStatusCode.Unauthorized;
            else if (context.Exception is UnauthorizedAccessException) statusCode = (int)HttpStatusCode.Forbidden;
            else // özel hatalarda
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
            }

            string methodDescriptor = string.Format("{0}.{1}.{2}", controllerActionDescriptor.MethodInfo.ReflectedType.Namespace,
                controllerActionDescriptor.MethodInfo.ReflectedType.Name,
                controllerActionDescriptor.MethodInfo.Name);

            ApiResponse<object> responseModel = new ApiResponse<object>
            {
                Data = null,
                Success = false,
                ErrorCode = Convert.ToInt32(statusCode).ToString(),
                Message = context.Exception.Message.ToString(),
                TransactionCode = _transactionContextManager.GetTransaction()
            };

            ObjectResult result = new ObjectResult(responseModel)
            {
                StatusCode = statusCode
            };

            context.Result = result;

            _logManager.Error($"Exception Message : {context.Exception.Message} || methodDescriptor : {methodDescriptor} {context.Exception.StackTrace}");

        }
    }
}
