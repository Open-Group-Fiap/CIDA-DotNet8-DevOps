# CIDA-DotNet8

## Integrantes
- Cauã Alencar Rojas Romero - RM98638
- Jaci Teixeira Santos – RM99627
- Leonardo dos Santos Guerra - RM99738
- Maria Eduarda Ferreira da Mata - RM99004

## Como fazer o deploy
1. Usar a IDE Rider da JetBrains
2. Alterar as connections string no arquivo appsettings.json (As connections strings foram enviadas na entrega da sprint)
3. Baixar a extensão Azure Explorer
4. Abrir a extensão e autenticar com a sua conta e escolher a subscription desejada
5. Dentro da extensão clicar em Azure
6. Ir na opção de Web Apps e clicar com o botão direito do mouse
7. Escolher a opções 'Create'
8. Alterar o Project para CIDA.Api
9. Ir no símbolo de '+' no campo 'Web App'
10. Criar um resource group e escolher um nome para o Web App
11. Clicar na opção 'More settings'
12. Escolher o sistema operacional (Recomendo Linux)
13. Escolher a região (Recomendo Brazil South)
14. Criar um novo plano de serviço, clicar no símbolo '+'
15. Escolher um nome e definir o Pricing Tier (Recomendo Free F1)
16. Na tela atual clicar em OK
17. Irá voltar para a tela anterior, ative na opção 'Open browser after deplyment'
18. Clique em Apply depois OK
19. Espere o Deploy que logo depois irá abrir no site

## Testes
1. Usar o postman
2. Importar a Coleção que tem dentro do projeto
3. Clicar em cima da coleção 'CidaApi'
4. Clicar na Tab Variables
5. Alterar a variavel url para o url do deploy na Azure, sem a barra no final (No modelo que estava o localhost que vem por padrão)
6. Clicar com botão direito do mouse em cima do coleção 'CidaApi' e ir em Run Collection
7. Dentro de Run Configuration marcar a opção 'Persist responses for a session'
8. Clicar no botão 'Run CidaApi'