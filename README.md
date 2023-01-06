# uCondo Hands On

Este projeto foi desenvolvido para a [uCondo](https://www.ucondo.com.br), como parte do processo seletivo para ser um [#condomaker](https://www.ucondo.com.br/blog/o-que-e-ser-condomaker-e-porque-isso-e-tao-importante-para-nos) (como carinhosamente são chamados os colaboradores da uCondo). 

Inclusive, tem mais vagas abertas lá no [Abler da uCondo](https://ucondo.abler.com.br), da uma conferida. ;)

A proposta do projeto é a criação de um CRUD de plano de contas, além de uma API que auxilia na criação da numeração do plano de contas filho. Se você não faz ideia do que é um plano de contas, o Conta Azul resumiu de forma didática [nesse link](https://ajuda.contaazul.com/hc/pt-br/articles/360019773271-O-que-%C3%A9-Plano-de-Contas-).

## Tecnologias

- C# 
- .Net Core 6
- Azure Sql Server

## Instruções de execução

Para facilitar a execução do projeto, ele já foi publicado no GitHub com uma string de conexão válida para um banco SQL Server no Azure. Dessa forma basta executá-lo em sua IDE preferida ou navegar até a pasta do projeto de API via terminal e executar o comando:

    dotnet run

Basta ter o SDK do .net core 6 instalado em sua máquina. Lembrando que o projeto é multiplataforma, podendo ser executado no Windows, Linux ou MacOS. 

Se você preferir utilizar um novo banco de dados, local ou em nuvem, basta alterar a string de conexão no arquivo  appsettings.Development.json, disponível no caminho:

    uCondo.HandsOn.API/appsettings.Development.json

    "ConnectionStrings": {
      "HandsOnDatabase": "sua_connection_string"
    },
  
O projeto utiliza Entity Framework com Code First, e a execução do Migration é automática. Ou seja, basta trocar a connection string e executar o projeto que as tabelas necessárias seram criadas automágicamente.

*Ps.: A string de conexão publicada estará disponível apenas para validação/apresentação do projeto, e depois o banco será apagado e a senha do servidor de banco de dados resetada.* 

## Testes unitários

O projeto também possui testes unitários, que também podem ser executados utilizando sua IDE preferida ou via terminal com o comando:

    dotnet test
Para o projeto de testes não é necessário se preocupar com o banco de dados, pois ele utiliza Entity Framework InMemory.

## Consumo dos endpoints

Ao executar o projeto, ele passa a aceitar requisições HTTP em um servidor local que responde no caminho:

    http://localhost:37058

Os endpoints disponíveis são:

    GET http://localhost:37058/api/accounts (listagem do plano de contas)
    POST http://localhost:37058/api/accounts (criação de nova conta)
    DELETE http://localhost:37058/api/accounts/<code> (exclusão de conta - e seus filhos)
    GET http://localhost:37058/api/accounts/<code>/next (sugestão de numeração do próximo filho)

Para a listagem do plano de contas também é possível realizar alguns filtros via query string:

    search=<name_or_code> (filtra por nome ou código)
    type=<income|expense> (filtra pelo tipo da conta)
    allowEntries=<true|false> (filtra por contas que aceitam ou não lançamentos)

Para a criação da conta, o seguinte JSON pode ser utilizado:

    {
       "code":  "1.5",
       "parentCode":  "1",
       "name":  "Vale Transporte",
       "type":  "<Income|Expense>",
       "allowEntries":  <true|false>
    }
