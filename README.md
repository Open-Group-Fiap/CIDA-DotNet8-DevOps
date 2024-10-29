# CIDA-DotNet8

## Integrantes
- Cauã Alencar Rojas Romero - RM98638
- Jaci Teixeira Santos – RM99627
- Leonardo dos Santos Guerra - RM99738
- Maria Eduarda Ferreira da Mata - RM99004

## Como fazer o deploy
1. Adicionar o repositorio em uma pipeline do Azure Dev Ops
2. Criar uma pipeline de build
3. Utilizar o template do asp.net
4. adicionar um processo de token replace
5. Adicionar as seguintes variáveis de ambiente
    - azureAIConn (Url da Azure AI)
    - azureAIKey (Key da Azure AI)
    - azureConnString (Conn string da Azure SQL)
    - azureStoreConn (Conn string da Azure SQL)
    - geminiApiKey (Key da API do gemini)
6. Rodar o pipeline e gerar o artefato
7. Criar um webapp no Azure
8. Criar uma pipeline de deploy
9. Adicionar o artefato gerado no passo 6
10. Enviar o artefato para o webapp

## Testes
1. Usar o postman
2. Importar a Coleção que tem dentro do projeto
3. Clicar em cima da coleção 'CidaApi'
4. Clicar na Tab Variables
5. Alterar a variavel url para o url do deploy na Azure, sem a barra no final (No modelo que estava o localhost que vem por padrão)
6. Clicar com botão direito do mouse em cima do coleção 'CidaApi' e ir em Run Collection
7. Dentro de Run Configuration marcar a opção 'Persist responses for a session'
8. Clicar no botão 'Run CidaApi'
