FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["SubscriptionManager.csproj", "."]
RUN dotnet restore "SubscriptionManager.csproj"

COPY . .
RUN dotnet publish "SubscriptionManager.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SubscriptionManager.dll"]
