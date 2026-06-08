using System.Security.Claims;

namespace RunGroupWebApp.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            //Retrives first claim with the specified claim type.
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
