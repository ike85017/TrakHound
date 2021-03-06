﻿// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Xml;
using System.IO;

using TH_Global.Functions;

namespace TH_Global.TrakHound.Users
{
    public static class UserLoginFile
    {
        public static bool Create(UserConfiguration userConfig)
        {
            bool result = false;

            FileLocations.CreateAppDataDirectory();
            string path = FileLocations.AppData + @"\nigolresu";

            Remove(path);

            if (userConfig != null)
            {
                var xml = CreateDocument(userConfig);
                WriteDocument(xml, path);
            }

            return result;
        }

        public class LoginData
        {
            public string Username { get; set; }
            public string Token { get; set; }
        }

        public static LoginData Read()
        {
            LoginData result = null;

            string path = FileLocations.AppData + @"\nigolresu";
            if (File.Exists(path))
            {
                try
                {
                    var xml = new XmlDocument();
                    xml.Load(path);

                    string username = XML_Functions.GetInnerText(xml, "Username");
                    string token = XML_Functions.GetInnerText(xml, "Token");

                    if (username != null && token != null)
                    {
                        result = new LoginData();
                        result.Username = username;
                        result.Token = token;
                    }
                }
                catch (Exception ex) { Logger.Log("UserLoginFile.Read() :: Exception :: " + ex.Message); }
            }

            return result;
        }


        private static XmlDocument CreateDocument(UserConfiguration userConfig)
        {
            var result = new XmlDocument();

            XmlNode docNode = result.CreateXmlDeclaration("1.0", "UTF-8", null);
            result.AppendChild(docNode);

            XmlNode root = result.CreateElement("UserLogin");
            result.AppendChild(root);

            // Username
            XmlNode username = result.CreateElement("Username");
            username.InnerText = userConfig.Username;
            root.AppendChild(username);

            // Token
            XmlNode token = result.CreateElement("Token");
            token.InnerText = userConfig.Token;
            root.AppendChild(token);

            return result;
        }

        public static void Remove()
        {
            string path = FileLocations.AppData + @"\nigolresu";
            Remove(path);
        }

        private static void Remove(string path)
        {
            if (File.Exists(path)) File.Delete(path);
        }

        private static void WriteDocument(XmlDocument doc, string path)
        {
            var settings = new XmlWriterSettings();
            settings.Indent = false;

            try
            {
                using (var writer = XmlWriter.Create(path, settings))
                {
                    doc.Save(writer);
                }
            }
            catch (Exception ex) { Logger.Log("UserLoginFile.WriteDocument() :: Exception :: " + ex.Message); }
        }

    }
}
