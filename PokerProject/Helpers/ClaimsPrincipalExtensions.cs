using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var claim = user.FindFirst(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID claim not found");

        if (!int.TryParse(claim.Value, out var userId))
            throw new UnauthorizedAccessException("Invalid user ID claim");

        return userId;
    }
    public static string GetUserRole(this ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";
    }
}