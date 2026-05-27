using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistroReceitas.Data;
using RegistroReceitas.Enums;
using RegistroReceitas.Filters;
using RegistroReceitas.Helpers;
using RegistroReceitas.Models;
using RegistroReceitas.Services;

namespace RegistroReceitas.Controllers;

[AutenticadoFilter]
public class ReceitasController(RegistroReceitasContext context, IEmailService emailService) : Controller
{
    private readonly RegistroReceitasContext _context = context;
    private readonly IEmailService _emailService = emailService;

    // GET: Receitas
    public async Task<IActionResult> Index(DateTime? dataInicio,  DateTime? dataFim, TipoReceita? tipoReceita)
    {
        var query = _context.Receita.AsQueryable();

        if (dataInicio.HasValue)
            query = query.Where(r => r.DataRegistro >= dataInicio.Value);
        
        if(dataFim.HasValue)
            query = query.Where(r => r.DataRegistro <= dataFim.Value);

        if(tipoReceita.HasValue)
            query = query.Where(r => r.TipoReceita == tipoReceita.Value);

        var receitas = await query.ToListAsync();

        ViewBag.TiposReceitas = Enum.GetValues<TipoReceita>();

        return View(receitas);
    }

    // GET: Receitas/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var receita = await _context.Receita
            .FirstOrDefaultAsync(m => m.Id == id);
        if (receita == null)
        {
            return NotFound();
        }

        return View(receita);
    }

    // GET: Receitas/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Receitas/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,Custo,TipoReceita")] Receita receita)
    {
        if (ModelState.IsValid)
        {
            receita.Id = Guid.NewGuid();
            receita.DataRegistro = DateTime.UtcNow;
            _context.Add(receita);
            await _context.SaveChangesAsync();
            await EnviarEmailNotificacao(receita);
            return RedirectToAction(nameof(Index));
        }
        return View(receita);
    }

    private async Task EnviarEmailNotificacao(Receita receita)
    {
        var user = await _context.Usuario.FirstAsync(x => x.Id == HttpContext.Session.GetUsuarioId());
        var subject = "Nova Receita Criada";
        var message = $"A receita '{receita.Nome}' foi criada com sucesso.";
        await _emailService.SendAsync(user.Email, subject, message);
    }

    // GET: Receitas/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var receita = await _context.Receita.FindAsync(id);
        if (receita == null)
        {
            return NotFound();
        }
        return View(receita);
    }

    // POST: Receitas/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nome,Descricao,DataRegistro,Custo,TipoReceita")] Receita receita)
    {
        if (id != receita.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(receita);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReceitaExists(receita.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(receita);
    }

    // GET: Receitas/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var receita = await _context.Receita
            .FirstOrDefaultAsync(m => m.Id == id);
        if (receita == null)
        {
            return NotFound();
        }

        return View(receita);
    }

    // POST: Receitas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var receita = await _context.Receita.FindAsync(id);
        if (receita != null)
        {
            _context.Receita.Remove(receita);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ReceitaExists(Guid id)
    {
        return _context.Receita.Any(e => e.Id == id);
    }
}
