using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WDC_F24.Application.DTOs.Requests;
using WDC_F24.Application.DTOs.Responses;
using WDC_F24.Application.Interfaces;
using WDC_F24.Domain.Entities;
using WDC_F24.infrastructure.interfaces;
using WDC_F24.infrastructure.Repositories;
using WDC_F24.UtilityServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

[ApiController]
[Route("[controller]")]



public class UsersController : ControllerBase
{

    private readonly UserManager<User> _userManager;
    private readonly IUserService _usersService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJwtService _jwtService;


    public UsersController(IUserService userservice, IHttpContextAccessor httpContextAccessor, IJwtService jwtService)
    {
        _usersService = userservice;
        _httpContextAccessor = httpContextAccessor;
        _jwtService = jwtService;
    }
    
    [HttpGet("Profiles")]
    public async Task<IActionResult> Get()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var Result = await _usersService.GetAllAsync();
            var Response = Result.ToActionResult();
            return Response;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var Result = await _usersService.GetByIdAsync(id);
            var Response = Result.ToActionResult();
            return Response;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var Result = await _usersService.DeleteAsync(id);
            var Response = Result.ToActionResult();
            return Response;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
   
    [HttpPost("Register")]
    public async Task<IActionResult> Add([FromBody]RegisterRequestDto register)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var Result = await _usersService.RegisterAsync(register);
            var Response = Result.ToActionResult();
            return Response;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPost("Login")]
    public async Task<IActionResult> login([FromBody] LoginRequestDto login)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var Result = await _usersService.LoginAsync(login);
            var Response = Result.ToActionResult();
            return Response;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    [HttpPatch("Update")]
    public async Task<IActionResult> Update(Guid id,UpdateRequestDto user )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var Result = await _usersService.UpdateAsync(user, id);
            var Response = Result.ToActionResult();
            return Response;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
}
