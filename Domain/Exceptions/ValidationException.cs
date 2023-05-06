using System.Runtime.Serialization;
using FluentValidation.Results;

namespace Domain.Exceptions;

[Serializable]
public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(ValidationFailure failure)
        : this(new[] { failure }) { }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }

    protected ValidationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        Errors = (Dictionary<string, string[]>)info.GetValue("Errors", typeof(Dictionary<string, string[]>))!;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("Errors", Errors);
    }
}