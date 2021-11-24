FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY src/A.UI.MVC/*.csproj A.UI.MVC/
RUN dotnet restore A.UI.MVC
COPY src/B.Dll.API/*.csproj B.Dll.API/
RUN dotnet restore B.Dll.API
COPY src/C.Dll.Application/*.csproj C.Dll.Application/
RUN dotnet restore C.Dll.Application
COPY src/D.Dll.Infra.EFCore/*.csproj D.Dll.Infra.EFCore/
RUN dotnet restore D.Dll.Infra.EFCore
COPY src/E.Dll.Domain/*.csproj E.Dll.Domain/
RUN dotnet restore E.Dll.Domain
COPY src/External.Dll.SwissKnife/*.csproj External.Dll.SwissKnife/
RUN dotnet restore External.Dll.SwissKnife

COPY src/. .
RUN dotnet build "A.UI.MVC" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "A.UI.MVC" -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanMVC.dll"]