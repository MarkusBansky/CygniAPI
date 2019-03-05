# CygniAPI
**CygniAPI is a lightweight sowftware package that helps you easily create and host API requests and create a very basic RESTful service.**

The service can hold basic requests types such as:

 - GET
 - POST
 - PUT
 - DELETE

Every instance of the service can hold it's own configuration. You can set your configuration by passing a new *struct* to the constructor method:

```csharp
new CygniConfiguration
{
    AllowedRequestTypes = new[] { RequestType.GET, RequestType.POST },
    ListeningPort = 5050, // this is the default port
    ListeningPath = "*" // this will be listening on any hostname
}
```

## Getting started

To get started with this package first please install it from our NuGet Manager Console or UI by searching `CygniAPI` or performing the next command:

```sh
Install-Package CygniAPI
```
