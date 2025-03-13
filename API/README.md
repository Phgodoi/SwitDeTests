# Projeto Minimal API

Este projeto � uma API minimalista desenvolvida em .NET 8, utilizando C# 12.0. A API fornece funcionalidades para gerenciar administradores e ve�culos, al�m de autentica��o.

## Funcionalidades

- **Autentica��o**
  - Login de administradores
  - Gera��o de token JWT

- **Administradores**
  - Cria��o de novos administradores
  - Listagem de administradores
  - Consulta de administrador por ID

- **Ve�culos**
  - Cria��o de novos ve�culos
  - Listagem de ve�culos
  - Consulta de ve�culo por ID

## Tecnologias Utilizadas

- .NET 8
- C# 12.0
- ASP.NET Core
- Entity Framework Core
- JWT para autentica��o
- Xunit para testes

## Estrutura do Projeto

- **API/Controllers**: Cont�m os controladores da API.
- **API/Domain/DTOs**: Cont�m os Data Transfer Objects.
- **API/Domain/Entities**: Cont�m as entidades do dom�nio.
- **API/Domain/Interfaces**: Cont�m as interfaces dos servi�os.
- **API/Domain/ModelViews**: Cont�m as model views.
- **API/Domain/Services**: Cont�m as implementa��es dos servi�os.
- **API**: Cont�m a configura��o inicial da aplica��o (Startup, Program).


   