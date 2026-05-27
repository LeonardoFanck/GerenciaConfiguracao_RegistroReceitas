using FluentAssertions;
using RegistroReceitas.Models;
using RegistroReceitas.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegistroReceitas.Tests.Services;

public class PdfServiceTests
{
    [Fact]
    public void Deve_Gerar_Pdf()
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        // Arrange
        var service = new PdfService();

        var receitas = new List<Receita>
        {
            new ()
            {
                Nome = "Teste",
                Custo = 100
            }
        };

        // Act
        var pdf = service.GerarPdfRelatorio(receitas);

        // Assert
        pdf.Should().NotBeNull();
        pdf.Length.Should().BeGreaterThan(0);
    }
}
