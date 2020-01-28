FROM microsoft/dotnet:3.0-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:3.0-sdk AS build
WORKDIR /src
COPY ["KelloWorld/KelloWorld.csproj", "KelloWorld/"]
RUN dotnet restore "KelloWorld/KelloWorld.csproj"
COPY . .
WORKDIR "/src/KelloWorld"
RUN dotnet build "KelloWorld.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "KelloWorld.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KelloWorld.dll"]