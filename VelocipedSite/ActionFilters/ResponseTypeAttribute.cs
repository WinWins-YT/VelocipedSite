using Microsoft.AspNetCore.Mvc;

namespace VelocipedSite.ActionFilters;

public class ResponseTypeAttribute : ProducesResponseTypeAttribute
{
    public ResponseTypeAttribute(int statusCode) : base(typeof(ErrorResponse), statusCode)
    {
    }
}