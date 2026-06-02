FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# =========================================
# COPIA APENAS CSProj
# =========================================

COPY ["src/RegistroReceitas/RegistroReceitas/RegistroReceitas.csproj", "src/RegistroReceitas/RegistroReceitas/"]

# =========================================
# RESTORE (FICA EM CACHE)
# =========================================

RUN dotnet restore "src/RegistroReceitas/RegistroReceitas/RegistroReceitas.csproj"

# =========================================
# COPIA RESTANTE DOS ARQUIVOS
# =========================================

COPY . .

WORKDIR "/src/src/RegistroReceitas/RegistroReceitas"

# =========================================
# PUBLISH
# =========================================

RUN dotnet publish "RegistroReceitas.csproj" \
    -c Release \
    -o /app/publish \
#    --no-restore

# =========================================
# RUNTIME
# =========================================

FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "RegistroReceitas.dll"]
