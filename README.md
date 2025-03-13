# Projeto Minimal API

Este projeto é uma API minimalista desenvolvida em .NET 8, utilizando C# 12.0. A API fornece funcionalidades para gerenciar administradores e veículos, além de autenticação.

## Funcionalidades

- **Autenticação**
  - Login de administradores
  - Geração de token JWT

- **Administradores**
  - Criação de novos administradores
  - Listagem de administradores
  - Consulta de administrador por ID

- **Veículos**
  - Criação de novos veículos
  - Listagem de veículos
  - Consulta de veículo por ID

## Tecnologias Utilizadas

- .NET 8
- C# 12.0
- ASP.NET Core
- Entity Framework Core
- JWT para autenticação
- Xunit para testes

## Estrutura do Projeto

- **API/Controllers**: Contém os controladores da API.
- **API/Domain/DTOs**: Contém os Data Transfer Objects.
- **API/Domain/Entities**: Contém as entidades do domínio.
- **API/Domain/Interfaces**: Contém as interfaces dos serviços.
- **API/Domain/ModelViews**: Contém as model views.
- **API/Domain/Services**: Contém as implementações dos serviços.
- **API**: Contém a configuração inicial da aplicação (Startup, Program).


   
