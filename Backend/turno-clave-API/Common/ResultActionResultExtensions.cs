using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace turno_clave_API.Common
{
    public static class ResultActionResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller, Func<T, IActionResult> onSuccess)
        {
            if (result.IsSuccess)
                return onSuccess(result.Value!);

            var error = result.Error ?? "An error occurred";

            if (error.Contains("not found", StringComparison.OrdinalIgnoreCase))
            {
                return controller.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Not Found",
                    detail: error,
                    type: "/errors/NotFound",
                    instance: controller.HttpContext.Request.Path
                );
            }

            if (error.Contains("taken", StringComparison.OrdinalIgnoreCase))
            {
                return controller.Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Conflict",
                    detail: error,
                    type: "/errors/Conflict",
                    instance: controller.HttpContext.Request.Path
                );
            }

            return controller.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request",
                detail: error,
                type: "/errors/BadRequest",
                instance: controller.HttpContext.Request.Path
            );
        }
    }
}
