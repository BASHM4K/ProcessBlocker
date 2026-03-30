using System;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.Win32;

namespace TapokBlock
{
    public class ProcessBlocker
    {
        private const string REGISTRY_PATH = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";
        private const string VALUE_NAME = "DisallowRun";

        public static bool IsRunningAsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public static bool BlockProcess(string processName)
        {
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
                {
                    if (key == null)
                    {
                        // Create the key if it doesn't exist
                        using (RegistryKey newKey = Registry.CurrentUser.CreateSubKey(REGISTRY_PATH))
                        {
                            return BlockProcess(processName);
                        }
                    }

                    // Enable DisallowRun
                    key.SetValue("DisallowRun", 1, RegistryValueKind.DWord);

                    // Create or open the DisallowRun subkey
                    using (RegistryKey? disallowRunKey = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH + @"\DisallowRun", true))
                    {
                        if (disallowRunKey == null)
                        {
                            using (RegistryKey newDisallowRunKey = Registry.CurrentUser.CreateSubKey(REGISTRY_PATH + @"\DisallowRun"))
                            {
                                return AddProcessToDisallowList(newDisallowRunKey, processName);
                            }
                        }
                        else
                        {
                            return AddProcessToDisallowList(disallowRunKey, processName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error blocking process: {ex.Message}");
                return false;
            }
        }

        private static bool AddProcessToDisallowList(RegistryKey key, string processName)
        {
            try
            {
                // Get existing entries
                string[]? valueNames = key.GetValueNames();
                int nextNumber = 1;

                if (valueNames != null && valueNames.Length > 0)
                {
                    // Find the next available number
                    foreach (string valueName in valueNames)
                    {
                        if (int.TryParse(valueName, out int number))
                        {
                            if (number >= nextNumber)
                            {
                                nextNumber = number + 1;
                            }
                        }
                    }
                }

                // Remove .exe if present
                string cleanProcessName = processName.ToLower().Replace(".exe", "");
                
                // Add the process to the list
                key.SetValue(nextNumber.ToString(), cleanProcessName, RegistryValueKind.String);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding process to disallow list: {ex.Message}");
                return false;
            }
        }

        public static bool UnblockProcess(string processName)
        {
            try
            {
                using (RegistryKey? disallowRunKey = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH + @"\DisallowRun", true))
                {
                    if (disallowRunKey == null)
                    {
                        Console.WriteLine("No blocked processes found.");
                        return true;
                    }

                    string[]? valueNames = disallowRunKey.GetValueNames();
                    bool found = false;

                    if (valueNames != null)
                    {
                        string cleanProcessName = processName.ToLower().Replace(".exe", "");
                        
                        foreach (string valueName in valueNames)
                        {
                            string? value = disallowRunKey.GetValue(valueName)?.ToString();
                            if (value != null && value.ToLower() == cleanProcessName)
                            {
                                disallowRunKey.DeleteValue(valueName);
                                found = true;
                                Console.WriteLine($"Process '{processName}' unblocked successfully.");
                            }
                        }
                    }

                    if (!found)
                    {
                        Console.WriteLine($"Process '{processName}' was not found in the block list.");
                    }

                    // Check if there are any remaining blocked processes
                    string[]? remainingValues = disallowRunKey.GetValueNames();
                    if (remainingValues == null || remainingValues.Length == 0)
                    {
                        // Disable DisallowRun if no processes are blocked
                        using (RegistryKey? parentKey = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
                        {
                            parentKey?.DeleteValue("DisallowRun", false);
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error unblocking process: {ex.Message}");
                return false;
            }
        }

        public static List<string> GetBlockedProcesses()
        {
            List<string> blockedProcesses = new List<string>();

            try
            {
                using (RegistryKey? disallowRunKey = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH + @"\DisallowRun", false))
                {
                    if (disallowRunKey != null)
                    {
                        string[]? valueNames = disallowRunKey.GetValueNames();
                        if (valueNames != null)
                        {
                            foreach (string valueName in valueNames)
                            {
                                string? value = disallowRunKey.GetValue(valueName)?.ToString();
                                if (value != null)
                                {
                                    blockedProcesses.Add(value);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting blocked processes: {ex.Message}");
            }

            return blockedProcesses;
        }

        public static void ClearAllBlocks()
        {
            try
            {
                // Delete the entire DisallowRun subkey
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
                {
                    if (key != null)
                    {
                        key.DeleteSubKeyTree("DisallowRun", false);
                        key.DeleteValue("DisallowRun", false);
                        Console.WriteLine("All process blocks have been cleared.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing all blocks: {ex.Message}");
            }
        }
    }
}
