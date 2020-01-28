FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
ADD . .

RUN dotnet publish -c Release -o out

FROM base AS final
WORKDIR /app
COPY --from=build /src/out .
ENTRYPOINT ["dotnet", "KelloWorld.dll"]