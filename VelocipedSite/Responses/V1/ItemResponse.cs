namespace VelocipedSite.Responses.V1;

public record ItemResponse(long Id, string Name, string Description, string PathToImg, double Price);