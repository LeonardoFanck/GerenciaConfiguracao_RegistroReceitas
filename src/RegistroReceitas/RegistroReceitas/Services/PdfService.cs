using QuestPDF.Fluent;
using RegistroReceitas.Models;

namespace RegistroReceitas.Services;

public class PdfService : IPdfService
{
    public byte[] GerarPdfRelatorio(List<Receita> receitas)
    {
        if(receitas == null || receitas.Count == 0)
        {
            throw new ArgumentException("A lista de receitas não pode ser nula ou vazia.");
        }

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Content().Column(col =>
                {
                    col.Item().Text("Relatório de Receitas").FontSize(20).Bold().AlignCenter();

                    foreach(var receita in receitas)
                    {
                        col.Item().Text($"Nome: {receita.Nome}").FontSize(14).Bold();
                        col.Item().Text($"Descrição: {receita.Descricao}");
                        col.Item().Text($"Data de Registro: {receita.DataRegistro:dd/MM/yyyy}");
                        col.Item().Text($"Custo: R$ {receita.Custo:F2}");
                        col.Item().Text($"Tipo de Receita: {receita.TipoReceita}");
                        col.Item().LineHorizontal(1);
                        col.Item().PaddingBottom(10);
                    }
                });
            });
        }).GeneratePdf();
    }
}
