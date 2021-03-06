﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Xml;

using TH_Configuration;
using TH_Global;

namespace TH_UserManagement.Management
{
    public class UserConfiguration
    {
        public string username { get; set; }
        public string hash { get; set; }
        public int salt { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string company { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string image_url { get; set; }
        public DateTime last_login { get; set; }
        public int device_limit { get; set; }
        public int device_count { get; set; }
        public int user_level { get; set; }
        public int plan_type { get; set; }

        public static UserConfiguration FromNewUserConfiguration(TH_Global.TrakHound.Users.UserConfiguration userConfig)
        {
            var result = new UserConfiguration();

            result.username = userConfig.Username;

            return result;
        }

        public static UserConfiguration GetFromDataRow(DataRow row)
        {
            UserConfiguration result = new UserConfiguration();

            foreach (System.Reflection.PropertyInfo info in typeof(UserConfiguration).GetProperties())
            {
                if (info.Name == "last_login") result.last_login = DateTime.UtcNow;
                else
                {
                    if (row.Table.Columns.Contains(info.Name))
                    {
                        object value = row[info.Name];

                        if (value != DBNull.Value)
                        {
                            Type t = info.PropertyType;
                            info.SetValue(result, Convert.ChangeType(value, t), null);
                        }
                    }
                }
            }

            return result;
        }
    }

    public class UserManagementSettings
    {

        public static Database_Settings Database { get; set; }

        public static UserManagementSettings ReadConfiguration()
        {
            string path = FileLocations.TrakHound + @"\UserConfiguration.xml";
            return ReadConfiguration(path);
        }

        public static UserManagementSettings ReadConfiguration(string filepath)
        {
            UserManagementSettings result = null;

            string rootPath;
            rootPath = System.IO.Path.GetDirectoryName(filepath);
            rootPath += @"\";

            if (System.IO.File.Exists(filepath))
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filepath);

                    result = new UserManagementSettings();

                    foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                    {
                        switch (node.Name.ToLower())
                        {
                            //case "users": result.Users = Process_Users(node); break;
                            case "databases": Database = Process_Databases(node); break;
                            case "database": Database = Process_Databases(node); break;
                        }
                    }

                    Logger.Log("User Configuration Successfully Read From : " + filepath);
                }
                catch (Exception ex)
                {
                    Logger.Log("Error During User Configuration Read :: " + filepath);
                }
            }

            return result;
        }

        static Database_Settings Process_Databases(XmlNode node)
        {
            Database_Settings result = new Database_Settings();

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    var db = new Database_Configuration();
                    db.Type = child.Name;
                    db.Node = child;
                    result.Databases.Add(db);
                }
            }

            TH_Database.Global.Initialize(result);

            return result;
        }

    }
}
