//namespace RegistroReceitas.Helpers;

//public static class SessionHelper
//{
//    public const string UsuarioId = "UsuarioId";
//    public const string UsuarioNome = "UsuarioNome";

//    public static bool EstaLogado(this ISession accessor)
//        => accessor.GetString(UsuarioId) != null;

//    public static string GetUsuarioNome(this ISession accessor)
//        => accessor.GetString(UsuarioNome) ?? string.Empty;

//    public static Guid GetUsuarioId(this ISession accessor)
//        => Guid.Parse(accessor.GetString(UsuarioId) ?? Guid.Empty.ToString());
//}