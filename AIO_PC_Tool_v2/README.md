# JGoode's AIO PC Tool v2

**Professional Windows 11 optimization, privacy, and gaming performance tool**

*53 safe tweaks · Real-time hardware monitoring · System cleaner · DNS manager · Restore points · Chris Titus Tech utility*

![Version](https://img.shields.io/badge/version-4.0.0-purple?style=flat-square)
![Platform](https://img.shields.io/badge/platform-Windows%2011-blue?style=flat-square)
![Framework](https://img.shields.io/badge/framework-.NET%208%20WPF-green?style=flat-square)

---

## ✨ Features

### 🔧 System Tweaks (53 Safe Optimizations)

All tweaks are **100% safe** and **fully reversible**. Each tweak:
- Restores exact Windows 11 defaults when reverted (no registry deletion!)
- Has real-time auto-detection
- Shows clear OPTIMIZED / DEFAULT status
- Includes warning labels for special cases

| Category | Count | Examples |
|----------|-------|----------|
| **Privacy** | 10 | Disable telemetry, advertising ID, Copilot, activity tracking |
| **Performance** | 12 | Ultimate Performance plan, disable SuperFetch, MPO fix |
| **Gaming** | 12 | Mouse acceleration, Game Bar, HAGS, Nagle's algorithm |
| **System** | 8 | Classic context menu, disable Widgets, align taskbar |
| **Network** | 5 | Delivery Optimization, SMBv1, RSS |
| **Services** | 6 | Print Spooler, Xbox services, DiagTrack |

**Presets:** Apply Gaming, Privacy, or Performance tweaks in one click!

---

### 🖥️ Hardware Monitor
Real-time monitoring using LibreHardwareMonitor:
- CPU usage, temperature, clock speed
- GPU usage and temperature  
- RAM usage percentage
- Disk activity

---

### 🧹 System Cleaner
Scans and removes junk from 15 categories:
- Windows temp files & prefetch
- Browser caches (Chrome, Edge, Firefox)
- App caches (Discord, Spotify, Steam)
- GPU shader caches (NVIDIA, AMD, DirectX)
- Recycle Bin

---

### 🌐 DNS Manager
Switch DNS providers with one click:
- Cloudflare (1.1.1.1) - Fast & private
- Cloudflare Malware Block
- Google (8.8.8.8)
- Quad9 - Threat blocking
- OpenDNS, NextDNS, AdGuard
- Reset to DHCP default

---

### 💾 Restore Points
Create and view Windows System Restore points directly in the app.

---

### 🛠️ Utilities

**Featured: Chris Titus Tech Windows Utility**
- Launches `iwr -useb https://christitus.com/win | iex` in admin PowerShell

Plus 20+ quick tools:
- System repair: SFC, DISM, CHKDSK
- Network: Flush DNS, Reset Winsock, IP release/renew
- Windows tools: Disk Cleanup, Defrag, Device Manager
- Power reports, Event Viewer, Services manager

---

## 🚀 Quick Start

### Requirements
- **Windows 10/11** (Windows 11 recommended)
- **.NET 8.0 Desktop Runtime**
- **Administrator privileges** (for applying tweaks)

### Download & Install

#### Option 1: Build from Source
```cmd
# Clone the repository
git clone https://github.com/JaidenGoode/AIO_PC_Tool_v2.git
cd AIO_PC_Tool_v2

# Run the build script
BUILD.bat
```

The executable will be at: `publish\AIO_PC_Tool_v2.exe`

#### Option 2: Download Release
Download the latest release from the [Releases page](https://github.com/JaidenGoode/AIO_PC_Tool_v2/releases).

### Running
**Right-click the .exe → Run as Administrator**

---

## 🔒 Safety Guarantees

### Every Tweak is Safe:
1. **No registry deletion** - Reverts restore exact Windows default values
2. **Documented defaults** - Each script includes the Windows 11 default value
3. **Error handling** - Scripts fail safely without corrupting settings
4. **Real-time detection** - See actual system state, not assumed state

### Example: Telemetry Tweak
```
Enable:  AllowTelemetry = 0 (Security level)
Revert:  AllowTelemetry = 3 (Full - Windows 11 default)
```

---

## 📁 Project Structure

```
AIO_PC_Tool_v2/
├── Models/           # Data models (Tweak, SystemInfo, etc.)
├── ViewModels/       # MVVM ViewModels with real-time binding
├── Views/            # WPF Pages (Dashboard, Tweaks, Cleaner, etc.)
├── Services/         # Business logic
│   ├── TweakService.cs      # 53 safe tweaks with auto-detection
│   ├── CleanerService.cs    # 15 clean categories
│   ├── UtilitiesService.cs  # 24 utilities including CTT
│   └── ...
├── Helpers/          # Value converters
├── Styles/           # XAML styles and themes
├── BUILD.bat         # One-click build script
└── README.md
```

---

## 🛠️ Tech Stack

| Component | Technology |
|-----------|------------|
| Framework | WPF (.NET 8) |
| UI | Material Design In XAML Toolkit |
| MVVM | CommunityToolkit.Mvvm |
| Hardware | LibreHardwareMonitorLib |
| JSON | Newtonsoft.Json |
| Tweaks | PowerShell via System.Diagnostics.Process |

---

## ⚠️ Disclaimer

This tool modifies Windows registry keys, services, and system settings. All changes are **fully reversible** - every tweak has a revert script that restores exact Windows defaults.

**Recommended:** Create a restore point before applying multiple tweaks (the app can do this for you).

---

## 📜 License

MIT License - see [LICENSE](LICENSE) file.

---

Made with ❤️ by [JaidenGoode](https://github.com/JaidenGoode)
