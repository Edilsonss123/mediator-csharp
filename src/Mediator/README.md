
# Mediator

Uma implementação leve de **Mediator** em .NET, inspirada no padrão [Mediator](https://refactoring.guru/design-patterns/mediator), permitindo o uso de múltiplos handlers para um único request, que implementa Mediator-csharp-edilsonss123

## 🛠 Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## 📦 Instalação

### Instalar a implementação completa:
```bash
dotnet add package Mediator-csharp-edilsonss123 --version 1.0.9
```
Ou via NuGet:
```bash
nuget install Mediator-csharp-edilsonss123 -Version 1.0.9
```
## 🚀 Como Usar

### 1. Registrar o Mediator

Adicione o Mediator no seu `Startup.cs` ou `Program.cs`:
```csharp
using Mediator.Extensions;
using System.Reflection;

services.AddMediator(Assembly.GetExecutingAssembly());
```

### 2. Enviar uma Requisição

Crie um comando e envie uma requisição:
```csharp
var mediator = serviceProvider.GetRequiredService<IMediator>();

var request = new CreateAccountCommand { UserName = "testuser", Password = "testpassword" };
var result = await mediator.SendAsync(request);
```

### Exemplo Completo

Defina o comando e os handlers:
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
        return Task.FromResult($"{request.UserName} account created");
    }
}

public class NotifyCreateAccountHandler : IHandler<CreateAccountCommand, string>
{
    public Task<string> HandleAsync(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult("Notification sent");
    }
}
```

Resultado esperado:
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

- Suporte a múltiplos handlers por requisição.
- Registro automático via reflection com DI.
- Design simples, sem dependência de bibliotecas externas.
