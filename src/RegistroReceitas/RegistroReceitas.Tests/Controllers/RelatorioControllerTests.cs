using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using RegistroReceitas.Controllers;
using RegistroReceitas.Data;
using RegistroReceitas.Enums;
using RegistroReceitas.Models;
using RegistroReceitas.Services;

namespace RegistroReceitas.Tests.Controllers;

public class RelatorioControllerTests
{
    [Fact]
    public async Task Deve_Retornar_Pdf_Com_Filtro()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegistroReceitasContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new RegistroReceitasContext(options);

        context.Receita.AddRange(
            new Receita
            {
                Nome = "Salário",
                DataRegistro = new DateTime(2025, 1, 10),
                TipoReceita = TipoReceita.Salgada,
                Custo = 5000
            },
            new Receita
            {
                Nome = "Freelance",
                DataRegistro = new DateTime(2025, 2, 10),
                TipoReceita = TipoReceita.Doce,
                Custo = 1000
            });

        await context.SaveChangesAsync();

        var pdfService = new Mock<IPdfService>();

        pdfService
            .Setup(x => x.GerarPdfRelatorio(It.IsAny<List<Receita>>()))
            .Returns([1, 2, 3]);

        var controller = new RelatorioController(
            context,
            pdfService.Object);

        // Act
        var result = await controller.Visualizar(
            new DateTime(2025, 1, 1),
            new DateTime(2025, 1, 31),
            TipoReceita.Doce);

        // Assert
        result.Should().BeOfType<FileContentResult>();

        var fileResult = result as FileContentResult;

        fileResult!.ContentType.Should().Be("application/pdf");
        fileResult.FileContents.Should().NotBeEmpty();
    }
}
