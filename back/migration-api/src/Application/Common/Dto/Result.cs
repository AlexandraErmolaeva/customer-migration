namespace Application.Common.Dto;

public class Result<T>
{
    public T ResultData { get; private set; }
    public bool Succeeded { get; private set; }
    public string ErrorMessage { get; private set; }

    private Result(bool succeeded, T resultData, string errorMessage)
    {
        Succeeded = succeeded;
        ResultData = resultData;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Success(T resultData)
        => new Result<T>(true, resultData, string.Empty);

    public static Result<T> Failure(IEnumerable<string> errors)
        => new Result<T>(false, default, string.Join("; ", errors));

    public static Result<T> Failure(string error)
        => new Result<T>(false, default, error);

    public static implicit operator Result<T>(T value) => Success(value);
}
