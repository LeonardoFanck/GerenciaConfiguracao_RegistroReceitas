using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RegistroReceitas.Controllers;
using RegistroReceitas.Enums;
using RegistroReceitas.Models;
using RegistroReceitas.Services;
using RegistroReceitas.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace RegistroReceitas.Tests.Controllers;

public class ReceitasControllerTests
{
	[Fact]
	public async Task Details_DeveRetornarNotFound_QuandoIdForNulo()
	{
		var context = Context.GetContext();
		var email = new Mock<IEmailService>();

		var controller = new ReceitasController(context, email.Object);

		var result = await controller.Details(null);

		Assert.IsType<NotFoundResult>(result);
	}

	[Fact]
	public async Task Details_DeveRetornarNotFound_QuandoReceitaNaoExiste()
	{
		var context = Context.GetContext();
		var email = new Mock<IEmailService>();

		var controller = new ReceitasController(context, email.Object);

		var result = await controller.Details(Guid.NewGuid());

		Assert.IsType<NotFoundResult>(result);
	}

	[Fact]
	public async Task Create_ModelStateInvalido_DeveRetornarView()
	{
		var context = Context.GetContext();
		var email = new Mock<IEmailService>();

		var controller = new ReceitasController(context, email.Object);

		controller.ModelState.AddModelError("Nome", "Obrigatório");

		var receita = new Receita();

		var result = await controller.Create(receita);

		var view = Assert.IsType<ViewResult>(result);

		Assert.Equal(receita, view.Model);
	}

	[Fact]
	public async Task Delete_DeveRetornarNotFound_QuandoReceitaNaoExiste()
	{
		var context = Context.GetContext();
		var email = new Mock<IEmailService>();

		var controller = new ReceitasController(context, email.Object);

		var result = await controller.Delete(Guid.NewGuid());

		Assert.IsType<NotFoundResult>(result);
	}

	[Fact]
	public async Task DeleteConfirmed_DeveRemoverReceita()
	{
		var context = Context.GetContext();

		var receita = new Receita
		{
			Id = Guid.NewGuid(),
			Nome = "Teste",
			Descricao = "Desc",
			Custo = 100,
			DataRegistro = DateTime.Now,
			TipoReceita = TipoReceita.Doce,
		};

		context.Receita.Add(receita);
		await context.SaveChangesAsync();

		var email = new Mock<IEmailService>();

		var controller = new ReceitasController(context, email.Object);

		var result = await controller.DeleteConfirmed(receita.Id);

		Assert.IsType<RedirectToActionResult>(result);

		Assert.Empty(context.Receita);
	}

	[Fact]
	public async Task Edit_DeveRetornarNotFound_QuandoIdsForemDiferentes()
	{
		var context = Context.GetContext();
		var email = new Mock<IEmailService>();

		var controller = new ReceitasController(context, email.Object);

		var receita = new Receita
		{
			Id = Guid.NewGuid(),
			Nome = "Receita"
		};

		var result = await controller.Edit(Guid.NewGuid(), receita);

		Assert.IsType<NotFoundResult>(result);
	}

	[Fact]
	public async Task Edit_DeveAtualizarReceita()
	{
		var context = Context.GetContext();

		var receita = new Receita
		{
			Id = Guid.NewGuid(),
			Nome = "Receita Antiga",
			Descricao = "Descricao",
			Custo = 100,
			DataRegistro = DateTime.Today,
			TipoReceita = TipoReceita.Salgada
		};

		context.Receita.Add(receita);
		await context.SaveChangesAsync();

		receita.Nome = "Receita Atualizada";
		receita.Custo = 250;

		var email = new Mock<IEmailService>();

		var controller = new ReceitasController(context, email.Object);

		var result = await controller.Edit(receita.Id, receita);

		Assert.IsType<RedirectToActionResult>(result);

		var receitaBanco = await context.Receita.FindAsync(receita.Id);

		Assert.Equal("Receita Atualizada", receitaBanco!.Nome);
		Assert.Equal(250, receitaBanco.Custo);
	}

	[Fact]
	public async Task Index_DeveRetornarTodasReceitas()
	{
		var context = Context.GetContext();

		context.Receita.Add(new Receita
		{
			Id = Guid.NewGuid(),
			Nome = "Receita 1"
		});

		context.Receita.Add(new Receita
		{
			Id = Guid.NewGuid(),
			Nome = "Receita 2"
		});

		await context.SaveChangesAsync();

		var email = new Mock<IEmailService>();

		var controller = new ReceitasController(context, email.Object);

		var result = await controller.Index(null, null, null);

		var view = Assert.IsType<ViewResult>(result);

		var model = Assert.IsAssignableFrom<IEnumerable<Receita>>(view.Model);

		Assert.Equal(2, model.Count());
	}

	[Fact]
	public async Task Details_DeveRetornarView_QuandoReceitaExiste()
	{
		var context = Context.GetContext();

		var receita = new Receita
		{
			Id = Guid.NewGuid(),
			Nome = "Notebook"
		};

		context.Receita.Add(receita);

		await context.SaveChangesAsync();

		var email = new Mock<IEmailService>();

		var controller = new ReceitasController(context, email.Object);

		var result = await controller.Details(receita.Id);

		var view = Assert.IsType<ViewResult>(result);

		var model = Assert.IsType<Receita>(view.Model);

		Assert.Equal(receita.Id, model.Id);
	}

	[Fact]
	public async Task DeleteConfirmed_DeveRedirecionarParaIndex()
	{
		var context = Context.GetContext();

		var receita = new Receita
		{
			Id = Guid.NewGuid(),
			Nome = "Excluir"
		};

		context.Receita.Add(receita);

		await context.SaveChangesAsync();

		var email = new Mock<IEmailService>();

		var controller = new ReceitasController(context, email.Object);

		var result = await controller.DeleteConfirmed(receita.Id);

		var redirect = Assert.IsType<RedirectToActionResult>(result);

		Assert.Equal("Index", redirect.ActionName);
	}

	[Fact]
	public async Task Create_DeveEnviarEmail()
	{
		var context = Context.GetContext();

		var emailMock = new Mock<IEmailService>();

		var controller = new ReceitasController(context, emailMock.Object);
		var controllerAuth = new AutenticacaoController(context);

		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
			new(ClaimTypes.Name, "Leonardo"),
			new(ClaimTypes.Email, "teste@teste.com")
		};

		var identity = new ClaimsIdentity(claims, "TestAuth");
		var principal = new ClaimsPrincipal(identity);

		var httpContext = new DefaultHttpContext
		{
			User = principal
		};

		controller.ControllerContext = new ControllerContext
		{
			HttpContext = httpContext
		};

		context.Usuario.Add(new Usuario
		{
			Id = Guid.Parse(claims[0].Value),
			Nome = claims[1].Value,
			Email = claims[2].Value,
			Senha = "123456"
		});
		await context.SaveChangesAsync();
		await controllerAuth.Login(new ViewModel.LoginViewModel
		{
			Login = claims[1].Value,
			Senha = "123456"
		});
	

	var receita = new Receita
		{
			Id = Guid.NewGuid(),
			Nome = "Nova Receita",
			Descricao = "Teste",
			Custo = 100,
			DataRegistro = DateTime.Now,
			TipoReceita = TipoReceita.Salgada
		};

		await controller.Create(receita);

		emailMock.Verify(x =>
			x.SendAsync(
				It.IsAny<string>(),
				It.IsAny<string>(),
				It.IsAny<string>()),
			Times.Once);
	}
}
