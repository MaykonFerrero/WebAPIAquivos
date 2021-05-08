# Web API Arquivos

## Execução
Para executar, abra a solução no Visual Studio, efetue a construção do projeto com o comando Build e em seguida rodar o projeto clicando no botão "IIS Express".

## Teste
1. Para utilizar a solução é preciso ter algum cliente REST instalado, como o Insomnia (https://insomnia.rest/download) ou Postman.
2. Crie uma requisição do tipo POST com o endereço http://localhost:5001/api/arquivo/upload
3. Selecione o tipo de dados como Multipart Form
4. Dê um nome ao campo e selecione o arquivo
5. Envie a requisição
6. O servidor deverá responder com código [200 OK] e exibir as informações sobre o arquivo.

![imagem exemplo](docs/utilizacao.png)

## Referências
Material consultado para a elaboração da solução:
- https://docs.microsoft.com/pt-br/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio
- http://www.macoratti.net/20/02/aspc_apimgup1.htm
- http://csharphelper.com/blog/2018/04/calculate-hash-codes-for-a-file-in-c/
- https://emn178.github.io/online-tools/sha256_checksum.html