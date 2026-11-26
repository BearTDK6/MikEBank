using Microsoft.EntityFrameworkCore;
using MikEBank.Api.Requests;
using MikEBank.Api.Responses;
using MikEBank.Api.Models;
using MikEBank.Api.Repositories;
using Npgsql;

namespace MikEBank.Api.Services;

public sealed class UserService(UserRepository repository)
{
    public async Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await repository.GetAllAsync(cancellationToken);
        return users.Select(ToResponse).ToList();
    }

    public async Task<UserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(id, cancellationToken);
        return user is null ? null : ToResponse(user);
    }

    public async Task<(UserResponse? User, Dictionary<string, string[]> Errors)> CreateAsync(
        UserRequest request, CancellationToken cancellationToken)
    {
        var errors = ValidateDates(request);
        if (errors.Count > 0)
            return (null, errors);

        var user = new User();
        ApplyRequest(user, request);
        repository.Add(user);
        await SaveUserAsync(cancellationToken);
        return (ToResponse(user), errors);
    }

    public async Task<(UserResponse? User, Dictionary<string, string[]> Errors)> UpdateAsync(
        int id, UserRequest request, CancellationToken cancellationToken)
    {
        var errors = ValidateDates(request);
        if (errors.Count > 0)
            return (null, errors);

        var user = await repository.GetByIdAsync(id, cancellationToken);
        if (user is null)
            return (null, errors);

        ApplyRequest(user, request);
        await SaveUserAsync(cancellationToken);
        return (ToResponse(user), errors);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(id, cancellationToken);
        if (user is null)
            return false;

        repository.Remove(user);
        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task SaveUserAsync(CancellationToken cancellationToken)
    {
        try
        {
            await repository.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException
        {
            SqlState: PostgresErrorCodes.UniqueViolation,
            ConstraintName: "IX_users_Email" or "IX_users_IdentityNumber"
        })
        {
            throw new DuplicateUserException(exception);
        }
    }

    private static Dictionary<string, string[]> ValidateDates(UserRequest request)
    {
        var errors = new Dictionary<string, string[]>();
        var today = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(7));
        if (request.DateOfBirth == default || request.DateOfBirth > today)
            errors[nameof(request.DateOfBirth)] = ["Ngày sinh phải là ngày hợp lệ và không ở tương lai."];
        if (request.DateOfIssue == default || request.DateOfIssue > today || request.DateOfIssue < request.DateOfBirth)
            errors[nameof(request.DateOfIssue)] = ["Ngày cấp phải từ ngày sinh đến hôm nay."];
        if (request.DateOfExpiry <= request.DateOfIssue)
            errors[nameof(request.DateOfExpiry)] = ["Ngày hết hạn phải sau ngày cấp."];
        return errors;
    }

    private static void ApplyRequest(User user, UserRequest request)
    {
        user.FullName = request.FullName;
        user.IdentityNumber = request.IdentityNumber;
        user.DateOfBirth = request.DateOfBirth;
        user.Gender = request.Gender;
        user.Nationality = request.Nationality;
        user.PlaceOfOrigin = request.PlaceOfOrigin;
        user.PlaceOfResidence = request.PlaceOfResidence;
        user.DateOfIssue = request.DateOfIssue;
        user.DateOfExpiry = request.DateOfExpiry;
        user.Email = request.Email;
        user.Phone = request.Phone;
    }

    private static UserResponse ToResponse(User user) =>
        new(user.Id, user.FullName, user.IdentityNumber, user.DateOfBirth,
            user.Gender, user.Nationality, user.PlaceOfOrigin, user.PlaceOfResidence,
            user.DateOfIssue, user.DateOfExpiry, user.Email, user.Phone, user.Role);
}
