using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistroReceitas.Data;
using RegistroReceitas.Enums;
using RegistroReceitas.Models;
using RegistroReceitas.Services;

namespace RegistroReceitas.Controllers;

public class RelatorioController(RegistroReceitasContext context, IPdfService pdfService) : Controller
{
    private readonly RegistroReceitasContext _context = context;
    private readonly IPdfService _pdfService = pdfService;

    public async Task<IActionResult> Visualizar(DateTime? dataInicio, DateTime? dataFim, TipoReceita? tipoReceita)
    {
        var query = _context.Receita.AsQueryable();

        if (dataInicio.HasValue)
            query = query.Where(r => r.DataRegistro >= dataInicio.Value);

        if (dataFim.HasValue)
            query = query.Where(r => r.DataRegistro <= dataFim.Value);

        if (tipoReceita.HasValue)
            query = query.Where(r => r.TipoReceita == tipoReceita.Value);

        var receitas = await query.ToListAsync();

        var pdf = _pdfService.GerarPdfRelatorio(receitas);
        return File(pdf, "application/pdf");
    }
}
