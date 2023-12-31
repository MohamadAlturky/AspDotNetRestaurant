# Use the official .NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET SDK as a parent image to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Presentation/Presentation.csproj", "YourApi/"]
RUN dotnet restore "YourApi/YourApi.csproj"
COPY . .
WORKDIR "/src/Presentation"
RUN dotnet build "Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Presentation.csproj" -c Release -o /app/publish

# Set the final image to be used
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.dll"]
