run commands for api:

install httprepl:
dotnet tool install -g Microsoft.dotnet-httprepl

terminal 1
dotnet run --project src/InvoiceApp.Api/InvoiceApp.Api.csproj --launch-profile https

terminal 2 (using https localhost from terminal 1 command)
dotnet dev-certs https --trust
httprepl https://localhost:7056

db setup: https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?utm_source=chatgpt.com&tabs=netcore-cli

dotnet ef migrations add InitialAzureCreate
