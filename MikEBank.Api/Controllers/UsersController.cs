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

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create(
        UserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await service.CreateAsync(request, cancellationToken);
            if (result.Errors.Count > 0)
                return ValidationProblem(new ValidationProblemDetails(result.Errors));

            var user = result.User ?? throw new InvalidOperationException("Không nhận được người dùng sau khi tạo.");
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
        try
        {
            var result = await service.UpdateAsync(id, request, cancellationToken);
            if (result.Errors.Count > 0)
                return ValidationProblem(new ValidationProblemDetails(result.Errors));

            return result.User is null ? NotFound() : Ok(result.User);
        }
        catch (DuplicateUserException ex)
        {
            return Conflict(new ProblemDetails { Status = 409, Title = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken) =>
        await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}
