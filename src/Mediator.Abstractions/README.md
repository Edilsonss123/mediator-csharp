
# Mediator.Abstraction

**Mediator.Abstraction** é uma implementação leve do padrão **Mediator** em .NET, permitindo o uso de múltiplos handlers para um único request. Ele abstrai a comunicação entre componentes e facilita a organização do código.

## 🛠 Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## 📦 Instalação

### Instalar o pacote:

```bash
dotnet add package Mediator.Abstraction --version 1.0.0
```

Ou via **NuGet CLI**:

```bash
nuget install Mediator.Abstraction -Version 1.0.0
```

## 🚀 Como Usar

Para usar o **Mediator.Abstraction**, você precisa de outra biblioteca, como o [Mediator-csharp-edilsonss123](https://www.nuget.org/packages/Mediator-csharp-edilsonss123).

### 1. Registrar o Mediator

No seu `Program.cs` ou `Startup.cs`:

```csharp
using Mediator.Abstraction.Extensions;
using System.Reflection;

services.AddMediator(Assembly.GetExecutingAssembly());
```

### 2. Criar e Enviar um Comando

```csharp
var mediator = serviceProvider.GetRequiredService<IMediator>();

var request = new CreateAccountCommand { UserName = "testuser", Password = "testpassword" };
var result = await mediator.SendAsync(request);
```

## 🔧 Interfaces Principais

```csharp
public interface IRequest<TResponse> { }

public interface IHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IMediator
{
    Task<List<TResponse>> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
```

## 📚 Recursos

- Suporte a múltiplos handlers por requisição.
- Registro automático via reflexão com DI.
- Design simples, sem dependências externas além do .NET.
