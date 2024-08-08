using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidationFilter : IActionFilter
{
    public ValidationFilter(ILogger<ValidationFilter> logger)
    {
        _logger = logger;
    }
    private readonly ILogger<ValidationFilter> _logger;
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        //Executing выполняетться до, а после Executed
        //https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/controllers-and-routing/understanding-action-filters-cs

        if (!context.ModelState.IsValid)
        {
            _logger.LogError("Model is valid");
            context.Result = new BadRequestResult();
        }
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class ValidationFilterAttribute : ServiceFilterAttribute
{
    public ValidationFilterAttribute() : base(typeof(ValidationFilter))
    {

    }
}