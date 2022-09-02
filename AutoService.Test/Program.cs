// See https://aka.ms/new-console-template for more information
using Meteors;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

ServiceProvider _serviceProvider;
var service = new ServiceCollection();

service.AddAutoService();

_serviceProvider = service.BuildServiceProvider();

_serviceProvider.GetService(typeof(IClass1));  

public class MainClass
{
    private readonly IClass1 class1;

    public MainClass(IClass1 class1)
    {
        Console.WriteLine("hello");
        this.class1 = class1;
    }
}


[AutoService]
public class Class1 : IClass1
{
    public Class1()
    {
        Console.WriteLine("hellasdo");

    }
}


public interface IClass1
{

}