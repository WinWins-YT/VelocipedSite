using Microsoft.AspNetCore.Mvc;

namespace VelocipedSite.Controllers.V1;

[ApiController]
[Route("/api/v1/[controller]/[action]")]
public class ProfileController : ControllerBase
{
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(ILogger<ProfileController> logger)
    {
        _logger = logger;
    }
}