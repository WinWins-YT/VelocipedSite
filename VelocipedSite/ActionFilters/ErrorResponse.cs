using System.Net;

namespace VelocipedSite.ActionFilters;

public record ErrorResponse(HttpStatusCode StatusCode, string Message);