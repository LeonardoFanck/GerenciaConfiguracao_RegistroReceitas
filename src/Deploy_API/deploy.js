import express from "express";
import { exec } from "child_process";

const app = express();
app.use(express.json());

const DEPLOY_SERVICE_URL =
  process.env.DEPLOY_SERVICE_URL || "http://host.docker.internal:9999";

app.post("/deploy", async (req, res) => {
    try {
        // 🔐 segurança simples (GitHub secret)
        const secret = req.headers["x-secret"];
        if (!secret || secret !== process.env.DEPLOY_SECRET) {
            return res.status(403).send("Forbidden");
        }

        console.log("Webhook recebido do GitHub. Iniciando deploy...");

        // 🚀 chama serviço no HOST (não executa script aqui)
        exec(
            `curl -s -X POST ${DEPLOY_SERVICE_URL}/internal-deploy -H "x-secret: ${process.env.DEPLOY_SECRET}"`,
            (err, stdout, stderr) => {
                if (err) {
                    console.error(stderr);
                    return res.status(500).send("Erro ao disparar deploy");
                }

                console.log(stdout);
                res.send("Deploy iniciado com sucesso");
            }
        );
    } catch (err) {
        console.error(err);
        res.status(500).send("Erro interno");
    }
});

app.get("/health", (req, res) => {
    res.send("OK");
});

app.listen(3000, () => {
    console.log("API rodando na porta 3000");
});