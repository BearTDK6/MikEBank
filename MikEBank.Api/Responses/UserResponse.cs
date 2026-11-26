using MikEBank.Api.Models;

namespace MikEBank.Api.Responses;

/// <summary>Thông tin người dùng trả về từ API.</summary>
public sealed record UserResponse(
    int Id,
    string FullName,
    string IdentityNumber,
    DateOnly DateOfBirth,
    string Gender,
    string Nationality,
    string PlaceOfOrigin,
    string PlaceOfResidence,
    DateOnly DateOfIssue,
    DateOnly DateOfExpiry,
    string Email,
    string? Phone,
    UserRole Role);
