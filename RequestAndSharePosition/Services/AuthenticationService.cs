using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RequestAndSharePosition.Data;
using RequestAndSharePosition.Shared;
using System.Collections.Frozen;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RequestAndSharePosition.Services
{
    internal sealed class AuthenticationService(ApplicationDbContext dbContext, ILogger<AuthenticationService> logger,
        SignInManager<IdentityUser> signInManager, IConfiguration configuration,
        UserManager<IdentityUser> userManager) : IAuthenticationService
    {
        public async ValueTask<object> SinIngAsync(LoginRequest login)
        {
            if (string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
            {
                logger.LogError("There is some input fields that still empty,Every input fields must be fill before you submit , please check them and retry");
                return new
                {
                    status = false,
                    message = "Veuillez saisir votre nom d'utilisateur" +
                    "et votre mot de passe.",
                    data = string.Empty
                };
            }

            var user = await userManager.FindByEmailAsync(login.Username);
            if (user == null)
            {
                logger.LogError("User not found");
                return new
                {
                    status = false,
                    message = "Utilisateur non trouvé. Veuillez vérifier vos informations de connexion.",
                    data = string.Empty
                };
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, login.Password, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                logger.LogInformation("User Logged in");
                var tokenhandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.Email)
                    }),
                    Expires = DateTime.UtcNow.AddDays(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                };

                //generate token
                var token = tokenhandler.CreateToken(tokenDescriptor);

                //return token
                return new { Status = true, Message = "Token Retourné", Data = tokenhandler.WriteToken(token) };
            }

            logger.LogError("An Error occured while processing, Faild to logIn user.");
            return new
            {
                status = false,
                message = "Erreur de connexion au compte. verifiez vos accès. " +
                "Si cela persite veuillez vous rendre aupres de l'ecole pour plus d'informations.",
                data = string.Empty
            };
        }

        public async ValueTask<object> SignUpAsync(LoginRequest login)
        {
            if (string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
            {
                logger.LogError("There is some input    fields that still empty,Every input fields must be fill before you submit , please check them and retry");
                return new
                {
                    status = false,
                    message = "Veuillez saisir votre nom d'utilisateur" +
                    "et votre mot de passe.",
                    data = string.Empty
                };
            }

            var user = new IdentityUser
            {
                UserName = login.Username,
                Email = login.Username,
            };

            var result = await userManager.CreateAsync(user, login.Password);
            if (result.Succeeded)
            {
                logger.LogInformation("User Created");
                return new
                {
                    status = true,
                    message = "Compte créé avec succès. Veuillez vérifier votre boîte de réception pour confirmer votre adresse e-mail.",
                    data = string.Empty
                };
            }

            logger.LogError("An Error occured while processing, Faild to create user.");
            return new
            {
                status = false,
                message = "Erreur de création de compte. Veuillez vérifier vos informations et réessayer.",
                data = string.Empty
            };
        }

        public async ValueTask<object> GetInfoAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                logger.LogError("User not found");
                return new
                {
                    status = false,
                    message = "Utilisateur non trouvé. Veuillez vérifier vos informations de connexion.",
                    data = string.Empty
                };
            }

            return new
            {
                status = true,
                message = "Welcome to RequestAndSharePosition API",
                data = new
                {
                    UserId = user.Id,
                    userName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    IsEmailConfirm = user.EmailConfirmed,
                }
            };
        }

        public async ValueTask<object> GetUserAsync()
        {
            var users = userManager.Users.Select((u) =>
                new
                {
                    UserId = u.Id,
                    userName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    IsEmailConfirm = u.EmailConfirmed
                }).ToFrozenSet();

            await Task.Delay(500);

            return new
            {
                status = true,
                message = "List of all users",
                Data = users
            };
        }
    }
}
