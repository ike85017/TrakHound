﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

using TH_Configuration;
using TH_Database;
using TH_Global;
using TH_Global.Functions;
using TH_UserManagement;
using TH_Plugins.Database;

namespace TH_UserManagement.Management.Local
{

    public static class Configurations
    {
        static string[] primaryKey = { "address" };


        public static bool Add(UserConfiguration userConfig, DeviceConfiguration configuration, Database_Settings db)
        {
            Users.CreateUserTable(db);

            // Set Table Name
            string tableName = CreateTableName(userConfig);
            configuration.TableName = tableName;
            XML_Functions.SetInnerText(configuration.ConfigurationXML, "TableName", tableName);

            // Set new Unique Id
            string uniqueId = DeviceConfiguration.GenerateUniqueID();
            configuration.UniqueId = uniqueId;
            XML_Functions.SetInnerText(configuration.ConfigurationXML, "UniqueId", uniqueId);

            // Set Enabled to False
            //configuration.ClientEnabled = false;
            //configuration.ServerEnabled = false;
            //XML_Functions.SetInnerText(configuration.ConfigurationXML, "ClientEnabled", "false");
            //XML_Functions.SetInnerText(configuration.ConfigurationXML, "ServerEnabled", "false");

            DataTable dt = TH_Configuration.Converter.XMLToTable(configuration.ConfigurationXML);
            if (dt != null)
            {
                Create(tableName, db);

                Update(tableName, dt, db);
            }

            return true;
        }

        public static string[] GetList(UserConfiguration userConfig, Database_Settings db)
        {
            return Table.List(db, "LIKE '" + userConfig.username + "%'");
        }

        public static List<DeviceConfiguration> Get(UserConfiguration userConfig, Database_Settings db)
        {
            var result = new List<DeviceConfiguration>();

            string[] tables = Table.List(db, "LIKE '" + userConfig.username + "%'");
            if (tables != null)
            {
                foreach (string table in tables)
                {
                    DataTable dt = Table.Get(db, table);
                    if (dt != null)
                    {
                        XmlDocument xml = Converter.TableToXML(dt);
                        if (xml != null)
                        {
                            var config = DeviceConfiguration.Read(xml);
                            if (config != null)
                            {
                                //config.Remote = true;
                                config.TableName = table;
                                result.Add(config);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static List<DataTable> GetDeviceInfos(UserConfiguration userConfig, Database_Settings db)
        {
            var result = new List<DataTable>();

            string[] tables = Table.List(db, "LIKE '" + userConfig.username + "%'");
            if (tables != null)
            {
                foreach (string table in tables)
                {
                    DataTable dt = Table.Get(db, table);
                    if (dt != null)
                    {
                        dt.TableName = table;
                        result.Add(dt);
                    }
                }
            }

            return result;
        }

        public static DataTable GetTable(string table, Database_Settings db)
        {
            return Table.Get(db, table);
        }

        public static bool Create(string tableName, Database_Settings db)
        {
            ColumnDefinition[] Columns = new ColumnDefinition[]
            {
                new ColumnDefinition("address", DataType.LargeText),
                new ColumnDefinition("name", DataType.LargeText),
                new ColumnDefinition("value", DataType.LargeText),
                new ColumnDefinition("attributes", DataType.LargeText)
            };

            Table.Create(db, tableName, Columns, primaryKey);

            return true;
        }

        public static bool UpdateIndexes(List<Tuple<string, int>> items, Database_Settings db)
        {
            bool result = false;

            if (items != null)
            {
                string columns = " (address, value) ";
                string query = "";

                foreach (var item in items)
                {
                    string tablename = item.Item1;
                    string index = item.Item2.ToString();

                    string set = " VALUES ('/Index', '" + index + "')";

                    string update = " ON DUPLICATE KEY UPDATE value='" + index + "'";

                    query += "INSERT IGNORE INTO " + tablename + columns + set + update + "; ";
                }

                Etc.CustomCommand(db, query);
            }

            return result;
        }

        public static bool Update(string tableName, DataTable dt, Database_Settings db)
        {
            bool result = false;

            //DataTable dt = TH_Configuration.Converter.XMLToTable(configuration.ConfigurationXML);
            if (dt != null)
            {
                // Add Columns
                List<string> columns = new List<string>();
                foreach (DataColumn col in dt.Columns) columns.Add(col.ColumnName);

                List<List<object>> rowValues = new List<List<object>>();

                foreach (DataRow row in dt.Rows)
                {
                    List<object> values = new List<object>();
                    foreach (object val in row.ItemArray) values.Add(val);
                    rowValues.Add(values);
                }

                //string table = GetConfigurationTableName(tableName, userConfig, configuration);

                Row.Insert(db, tableName, columns.ToArray(), rowValues, primaryKey, true);

                result = true;
            }

            return result;
        }

        public static bool Update(string address, string value, string tableName, Database_Settings db)
        {
            bool result = false;

            // Add Columns
            List<string> columns = new List<string>();
            columns.Add("address");
            columns.Add("value");

            // Add Values
            var values = new List<object>();
            values.Add(address);
            values.Add(value);

            Row.Insert(db, tableName, columns.ToArray(), values.ToArray(), primaryKey, true);

            result = true;

            return result;
        }

        public static bool Update(string address, string value, string attributes, string tableName, Database_Settings db)
        {
            bool result = false;

            // Add Columns
            List<string> columns = new List<string>();
            columns.Add("address");
            columns.Add("value");
            columns.Add("attributes");

            // Add Values
            var values = new List<object>();
            values.Add(address);
            values.Add(value);
            values.Add(attributes);

            Row.Insert(db, tableName, columns.ToArray(), values.ToArray(), primaryKey, true);

            result = true;

            return result;
        }

        public static bool Clear(string tableName, Database_Settings db)
        {
            Table.Truncate(db, tableName);

            return true;
        }

        public static bool Remove(string tableName, Database_Settings db)
        {
            Table.Drop(db, tableName);

            return true;
        }

        static string CreateTableName(UserConfiguration userConfig)
        {
            return userConfig.username.ToLower() + "_" + String_Functions.RandomString(20);
        }

    }

}
