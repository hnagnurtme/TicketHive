using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Collections.Generic;
using System.Linq;

namespace TicketHive.Api.Common
{

    public static class BaseControllerHelpers
    {
        public static IActionResult HandleResult<T>(T result, int statusCode, string message = null)
        {
            if (result == null)
                return new StatusCodeResult(204);

            var response = new ApiResponse<T>
            {
                Success = true,
                Message = message,
                StatusCode = statusCode,
                Data = result
            };

            return new ObjectResult(response) { StatusCode = statusCode };
        }

        public static IActionResult HandleError(List<ApplicationError> errors)
        {
            var firstError = errors.First();
            var response = new ApiResponse<object>
            {
                Success = false,
                Message = firstError.Description,
                StatusCode = (int)firstError.StatusCode,
                Data = null,
                ErrorCode = firstError.Code
            };

            return new ObjectResult(response) { StatusCode = (int)firstError.StatusCode };
        }
    }

    // Minimal ApplicationError used by BaseControllerHelpers; adjust or remove if a shared definition exists elsewhere.
    public class ApplicationError
    {
        public string Description { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Code { get; set; }
    }
}