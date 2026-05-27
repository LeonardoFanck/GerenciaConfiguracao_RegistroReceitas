using RegistroReceitas.Models;

namespace RegistroReceitas.Services;

public interface IPdfService
{
    byte[] GerarPdfRelatorio(List<Receita> receitas);
}
