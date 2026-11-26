namespace MikEBank.Api.Services;

public sealed class DuplicateUserException(Exception innerException)
    : Exception("Email hoặc số định danh đã tồn tại.", innerException);
