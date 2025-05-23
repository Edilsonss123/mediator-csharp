# Mediator

Um sistema leve de **Mediator** em .NET inspirado no padrão [Mediator](https://refactoring.guru/design-patterns/mediator), com suporte a múltiplos handlers para um único request.

## 🛠 Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## 📦 Estrutura do Projeto

- **src/Mediator**: Implementação principal do mediator.
- **src/Mediator.Abstractions**: Interfaces e contratos (como `IMediator`, `IRequest<>`, `IHandler<,>`).
- **Mediator.Samples**: Exemplo de uso prático, incluindo registro via DI, envio de comandos e múltiplos handlers.

## 📦 Instalação

### Instalar implementação completa:
```bash
dotnet add package Mediator-csharp-edilsonss123 --version 1.0.9
```
Ou via NuGet:
```bash
nuget install Mediator-csharp-edilsonss123 -Version 1.0.9
```

### Instalar apenas as abstrações (útil apenas se for consumir sem implementar):
```bash
dotnet add package Mediator.Abstractions-csharp-edilsonss123 --version 1.0.9
```
Ou via NuGet:
```bash
nuget install Mediator-csharp-edilsonss123 -Version 1.0.9
```
> ⚠️ O pacote `Mediator-csharp-edilsonss123` **já inclui as abstrações**, você só precisa do pacote separado se estiver apenas **definindo contratos** em outra solução.

## 🚀 Como Usar

### 1. Registrar o Mediator


```csharp
using Mediator.Extensions;
using System.Reflection;

services.AddMediator(Assembly.GetExecutingAssembly());
```

Esse método registra automaticamente todos os handlers que implementam `IHandler<TRequest, TResponse>` encontrados nos assemblies especificados.

### 2. Enviar uma Requisição

```csharp
var mediator = serviceProvider.GetRequiredService<IMediator>();

var request = new CreateAccountCommand { UserName = "testuser", Password = "testpassword" };
var result = await mediator.SendAsync(request);
```

Todos os handlers registrados para o tipo de requisição serão executados, e seus resultados retornados como uma `List<TResponse>`.

## 🧩 Exemplo Completo

```csharp
public class CreateAccountCommand : IRequest<string>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class CreateAccountHandler : IHandler<CreateAccountCommand, string>
{
    public Task<string> HandleAsync(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        // lógica para criar conta
        return Task.FromResult($"{request.UserName} account created");
    }
}

public class NotifyCreateAccountHandler : IHandler<CreateAccountCommand, string>
{
    public Task<string> HandleAsync(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        // lógica de notificação
        return Task.FromResult("Notification sent");
    }
}
```

### Resultado esperado

```bash
AccountRepository.Save()
testuser account created
Notification sent
```

## 🔧 Interfaces Principais

```csharp
public interface IRequest<TResponse> { }

public interface IHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IMediator
{
    Task<List<TResponse>> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
```

## 📚 Recursos

- Suporte a múltiplos handlers por requisição
- Registro automático via reflection com DI
- Design simples, sem dependência de bibliotecas externas