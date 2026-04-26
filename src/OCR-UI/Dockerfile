FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5002
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/OCR-UI/OCR-UI.csproj", "src/OCR-UI/"]
COPY ["src/OCR-Core/OCR-Core.csproj", "src/OCR-Core/"]
RUN dotnet restore "src/OCR-UI/OCR-UI.csproj"
COPY . .
WORKDIR "/src/src/OCR-UI"
RUN dotnet build "OCR-UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OCR-UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OCR-UI.dll"]
