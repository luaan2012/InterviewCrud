using FluentValidation.Results;

namespace InterviewCrud.Api.Identity.Validations;

public record ValidationFailed(IEnumerable<ValidationFailure> Errors)
{
    public ValidationFailed(ValidationFailure error) : this(new[] { error })
    {}
}