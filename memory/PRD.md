# JGoode's AIO PC Tool v2 - PRD

## Problem Statement
Rebuild original Electron-based AIO PC Tool as a professional WPF .NET 8 Windows desktop application with a stunning Red + Black theme, featuring a Cortex-like advanced system cleaner, comprehensive safe tweaks, and professional UI across all features.

## Architecture
- **Framework**: WPF (.NET 8 LTS)
- **Pattern**: MVVM with CommunityToolkit.Mvvm
- **UI**: Custom themed styles (Red/Black default, multiple accent colors)
- **Hardware**: LibreHardwareMonitorLib
- **Dependencies**: Microsoft.Extensions.DependencyInjection

## What's Implemented (Dec 2025)

### Advanced System Cleaner (Cortex-style) ✅ NEW
**25+ Cleaning Categories:**

**Gaming:**
- DirectX Shader Cache
- NVIDIA Cache (DXCache, GLCache, ComputeCache)
- AMD Cache (DxCache, DxcCache, GLCache, VkCache)
- Intel Graphics Shader Cache
- Steam Cache & Logs
- Epic Games Cache
- EA App Cache
- Ubisoft Connect Cache
- GOG Galaxy Cache

**System:**
- Windows Temp Files (System + User)
- Windows Update Cache & Logs
- Windows Logs (CBS, DISM, WER)
- Prefetch Data
- Thumbnail Cache (thumbcache, iconcache)
- Recycle Bin

**Browser:**
- Chrome Cache (Cache, Code Cache, GPUCache, ShaderCache)
- Edge Cache
- Firefox Cache
- Opera GX Cache
- Brave Browser Cache

**Apps:**
- Discord Cache
- Spotify Cache
- VS Code Cache

**Features:**
- Scan all with size breakdown per category
- Select/Deselect All functionality
- Category grouping with icons
- SAFE badges on all categories
- File count per category
- Last cleaned timestamp

### System Tweaks (35+ Safe Tweaks) ✅ ENHANCED
**Gaming (9 tweaks):**
- Enable Game Mode
- Disable Xbox Game Bar
- Disable Background Recording (Game DVR)
- Hardware-Accelerated GPU Scheduling
- Disable Fullscreen Optimizations
- Boost Game Process Priority
- Maximum GPU Priority for Games
- High-Performance Scheduling Category
- High-Priority SFIO for Games

**Performance (8 tweaks):**
- Disable SysMain (Superfetch)
- Disable Prefetch
- Disable Hibernation
- Disable Fast Startup
- Optimize CPU Scheduling
- Disable Transparency Effects
- Reduce Visual Animations
- Maximize System Responsiveness

**Privacy (7 tweaks):**
- Disable Telemetry
- Disable Advertising ID
- Disable Activity History
- Disable Feedback Requests
- Disable App Suggestions
- Disable Tailored Experiences
- Disable Location Tracking

**Network (3 tweaks):**
- Disable Nagle's Algorithm
- Disable Network Throttling
- Enable Large MTU Packets

**Visual (3 tweaks):**
- Show File Extensions
- Show Hidden Files
- Classic Context Menu (Win11)

**Power (3 tweaks):**
- Disable USB Selective Suspend
- Disable Power Throttling
- Disable CPU Core Parking

**Security (2 tweaks):**
- Disable AutoRun
- Disable Remote Assistance

**Storage (3 tweaks):**
- Disable Storage Sense
- Optimize NTFS Performance
- Disable 8.3 Filename Creation

**Features:**
- Instant status detection (OPTIMIZED/DEFAULT badges)
- Category filtering with colored badges
- Grouped display when viewing "All"
- Apply/Revert All in Category
- Warning labels for advanced tweaks
- Windows version indicators

### Professional UI ✅ ENHANCED
- Consistent design language across all pages
- Modern card-based layouts
- Category headers with icons and accent colors
- Hover effects on all interactive elements
- Stats counters in headers
- Empty state designs
- Confirmation dialogs for destructive actions

### All Pages Updated:
1. **Dashboard**: Hardware monitoring, quick access, active tweaks count
2. **Cleaner**: Full Cortex-style redesign with 25+ categories
3. **Tweaks**: Category filtering, grouped display, Apply/Revert All
4. **Utilities**: Featured CTT card, categorized utilities
5. **DNS Manager**: Selection highlighting, 6 providers
6. **Restore Points**: Empty state, card design, refresh
7. **Settings**: Stats grid, accent color picker

### Build System
- BUILD.bat one-click Windows build script
- Single executable publish with self-contained .NET 8
- Admin manifest for registry access

## Project Structure
```
/app/AIO_PC_Tool_v2/
├── App.xaml / App.xaml.cs          # Entry point, DI setup, error handling
├── Helpers/
│   ├── Converters.cs               # XAML value converters
│   └── ThemeManager.cs             # Runtime theme switching
├── Models/Tweak.cs                 # Data models (Tweak, CleanerCategory, etc.)
├── Services/
│   ├── TweakService.cs             # 35+ safe tweaks
│   ├── CleanerService.cs           # 25+ cleaning categories
│   ├── HardwareMonitorService.cs   # Real-time hardware monitoring
│   ├── OtherServices.cs            # DNS, RestorePoint services
│   └── UtilitiesService.cs         # System utilities
├── Styles/
│   ├── ThemeColors.xaml            # Color definitions (Red+Black default)
│   └── CustomStyles.xaml           # Button & UI styles
├── ViewModels/ViewModels.cs        # MVVM view models
├── Views/
│   ├── MainWindow.xaml/.cs         # Main shell with navigation
│   ├── DashboardPage.xaml/.cs      # Hardware monitoring dashboard
│   ├── CleanerPage.xaml/.cs        # Advanced cleaner (Cortex-style)
│   ├── TweaksPage.xaml/.cs         # System tweaks with categories
│   ├── UtilitiesPage.xaml/.cs      # System utilities
│   ├── DnsPage.xaml/.cs            # DNS manager
│   ├── RestorePointsPage.xaml/.cs  # System restore
│   └── SettingsPage.xaml/.cs       # App settings
└── BUILD.bat                       # Windows build script
```

## Testing Instructions
1. Clone/pull repository to Windows machine
2. Run `BUILD.bat` as Administrator
3. Test each feature:
   - Dashboard: Verify hardware monitoring works
   - Cleaner: Scan -> review categories -> Clean selected
   - Tweaks: Apply/revert tweaks, check instant detection
   - DNS: Select provider, apply
   - Utilities: Run CTT, run repair utilities
   - Restore: Create restore point
   - Settings: Change accent color

## Known Requirements
- Run as Administrator for registry tweaks
- Windows 10/11
- .NET 8 Runtime (self-contained in build)

## Future Enhancements (Backlog)
- P1: System tray icon with minimize to tray
- P1: Auto-update functionality
- P2: Export/import tweak profiles
- P2: Scheduled cleaning
- P2: Custom tweak editor
- P2: More Windows 11 24H2 specific tweaks
