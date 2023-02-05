namespace ADT.Api.Models;

public class OperationResult<T>
{
    public OperationResult(T value)
    {
        Data = value;
        Status = OperationResultStatus.Success;
    }

    public OperationResult(OperationResultStatus status, string errorMessage)
    {
        Status = status;
        ErrorMessage = errorMessage;
    }

    public T? Data { get; }
    public OperationResultStatus Status { get; }
    public string? ErrorMessage { get; set; }

    public bool IsSuccess => Status == OperationResultStatus.Success;

    public static implicit operator OperationResult<T>(T value)
    {
        return new OperationResult<T>(value);
    }

    public static implicit operator OperationResult<T>(OperationResultStatus status)
    {
        return new OperationResult<T>(status, string.Empty);
    }
}