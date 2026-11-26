using Microsoft.AspNetCore.Mvc;
using MikEBank.Api.Requests;
using MikEBank.Api.Responses;
using MikEBank.Api.Services;

namespace MikEBank.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController(UserService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await service.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var user = await service.GetByIdAsync(id, cancellationToken);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpGet("{id:int}/detail")]
    public async Task<ActionResult<UserResponse>> GetDetail(int id, CancellationToken cancellationToken)
    {
        var user = await service.GetByIdAsync(id, cancellationToken);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create(
        UserRequest request, CancellationToken cancellationToken)
    {
        ValidateDates(request);
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var user = await service.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (DuplicateUserException ex)
        {
            return Conflict(new ProblemDetails { Status = 409, Title = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserResponse>> Update(
        int id, UserRequest request, CancellationToken cancellationToken)
    {
        ValidateDates(request);
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var user = await service.UpdateAsync(id, request, cancellationToken);
            return user is null ? NotFound() : Ok(user);
        }
        catch (DuplicateUserException ex)
        {
            return Conflict(new ProblemDetails { Status = 409, Title = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken) =>
        await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();

    private void ValidateDates(UserRequest request)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(7));
        if (request.DateOfBirth == default || request.DateOfBirth > today)
            ModelState.AddModelError(nameof(request.DateOfBirth), "Ngày sinh phải là ngày hợp lệ và không ở tương lai.");
        if (request.DateOfIssue == default || request.DateOfIssue > today || request.DateOfIssue < request.DateOfBirth)
            ModelState.AddModelError(nameof(request.DateOfIssue), "Ngày cấp phải từ ngày sinh đến hôm nay.");
        if (request.DateOfExpiry <= request.DateOfIssue)
            ModelState.AddModelError(nameof(request.DateOfExpiry), "Ngày hết hạn phải sau ngày cấp.");
    }

}
