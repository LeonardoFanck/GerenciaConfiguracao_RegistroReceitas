using Microsoft.EntityFrameworkCore;
using RegistroReceitas.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegistroReceitas.Tests.Utils;

public static class Context
{
	public static RegistroReceitasContext GetContext()
	{
		var options = new DbContextOptionsBuilder<RegistroReceitasContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;

		return new RegistroReceitasContext(options);
	}
}
