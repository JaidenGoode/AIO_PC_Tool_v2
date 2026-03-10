# JGoode's AIO PC Tool v2

A professional Windows optimization and system maintenance tool built with WPF and .NET 8.

## Features

- **Dashboard**: Real-time hardware monitoring (CPU, GPU, RAM, Disk)
- **Tweaks**: 40+ safe, reversible system optimizations with detailed descriptions
- **Cleaner**: Remove temp files, browser cache, and system junk
- **DNS Manager**: Easy DNS server switching (Cloudflare, Google, etc.)
- **Restore Points**: Create and manage Windows restore points
- **Utilities**: System repair tools (SFC, DISM, etc.) and Chris Titus Tech utility

## Requirements

- Windows 10/11
- .NET 8 SDK (for building)
- Administrator privileges (for applying tweaks)

## Building

1. Install .NET 8 SDK from https://dotnet.microsoft.com/download/dotnet/8.0
2. Run `BUILD.bat`
3. Find the executable in the `publish` folder

## Safety

All tweaks are:
- 100% reversible
- Do not delete any registry keys
- Include detailed descriptions and warnings
- Show Windows version compatibility

## Credits

- Created by JaidenGoode
- Built with Material Design in XAML
- Hardware monitoring by LibreHardwareMonitor
