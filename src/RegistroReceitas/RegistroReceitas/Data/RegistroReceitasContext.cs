using Microsoft.EntityFrameworkCore;
using RegistroReceitas.Models;

namespace RegistroReceitas.Data
{
    public class RegistroReceitasContext(DbContextOptions<RegistroReceitasContext> options) : DbContext(options)
    {
        public DbSet<Receita> Receita { get; set; } = default!;
        public DbSet<Usuario> Usuario { get; set; } = default!;
        public DbSet<Teste> Teste { get; set; } = default!;
        public DbSet<Categoria> Categoria { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario()
                {
                    Id = new Guid("F736BE24-92AB-46DB-9059-F31E73C23523"),
                    Email = "leonardofanck@gmail.com",
                    Login = "leonardo",
                    Nome = "Leonardo Fanck",
                    Senha = "123",
                    Situacao = false,
                }
                );
        }
    }
}
