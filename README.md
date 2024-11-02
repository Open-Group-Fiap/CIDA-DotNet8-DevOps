# CIDA-DotNet8

## Integrantes
- Cauã Alencar Rojas Romero - RM98638
- Jaci Teixeira Santos – RM99627
- Leonardo dos Santos Guerra - RM99738
- Maria Eduarda Ferreira da Mata - RM99004

## Link do Azure Boards
https://dev.azure.com/RM98638/Pipeline

## Link do web app
https://cida-api.azurewebsites.net

## Como fazer o deploy
1. Configurar o repositório em uma Pipeline do Azure DevOps
2. Criar uma Pipeline de Build
3. Implementar o template ASP.NET
4. Configurar o processo de Token Replace
5. Configurar no token replace o token como `${}`
6. Configurar as seguintes variáveis de ambiente:
    - azureAIConn (URL de conexão do Azure AI)
    - azureAIKey (Chave de acesso do Azure AI)
    - azureConnString (String de conexão do Azure SQL)
    - azureStoreConn (String de conexão do Azure Blob Storage)
    - geminiApiKey (Chave de acesso da API Gemini)
    
    *Observação: As variáveis devem estar dentro de aspas duplas `""`*

7. Executar a Pipeline e gerar o artefato
8. Criar um Web App no Azure
9. Criar uma Pipeline de Deploy
10. Importar o artefato gerado na etapa 6
11. Realizar o Deploy do artefato no Web App

## Testes
1. Substituir o arquivo appsettings.json localizado na pasta CIDA.Api com o arquivo de exemplo appsettings.json enviado junto a entrega
2. Usar o postman
3. Importar a Coleção que tem dentro do projeto
4. Clicar em cima da coleção 'CidaApi'
5. Clicar na Tab Variables
6. Alterar a variavel url para o url do deploy na Azure, sem a barra no final (No modelo que estava o localhost que vem por padrão)
7. Clicar com botão direito do mouse em cima do coleção 'CidaApi' e ir em Run Collection
8. Dentro de Run Configuration marcar a opção 'Persist responses for a session'
9. Clicar no botão 'Run CidaApi'
