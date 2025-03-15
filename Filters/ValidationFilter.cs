using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;


namespace UserProfileAPI.Filters
{
    public class ValidationFilter : ActionFilterAttribute
    {
        private readonly ILogger<ValidationFilter> _logger;
        public ValidationFilter(ILogger<ValidationFilter> logger)
        {
            _logger = logger;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("ValidationFilter გაეშვა მოთხოვნაზე: {Action}", context.ActionDescriptor.DisplayName);

            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(m => m.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                
                foreach (var error in errors)
                {
                    _logger.LogWarning("ModelState შეცდომა ველზე {Field}: {Errors}", error.Key, string.Join(", ", error.Value));
                }

                context.Result = new BadRequestObjectResult(new
                {
                    Message = "მონაცემები არავალიდურია",
                    Errors = errors
                });
                return;
            }


            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null)
                {
                    _logger.LogWarning("Argument null-ია");
                    context.Result = new BadRequestObjectResult(new
                    {
                        Message = "მონაცემები არავალიდურია",
                        Errors = new { General = new[] { "მოთხოვნის ტანი ცარიელია" } }
                    });
                    return;
                }

                _logger.LogDebug("Argument: {@Argument}", argument);
                var type = argument.GetType();

                var dateOfBirthProp = type.GetProperty("DateOfBirth");
                if (dateOfBirthProp != null)
                {
                    var dateOfBirthValue = dateOfBirthProp.GetValue(argument);

                    if (dateOfBirthValue is DateTime dateOfBirth)
                    {
                        var today = DateTime.Today;
                        var age = today.Year - dateOfBirth.Year;
                        if (dateOfBirth.Date > today.AddYears(-age))
                        {
                            age--;
                        }
                        if (age < 18)
                        {
                            _logger.LogWarning("მომხმარებლის ასაკი {Age} < 18", age);
                            context.Result = new BadRequestObjectResult(new
                            {
                                Message = "მონაცემები არავალიდურია",
                                Errors = new { DateOfBirth = new[] { "მომხმარებელი უნდა იყოს მინიმუმ 18 წლის" } }
                            });
                            return;
                        }
                    }
                }
            }

            _logger.LogInformation("ვალიდაცია წარმატებით დასრულდა");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}