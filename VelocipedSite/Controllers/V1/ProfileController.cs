using Microsoft.AspNetCore.Mvc;
using VelocipedSite.DAL.Exceptions;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.Models;
using VelocipedSite.Requests.V1.Profile;
using VelocipedSite.Responses.V1.Profile;
using VelocipedSite.Utilities;

namespace VelocipedSite.Controllers.V1;

[ApiController]
[Route("/api/v1/[controller]/[action]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileRepository _profileRepository;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(IProfileRepository profileRepository, ILogger<ProfileController> logger)
    {
        _profileRepository = profileRepository;
        _logger = logger;
    }

    [HttpPost]
    public async Task<CheckTokenResponse> CheckToken(CheckTokenRequest request)
    {
        try
        {
            var token = await _profileRepository.GetToken(new TokenQuery
            {
                Token = Guid.Parse((ReadOnlySpan<char>)request.Token)
            });

            var isValid = DateTime.UtcNow.Subtract(token.ValidUntil).TotalSeconds > 0;

            _logger.LogInformation("Checked token {Token}, result is {IsValid}", request.Token, isValid);
            return new CheckTokenResponse
            {
                IsValid = isValid,
                ValidUntil = token.ValidUntil
            };
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogError("Checked token {Token}: {Message}", request.Token, ex.Message);
            return new CheckTokenResponse
            {
                IsValid = false,
                ValidUntil = new DateTime()
            };
        }
    }

    [HttpPost]
    public async Task<GetUserByTokenResponse> GetUserByToken(GetUserByTokenRequest request)
    {
        try
        {
            Guid.TryParse(request.Token, out var guid);
            var token = await _profileRepository.GetToken(new TokenQuery
            {
                Token = guid
            });

            var isValid = token.ValidUntil.Subtract(DateTime.UtcNow).TotalSeconds > 0;
            
            _logger.LogInformation("Getting user by token {Token}, IsValid: {IsValid}", request.Token, isValid);
            if (!isValid)
                return new GetUserByTokenResponse
                {
                    User = null,
                    IsValid = false
                };

            var user = await _profileRepository.GetUserFromToken(new TokenQuery
            {
                Token = token.Token
            });

            return new GetUserByTokenResponse
            {
                IsValid = true,
                User = new User(user.Id, user.Email, user.FirstName, user.LastName, user.Address, user.Phone)
            };
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogError("Getting user by token {Token}: {Message}", request.Token, ex.Message);
            return new GetUserByTokenResponse
            {
                IsValid = false,
                User = null
            };
        }
    }

    [HttpPost]
    public async Task InvalidateToken(InvalidateTokenRequest request)
    {
        _logger.LogInformation("Invalidating token {Token}", request.Token);
        var ids = await _profileRepository.RemoveToken(new TokenQuery
        {
            Token = Guid.Parse((ReadOnlySpan<char>)request.Token)
        });
        _logger.LogInformation("Invalidation succeeded, deleted token with ID {Id}", ids.First());
    }

    [HttpPost]
    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
    {
        try
        {
            _logger.LogInformation("Authenticating user with e-mail: {Email}", request.Email);
            var user = await _profileRepository.GetUserByEmail(new EmailQuery
            {
                Email = request.Email
            });

            var crypt = new PasswordCrypt(Convert.FromBase64String(user.Password));

            if (!crypt.Verify(request.Password))
            {
                _logger.LogError("Authentication of user with e-mail {Email} is failed: Password is invalid", request.Email);
                return new AuthenticateResponse(false, "", "Неправильный пароль, попробуйте еще раз");
            }

            if (!user.Activated)
            {
                _logger.LogError("Authentication of user with e-mail {Email} is failed: Account is not activated", request.Email);
                return new AuthenticateResponse(false, "", "Аккаунт не активирован. На почту, указанную при регистрации, было отправлено письмо со ссылкой для активации");
            }
            
            var token = await _profileRepository.CreateTokenForUser(new UserIdQuery
            {
                UserId = user.Id
            });

            _logger.LogInformation("Authentication of user with e-mail {Email} is succeeded, token: {Token}", request.Email, token.Token);
            return new AuthenticateResponse(true, token.Token.ToString(), "");
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogError("Authentication of user with e-mail {Email} failed: {Message}", request.Email, ex.Message);
            return new AuthenticateResponse(false, "", "Неправильный E-mail, попробуйте еще раз. Если у вас нет аккаунта, воспользуйтесь кнопкой Регистрация");
        }
    }

    [HttpPost]
    public async Task<RegisterResponse> Register(RegisterRequest request)
    {
        _logger.LogInformation("Registering user with Email {Email}...", request.Email);
        long user = -1;
        try
        {
            var crypt = new PasswordCrypt(request.Password);

            user = await _profileRepository.AddUser(new UserQuery
            {
                Email = request.Email,
                Address = request.Address,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = crypt.ToString(),
                Phone = request.Phone
            });

            var token = await _profileRepository.CreateTokenForUser(new UserIdQuery
            {
                UserId = user
            });

            _logger.LogInformation(
                "Registration of user {Email} is succeeded, token: {Token}, sending activation mail...", request.Email,
                token.Token.ToString());
            await Mail.SendActivationEmail(request.Email, token.Token.ToString());

            _logger.LogInformation("Registration of user {Email} is succeeded, mail sent", request.Email);
            return new RegisterResponse(true, "");
        }
        catch (EntityAlreadyExistsException exception)
        {
            _logger.LogError("Registration of user {Email} failed: {Message}", request.Email, exception.Message);
            return new RegisterResponse(false, "Пользователь с таким E-mail уже существует");
        }
        catch (Exception ex)
        {
            _logger.LogError("Unhandled exception: {Exception}", ex.Message);
            await _profileRepository.RemoveUser(new UserIdQuery
            {
                UserId = user
            });
            return new RegisterResponse(false, "Че-то сломалось, ща починим");
        }
    }

    [HttpGet("{token}")]
    public async Task<string> Activation(string token)
    {
        try
        {
            _logger.LogInformation("Activating user from token {Token}", token);
            Guid.TryParse(token, out var guid);
            var user = await _profileRepository.GetUserFromToken(new TokenQuery
            {
                Token = guid
            });

            await _profileRepository.ActivateUser(new UserIdQuery
            {
                UserId = user.Id
            });

            return "Активация успешна. Вы можете входить в аккаунт.";
        }
        catch (EntityNotFoundException exception)
        {
            _logger.LogError("Activation of user with token {Token} is failed, {Message}", exception.Message);
            return "Активация пользователя не удалась. Сообщение об ошибке: " + exception.Message;
        }
    }
}