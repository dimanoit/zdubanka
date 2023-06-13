using FluentValidation;

namespace Application.Validation;

public static class ValidationHelper
{
    public static void ValidateAndThrowAsync<TRequest>(
        this IValidator<TRequest> validator,
        TRequest request)
    {
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
    }
}