# CIDA-DotNet8

## Integrantes
- Cauã Alencar Rojas Romero - RM98638
- Jaci Teixeira Santos – RM99627
- Leonardo dos Santos Guerra - RM99738
- Maria Eduarda Ferreira da Mata - RM99004

## Como fazer o deploy
1. Buildar o Dockerfile:
   ```
   docker build -t cida-api-csharp .
   ```

2. Fazer push para o Docker Hub:
   ```
   docker push seu-usuario/cida-api-csharp:latest
   ```

3. Abrir o Azure App Services:
   - Acesse o portal do Azure (https://portal.azure.com)
   - Navegue até "App Services"
   - Clique em "Create" ou "Add" para criar um novo App Service

4. Configurar o deploy do container:
   - Na seção "Publish", selecione "Container"
   - Escolha "Docker Hub" como Image Source
   - Insira o nome da sua imagem: `seu-usuario/cida-api-csharp:latest`

5. Configurar as variáveis de ambiente:
   - Na seção "Settings" do seu App Service, adicione as seguintes Connection Strings em Enviroment Variables:
     - Chave: `ConnectionStrings__AzureSQLConnection`
       Tipo: SQLAzure
       Valor: [Valor fornecido na entrega]
     - Chave: `ConnectionStrings__AzureStoreConnection`
       Tipo: Custom
       Valor: [Valor fornecido na entrega]

6. Finalizar a criação do App Service e aguardar o deploy ser concluído.

Nota: Os valores exatos para as variáveis de ambiente `ConnectionStrings__AzureSQLConnection` e `ConnectionStrings__AzureStoreConnection` foram fornecidos junto com a entrega do projeto.

[Link na Azure](cida-api.azurewebsites.net)

## Testes
1. Usar o postman
2. Importar a Coleção que tem dentro do projeto
3. Clicar em cima da coleção 'CidaApi'
4. Clicar na Tab Variables
5. Alterar a variavel url para o url do deploy na Azure, sem a barra no final (No modelo que estava o localhost que vem por padrão)
6. Clicar com botão direito do mouse em cima do coleção 'CidaApi' e ir em Run Collection
7. Dentro de Run Configuration marcar a opção 'Persist responses for a session'
8. Clicar no botão 'Run CidaApi'
