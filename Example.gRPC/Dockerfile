FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview-windowsservercore-ltsc2019 AS build
RUN curl -SL --output vs_buildtools.exe https://aka.ms/vs/17/release/vs_buildtools.exe
RUN vs_buildtools.exe --installPath C:\BuildTools --add Microsoft.VisualStudio.Component.VC.Tools.x86.x64 Microsoft.VisualStudio.Component.Windows10SDK.19041 --quiet --wait --norestart --nocache

ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Example.gRPC.csproj", "./"]

RUN dotnet restore "Example.gRPC.csproj"

COPY . ./

RUN dir
RUN if exist "Protos" dir Protos

RUN dotnet build "Example.gRPC.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Example.gRPC.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=true

FROM mcr.microsoft.com/dotnet/runtime:10.0-preview-nanoserver-1809 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=publish /app/publish .
ENTRYPOINT ["Example.gRPC.exe"]