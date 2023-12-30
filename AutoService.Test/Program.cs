// See https://aka.ms/new-console-template for more information
using Meteors;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

ServiceProvider _serviceProvider;
var service = new ServiceCollection();
service.AddAutoService();

_serviceProvider = service.BuildServiceProvider();



_serviceProvider.CompainTest<Class>()
    .CompainTest<Class1>()
    .CompainTest<Class2>()
    .CompainTest<Class3>()
    .CompainTest<Class4>()
    .CompainTest<Class5>()
    .CompainTest<Class6>()
    .CompainTest<Class7>()
    .CompainTest<Class8>() //throw right as expected
    .CompainTest<Class9>()
    .CompainTest<Class10>()
    .CompainTest<Class11>()
    .CompainTest<Class12>()
    .CompainTest<Class13>()
    .CompainTest<Class14>()  //throw right as expected
    .CompainTest<Class15>()
    .CompainTest<Class16>();



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

[AutoService(ServiceLifetime.Transient, false)]
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


[AutoService(typeof(IClass), true)]
public class Class10 : ClassBase, IClass
{
    public Class10() : base(nameof(Class10)) { }
}

[AutoService(typeof(IClass), false)]
public class Class11 : ClassBase, IClass
{
    public Class11() : base(nameof(Class11)) { }
}


//........................................

[AutoService(ServiceLifetime.Singleton, typeof(IClass), false)]
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


[AutoService("servicekey")]
public class Class16 : ClassBase, IClass
{
    public Class16() : base(nameof(Class16)) { }
}


















public static class Extensions
{
    public static ServiceProvider CompainTest<TClass>(this ServiceProvider _serviceProvider) where TClass : ClassBase
    {
        Console.ForegroundColor = ConsoleColor.Green;

        var attr = typeof(TClass).GetCustomAttributes(typeof(AutoServiceAttribute), false).Cast<AutoServiceAttribute>().
            Select(att => att).FirstOrDefault();

        Console.WriteLine($"{Environment.NewLine} {typeof(TClass).Name} {nameof(AutoServiceAttribute)} " + $" {attr}");

        Console.ResetColor();

        _serviceProvider.TwiceTest<TClass>(attr); //test

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"In Scope");
        Console.ResetColor();

        _serviceProvider.TwiceScopeTest<TClass>(attr); //test scope

        return _serviceProvider;
    }

    public static void TwiceTest<TClass>(this ServiceProvider _serviceProvider, AutoServiceAttribute? attr) where TClass : ClassBase
    {
        _serviceProvider.GenerateService<TClass>(attr);
        _serviceProvider.GenerateService<TClass>(attr);
    }

    public static void TwiceScopeTest<TClass>(this ServiceProvider _serviceProvider, AutoServiceAttribute? attr) where TClass : ClassBase
    {
        _serviceProvider.GenerateService<TClass>(provider =>
        {
            provider.PrintService<TClass>(attr);
            provider.PrintService<TClass>(attr);
        });
    }

    public static void GenerateService<TClass>(this ServiceProvider _serviceProvider, AutoServiceAttribute? attr) where TClass : ClassBase
    {
        using var scope = _serviceProvider.CreateScope();
        scope.ServiceProvider.PrintService<TClass>(attr);
    }


    public static void GenerateService<TClass>(this ServiceProvider _serviceProvider, Action<IServiceProvider> scopefunc) where TClass : ClassBase
    {
        using var scope = _serviceProvider.CreateScope();
        scopefunc(scope.ServiceProvider);
    }

    private static void PrintService<TClass>(this IServiceProvider provider, AutoServiceAttribute? attr) where TClass : ClassBase
    {
        if (attr is null)
        {
            var @class = (ClassBase)Activator.CreateInstance<TClass>();
            @class.Print();
            return;
        }


        //TClass
        //**this work when use-interface not selected and **not found interface same class name with I,
        // or ((then order)) when implemnt = false
        Type? implementationType = null;
        if (attr.ImplementationType is null && (attr.UseImplementation is null || attr.UseImplementation is true))
        {

             implementationType = typeof(TClass).GetInterface("I" + typeof(TClass).Name);
            if (implementationType is null)
            {
                var allimplementationTypes = typeof(TClass).GetInterfaces();
                var implementationTypes = allimplementationTypes.Except(
                                 allimplementationTypes.SelectMany(i => i.GetInterfaces()));

                if (implementationTypes.Count() > 0)
                    implementationType = implementationTypes.First();

                if (implementationType is null && attr.UseImplementation is null)
                    provider.GetServices<TClass>().OfType<TClass>().First().Print();//TClass
            }

        }

        if (implementationType is not null)
           provider.GetServicesOrKeyedServices<IClass>(attr).OfType<TClass>().First().Print(); //IClass


        if (attr.UseImplementation is false)
            provider.GetServicesOrKeyedServices<TClass>(attr).OfType<TClass>().First().Print(); //TClass

    }

    private static IEnumerable<OClass> GetServicesOrKeyedServices<OClass>(this IServiceProvider provider, AutoServiceAttribute? attr)
    {
        if(attr?.ServiceKey is not null)
             return provider.GetKeyedServices<OClass>(attr!.ServiceKey); //TClass
        
        return provider.GetServices<OClass>(); //TClass
    }

}
