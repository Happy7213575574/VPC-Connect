FROM mcr.microsoft.com/playwright:focal AS build
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

RUN curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -c 6.0 -InstallDir /usr/share/dotnet \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

WORKDIR /src
COPY VPCConnectSmokeTests.sln .
COPY ["ConnectApp/", "ConnectApp/"]
COPY ["PortalApi.Tests/", "PortalApi.Tests/"]

RUN dotnet restore "PortalApi.Tests/PortalApi.Tests.csproj"
RUN dotnet build "PortalApi.Tests/PortalApi.Tests.csproj" -c "Debug-LivePortal" -r "linux-x64"

# Install browsers
# WORKDIR /src/PortalApi.Tests
# RUN pwsh bin/Debug/net5.0/playwright.ps1

FROM build AS testrunner
WORKDIR /src/PortalApi.Tests

CMD ["dotnet", "test", "--no-restore", "--configuration","Debug-LivePortal"]
