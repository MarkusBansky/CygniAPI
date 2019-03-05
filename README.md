# CygniAPI
**CygniAPI is a lightweight sowftware package that helps you easily create and host API requests and create a very basic RESTful service.**

The service can hold basic requests types such as: `GET`, `POST`, `PUT`, `DELETE`.

Every instance of the service can hold it's own configuration. You can set your configuration by passing a new *struct* to the constructor method:

```csharp
new CygniConfiguration
{
    AllowedRequestTypes = new[] { RequestType.GET, RequestType.POST },
    ListeningPort = 5050, // this is the default port
    ListeningPath = "*" // this will be listening on any hostname
}
```

Depending on your rights and the system you are running your project, you probably would have to reserver the host and the port for your user to be able to host the server on your machine.

## Getting started

To get started with this package first please install it from our NuGet Manager Console or UI by searching `CygniAPI` or performing the next command:

```sh
Install-Package CygniAPI
```

## Creating basic example server

Here we would have a look at a very basic example to show you how easily it is to set up the server running in the background.

Lets get started adding the **using** directive: `using CygniAPI;` on top of your source file.

Then you need to create the instance of a server and configure it with your own configuration example of which can we found in the beginning of this file.

```csharp
// Create the instance of the server with a predefined custom configuration
var server = new CygniAPI.CygniApiServer(config);
```

Now we can register your first callback function, let us choose it to display current `DateTime` in UTC format:

```csharp
// This method adds a callback function replying to request type **GET**
server.Get("/date", (i, o) =>
{
    Console.WriteLine("The page displays current date in UTC.");
    o.Append(DateTime.UtcNow);
});
```

The `"/date"` means that this callback would be invoked when user opens page `http://localhost:8080/date` representing the substring after the host and port in the URL.

In `(i, o)` syntax, the `i` stands for `string` input that is read from the request body, and the `o` is the output `StringBuilder` that is pased to the response body.

Everything that happens in the function is called during the request and nything appended to the `o` as an output would be displayed to the user. In current example we would receive the string of current date and time.

Now, to make it work we would have to start the server using command:

```csharp
server.Start();
```

That's a YAY, you can no go to your browser windows and try it yourself. This is a very simple and basic example of how easily you can create a callback function in the server.
