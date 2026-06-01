# Imagem base oficial do Node
FROM node:20-alpine

# Diretório da aplicação dentro do container
WORKDIR /app

# Copia apenas package.json primeiro (melhora cache do Docker)
COPY package*.json ./

# Instala dependências
RUN npm install --production

# Copia o resto do código
COPY . .

# Porta que sua API usa
EXPOSE 3000

# Variáveis de ambiente padrão (podem ser sobrescritas no docker run)
ENV NODE_ENV=homologacao

# Comando de execução
CMD ["node", "deploy.js"]
