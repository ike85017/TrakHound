﻿// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Win32;

namespace TrakHound_Updater
{
    /// <summary>
    /// Functions to Set/Get/Delete Registry Keys for TrakHound Updater
    /// </summary>
    static class Registry
    {

        public const string ROOT_KEY = "Software";
        public const string APP_KEY = "TrakHound";


        public static void SetKey(string keyName, object keyValue, string groupName = null)
        {
            try
            {
                // Open CURRENT_USER/Software Key
                RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(ROOT_KEY, true);

                // Create/Open CURRENT_USER/Software/TrakHound Key
                key = key.CreateSubKey(APP_KEY);

                // Create/Open CURRENT_USER/Software/TrakHound/[groupName] Key
                if (groupName != null) key = key.CreateSubKey(groupName);

                // Update value for [keyName] to [keyValue]
                key.SetValue(keyName, keyValue);
            }
            catch { }
        }

        public static string GetKey(string keyName, string groupName = null)
        {
            string result = null;

            try
            {
                // Open CURRENT_USER/Software Key
                RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(ROOT_KEY, true);

                // Open CURRENT_USER/Software/TrakHound Key
                key = key.OpenSubKey(APP_KEY);

                // Open CURRENT_USER/Software/TrakHound/[groupName] Key
                if (groupName != null) key = key.OpenSubKey(groupName);

                // Read value for [keyName] to [keyValue]
                result = key.GetValue(keyName).ToString();
            }
            catch { }

            return result;
        }

        public static string[] GetKeyNames(string groupName = null)
        {
            string[] result = null;

            try
            {
                // Open CURRENT_USER/Software Key
                RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(ROOT_KEY, true);

                // Open CURRENT_USER/Software/TrakHound Key
                key = key.OpenSubKey(APP_KEY);

                // Open CURRENT_USER/Software/TrakHound/[groupName] Key
                if (groupName != null) key = key.OpenSubKey(groupName);

                result = key.GetSubKeyNames();
            }
            catch { }

            return result;
        }

        public static void DeleteValue(string keyName, string groupName = null)
        {
            try
            {
                // Open CURRENT_USER/Software Key
                RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(ROOT_KEY, true);

                // Open CURRENT_USER/Software/TrakHound Key
                key = key.OpenSubKey(APP_KEY, true);

                // Open CURRENT_USER/Software/TrakHound/[groupName] Key
                if (groupName != null) key = key.OpenSubKey(groupName, true);

                // Delete CURRENT_USER/Software/TrakHound/[groupName]/[keyName] Key
                key.DeleteValue(keyName, true);
            }
            catch { }
        }

    }
}