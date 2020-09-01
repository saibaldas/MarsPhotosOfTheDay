#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

# Install NodeJS 10
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install -y nodejs

RUN npm install

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MarsPhotosOfTheDay/MarsPhotosOfTheDay.Web.csproj", "MarsPhotosOfTheDay/"]
COPY ["MarsPhotosOfTheDay.API/MarsPhotosOfTheDay.API.csproj", "MarsPhotosOfTheDay.API/"]
COPY ["MarsPhotosOfTheDay.Services/MarsPhotosOfTheDay.Services.csproj", "MarsPhotosOfTheDay.Services/"]
RUN dotnet restore "MarsPhotosOfTheDay/MarsPhotosOfTheDay.Web.csproj"
COPY . .
WORKDIR "/src/MarsPhotosOfTheDay"
RUN dotnet build "MarsPhotosOfTheDay.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarsPhotosOfTheDay.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarsPhotosOfTheDay.Web.dll"]
