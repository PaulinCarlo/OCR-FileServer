FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/OCR-Api/OCR-Api.csproj", "src/OCR-Api/"]
COPY ["src/OCR-Core/OCR-Core.csproj", "src/OCR-Core/"]
RUN dotnet restore "src/OCR-Api/OCR-Api.csproj"
COPY . .
WORKDIR "/src/src/OCR-Api"
RUN dotnet build "OCR-Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OCR-Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OCR-Api.dll"]
