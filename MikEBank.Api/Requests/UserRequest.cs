using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MikEBank.Api.Requests;

/// <summary>Thông tin cá nhân để tạo hoặc cập nhật người dùng.</summary>
public sealed record UserRequest
{
    [Required, MaxLength(100)]
    public required string FullName { get; init; }

    [Required, RegularExpression("^[0-9]{12}$", ErrorMessage = "Số định danh phải gồm 12 chữ số.")]
    public required string IdentityNumber { get; init; }

    [JsonRequired]
    public DateOnly DateOfBirth { get; init; }

    [Required, MaxLength(20)]
    public required string Gender { get; init; }

    [Required, MaxLength(100)]
    public required string Nationality { get; init; }

    [Required, MaxLength(250)]
    public required string PlaceOfOrigin { get; init; }

    [Required, MaxLength(250)]
    public required string PlaceOfResidence { get; init; }

    [JsonRequired]
    public DateOnly DateOfIssue { get; init; }

    [JsonRequired]
    public DateOnly DateOfExpiry { get; init; }

    [Required, EmailAddress, MaxLength(150)]
    public required string Email { get; init; }

    [MaxLength(20)]
    public string? Phone { get; init; }
}
