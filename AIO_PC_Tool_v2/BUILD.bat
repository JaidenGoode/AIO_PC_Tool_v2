@echo off
title JGoode's AIO PC Tool v2 - Build Script
color 0A

echo.
echo  ========================================
echo   JGoode's AIO PC Tool v2 - Build Script
echo  ========================================
echo.

:: Check for .NET SDK
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo [ERROR] .NET SDK not found!
    echo Please install .NET 8 SDK from: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

echo [INFO] .NET SDK found
echo.

:: Clean previous builds
echo [STEP 1/4] Cleaning previous builds...
if exist "bin" rmdir /s /q "bin"
if exist "obj" rmdir /s /q "obj"
if exist "publish" rmdir /s /q "publish"
echo Done.
echo.

:: Restore packages
echo [STEP 2/4] Restoring NuGet packages...
dotnet restore
if errorlevel 1 (
    echo [ERROR] Failed to restore packages!
    pause
    exit /b 1
)
echo Done.
echo.

:: Build Release
echo [STEP 3/4] Building Release version...
dotnet build -c Release --no-restore
if errorlevel 1 (
    echo [ERROR] Build failed!
    pause
    exit /b 1
)
echo Done.
echo.

:: Publish single executable
echo [STEP 4/4] Publishing single executable...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -o "publish"
if errorlevel 1 (
    echo [ERROR] Publish failed!
    pause
    exit /b 1
)
echo Done.
echo.

echo  ========================================
echo   BUILD SUCCESSFUL!
echo  ========================================
echo.
echo  Output: publish\AIO_PC_Tool_v2.exe
echo.
echo  To run: Right-click the .exe and select
echo          "Run as Administrator"
echo.
pause
