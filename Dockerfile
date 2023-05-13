FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app

#copy .csproj and restore as distinct layers
# copia Reactivities.sln com o msm nome em /app do docker
COPY "Reactivities.sln" "Reactivities.sln"
COPY "API/API.csproj" "API/API.csproj"
COPY "Application/Application.csproj" "Application/Application.csproj"
COPY "Persistence/Persistence.csproj" "Persistence/Persistence.csproj"
COPY "Domain/Domain.csproj" "Domain/Domain.csproj"
COPY "Infrastructure/Infrastructure.csproj" "Infrastructure/Infrastructure.csproj"

RUN dotnet restore "Reactivities.sln"

# copia tudo para pasta app do docker
COPY . .
WORKDIR /app
# construir versao de publicaçao
RUN dotnet publish -c Release -o out

# runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "API.dll" ]