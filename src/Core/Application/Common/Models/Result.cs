namespace Application.Common.Models;

public class Result(bool succeeded, IEnumerable<string> errors)
{
    public bool Succeeded { get; set; } = succeeded;
    public IEnumerable<string> Errors { get; set; } = errors;

    public static Result Success() => new(true, []);
    public static Result Failure(IEnumerable<string> errors) => new(false, errors);
}