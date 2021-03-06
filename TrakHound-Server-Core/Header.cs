﻿// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;

using TH_Global;

namespace TrakHound_Server_Core
{
    public partial class Server
    {

        void PrintHeader()
        {
            Logger.Log("--------------------------------------------------");

            string trakhoundlogo_path = AppDomain.CurrentDomain.BaseDirectory + @"\" + "Header.txt";

            if (File.Exists(trakhoundlogo_path))
            {
                using (StreamReader reader = new StreamReader(trakhoundlogo_path))
                {
                    string trakhoundlogo = reader.ReadToEnd();

                    if (trakhoundlogo.Contains("[v]")) trakhoundlogo = trakhoundlogo.Replace("[v]", GetVersion());

                    Logger.Log(trakhoundlogo);
                }
            }

            Logger.Log("--------------------------------------------------");
        }

        static string GetVersion()
        {
            // Build Information
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;

            return "v" + version.Major.ToString() + "." + version.Minor.ToString() + "." + version.Build.ToString() + "." + version.Revision.ToString();
        }

    }
}
