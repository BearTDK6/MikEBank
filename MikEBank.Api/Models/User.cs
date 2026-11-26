namespace MikEBank.Api.Models;

public class User
{
    public int Id { get; set; }
    public UserRole Role { get; set; } = UserRole.Customer;
    public string FullName { get; set; } = "";
    public string IdentityNumber { get; set; } = "";
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = "";
    public string Nationality { get; set; } = "";
    public string PlaceOfOrigin { get; set; } = "";
    public string PlaceOfResidence { get; set; } = "";
    public DateOnly DateOfIssue { get; set; }
    public DateOnly DateOfExpiry { get; set; }
    public string Email { get; set; } = "";
    public string? Phone { get; set; }
}
