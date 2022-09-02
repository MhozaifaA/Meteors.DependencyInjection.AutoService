// See https://aka.ms/new-console-template for more information
using Meteors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

Console.WriteLine("Hello, World!");

ServiceProvider _serviceProvider;
var service = new ServiceCollection();

//service.AddAutoService();

service.AddScoped<Class4>();

_serviceProvider = service.BuildServiceProvider();


using var scope = _serviceProvider.CreateScope();
var asd = scope.ServiceProvider.GetServices<Class4>();
object asda = asd.First();
if ((asda is Class4))
{
    var asdasd = 0;
}

foreach (object? obj in asd)
{
    if (obj is Class4 result)
    {
        int x = 0;
    }
}

var adasda = scope.ServiceProvider.GetServices<IClass>().OfType<Class4>();
scope.ServiceProvider.GetServices<IClass>().OfType<Class4>().First().Print();



//_serviceProvider.CompainTest<Class1>();


//_serviceProvider.CompainTest<Class2>();


//_serviceProvider.CompainTest<Class3>();


_serviceProvider.CompainTest<Class4>();


//_serviceProvider.CompainTest<Class5>();


//_serviceProvider.CompainTest<Class6>();


//_serviceProvider.CompainTest<Class7>();


//_serviceProvider.CompainTest<Class8>();





public interface IClass { 
    public string Hash { get; set; }
    public void Print();
}
public class Class 
{
    public virtual string Hash { get; set; }
    protected  string TypeName { get; set; }
    public Class(string typename) =>  (TypeName, Hash )= (typename , Guid.NewGuid().ToString());
    public virtual void Print() => Console.WriteLine( TypeName + ":" + Hash);
}

[AutoService]
public class Class1 : Class, IClass
{
    public Class1() : base(nameof(Class1)) { }
}

[AutoService(ServiceLifetime.Transient)]
public class Class2 : Class, IClass  
{
    public Class2() : base(nameof(Class2)) { }
}

[AutoService(ServiceLifetime.Singleton)]
public class Class3 : Class, IClass
{
    public Class3() : base(nameof(Class3)) { }
}


[AutoService(false)]
public class Class4 : Class, IClass
{
    public Class4() : base(nameof(Class4)) { }
}






public static class Extensions
{
    public static void CompainTest<TClass>(this ServiceProvider _serviceProvider) where TClass : Class
    {
        Console.ForegroundColor = ConsoleColor.Green;
        //Console.WriteLine($"{Environment.NewLine} {typeof(TClass).Name} use AutoService  {typeof(TClass).GetCustomAttributes(typeof(AutoServiceAttribute), false).Cast<AutoServiceAttribute>().Select(att => (att.LifetimeType, att.ImplementationType, att.UseImplementation)).Single()}");
        Console.ResetColor();

        _serviceProvider.TwiceTest<TClass>(); //test

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"In Scope");
        Console.ResetColor();

        _serviceProvider.TwiceScopeTest<TClass>(); //test scope
    }

    public static void TwiceTest<TClass>(this ServiceProvider _serviceProvider) where TClass : Class
    {
        _serviceProvider.GenerateService<TClass>();
        _serviceProvider.GenerateService<TClass>();
    }

    public static void TwiceScopeTest<TClass>(this ServiceProvider _serviceProvider) where TClass : Class
    {
        _serviceProvider.GenerateService<TClass>(provider => {
            provider.PrintService<TClass>();
            provider.PrintService<TClass>();
        });
    }

    public static void GenerateService<TClass>(this ServiceProvider _serviceProvider) where TClass : Class
    {
        using var scope = _serviceProvider.CreateScope();
        var ada = typeof(TClass);   
        var asd = scope.ServiceProvider.GetServices<TClass>();
        object asda = asd.First();
        if ( (asda is TClass ))
        {
            var asdasd = 0;
        }

        foreach (object? obj in asd)
        {
            if (obj is TClass result)
            {
                int x = 0;
            }
        }

        var adasda = scope.ServiceProvider.GetServices<IClass>().OfType<TClass>();
        scope.ServiceProvider.GetServices<IClass>().OfType<TClass>().First().Print();
    }


    public static void GenerateService<TClass>(this ServiceProvider _serviceProvider, Action<IServiceProvider> scopefunc) where TClass : Class
    {
        using var scope = _serviceProvider.CreateScope();
        scopefunc(scope.ServiceProvider);
    }

    private static void PrintService<TClass>(this IServiceProvider provider) where TClass : Class
        => provider.GetServices<IClass>().OfType<TClass>().First().Print();
}