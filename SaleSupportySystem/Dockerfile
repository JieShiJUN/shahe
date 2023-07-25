#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV LANG=C.UTF-8

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SaleSupportySystem/SaleSupportySystem.csproj", "SaleSupportySystem/"]
RUN dotnet restore "SaleSupportySystem/SaleSupportySystem.csproj"
COPY . .
WORKDIR "/src/SaleSupportySystem"
RUN dotnet build "SaleSupportySystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SaleSupportySystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SaleSupportySystem.dll"]