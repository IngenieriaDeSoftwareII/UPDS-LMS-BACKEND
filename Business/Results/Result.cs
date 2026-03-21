namespace Business.Results;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public IEnumerable<string> Errors { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Errors = [];
    }

    private Result(IEnumerable<string> errors)
    {
        IsSuccess = false;
        Errors = errors;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(IEnumerable<string> errors) => new(errors);
}
