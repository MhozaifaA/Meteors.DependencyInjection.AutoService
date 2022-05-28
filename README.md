# DependencyInjection.AutoService

``` C#

build.Services.AddAutoService();

...
//[AutoService(ServiceLifeTime.Singleton)]
//[AutoService(ServiceLifeTime.Transient)]
[AutoService] //default Scoped
class AnyService : IAnyService
{

}



```
