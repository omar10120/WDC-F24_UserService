
using WDC_F24.infrastructure.Data;

using WDC_F24.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WDC_F24.Application.Interfaces;
using WDC_F24.Application.DTOs.Responses;
using WDC_F24.Application.DTOs.Requests;
using Microsoft.AspNetCore.Identity;
using WDC_F24.infrastructure.interfaces;
using System.Data;

using WDC_F24.Domain.Consts;



namespace WDC_F24.infrastructure.Repositories
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IRabbitMQPublisher _publisher;
        private readonly IJwtService _jwtService;
        private readonly UserManager<User> _userManager;



        public UserService(AppDbContext context, IRabbitMQPublisher publisher, IJwtService jwtService , UserManager<User> userManager)
        {
            _context = context;
            _publisher = publisher;
            _jwtService = jwtService;
            _userManager = userManager;

        }

        public async Task<GeneralResponse> GetAllAsync()
        {
            var ErrorMsg = "";
            try
            {

                var user = await _context.Users.ToListAsync();

                try
                {
                    _publisher.Publish("users-received", new
                    {
                        user = user  ,
                        Message = $"Users was received successfully"
                    });
                }
                catch (Exception pubEx)
                {
                    ErrorMsg = " , " + pubEx.Message + "publish Failed";
                }
                return GeneralResponse.Ok("Users received successfully"+ ErrorMsg, user );


            }
                catch (DbUpdateException dbEx)
                {
                    return GeneralResponse.BadRequest($"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}");
                }
                catch (Exception ex)
                {
                    return GeneralResponse.BadRequest($"Unexpected error: {ex.Message}");
                }
            }

        public async Task<GeneralResponse> GetByIdAsync(Guid id)
        {
            try
            {
                var ErrorMsg = "";
                if (string.IsNullOrEmpty(id.ToString()))
                {
                    return GeneralResponse.BadRequest("id is required");
                }
                var user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();


                try
                {
                    _publisher.Publish("user-received", new
                    {
                        user = user ,
                        Message = $"User was received successfully"
                    });
                }
                catch (Exception pubEx)
                {
                    ErrorMsg = " , " + pubEx.Message + "publish Failed";
                }
                return GeneralResponse.Ok("user received successfully"+ ErrorMsg, user );
            }

            catch (DbUpdateException dbEx)
            {
                return GeneralResponse.BadRequest($"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return GeneralResponse.BadRequest($"Unexpected error: {ex.Message}");
            }
        }
        public async Task<GeneralResponse> DeleteAsync(Guid id)
        {
            var ErrorMsg = "";
            try
            {

                if (string.IsNullOrEmpty(id.ToString()))
                {
                    return GeneralResponse.BadRequest("id is required");
                }
                var user = await _context.Users.Where(x=>x.Id == id ).FirstOrDefaultAsync();


                    _context.Users.Remove(user!);
                await _context.SaveChangesAsync();
                try
                {
                    _publisher.Publish("user-deleted", new
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        Email = user.Email,
                        Message = $"User {user.Id} was deleted successfully"
                    });
                }
                catch (Exception pubEx)
                {
                    ErrorMsg = " , " + pubEx.Message + "publish Failed";
                }

                return GeneralResponse.Ok("user deleted successfully" + ErrorMsg);
            }
            catch (DbUpdateException dbEx)
            {
                return GeneralResponse.BadRequest($"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return GeneralResponse.BadRequest($"Unexpected error: {ex.Message}");
            }
        } 
        public async Task<GeneralResponse> RegisterAsync(RegisterRequestDto user)
        {
            var ErrorMsg = "";
            try
            {

                if (await _userManager.FindByEmailAsync(user.Email) != null)
                    return GeneralResponse.BadRequest("Email is already in use.");

                if (await _userManager.Users.AnyAsync(s => s.UserName == user.Username))
                    return GeneralResponse.BadRequest("Name is already taken.");
              
                var addUser = new User
                {
                    UserName = user.Username,
                    PasswordHash = user.Password,
                    Email = user.Email,
                    PhoneNumber = user.Phone
                };

            


                var result = await _userManager.CreateAsync(addUser, user.Password);



                if (!result.Succeeded)
                    return GeneralResponse.BadRequest($"Registration failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");

                await _userManager.AddToRoleAsync(addUser, Roles.Admin.ToString());
                


                try
                {
                    _publisher.Publish("user-created", new
                    {
                        Id = addUser.Id,
                        Username = addUser.UserName,
                        Email = addUser.Email,
                        Message = $"User {addUser.Id} was created successfully"
                    });
                }
                catch (Exception pubEx)
                {
                    ErrorMsg = " , " + pubEx.Message + "publish Failed";
                }

                return GeneralResponse.Ok("user added successfully" + ErrorMsg , addUser);
            }
            catch (DbUpdateException dbEx)
            {
                return GeneralResponse.BadRequest($"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return GeneralResponse.BadRequest($"Unexpected error: {ex.Message}");
            }

        }

        public async Task<GeneralResponse> LoginAsync(LoginRequestDto loginDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginDto.UsernameOrEmail) || string.IsNullOrWhiteSpace(loginDto.Password))
                    return GeneralResponse.BadRequest("Username/Email and password are required.");

                // Try to find user by username or email
                var user = await _userManager.Users
                    .FirstOrDefaultAsync(u =>
                        u.UserName.ToLower() == loginDto.UsernameOrEmail.ToLower() ||
                        u.Email.ToLower() == loginDto.UsernameOrEmail.ToLower());

                if (user == null)
                    return GeneralResponse.BadRequest("Invalid username or email.");

                var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!isPasswordValid)
                    return GeneralResponse.BadRequest("Invalid password.");

                //var token = _jwtService.GenerateToken(user.Id.ToString(), user.UserName);

                var roles = await _userManager.GetRolesAsync(user); // user is the IdentityUser
                var token = _jwtService.GenerateToken(user, roles);


                return GeneralResponse.Ok("Login successful", new
                {
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return GeneralResponse.BadRequest($"Unexpected error: {ex.Message}");
            }
        }



        public async Task<GeneralResponse> UpdateAsync(UpdateRequestDto user , Guid id)
        {
            var ErrorMsg = "";
            try
            {

           
                var exsistuser = await _context.Users.Where(x => x.UserName == user.Username).FirstOrDefaultAsync();
                if (exsistuser != null)
                {
                    return GeneralResponse.BadRequest(user.Username + "Username already exist");
                }

                var Getuser = await _context.Users.Where(x => x.Id ==id ).FirstOrDefaultAsync();

                if (Getuser == null)
                {
                    return GeneralResponse.BadRequest("user not available");
                }
                Getuser.UserName = user.Username ?? Getuser.UserName;
                Getuser.UpdatedAt = DateTime.UtcNow;


                _context.Users.Update(Getuser);
                await _context.SaveChangesAsync();
                try
                {
                    _publisher.Publish("user-updated", new
                    {
                        Id = Getuser.Id,
                        Username = Getuser.UserName,
                        Email = Getuser.Email,
                        Message = $"User {Getuser.Id} was updated successfully"
                    });
                }
                catch (Exception pubEx)
                {
                    ErrorMsg = ", "+pubEx.Message + "publish Failed";
                }

                return GeneralResponse.Ok("user updated successfully " + ErrorMsg);

            }
            catch (DbUpdateException dbEx)
            {
                return GeneralResponse.BadRequest($"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return GeneralResponse.BadRequest($"Unexpected error: {ex.Message}");
            }
        }

      


    }
}
