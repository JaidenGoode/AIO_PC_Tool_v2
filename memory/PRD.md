# JGoode's AIO PC Tool v2 - PRD

## Problem Statement
Professional WPF .NET 8 Windows gaming optimizer with Cortex-style cleaner, comprehensive safe tweaks, and stunning 4K HD UI.

## Core Features

### 1. Smart System Cleaner (Better than Cortex)
**Key Feature: Only shows apps that exist on YOUR system**
- Scans system to detect installed apps
- Only displays categories with actual files to clean
- Shows "You're All Clean" screen when nothing found

**30+ Cleaning Categories:**
- **Gaming:** DirectX Shaders, NVIDIA/AMD/Intel GPU Cache, Steam, Epic Games, EA App, Ubisoft Connect, GOG Galaxy, Battle.net, Riot Games
- **System:** Windows Temp, Update Cache, Logs, Prefetch, Thumbnails, Recycle Bin
- **Browser:** Chrome, Edge, Firefox, Opera GX, Brave, Vivaldi
- **Apps:** Discord, Spotify, VS Code, Slack, Teams, Zoom

### 2. System Optimizations (35+ Safe Tweaks)
**Categories:**
- Gaming (9): Game Mode, GPU Scheduling, Game Bar, Process Priority
- Performance (8): SysMain, Prefetch, CPU Scheduling, Responsiveness
- Privacy (7): Telemetry, Advertising ID, Activity History, Location
- Network (3): Nagle's Algorithm, Network Throttling, Large MTU
- Visual (3): File Extensions, Hidden Files, Classic Context Menu
- Power (3): USB Suspend, Power Throttling, Core Parking
- Security (2): AutoRun, Remote Assistance
- Storage (3): Storage Sense, NTFS Optimization, 8.3 Names

### 3. Hardware Monitor Dashboard
- Real-time CPU, GPU, RAM, Disk monitoring
- Temperature readings
- Usage percentage bars
- Quick action cards

### 4. DNS Manager
- 6 providers: Cloudflare, Google, OpenDNS, Quad9, AdGuard, Default
- One-click DNS switching
- Status badges (RECOMMENDED, SECURE, AD BLOCKER)

### 5. System Utilities
- Chris Titus Tech Utility (featured)
- DISM health check
- SFC scan
- Disk cleanup
- Network reset

### 6. Restore Points
- Create named restore points
- View existing points
- One-click creation

## Technical Stack
- **Framework:** WPF .NET 8 LTS
- **Pattern:** MVVM with CommunityToolkit.Mvvm
- **Hardware:** LibreHardwareMonitorLib
- **UI:** Custom dark theme with Red accent

## Project Structure
```
/app/AIO_PC_Tool_v2/
├── App.xaml(.cs)         # Entry, DI, global error handling
├── Models/Tweak.cs       # All data models
├── Services/
│   ├── CleanerService.cs     # Smart cleaner with detection
│   ├── TweakService.cs       # 35+ safe tweaks
│   ├── HardwareMonitorService.cs
│   ├── OtherServices.cs      # DNS, RestorePoint
│   └── UtilitiesService.cs
├── Views/
│   ├── MainWindow.xaml(.cs)
│   ├── DashboardPage.xaml(.cs)
│   ├── CleanerPage.xaml(.cs)   # Smart cleaner UI
│   ├── TweaksPage.xaml(.cs)
│   ├── UtilitiesPage.xaml(.cs)
│   ├── DnsPage.xaml(.cs)
│   ├── RestorePointsPage.xaml(.cs)
│   └── SettingsPage.xaml(.cs)
├── Styles/
│   ├── CustomStyles.xaml
│   └── ThemeColors.xaml
├── ViewModels/ViewModels.cs
└── BUILD.bat
```

## UI/UX Design
- **Theme:** Dark (#0A0A0A background)
- **Accent:** Red (#DC2626)
- **Font:** Segoe UI (system native, 4K crisp)
- **Cards:** Rounded (12-16px), subtle borders
- **Spacing:** Generous margins (40px page padding)

## Build & Test
```batch
BUILD.bat
```
Run as Administrator for full functionality.

## Completed (Dec 2025)
- [x] Smart cleaner that only shows installed apps
- [x] "You're All Clean" empty state
- [x] 35+ safe tweaks with categories
- [x] 4K HD polished UI throughout
- [x] Real-time hardware monitoring
- [x] DNS manager with 6 providers
- [x] Quick actions on dashboard
- [x] Chris Titus Tech utility integration

## Future Backlog
- [ ] P1: Gaming Boost Mode (one-click apply all gaming tweaks)
- [ ] P1: System tray with minimize to tray
- [ ] P2: Export/import tweak profiles
- [ ] P2: Scheduled cleaning
- [ ] P2: Custom tweak editor
- [ ] P3: Auto-update functionality
