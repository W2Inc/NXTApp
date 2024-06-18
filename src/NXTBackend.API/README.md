# API

The central logic for the entire API is stored here. This includes the controllers, services, and any other classes that are used to interact with the API.
It mainly defines all the various routes that the API will expose to the client and the logic that will be executed when those routes are hit.

```bash
dotnet ef migrations add Init --project "NXTBackend.API.Infrastructure" -s "NXTBackend.API"
```