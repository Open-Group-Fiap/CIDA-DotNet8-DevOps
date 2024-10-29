# Documento de Testes de API

Este documento tem como objetivo apresentar as funcionalidades que serão testadas na API, descrevendo cada teste. Para cada funcionalidade, estão especificados os objetivos, a importância dos testes e as verificações esperadas. A realização desses testes é essencial para assegurar que todos os endpoints se comportem conforme o esperado em diferentes cenários.


## TESTES ARQUIVOS
### Funcionalidades Testadas

1. **PostArquivo_ReturnsArquivo**  
2. **PutArquivo_ReturnsArquivo_WhenArquivoExists**  
3. **DeleteArquivo_ReturnsNoContent_WhenArquivoExists**  
4. **GetArquivoByEmail_ReturnsArquivo_WhenArquivoExists**  
5. **GetArquivoById_ReturnsArquivo_WhenArquivoExists**  
6. **GetArquivoByIdUsuario_ReturnsArquivo_WhenArquivoExists**  
7. **GetArquivosSearch_ReturnsArquivo_WhenArquivoExists**  
8. **PostArquivo_ReturnsBadRequest_WhenSendRandomJson**


### Explicação e Importância de Cada Teste

#### 1. PostArquivo_ReturnsArquivo
- **Objetivo:** Verificar se o upload de um arquivo é realizado corretamente.
- **Importância:** Este teste assegura que a API consegue receber e processar um arquivo enviado via `POST`. Isso é fundamental para garantir o fluxo de upload de documentos, uma funcionalidade essencial na aplicação.
- **Verificação:** Certifica que o status de resposta é `201 Created` e que o arquivo criado não é nulo, garantindo que o processo de upload foi bem-sucedido.

#### 2. PutArquivo_ReturnsArquivo_WhenArquivoExists
- **Objetivo:** Verificar se é possível atualizar um arquivo existente.
- **Importância:** Esse teste garante que a funcionalidade de atualização de arquivos funciona corretamente, permitindo modificações nos dados de arquivos existentes.
- **Verificação:** O teste espera um status de sucesso e que os dados retornados estejam corretos, comparando o arquivo atualizado com os dados enviados.

#### 3. DeleteArquivo_ReturnsNoContent_WhenArquivoExists
- **Objetivo:** Verificar se a API permite deletar um arquivo existente.
- **Importância:** A exclusão de arquivos é uma funcionalidade essencial. Este teste garante que, ao deletar um arquivo, o status de sucesso `204 No Content` é retornado, validando que a remoção foi concluída com sucesso.
- **Verificação:** O status `204 No Content` é esperado para confirmar que o arquivo foi deletado.

#### 4. GetArquivoByEmail_ReturnsArquivo_WhenArquivoExists
- **Objetivo:** Verificar se a API retorna arquivos associados a um e-mail específico.
- **Importância:** Esse teste valida a capacidade da API de buscar arquivos com base em um critério específico (e-mail), o que pode ser essencial para a pesquisa de documentos.
- **Verificação:** O teste verifica se a resposta contém os arquivos correspondentes, validando que o sistema de busca por e-mail está funcional.

#### 5. GetArquivoById_ReturnsArquivo_WhenArquivoExists
- **Objetivo:** Verificar se a API retorna um arquivo específico pelo seu ID.
- **Importância:** Garante que a funcionalidade de busca por ID está funcionando corretamente, permitindo a recuperação de arquivos específicos de maneira direta.
- **Verificação:** O teste espera que o arquivo retornado não seja nulo, confirmando que o arquivo foi encontrado e recuperado com sucesso.

#### 6. GetArquivoByIdUsuario_ReturnsArquivo_WhenArquivoExists
- **Objetivo:** Verificar se a API retorna arquivos associados a um ID de usuário.
- **Importância:** A pesquisa por arquivos relacionados a um usuário é uma funcionalidade central em muitos sistemas de armazenamento. Este teste valida a integridade dessa operação.
- **Verificação:** O teste assegura que a resposta contém os arquivos associados ao usuário, garantindo a correção dessa funcionalidade.

#### 7. GetArquivosSearch_ReturnsArquivo_WhenArquivoExists
- **Objetivo:** Verificar se a API retorna arquivos com base em uma pesquisa genérica.
- **Importância:** Permite testar a funcionalidade de busca por arquivos sem critérios específicos, garantindo que a pesquisa genérica retorna resultados esperados.
- **Verificação:** O teste valida se a resposta contém arquivos, confirmando que a busca está operando corretamente.

#### 8. PostArquivo_ReturnsBadRequest_WhenSendRandomJson
- **Objetivo:** Verificar se a API retorna um erro ao receber um payload inválido no upload de arquivos.
- **Importância:** Esse teste garante que a API está validando adequadamente os dados recebidos e retorna o status correto (`415 Unsupported Media Type`) quando um formato inadequado é enviado.
- **Verificação:** A validação do retorno de erro é crucial para evitar que a API processe entradas inválidas, melhorando a segurança e robustez da aplicação.

---

## TESTES INSIGHTS
### Funcionalidades Testadas

1. **PutInsight_ReturnsInsight_WhenInsightExists**  
2. **GetInsightById_ReturnsInsight_WhenInsightExists**  
3. **GetInsightByEmail_ReturnsInsight_WhenInsightExists**  
4. **DeleteInsight_ReturnsNoContent_WhenInsightExists**  
5. **GetInsightsByUsuarioEmail_ReturnsInsight_WhenInsightExists**  
6. **GetInsightsSearch_ReturnsInsight_WhenInsightExists**  
7. **PutInsight_ReturnsBadRequest_WhenUsuarioNotExists**  
8. **PutInsight_ReturnsBadRequest_WhenResumoNotExists**  
9. **PutInsight_ReturnsNotFound_WhenInsightNotExists**  
10. **PutInsight_ReturnsBadRequest_WhenResumoIsAlreadyAssigned**  
11. **PutInsight_ReturnsBadRequest_WhenSendRandomJson**


### Explicação e Importância de Cada Teste

#### 1. PutInsight_ReturnsInsight_WhenInsightExists
- **Objetivo:** Verificar se a atualização de um insight existente é realizada corretamente.
- **Importância:** Garante que a funcionalidade de atualização de insights permite modificar os dados de um insight existente e retorna o objeto atualizado corretamente.
- **Verificação:** O teste assegura que o status de resposta é `200 OK` e que o insight retornado contém os dados atualizados.

#### 2. GetInsightById_ReturnsInsight_WhenInsightExists
- **Objetivo:** Verificar se é possível buscar um insight por ID.
- **Importância:** Valida a funcionalidade de busca de insights por um identificador único, garantindo que insights específicos possam ser recuperados.
- **Verificação:** O teste verifica se a resposta não é nula, confirmando que o insight foi encontrado com sucesso.

#### 3. GetInsightByEmail_ReturnsInsight_WhenInsightExists
- **Objetivo:** Verificar se a API retorna insights associados a um e-mail de usuário.
- **Importância:** Valida a busca de insights com base no e-mail, uma funcionalidade útil para listar insights específicos de um usuário.
- **Verificação:** O teste assegura que o insight retornado corresponde ao e-mail fornecido.

#### 4. DeleteInsight_ReturnsNoContent_WhenInsightExists
- **Objetivo:** Verificar se é possível deletar um insight existente.
- **Importância:** Garante que a funcionalidade de exclusão de insights está operando corretamente e que o insight foi removido do sistema.
- **Verificação:** O status de resposta `204 No Content` confirma que o insight foi excluído com sucesso.

#### 5. GetInsightsByUsuarioEmail_ReturnsInsight_WhenInsightExists
- **Objetivo:** Verificar se a API retorna insights associados a um e-mail de usuário.
- **Importância:** Garante que a API pode retornar insights baseados no e-mail de um usuário, essencial para buscas baseadas em usuários específicos.
- **Verificação:** O teste assegura que insights associados ao usuário são retornados corretamente.

#### 6. GetInsightsSearch_ReturnsInsight_WhenInsightExists
- **Objetivo:** Verificar se a API retorna insights com base em uma pesquisa genérica.
- **Importância:** Testa a funcionalidade de busca geral de insights, garantindo que a pesquisa retorne resultados esperados com base em critérios gerais.
- **Verificação:** O teste valida que insights são retornados corretamente pela pesquisa.

#### 7. PutInsight_ReturnsBadRequest_WhenUsuarioNotExists
- **Objetivo:** Verificar se a API retorna um erro ao tentar atualizar um insight com um usuário inexistente.
- **Importância:** Garante que a aplicação valida corretamente se o usuário existe antes de permitir a atualização de um insight, prevenindo operações inválidas.
- **Verificação:** O teste assegura que a resposta contém o status `400 Bad Request` e a mensagem "Usuário não encontrado".

#### 8. PutInsight_ReturnsBadRequest_WhenResumoNotExists
- **Objetivo:** Verificar se a API retorna um erro ao tentar atualizar um insight com um resumo inexistente.
- **Importância:** Valida a presença de um resumo antes de permitir a atualização, garantindo que as associações de dados sejam consistentes.
- **Verificação:** O status `400 Bad Request` e a mensagem "Resumo não encontrado" são esperados.

#### 9. PutInsight_ReturnsNotFound_WhenInsightNotExists
- **Objetivo:** Verificar se a API retorna `404 Not Found` ao tentar atualizar um insight inexistente.
- **Importância:** Garante que a aplicação trate corretamente a tentativa de atualizar insights que não existem no sistema.
- **Verificação:** O teste espera um status de `404 Not Found`, indicando que o insight não foi encontrado.

#### 10. PutInsight_ReturnsBadRequest_WhenResumoIsAlreadyAssigned
- **Objetivo:** Verificar se a API retorna um erro ao tentar associar um resumo que já está vinculado a outro insight.
- **Importância:** Assegura que o sistema valida adequadamente a associação de resumos, evitando duplicações e conflitos.
- **Verificação:** O status `400 Bad Request` e a mensagem "Já existe um insight para esse resumo" são esperados.

#### 11. PutInsight_ReturnsBadRequest_WhenSendRandomJson
- **Objetivo:** Verificar se a API retorna um erro ao receber dados inválidos ao tentar atualizar um insight.
- **Importância:** Garante que a API valida os dados recebidos e retorna o erro correto (`400 Bad Request`) para entradas inválidas.
- **Verificação:** O status `400 Bad Request` é esperado, confirmando que a API rejeita dados não conformes.

---

## TESTES LOGIN
### Funcionalidades Testadas

1. **PostLogin_ReturnsUnauthorized_WhenEmailAndSenhaAreInvalid**  
2. **PostLogin_ReturnsOk_WhenEmailAndSenhaAreValid**


### Explicação e Importância de Cada Teste

#### 1. PostLogin_ReturnsUnauthorized_WhenEmailAndSenhaAreInvalid
- **Objetivo:** Verificar se a API retorna o status correto quando credenciais inválidas são fornecidas.
- **Importância:** Este teste é essencial para garantir que o sistema de autenticação rejeita tentativas de login com e-mail ou senha incorretos. É fundamental para manter a segurança da aplicação, evitando acessos não autorizados.
- **Verificação:** O teste espera o status `401 Unauthorized` quando o e-mail e a senha são inválidos, confirmando que o sistema não permite acesso indevido.

#### 2. PostLogin_ReturnsOk_WhenEmailAndSenhaAreValid
- **Objetivo:** Verificar se a API permite o login com credenciais válidas.
- **Importância:** Este teste valida que usuários com e-mail e senha corretos conseguem realizar login com sucesso, permitindo acesso às funcionalidades protegidas da aplicação.
- **Verificação:** O teste verifica que o status de resposta é `200 OK`, assegurando que o login foi realizado corretamente com credenciais válidas.

---

## TESTES RESUMO
### Funcionalidades Testadas

1. **PutResumo_ReturnsResumo_WhenResumoExists**  
2. **DeleteResumo_ReturnsNoContent_WhenResumoExists**  
3. **GetResumosByUsuarioEmail_ReturnsResumo_WhenResumoExists**  
4. **GetResumosSearch_ReturnsResumo_WhenResumoExists**  
5. **GetResumoById_ReturnsResumo_WhenResumoExists**  
6. **PutResumo_ReturnsNotFound_WhenResumoNotExists**  
7. **PutResumo_ReturnsBadRequestResumo_WhenUsuarioNotExists**  
8. **PutResumo_ReturnsBadRequest_WhenSendRandomJson**  


### Explicação e Importância de Cada Teste

#### 1. PutResumo_ReturnsResumo_WhenResumoExists
- **Objetivo:** Verificar se a API retorna um resumo atualizado quando o ID do resumo existe.
- **Importância:** Este teste é fundamental para garantir que a funcionalidade de atualização de resumos está operando corretamente, permitindo modificações nos dados de resumos existentes.
- **Verificação:** O teste assegura que o status de resposta é `200 OK`, e que o resumo retornado contém a descrição atualizada.

#### 2. DeleteResumo_ReturnsNoContent_WhenResumoExists
- **Objetivo:** Verificar se a API permite a exclusão de um resumo existente.
- **Importância:** Este teste valida que a exclusão de resumos está funcionando corretamente, essencial para a gestão de dados na aplicação.
- **Verificação:** O status `204 No Content` é esperado, confirmando que o resumo foi deletado com sucesso.

#### 3. GetResumosByUsuarioEmail_ReturnsResumo_WhenResumoExists
- **Objetivo:** Verificar se a API retorna resumos associados a um e-mail específico.
- **Importância:** Este teste valida a capacidade da API de buscar resumos com base em um critério (e-mail), essencial para facilitar a pesquisa de informações.
- **Verificação:** O teste verifica se a resposta contém o resumo, confirmando que a busca está funcionando corretamente.

#### 4. GetResumosSearch_ReturnsResumo_WhenResumoExists
- **Objetivo:** Verificar se a API retorna resumos em uma pesquisa genérica.
- **Importância:** Testa a funcionalidade de busca de resumos, garantindo que a pesquisa retorne resultados esperados.
- **Verificação:** O teste assegura que a resposta contém resumos, validando a funcionalidade de busca.

#### 5. GetResumoById_ReturnsResumo_WhenResumoExists
- **Objetivo:** Verificar se a API retorna um resumo específico pelo seu ID.
- **Importância:** Garante que a funcionalidade de busca por ID está funcionando corretamente, permitindo a recuperação direta de resumos específicos.
- **Verificação:** O teste espera que o resumo retornado não seja nulo, confirmando que o resumo foi encontrado.

#### 6. PutResumo_ReturnsNotFound_WhenResumoNotExists
- **Objetivo:** Verificar se a API retorna o status correto quando o ID do resumo não existe.
- **Importância:** Este teste é essencial para garantir que a aplicação não permita atualizações em resumos inexistentes, melhorando a integridade dos dados.
- **Verificação:** O teste espera o status `404 Not Found`, indicando que o resumo não foi encontrado.

#### 7. PutResumo_ReturnsBadRequestResumo_WhenUsuarioNotExists
- **Objetivo:** Verificar se a API retorna um erro ao tentar atualizar um resumo com um usuário que não existe.
- **Importância:** Garante que a lógica de negócios da aplicação está validando corretamente os dados antes de processá-los.
- **Verificação:** O teste espera o status `400 Bad Request`, confirmando que a requisição não foi aceita devido à inexistência do usuário.

#### 8. PutResumo_ReturnsBadRequest_WhenSendRandomJson
- **Objetivo:** Verificar se a API retorna um erro ao receber um payload inválido para atualização de resumo.
- **Importância:** Este teste assegura que a API está validando adequadamente as entradas, prevenindo o processamento de dados inválidos.
- **Verificação:** O teste espera o status `400 Bad Request`, indicando que a requisição não foi aceita devido ao formato inadequado do JSON.

---

## TESTES USUÁRIO
### Funcionalidades Testadas

1. **PostUsuario_ReturnsUsuario**
2. **PutUsuario_ReturnsUsuario_WhenUsuarioExists**
3. **GetUsuarioById_ReturnsUsuario_WhenUsuarioExists**
4. **GetUsuarioByEmail_ReturnsUsuario_WhenUsuarioExists**
5. **DeleteUsuario_ReturnsNoContent_WhenUsuarioExists**
6. **PostUsuario_ReturnsBadRequest_WhenTipoDocumentoIsInvalid**
7. **PostUsuario_ReturnsBadRequest_WhenNumDocumentoIsAlreadyAssigned**
8. **PostUsuario_ReturnsBadRequest_WhenEmailIsAlreadyAssigned**
9. **PutUsuario_ReturnsBadRequest_WhenTipoDocumentoIsInvalid**
10. **PutUsuario_ReturnsNotFound_WhenUsuarioDoesNotExist**
11. **PutUsuario_ReturnsBadRequest_WhenNumDocumentoIsAlreadyAssigned**
12. **PutUsuario_ReturnsBadRequest_WhenEmailIsAlreadyAssigned**
13. **PutUsuario_ReturnsBadRequest_WhenSendRandomJson**
14. **PostUsuario_ReturnsBadRequest_WhenSendRandomJson**

### Explicação e Importância de Cada Teste

#### 1. PostUsuario_ReturnsUsuario
- **Objetivo:** Verificar se a criação de um novo usuário retorna um objeto de usuário válido.
- **Importância:** Este teste garante que o endpoint de criação (`POST /usuario`) funcione corretamente, permitindo a adição de novos usuários ao sistema.
- **Verificação:** Espera-se que a resposta contenha um usuário criado e um status `201 Created`.

#### 2. PutUsuario_ReturnsUsuario_WhenUsuarioExists
- **Objetivo:** Testar a atualização de um usuário existente.
- **Importância:** Assegura que a atualização de um usuário funcione corretamente, mantendo a integridade dos dados no sistema.
- **Verificação:** O teste verifica se o usuário atualizado é retornado com status `200 OK`.

#### 3. GetUsuarioById_ReturnsUsuario_WhenUsuarioExists
- **Objetivo:** Testar a recuperação de um usuário pelo ID.
- **Importância:** Confirma que a busca de um usuário por ID está operando corretamente, permitindo a consulta de informações de usuários específicos.
- **Verificação:** O teste espera que um usuário válido seja retornado e que a resposta não seja nula.

#### 4. GetUsuarioByEmail_ReturnsUsuario_WhenUsuarioExists
- **Objetivo:** Testar a busca de um usuário pelo e-mail.
- **Importância:** Avalia se a funcionalidade de busca por e-mail está implementada corretamente, permitindo que usuários sejam localizados de forma eficiente.
- **Verificação:** Espera-se que o usuário correspondente ao e-mail informado seja retornado.

#### 5. DeleteUsuario_ReturnsNoContent_WhenUsuarioExists
- **Objetivo:** Testar a exclusão de um usuário existente.
- **Importância:** Garante que a remoção de usuários funcione como esperado, permitindo a manutenção adequada dos dados.
- **Verificação:** O teste espera um status `204 No Content` após a exclusão.

#### 6. PostUsuario_ReturnsBadRequest_WhenTipoDocumentoIsInvalid
- **Objetivo:** Testar a criação de um usuário com tipo de documento inválido.
- **Importância:** Verifica se a validação de dados está sendo aplicada corretamente ao criar usuários, garantindo que apenas dados válidos sejam aceitos.
- **Verificação:** O teste deve retornar um status `400 Bad Request`.

#### 7. PostUsuario_ReturnsBadRequest_WhenNumDocumentoIsAlreadyAssigned
- **Objetivo:** Testar a criação de um usuário com número de documento duplicado.
- **Importância:** Garante que o sistema não permita a duplicação de números de documentos, respeitando as regras de unicidade.
- **Verificação:** Espera-se um `400 Bad Request` ao tentar criar um usuário com um número de documento já existente.

#### 8. PostUsuario_ReturnsBadRequest_WhenEmailIsAlreadyAssigned
- **Objetivo:** Testar a criação de um usuário com e-mail já cadastrado.
- **Importância:** Assegura que o sistema mantenha a unicidade dos e-mails, prevenindo conflitos de conta.
- **Verificação:** O teste deve retornar um `400 Bad Request`.

#### 9. PutUsuario_ReturnsBadRequest_WhenTipoDocumentoIsInvalid
- **Objetivo:** Testar a atualização de um usuário com tipo de documento inválido.
- **Importância:** Verifica se a validação de dados se aplica também nas atualizações, mantendo a integridade dos dados.
- **Verificação:** O teste deve retornar um `400 Bad Request` para dados inválidos.

#### 10. PutUsuario_ReturnsNotFound_WhenUsuarioDoesNotExist
- **Objetivo:** Testar a atualização de um usuário que não existe.
- **Importância:** Garante que o sistema retorne um erro apropriado quando se tenta atualizar um usuário inexistente, ajudando a evitar erros na aplicação.
- **Verificação:** O teste deve retornar um `404 Not Found`.

#### 11. PutUsuario_ReturnsBadRequest_WhenNumDocumentoIsAlreadyAssigned
- **Objetivo:** Testar a atualização de um usuário com número de documento já existente.
- **Importância:** Garante que a validação para duplicatas se aplique durante as atualizações, evitando conflitos de dados.
- **Verificação:** Espera-se um `400 Bad Request`.

#### 12. PutUsuario_ReturnsBadRequest_WhenEmailIsAlreadyAssigned
- **Objetivo:** Testar a atualização de um usuário com e-mail já existente.
- **Importância:** Assegura que o sistema mantenha a unicidade dos e-mails, mesmo durante as atualizações.
- **Verificação:** O teste deve retornar um `400 Bad Request`.

#### 13. PutUsuario_ReturnsInternalServerError_WhenSendRandomJson
- **Objetivo:** Testar a atualização de um usuário enviando um JSON aleatório.
- **Importância:** Avalia como o sistema lida com entradas inesperadas, garantindo a robustez da aplicação.
- **Verificação:** O teste deve retornar um `500 Internal Server Error`.

#### 14. PostUsuario_ReturnsInternalServerError_WhenSendRandomJson
- **Objetivo:** Testar a criação de um usuário com um JSON aleatório.
- **Importância:** Confirma que o sistema não falha ao receber dados inválidos, mantendo a estabilidade da aplicação.
- **Verificação:** Espera-se um `500 Internal Server Error`.



