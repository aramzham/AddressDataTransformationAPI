# Address data transformation API

The API is taking an input value of a user profile and transforms the address line in a specific way.

![image](https://user-images.githubusercontent.com/25085025/215225011-26959381-6bb8-4141-beb5-11ca374f2a1c.png)

Tech stack:
- Minimal API .net 7.0
- <a href="https://www.nuget.org/packages/Asp.Versioning.Http">Api.Versioning.Http<a/> for api versioning :smile:
- unit tests (xUnit, Moq, AutoFixture)
- <a href="https://www.nuget.org/packages/Mapster/">Mapster</a> for mapping
- <a href="https://github.com/Fody/MethodTimer">MethodTimer.Fody</a> for method execution time measuring
- UI is a Blazor webassembly app using <a href="https://blazorise.com/docs">Blazorise</a> for components
