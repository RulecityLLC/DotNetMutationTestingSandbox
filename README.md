# DotNetMutationTestingSandbox
Sample repo showing how to enforce mutation testing threshold gates on pull requests

## Building

```bash
dotnet build
```

## Running unit tests

```bash
# just running the unit tests
dotnet test

# running the tests with mutations
dotnet tool restore
dotnet stryker
```
## Running embedded web server

```bash
dotnet run --project RestApi
```

## Testing the API

With web server running, navigate to
```
http://localhost:5041/Swagger/
```
