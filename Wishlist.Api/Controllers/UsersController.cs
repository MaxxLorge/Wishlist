using Mapster;

using Microsoft.AspNetCore.Mvc;

using Wishlist.DAL.Entities;
using Wishlist.DAL.Repositories;

using Wishlist.Api.Dto.Users;

namespace Wishlist.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    [Route("{userId:int}")]
    public async Task<ActionResult<UserGetResponse>> Get([FromRoute] int userId, CancellationToken ct)
    {
        var user = await _userRepository.FindById(userId, ct);

        if (user == null)
            return NotFound(user);

        return user.Adapt<UserGetResponse>();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Post([FromBody] UserCreateRequest request, CancellationToken ct)
    {
        var userEntity = request.Adapt<User>();

        await _userRepository.AddUser(userEntity, ct);

        return CreatedAtAction(nameof(Get), new { userId = userEntity.Id });
    }
}