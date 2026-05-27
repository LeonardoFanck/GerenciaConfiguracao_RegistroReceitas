# RegistroReceitas - Configuração Inicial

# 1. Estrutura atual do projeto

```text
projeto-final/

├── infra/
│   └── local/
│       └── docker-compose.yml
│
├── src/
│   └── RegistroReceitas/
│       ├── Controllers/
│       ├── Models/
│       ├── Views/
│       ├── Services/
│       ├── Data/
│       ├── Migrations/
│       ├── appsettings.json
│       ├── appsettings.Development.json
│       └── RegistroReceitas.csproj
│
├── tests/
│   └── RegistroReceitas.Tests/
```

---

# 2. Como executar o projeto pela primeira vez

## Pré-requisitos

Instalar:

* Docker
* Docker Compose
* .NET 10 SDK
* Git

---

## 1. Clonar projeto

```bash
git clone https://github.com/LeonardoFanck/GerenciaConfiguracao_RegistroReceitas.git
```

Entrar na pasta:

```bash
cd SEU_REPOSITORIO
```

---

## 2. Criar appsettings.Development.json

Criar arquivo:

```text
src/RegistroReceitas/appsettings.Development.json
```

Conteúdo:

```json
{
  "ConnectionStrings": {
    "RegistroReceitasContext": "Host=localhost;Port=5432;Database=registro_receitas;Username=postgres;Password=postgres"
  }
}
```

---

## 3. Subir PostgreSQL Docker

Entrar na pasta:

```bash
cd infra/local
```

Executar:

```bash
docker compose up -d
```

Verificar containers:

```bash
docker ps
```

---

## 4. Restaurar dependências

Voltar para raiz do projeto:

```bash
cd ../../
```

Executar:

```bash
dotnet restore
```

---

## 5. Criar banco de dados

Executar migrations:

```bash
dotnet ef database update --project src/RegistroReceitas
```

---

## 6. Executar aplicação

```bash
dotnet run --project src/RegistroReceitas
```

---

## 7. Acessar aplicação

Abrir:

```text
http://localhost:5000
```

ou:

```text
https://localhost:5001
```

---