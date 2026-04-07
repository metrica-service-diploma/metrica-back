namespace metrica_back.src.Business.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? Error { get; }
    public int StatusCode { get; }

    private Result(bool isSuccess, T? data, string? error, int statusCode) =>
        (IsSuccess, Data, Error, StatusCode) = (isSuccess, data, error, statusCode);

    public static Result<T> Success(T data, int statusCode = 200) =>
        new(true, data, null, statusCode);

    public static Result<T> Failure(string error, int statusCode = 400) =>
        new(false, default, error, statusCode);
}
