# [Meteors]  DependencyInjection.AutoService
[<img alt="Nuget" src="https://img.shields.io/nuget/dt/Meteors.DependencyInjection.AutoService?color=green&logo=nuget&logoColor=blue&style=for-the-badge">](https://www.nuget.org/packages/Meteors.DependencyInjection.AutoService/)

[![](https://img.shields.io/nuget/v/Meteors.DependencyInjection.AutoService)](https://www.nuget.org/packages/Meteors.DependencyInjection.AutoService)

### ``Install-Package Meteors.DependencyInjection.AutoService -Version 10.0.0``


<p align="center">
<img width="10%" src="https://user-images.githubusercontent.com/48151918/175791394-3913f060-5551-435c-adda-5bc487964f1c.png" />
</p>

[Source Code](https://github.com/MhozaifaA/DependencyInjection.AutoService)




---

## Setup

```csharp
builder.Services.AddAutoService();                        // scans all assemblies
builder.Services.AddAutoService(typeof(MyClass));         // scans specific assembly
builder.Services.AddAutoService("MyProject.Services");    // scans by assembly name
```

---

## `[AutoService]` Attribute

Place on any class to register it automatically.

| Usage | Behavior |
|---|---|
| `[AutoService]` | Scoped, resolves `I{ClassName}` or first interface |
| `[AutoService(ServiceLifetime.Singleton)]` | Singleton |
| `[AutoService(ServiceLifetime.Transient)]` | Transient |
| `[AutoService(typeof(ICustomInterface))]` | Registers against the specified interface |
| `[AutoService(useImplementation: false)]` | Registers the class itself (no interface) |
| `[AutoService(serviceKey: "myKey")]` | Keyed service registration |
| `[AutoService(supportLazy: true)]` | Also registers `Lazy<T>` for deferred resolution |

---

## Multiple Registrations

Stack multiple `[AutoService]` attributes on the same class to register it against different interfaces:

```csharp
[AutoService(typeof(IOther))]
[AutoService]
public class A : IA, IOther { }

// Registers:  IA     → A  (Scoped)
//             IOther → A  (Scoped)
```

---

## Lazy Support

Pass `supportLazy: true` to also register a `Lazy<T>` wrapper, allowing deferred resolution:

```csharp
[AutoService(supportLazy: true)]
public class A : IA { }
```

Both constructor injection styles now work:

```csharp
public MyService(IA service) { }           // normal — resolved immediately
public MyService(Lazy service) { }     // lazy   — resolved on first .Value access
```

> `Lazy<T>` is registered as `Scoped` to ensure the captured `IServiceProvider` is always the current scope — preventing root provider capture and scoped service leaks.

---

## Interface Resolution Rules

When no `implementationType` is specified, the library resolves the interface in this order:

1. Interface named `I{ClassName}` (e.g. `class UserService` → `IUserService`)
2. First direct interface on the class
3. Throws `AmbiguousMatchException` if `useImplementation: true` but no interface found

---


``` C#

//build.Services.AddAutoService(Assembly..namesapces..type);

build.Services.AddAutoService();

...
[AutoService(ServiceLifeTime, InterfaceType)]
[AutoService(ServiceLifeTime, InterfaceType)]
   
[AutoService()]
[AutoService(LifetimeType)]
[AutoService(ImplementationType)]
[AutoService(UseImplementation)]
[AutoService(LifetimeType,ImplementationType)]
[AutoService(LifetimeType,UseImplementation)]
[AutoService(ImplementationType,UseImplementation)] 
[AutoService(LifetimeType,ImplementationType,UseImplementation,ServiceKey)]


[AutoService] //default Scoped
class AnyService : IAnyService { }


[AutoService(typeOf(ICustomInterfaceName))] //take Implementation
class AnyService : ICustomInterfaceName { }


[AutoService(typeOf(ICustomInterfaceName1))] //take Implementation 1
class AnyService : ICustomInterfaceName,ICustomInterfaceName1,ICustomInterfaceName2 { }



[AutoService] //take Implementation IAnyService, is not first but same I + service-name
class AnyService : ICustomInterfaceName,IAnyService,ICustomInterfaceName2 { }


[AutoService] //take class
class AnyService {}

[AutoService]//as UseImplementation=true or null and take first interface
class AnyService : ICustomInterfaceName { }

[AutoService(false)] //UseImplementation=false
class AnyService : ICustomInterfaceName { }

[AutoService("servicekeyed")] //take class, and get by servicekeyed
class AnyService {}


```




> This lib belongs to the **Meteors**,
> Meteorites helps you write less and clean code with the power of design patterns and full support for the most popular programming for perpetual projects
>
> All you need in your project is to use meteorites,
> Simplicity is one in all,

