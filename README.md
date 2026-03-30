This is AI code
# TapokBlock - Process Blocker

A C# application for managing Windows process execution through Registry settings.

## Description

TapokBlock is a console application that allows you to block and unblock specific processes from running on Windows by modifying the system registry. This is useful for preventing unwanted applications from launching.

## Features

- **Block Processes**: Prevent specific processes from running (e.g., Yandex Browser's `browser.exe`)
- **Unblock Processes**: Remove process blocks when needed
- **View Blocked Processes**: See all currently blocked processes
- **Clear All Blocks**: Remove all process blocks at once
- **Administrator Rights Check**: Ensures proper privileges for registry modifications

## Security

This application requires **administrator privileges** to modify the Windows registry. Always run as Administrator.

## Getting Started

### Prerequisites

- Windows operating system
- .NET 8.0 SDK or later
- Administrator privileges

### Installation

1. Clone or download the project
2. Navigate to the project directory
3. Build the application:
   ```bash
   dotnet build
   ```

### Running the Application

1. **Run as Administrator**:
   - Right-click on Command Prompt or PowerShell
   - Select "Run as administrator"
   - Navigate to the project directory
   - Run:
     ```bash
   dotnet run
     ```

2. **Or build and run the executable**:
   ```bash
   dotnet build -c Release
   ```
   Then run the generated `.exe` file as administrator.

## Usage

### Main Menu Options

1. **Block a process**: Enter a process name (without .exe extension)
   - Example: `browser` for Yandex Browser
   - Example: `chrome` for Google Chrome

2. **Unblock a process**: Remove a process from the block list

3. **View blocked processes**: See all currently blocked processes

4. **Clear all blocks**: Remove all process blocks (requires confirmation)

5. **Exit**: Close the application

### Example Usage

To block Yandex Browser:
1. Select option 1 (Block a process)
2. Enter: `browser`
3. Confirm the action

To unblock Yandex Browser:
1. Select option 2 (Unblock a process)
2. Enter: `browser`
3. Confirm the action

## How It Works

The application modifies Windows Registry settings:
- Creates/Modifies: `HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer`
- Sets `DisallowRun=1` to enable process blocking
- Adds process names to `HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\DisallowRun`

## Important Notes

- **Restart Required**: Changes may require a system restart or log off to take effect
- **Process Names**: Use only the process name without `.exe` extension
- **Backup**: The application doesn't automatically backup registry settings
- **System-wide**: These settings apply to the current user only

## Troubleshooting

### "Access Denied" Errors
- Ensure you're running as Administrator
- Check if your user account has registry modification permissions

### Changes Not Taking Effect
- Restart your computer or log off and log back in
- Verify the process name is correct (check Task Manager for exact process name)

### Cannot Find Blocked Processes
- Use option 3 to view all blocked processes
- Check if the process name was entered correctly

## Project Structure

- `Program.cs` - Main application with user interface
- `ProcessBlocker.cs` - Core registry operations
- `TapokBlock.csproj` - Project configuration

## Technologies Used

- C# (.NET 8.0)
- Windows Registry API
- Console Application Framework

## License

This project is open source and available under the MIT License.

## Disclaimer

This application modifies Windows Registry settings. Use at your own risk. Always backup your system before making registry changes.
