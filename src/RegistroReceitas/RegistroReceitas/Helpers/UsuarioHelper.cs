using System.Security.Claims;

namespace RegistroReceitas.Helpers;

public static class UsuarioHelper
{
    public static bool EstaLogado(this ClaimsPrincipal user)
        => user.Identity?.IsAuthenticated ?? false;

    public static string GetUsuarioNome(this ClaimsPrincipal user)
        => user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

    public static Guid GetUsuarioId(this ClaimsPrincipal user)
    {
        int i = 10;
        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Guid.TryParse(id, out var guid)
            ? guid
            : Guid.Empty;
    }

}
