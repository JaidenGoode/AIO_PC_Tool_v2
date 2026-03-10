# JGoode's AIO PC Tool v2 - PRD

## Problem Statement
Rebuild original Electron-based AIO PC Tool as a professional WPF .NET 8 Windows desktop application with a stunning Red + Black theme, excluding all debloat tweaks, with focus on safety, proper revert behavior, and professional customizable UX.

## Architecture
- **Framework**: WPF (.NET 8 LTS)
- **Pattern**: MVVM with CommunityToolkit.Mvvm
- **UI**: Custom themed styles (Red/Black default, multiple accent colors, light/dark mode support)
- **Hardware**: LibreHardwareMonitorLib

## Core Requirements
1. ✅ Professional WPF desktop app with custom theme system
2. ✅ Removed ALL debloat tweaks as requested
3. ✅ Removed "Disable Background Apps (Legacy)" tweak
4. ✅ 53 safe Windows 11 tweaks with PROPER revert to Windows defaults
5. ✅ Real-time auto-detection system
6. ✅ OPTIMIZED/DEFAULT status badges
7. ✅ Hardware monitoring (CPU, GPU, RAM, Disk)
8. ✅ System cleaner with 15 categories
9. ✅ DNS manager with 8 providers
10. ✅ Restore points management
11. ✅ Chris Titus Tech utility integration (admin PowerShell)
12. ✅ 24 utilities organized by category

## What's Implemented (Dec 2025)

### Theme System (NEW - Completed)
- **Default Theme**: Red accent on Black background
- **Dark Mode Colors**: True black (#0A0A0C) backgrounds
- **Light Mode Colors**: Clean white backgrounds with accent colors
- **9 Accent Colors**: Red (default), Blue, Purple, Pink, Teal, Green, Orange, Gold, Cyan
- **Theme Manager**: Runtime theme switching via Settings page
- **DynamicResource**: All UI elements update instantly when theme changes

### Tweaks (53 total) - SAFETY AUDITED ✅
- Privacy: 10 tweaks
- Performance: 12 tweaks  
- Gaming: 12 tweaks
- System: 8 tweaks
- Network: 5 tweaks
- Services: 6 tweaks

### Safety Features - VERIFIED ✅
- ✅ NO registry key deletion - all reverts set explicit Windows default values
- ✅ Audited all 53 tweaks for correct default values
- ✅ Fixed 3 tweaks with incorrect/duplicate revert scripts:
  - SSD TRIM (Enable only - no safe reason to disable)
  - RSS (Enable only - always should be on)
  - Remote Registry (stays disabled - security best practice)
- ✅ Real-time detection after every apply/revert
- ✅ Event-driven UI updates
- ✅ Clear status badges (OPTIMIZED/DEFAULT)
- ✅ Warning labels for special cases

### Chris Titus Tech Utility
- Featured prominently in Utilities page
- Launches: `iwr -useb https://christitus.com/win | iex` in admin PowerShell

### Build System
- BUILD.bat one-click Windows build script
- build.sh for PowerShell/WSL
- Single executable publish with self-contained .NET 8

## Project Structure
```
/app/AIO_PC_Tool_v2/
├── App.xaml / App.xaml.cs          # Entry point, DI setup
├── Helpers/
│   ├── Converters.cs               # XAML value converters
│   └── ThemeManager.cs             # Runtime theme switching (NEW)
├── Models/Tweak.cs                 # Data models
├── Services/
│   ├── TweakService.cs             # 53 safe tweaks (AUDITED)
│   ├── CleanerService.cs
│   ├── HardwareMonitorService.cs
│   ├── OtherServices.cs
│   └── UtilitiesService.cs
├── Styles/
│   ├── ThemeColors.xaml            # Color definitions (Red+Black default)
│   └── CustomStyles.xaml           # UI component styles
├── ViewModels/ViewModels.cs        # MVVM view models
├── Views/
│   ├── MainWindow.xaml             # Main shell (updated for themes)
│   ├── DashboardPage.xaml          # Dashboard (updated for themes)
│   ├── TweaksPage.xaml             # Tweaks (updated for themes)
│   ├── SettingsPage.xaml           # Settings with theme picker (NEW)
│   └── [other pages]
└── BUILD.bat                       # Windows build script
```

## Next Steps for User
1. **Build on Windows**: Clone to Windows machine, run `BUILD.bat`
2. **Test Theme System**: Go to Settings, try different accent colors and light/dark toggle
3. **Test Tweaks**: Apply/revert several tweaks, verify detection works instantly
4. **Push to GitHub**: After testing, push to https://github.com/JaidenGoode/AIO_PC_Tool_v2.git

## Future Enhancements (Backlog)
- P1: System tray icon with minimize to tray
- P1: Auto-update functionality
- P2: Export/import tweak profiles
- P2: Scheduled cleaning
- P2: More Windows 11 24H2 specific tweaks
- P2: Custom tweak editor
