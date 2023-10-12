namespace VelocipedSite.DAL.Settings;

public record DalOptions
{
    public string ConnectionString { get; init; } = "";
}