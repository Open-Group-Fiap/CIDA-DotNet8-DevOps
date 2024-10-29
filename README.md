# CIDA-DotNet8

## Integrantes
- Cauã Alencar Rojas Romero - RM98638
- Jaci Teixeira Santos – RM99627
- Leonardo dos Santos Guerra - RM99738
- Maria Eduarda Ferreira da Mata - RM99004

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
    - azureStoreConn (String de conexão do Azure SQL)
    - geminiApiKey (Chave de acesso da API Gemini)
    *Observação: As variáveis devem estar dentro de aspas duplas `""`*
7. Executar a Pipeline e gerar o artefato
8. Criar um Web App no Azure
9. Criar uma Pipeline de Deploy
10. Importar o artefato gerado na etapa 6
11. Realizar o Deploy do artefato no Web App

## Testes
1. Usar o postman
2. Importar a Coleção que tem dentro do projeto
3. Clicar em cima da coleção 'CidaApi'
4. Clicar na Tab Variables
5. Alterar a variavel url para o url do deploy na Azure, sem a barra no final (No modelo que estava o localhost que vem por padrão)
6. Clicar com botão direito do mouse em cima do coleção 'CidaApi' e ir em Run Collection
7. Dentro de Run Configuration marcar a opção 'Persist responses for a session'
8. Clicar no botão 'Run CidaApi'
