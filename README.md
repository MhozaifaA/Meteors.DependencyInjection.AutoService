# [Meteors]  DependencyInjection.AutoService
[<img alt="Nuget" src="https://img.shields.io/nuget/dt/Meteors.DependencyInjection.AutoService?color=green&logo=nuget&logoColor=blue&style=for-the-badge">](https://www.nuget.org/packages/Meteors.DependencyInjection.AutoService/)

### ``Install-Package Meteors.DependencyInjection.AutoService -Version 6.0.0``

> `version 1.0 net6.0`
[![Meteor logo pack](https://user-images.githubusercontent.com/48151918/175791394-3913f060-5551-435c-adda-5bc487964f1c.png)

[Source Code](https://github.com/MhozaifaA/DependencyInjection.AutoService)

``` C#

//build.Services.AddAutoService(Assembly..namesapces..type);

build.Services.AddAutoService();

...
[AutoService(ServiceLifeTime, InterfaceType)]

[AutoService] //default Scoped
class AnyService : IAnyService { }


[AutoService(typeOf(ICustomInterfaceName))]
class AnyService : ICustomInterfaceName { }


```




> This lib belongs to the **Meteors**,
> Meteorites helps you write less and clean code with the power of design patterns and full support for the most popular programming for perpetual projects
>
> All you need in your project is to use meteorites,
> Simplicity is one in all,

