using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RegistroReceitas.Helpers;

namespace RegistroReceitas.Filters;

public class AutenticadoFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        //var logado = context.HttpContext.Session.EstaLogado();
        var logado = context.HttpContext.User.EstaLogado();

        if (!logado)
        {
            (context.Controller as Controller)?.TempData["Erro"] = "Autenticação necessária.";
            context.Result = new RedirectToActionResult("Login", "Autenticacao", null);
        }

        base.OnActionExecuting(context);
    }
}
