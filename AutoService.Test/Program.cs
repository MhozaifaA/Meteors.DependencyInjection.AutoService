// See https://aka.ms/new-console-template for more information
using Meteors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

Console.WriteLine("Hello, World!");

ServiceProvider _serviceProvider;
var service = new ServiceCollection();

service.AddAutoService();

_serviceProvider = service.BuildServiceProvider();



_serviceProvider.CompainTest<Class>();


_serviceProvider.CompainTest<Class1>();


_serviceProvider.CompainTest<Class2>();


_serviceProvider.CompainTest<Class3>();


_serviceProvider.CompainTest<Class4>(true);


_serviceProvider.CompainTest<Class5>(true);


_serviceProvider.CompainTest<Class6>(true);


_serviceProvider.CompainTest<Class7>(true);


try
{
  //  _serviceProvider.CompainTest<Class8>(); //throw right as expected
}
finally{}


_serviceProvider.CompainTest<Class9>();


_serviceProvider.CompainTest<Class10>();


_serviceProvider.CompainTest<Class11>(true);


_serviceProvider.CompainTest<Class12>(true);


_serviceProvider.CompainTest<Class13>();

try
{
//_serviceProvider.CompainTest<Class14>();
}
finally { }

_serviceProvider.CompainTest<Class15>(true);














public interface IClass
{
    public string Hash { get; set; }
    public void Print();
}
public class ClassBase
{
    public virtual string Hash { get; set; }
    protected string TypeName { get; set; }
    public ClassBase(string typename) => (TypeName, Hash) = (typename, Guid.NewGuid().ToString());
    public virtual void Print() => Console.WriteLine(TypeName + ":" + Hash);
}

//................

[AutoService]
public class Class : ClassBase, IClass
{
    public Class() : base(nameof(Class)) { }
}

//.......

[AutoService]
public class Class1 : ClassBase, IClass
{
    public Class1() : base(nameof(Class1)) { }
}

[AutoService(ServiceLifetime.Transient)]
public class Class2 : ClassBase, IClass
{
    public Class2() : base(nameof(Class2)) { }
}

[AutoService(ServiceLifetime.Singleton)]
public class Class3 : ClassBase, IClass
{
    public Class3() : base(nameof(Class3)) { }
}

//........................................

[AutoService(false)]
public class Class4 : ClassBase, IClass
{
    public Class4() : base(nameof(Class4)) { }
}

[AutoService(ServiceLifetime.Transient,false)]
public class Class5 : ClassBase, IClass
{
    public Class5() : base(nameof(Class5)) { }
}

[AutoService(ServiceLifetime.Singleton, false)]
public class Class6 : ClassBase, IClass
{
    public Class6() : base(nameof(Class6)) { }
}

//........................................



[AutoService]
public class Class7 : ClassBase//, IClass
{
    public Class7() : base(nameof(Class7)) { }
}

//[AutoService(useImplementation:true)]
public class Class8 : ClassBase//, IClass
{
    public Class8() : base(nameof(Class8)) { }
}


//........................................


[AutoService(typeof(IClass))]
public class Class9 : ClassBase, IClass
{
    public Class9() : base(nameof(Class9)) { }
}


[AutoService(typeof(IClass),true)]
public class Class10 : ClassBase, IClass
{
    public Class10() : base(nameof(Class10)) { }
}

[AutoService(typeof(IClass),false)]
public class Class11 : ClassBase, IClass
{
    public Class11() : base(nameof(Class11)) { }
}


//........................................

[AutoService(ServiceLifetime.Singleton, typeof(IClass),false)]
public class Class12 : ClassBase, IClass
{
    public Class12() : base(nameof(Class12)) { }
}


[AutoService(ServiceLifetime.Scoped, typeof(IClass), true)]
public class Class13 : ClassBase, IClass
{
    public Class13() : base(nameof(Class13)) { }
}

//[AutoService(ServiceLifetime.Transient, typeof(IClass), true)]
public class Class14 : ClassBase//,IClass
{
    public Class14() : base(nameof(Class14)) { }
}

[AutoService(ServiceLifetime.Transient, typeof(IClass), false)]
public class Class15 : ClassBase //,IClass
{
    public Class15() : base(nameof(Class15)) { }
}




















public static class Extensions
{
    public static void CompainTest<TClass>(this ServiceProvider _serviceProvider, bool isclass = false) where TClass : ClassBase
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{Environment.NewLine} {typeof(TClass).Name} use AutoService  {typeof(TClass).GetCustomAttributes(typeof(AutoServiceAttribute), false).Cast<AutoServiceAttribute>().Select(att => (att.LifetimeType, att.ImplementationType, att.UseImplementation)).Single()} is class: {isclass}");
        Console.ResetColor();

        _serviceProvider.TwiceTest<TClass>(isclass); //test

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"In Scope");
        Console.ResetColor();

        _serviceProvider.TwiceScopeTest<TClass>(isclass); //test scope
    }

    public static void TwiceTest<TClass>(this ServiceProvider _serviceProvider, bool isclass = false) where TClass : ClassBase
    {
        _serviceProvider.GenerateService<TClass>(isclass);
        _serviceProvider.GenerateService<TClass>(isclass);
    }

    public static void TwiceScopeTest<TClass>(this ServiceProvider _serviceProvider, bool isclass = false) where TClass : ClassBase
    {
        _serviceProvider.GenerateService<TClass>(provider =>
        {
            provider.PrintService<TClass>(isclass);
            provider.PrintService<TClass>(isclass);
        });
    }

    public static void GenerateService<TClass>(this ServiceProvider _serviceProvider, bool isclass = false) where TClass : ClassBase
    {
        using var scope = _serviceProvider.CreateScope();
        scope.ServiceProvider.PrintService<TClass>(isclass);
    }


    public static void GenerateService<TClass>(this ServiceProvider _serviceProvider, Action<IServiceProvider> scopefunc, bool isclass = false) where TClass : ClassBase
    {
        using var scope = _serviceProvider.CreateScope();
        scopefunc(scope.ServiceProvider);
    }

    private static void PrintService<TClass>(this IServiceProvider provider, bool isclass = false) where TClass : ClassBase
    {
        if (isclass)
        {
            provider.GetServices<TClass>().OfType<TClass>().First().Print();
            return;
        }

        provider.GetServices<IClass>().OfType<TClass>().First().Print();
    }
}