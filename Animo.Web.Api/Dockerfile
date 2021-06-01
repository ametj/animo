#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Animo.Web.Api/Animo.Web.Api.csproj", "Animo.Web.Api/"]
RUN dotnet restore "Animo.Web.Api/Animo.Web.Api.csproj"
COPY . .
WORKDIR "/src/Animo.Web.Api"
RUN dotnet build "Animo.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Animo.Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Animo.Web.Api.dll"]