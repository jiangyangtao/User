#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/User.Application/User.Application.csproj", "src/User.Application/"]
RUN dotnet restore "src/User.Application/User.Application.csproj"
COPY . .
WORKDIR "/src/src/User.Application"
RUN dotnet build "User.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "User.Application.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "User.Application.dll"]