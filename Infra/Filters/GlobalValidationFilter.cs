using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Infra.Filters;

public class GlobalValidationFilter : IExceptionFilter, IActionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not ValidationException validationException) return;
        var errors = new ModelStateDictionary();
        errors.AddModelError(validationException.ValidationResult.MemberNames.First(), validationException.Message);
        var problemDetails = GetProblemDetails(errors);

        context.Result = new BadRequestObjectResult(problemDetails);
        context.ExceptionHandled = true;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return;
        var problemDetails = GetProblemDetails(context.ModelState);
        context.Result = new BadRequestObjectResult(problemDetails);
    }

    private static ValidationProblemDetails GetProblemDetails(ModelStateDictionary errors)
    {
        var problemDetails = new ValidationProblemDetails(errors)
        {
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest
        };
        return problemDetails;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}