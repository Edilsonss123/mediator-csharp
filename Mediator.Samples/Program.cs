using Mediator.Abstractions;
using Mediator.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var services = new ServiceCollection();
services.AddMediator(Assembly.GetExecutingAssembly());

services.AddTransient<AccountRepository>();

var servicesProvider = services.BuildServiceProvider();
var mediator = servicesProvider.GetRequiredService<IMediator>();

var request = new CreateAccountCommand { UserName = "testuser",    Password = "testpassword" };
var result = await mediator.SendAsync(request, CancellationToken.None);
Console.WriteLine(string.Join(Environment.NewLine, result));

public class AccountRepository
{
    public void Save()
    {
        Console.WriteLine("AccountRepository.Save()");
    }
}

public class CreateAccountCommand : IRequest<string>
{
    public string UserName { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
}

public class CreateAccountHandler(AccountRepository accountRepository) : IHandler<CreateAccountCommand, string>
{
    public Task<string> HandleAsync(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        accountRepository.Save();
        return Task.FromResult($"{request.UserName} account created");
    }
}

public class NotifyCreateAccountHandler() : IHandler<CreateAccountCommand, string>
{
    public Task<string> HandleAsync(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult($"NotifyCreateAccountHandler");
    }
}