using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistroReceitas.Data;
using RegistroReceitas.ViewModel;

namespace RegistroReceitas.Controllers;

public class AutenticacaoController(RegistroReceitasContext context) : Controller
{
    private readonly RegistroReceitasContext _context = context;

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model); // leonardo teste novo

        var usuario = await _context.Usuario
            .FirstOrDefaultAsync(u => u.Login == model.Login);

        if (usuario == null)
        {
            ModelState.AddModelError(string.Empty, "Login ou senha inválidos.");
            return View(model);
        }

        if(!string.Equals(model.Senha, usuario.Senha, StringComparison.Ordinal))
        {
            ModelState.AddModelError(string.Empty, "Login ou senha inválidos.");
            return View(model);
        }

        if (usuario.Situacao)
        {
            ModelState.AddModelError(string.Empty, "Usuário inativo.");
            return View(model);
        }

        // Salva na session
        HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
        HttpContext.Session.SetString("UsuarioNome", usuario.Nome);

        return RedirectToAction("Index", "Receitas");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Home");
    }
}
