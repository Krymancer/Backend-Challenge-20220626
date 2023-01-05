#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Backend Challenge 20220626/Backend Challenge 20220626.csproj", "Backend Challenge 20220626/"]
RUN dotnet restore "Backend Challenge 20220626/Backend Challenge 20220626.csproj"
COPY . .
WORKDIR "/src/Backend Challenge 20220626"
RUN dotnet build "Backend Challenge 20220626.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Backend Challenge 20220626.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend Challenge 20220626.dll"]