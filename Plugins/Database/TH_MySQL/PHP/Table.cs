﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Collections.Specialized;
using System.Data;
using System.Text;

using TH_Global;
using TH_Global.Web;
using TH_Plugins.Database;

namespace TH_MySQL.PHP
{
    public static class Table
    {
        const string TABLE_DELIMITER_START = "^^^";
        const string TABLE_DELIMITER_END = "~~~";

        public static bool Create(MySQL_Configuration config, string tablename, ColumnDefinition[] columnDefinitions, string[] primaryKey)
        {

            bool Result = false;

            NameValueCollection values = new NameValueCollection();
            if (config.Port > 0) values["server"] = config.Server + ":" + config.Port;
            else values["server"] = config.Server;

            values["user"] = config.Username;
            values["password"] = config.Password;
            values["db"] = config.Database;

            string coldef = "";

            //Create Column Definition string
            for (int x = 0; x <= columnDefinitions.Length - 1; x++)
            {
                coldef += MySQL_Tools.ConvertColumnDefinition(columnDefinitions[x]).ToString();
                if (x < columnDefinitions.Length - 1) coldef += ",";
            }

            string Keydef = "";
            if (primaryKey != null)
            {
                //Keydef = ", PRIMARY KEY (" + primaryKey.ToLower() + ")";
                Keydef = ", PRIMARY KEY (";

                for (var k = 0; k <= primaryKey.Length - 1; k++)
                {
                    Keydef += primaryKey[k];
                    if (k < primaryKey.Length - 1) Keydef += ", ";
                }

                Keydef += ")";
            }

            // Create Table
            values["query1"] = "CREATE TABLE IF NOT EXISTS " + tablename + " (" + coldef + Keydef + ")";

            // Add Missing Columns (if any)

            // Drop Procedure (make sure doesn't already exist)
            values["query2"] = "DROP PROCEDURE IF EXISTS addcolumns";

            // Create Procedure
            string procedure = "CREATE PROCEDURE addcolumns() BEGIN";

            for (int x = 0; x <= columnDefinitions.Length - 1; x++)
            {
                procedure += " IF NOT EXISTS(" +
                    "(SELECT * FROM information_schema.COLUMNS" +
                     " WHERE TABLE_SCHEMA=DATABASE()" + 
                     " AND COLUMN_NAME='" + columnDefinitions[x].ColumnName + "'" +
                     " AND TABLE_NAME='" + tablename + "'))" + 
                     " THEN" +
                     " ALTER TABLE " + tablename + " ADD " + MySQL_Tools.ConvertColumnDefinition(columnDefinitions[x]).ToString() + ";" +
                     " END IF;";
            }

            procedure += "END";

            values["query3"] = procedure;

            values["query4"] = "CALL addcolumns()";

            
            string PHP_Directory = "";
            if (config.PHP_Directory != "") PHP_Directory = "/" + config.PHP_Directory;

            string url = "http://" + config.PHP_Server + PHP_Directory + "/create_table.php";

            string responseString = HTTP.POST(url, values);

            if (responseString.Trim() == "true") Result = true;

            return Result;

        }

        public static bool Replace(MySQL_Configuration config, string tablename, ColumnDefinition[] columnDefinitions, string[] primaryKey)
        {

            bool Result = false;

            NameValueCollection values = new NameValueCollection();
            if (config.Port > 0) values["server"] = config.Server + ":" + config.Port;
            else values["server"] = config.Server;

            values["user"] = config.Username;
            values["password"] = config.Password;
            values["db"] = config.Database;

            string coldef = "";

            //Create Column Definition string
            for (int x = 0; x <= columnDefinitions.Length - 1; x++)
            {
                coldef += MySQL_Tools.ConvertColumnDefinition(columnDefinitions[x]).ToString();
                if (x < columnDefinitions.Length - 1) coldef += ",";
            }

            string Keydef = "";
            if (primaryKey != null)
            {
                //Keydef = ", PRIMARY KEY (" + primaryKey.ToLower() + ")";
                Keydef = ", PRIMARY KEY (";

                for (var k = 0; k <= primaryKey.Length - 1; k++)
                {
                    Keydef += primaryKey[k];
                    if (k < primaryKey.Length - 1) Keydef += ", ";
                }

                Keydef += ")";
            }

            // Drop Table (Replace)
            values["query0"] = "DROP TABLE IF EXISTS " + tablename;

            // Create Table
            values["query1"] = "CREATE TABLE IF NOT EXISTS " + tablename + " (" + coldef + Keydef + ")";

            // Add Missing Columns (if any)

            // Drop Procedure (make sure doesn't already exist)
            values["query2"] = "DROP PROCEDURE IF EXISTS addcolumns";

            // Create Procedure
            string procedure = "CREATE PROCEDURE addcolumns() BEGIN";

            for (int x = 0; x <= columnDefinitions.Length - 1; x++)
            {
                procedure += " IF NOT EXISTS(" +
                    "(SELECT * FROM information_schema.COLUMNS" +
                     " WHERE TABLE_SCHEMA=DATABASE()" +
                     " AND COLUMN_NAME='" + columnDefinitions[x].ColumnName + "'" +
                     " AND TABLE_NAME='" + tablename + "'))" +
                     " THEN" +
                     " ALTER TABLE " + tablename + " ADD " + MySQL_Tools.ConvertColumnDefinition(columnDefinitions[x]).ToString() + ";" +
                     " END IF;";
            }

            procedure += "END";

            values["query3"] = procedure;

            values["query4"] = "CALL addcolumns()";


            string PHP_Directory = "";
            if (config.PHP_Directory != "") PHP_Directory = "/" + config.PHP_Directory;

            string url = "http://" + config.PHP_Server + PHP_Directory + "/replace_table.php";

            string responseString = HTTP.POST(url, values);

            if (responseString.Trim() == "true") Result = true;

            return Result;

        }


        public static bool Drop(MySQL_Configuration config, string tablename)
        {

            bool Result = false;

            NameValueCollection values = new NameValueCollection();
            if (config.Port > 0) values["server"] = config.Server + ":" + config.Port;
            else values["server"] = config.Server;

            values["user"] = config.Username;
            values["password"] = config.Password;
            values["db"] = config.Database;

            values["query"] = "DROP TABLE IF EXISTS " + tablename;


            string PHP_Directory = "";
            if (config.PHP_Directory != "") PHP_Directory = "/" + config.PHP_Directory;

            string url = "http://" + config.PHP_Server + PHP_Directory + "/Send.php";


            if (HTTP.POST(url, values) == "true") Result = true;

            return Result;

        }

        public static bool Drop(MySQL_Configuration config, string[] tablenames)
        {

            bool Result = false;

            NameValueCollection values = new NameValueCollection();
            if (config.Port > 0) values["server"] = config.Server + ":" + config.Port;
            else values["server"] = config.Server;

            values["user"] = config.Username;
            values["password"] = config.Password;
            values["db"] = config.Database;

            string sTablenames = "";
            for (int x = 0; x <= tablenames.Length - 1; x++)
            {
                sTablenames += sTablenames[x];
                if (x < tablenames.Length - 1) sTablenames += ", ";
            }

            values["query"] = "DROP TABLE IF EXISTS " + sTablenames;


            string PHP_Directory = "";
            if (config.PHP_Directory != "") PHP_Directory = "/" + config.PHP_Directory;

            string url = "http://" + config.PHP_Server + PHP_Directory + "/Send.php";


            if (HTTP.POST(url, values) == "true") Result = true;

            return Result;

        }

        public static bool Truncate(MySQL_Configuration config, string tablename)
        {

            bool Result = false;

            NameValueCollection values = new NameValueCollection();
            if (config.Port > 0) values["server"] = config.Server + ":" + config.Port;
            else values["server"] = config.Server;

            values["user"] = config.Username;
            values["password"] = config.Password;
            values["db"] = config.Database;


            values["query"] = "TRUNCATE TABLE " + tablename;


            string PHP_Directory = "";
            if (config.PHP_Directory != "") PHP_Directory = "/" + config.PHP_Directory;

            string url = "http://" + config.PHP_Server + PHP_Directory + "/Send.php";


            if (HTTP.POST(url, values) == "true") Result = true;

            return Result;

        }


        public static string[] List(MySQL_Configuration config)
        {

            List<string> Result = new List<string>();

            NameValueCollection values = new NameValueCollection();
            if (config.Port > 0) values["server"] = config.Server + ":" + config.Port;
            else values["server"] = config.Server;

            values["user"] = config.Username;
            values["password"] = config.Password;
            values["db"] = config.Database;

            values["query"] = "SHOW TABLES";

            string PHP_Directory = "";
            if (config.PHP_Directory != "") PHP_Directory = "/" + config.PHP_Directory;

            string url = "http://" + config.PHP_Server + PHP_Directory + "/Retrieve.php";


            string responseString = HTTP.POST(url, values);

            DataTable dt = JSON.ToTable(responseString);
            if (dt != null) foreach (DataRow Row in dt.Rows) Result.Add(Row[0].ToString()); 

            return Result.ToArray();

        }

        public static string[] List(MySQL_Configuration config, string filterExpression)
        {

            List<string> Result = new List<string>();

            NameValueCollection values = new NameValueCollection();
            if (config.Port > 0) values["server"] = config.Server + ":" + config.Port;
            else values["server"] = config.Server;

            values["user"] = config.Username;
            values["password"] = config.Password;
            values["db"] = config.Database;

            values["query"] = "SHOW TABLES " + filterExpression;

            string PHP_Directory = "";
            if (config.PHP_Directory != "") PHP_Directory = "/" + config.PHP_Directory;

            string url = "http://" + config.PHP_Server + PHP_Directory + "/Retrieve.php";


            string responseString = HTTP.POST(url, values);

            DataTable dt = JSON.ToTable(responseString);
            if (dt != null) foreach (DataRow Row in dt.Rows) Result.Add(Row[0].ToString()); 

            return Result.ToArray();

        }

        public static Int64 GetRowCount(MySQL_Configuration config, string tablename)
        {

            Int64 Result = -1;

            NameValueCollection values = new NameValueCollection();
            if (config.Port > 0) values["server"] = config.Server + ":" + config.Port;
            else values["server"] = config.Server;

            values["user"] = config.Username;
            values["password"] = config.Password;
            values["db"] = config.Database;

            values["query"] = "SELECT COUNT(*) FROM " + tablename;

            string PHP_Directory = "";
            if (config.PHP_Directory != "") PHP_Directory = "/" + config.PHP_Directory;

            string url = "http://" + config.PHP_Server + PHP_Directory + "/Retrieve.php";


            string responseString = HTTP.POST(url, values);

            DataTable dt = JSON.ToTable(responseString);
            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    Int64 rowCount = -1;
                    Int64.TryParse(dt.Rows[0][0].ToString(), out rowCount);
                    if (rowCount >= 0) Result = rowCount;
                }

            return Result;

        }

        public static Int64 GetSize(MySQL_Configuration config, string tablename)
        {

            Int64 Result = -1;

            NameValueCollection values = new NameValueCollection();
            if (config.Port > 0) values["server"] = config.Server + ":" + config.Port;
            else values["server"] = config.Server;

            values["user"] = config.Username;
            values["password"] = config.Password;
            values["db"] = config.Database;

            values["query"] = "SELECT data_length + index_length 'Total Size bytes' FROM information_schema.TABLES WHERE table_schema = '" + config.Database + "' AND table_name = '" + tablename + "'";

            string PHP_Directory = "";
            if (config.PHP_Directory != "") PHP_Directory = "/" + config.PHP_Directory;


            string url = "http://" + config.PHP_Server + PHP_Directory + "/Retrieve.php";


            string responseString = HTTP.POST(url, values);

            DataTable dt = JSON.ToTable(responseString);
            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    Int64 size = -1;
                    Int64.TryParse(dt.Rows[0][0].ToString(), out size);
                    if (size >= 0) Result = size;
                }

            return Result;

        }


        public static DataTable Get(MySQL_Configuration config, string tablename)
        {

            DataTable Result = null;

            NameValueCollection values = new NameValueCollection();
            if (config.Port > 0) values["server"] = config.Server + ":" + config.Port;
            else values["server"] = config.Server;

            values["user"] = config.Username;
            values["password"] = config.Password;
            values["db"] = config.Database;

            values["query"] = "SELECT * FROM " + tablename;

            string PHP_Directory = "";
            if (config.PHP_Directory != "") PHP_Directory = "/" + config.PHP_Directory;

            string url = "http://" + config.PHP_Server + PHP_Directory + "/Retrieve.php";


            string responseString = HTTP.POST(url, values);

            Result = JSON.ToTable(responseString);

            return Result;

        }

        public static DataTable Get(MySQL_Configuration config, string tablename, Int64 limit, Int64 offset)
        {

            DataTable Result = null;

            NameValueCollection values = new NameValueCollection();
            if (config.Port > 0) values["server"] = config.Server + ":" + config.Port;
            else values["server"] = config.Server;

            values["user"] = config.Username;
            values["password"] = config.Password;
            values["db"] = config.Database;

            values["query"] = "SELECT * FROM " + tablename + " LIMIT " + limit.ToString() + " OFFSET " + offset.ToString();

            string PHP_Directory = "";
            if (config.PHP_Directory != "") PHP_Directory = "/" + config.PHP_Directory;

            string url = "http://" + config.PHP_Server + PHP_Directory + "/Retrieve.php";


            string responseString = HTTP.POST(url, values);

            Result = JSON.ToTable(responseString);

            return Result;

        }

        public static DataTable Get(MySQL_Configuration config, string tablename, string FilterExpression)
        {

            DataTable Result = null;

            NameValueCollection values = new NameValueCollection();
            if (config.Port > 0) values["server"] = config.Server + ":" + config.Port;
            else values["server"] = config.Server;

            values["user"] = config.Username;
            values["password"] = config.Password;
            values["db"] = config.Database;

            values["query"] = "SELECT * FROM " + tablename + " " + FilterExpression;

            string PHP_Directory = "";
            if (config.PHP_Directory != "") PHP_Directory = "/" + config.PHP_Directory;

            string url = "http://" + config.PHP_Server + PHP_Directory + "/Retrieve.php";


            string responseString = HTTP.POST(url, values);

            Result = JSON.ToTable(responseString);

            return Result;

        }

        public static DataTable Get(MySQL_Configuration config, string tablename, string FilterExpression, string Columns)
        {

            DataTable Result = null;


            NameValueCollection values = new NameValueCollection();
            if (config.Port > 0) values["server"] = config.Server + ":" + config.Port;
            else values["server"] = config.Server;

            values["user"] = config.Username;
            values["password"] = config.Password;
            values["db"] = config.Database;

            values["query"] = "SELECT " + Columns + " FROM " + tablename + " " + FilterExpression;

            string PHP_Directory = "";
            if (config.PHP_Directory != "") PHP_Directory = "/" + config.PHP_Directory;

            string url = "http://" + config.PHP_Server + PHP_Directory + "/Retrieve.php";


            string responseString = HTTP.POST(url, values);

            Result = JSON.ToTable(responseString);

            return Result;

        }

        public static DataTable[] Get(MySQL_Configuration config, string[] tablenames)
        {
            DataTable[] result = null;

            NameValueCollection values = new NameValueCollection();
            if (config.Port > 0) values["server"] = config.Server + ":" + config.Port;
            else values["server"] = config.Server;

            values["user"] = config.Username;
            values["password"] = config.Password;
            values["db"] = config.Database;

            string tableNames = "";

            foreach (string tablename in tablenames)
            {
                tableNames += tablename + ";";
            }

            values["tablenames"] = tableNames;


            string PHP_Directory = "";
            if (config.PHP_Directory != "") PHP_Directory = "/" + config.PHP_Directory;

            string url = "http://" + config.PHP_Server + PHP_Directory + "/get_tables.php";


            string responseString = HTTP.POST(url, values);

            if (responseString != null)
            {
                var tables = new List<DataTable>();

                string[] tableDatas = responseString.Split(TABLE_DELIMITER_START.ToCharArray());

                foreach (string tableData in tableDatas)
                {
                    if (!String.IsNullOrEmpty(tableData))
                    {
                        var delimiter = tableData.IndexOf(TABLE_DELIMITER_END);
                        if (delimiter > 0)
                        {
                            string tablename = tableData.Substring(0, delimiter);
                            string tabledata = tableData.Substring(delimiter + TABLE_DELIMITER_END.Length);

                            DataTable dt = JSON.ToTable(tabledata);
                            if (dt != null)
                            {
                                dt.TableName = tablename;
                                tables.Add(dt);
                            }
                        }
                    }
                }

                result = tables.ToArray();
            }

            return result;
        }

        public static DataTable[] Get(MySQL_Configuration config, string[] tablenames, string[] filterExpressions)
        {
            DataTable[] result = null;

            NameValueCollection values = new NameValueCollection();
            if (config.Port > 0) values["server"] = config.Server + ":" + config.Port;
            else values["server"] = config.Server;

            values["user"] = config.Username;
            values["password"] = config.Password;
            values["db"] = config.Database;

            // Table Names
            string tableNames = "";
            foreach (string tablename in tablenames)
            {
                tableNames += tablename + ";";
            }

            // Filter Expressions
            string filters = "";
            foreach (string filterExpression in filterExpressions)
            {
                filters += filterExpression + ";";
            }

            values["tablenames"] = tableNames;
            values["filterexpressions"] = filters;


            string PHP_Directory = "";
            if (config.PHP_Directory != "") PHP_Directory = "/" + config.PHP_Directory;

            string url = "http://" + config.PHP_Server + PHP_Directory + "/get_tables.php";


            string responseString = HTTP.POST(url, values);

            if (responseString != null)
            {
                var tables = new List<DataTable>();

                string[] tableDatas = responseString.Split(TABLE_DELIMITER_START.ToCharArray());

                foreach (string tableData in tableDatas)
                {
                    if (!String.IsNullOrEmpty(tableData))
                    {
                        var delimiter = tableData.IndexOf(TABLE_DELIMITER_END);
                        if (delimiter > 0)
                        {
                            string tablename = tableData.Substring(0, delimiter);
                            string tabledata = tableData.Substring(delimiter + TABLE_DELIMITER_END.Length);

                            DataTable dt = JSON.ToTable(tabledata);
                            if (dt != null)
                            {
                                dt.TableName = tablename;
                                tables.Add(dt);
                            }
                        }
                    }
                }

                result = tables.ToArray();
            }

            return result;
        }

        public static DataTable[] Get(MySQL_Configuration config, string[] tablenames, string[] filterExpressions, string[] columns)
        {
            DataTable[] result = null;

            NameValueCollection values = new NameValueCollection();
            if (config.Port > 0) values["server"] = config.Server + ":" + config.Port;
            else values["server"] = config.Server;

            values["user"] = config.Username;
            values["password"] = config.Password;
            values["db"] = config.Database;

            // Table Names
            string tableNameString = "";
            foreach (string tablename in tablenames)
            {
                tableNameString += tablename + ";";
            }

            // Filter Expressions
            string filterString = "";
            foreach (string filterExpression in filterExpressions)
            {
                filterString += filterExpression + ";";
            }

            // Columns
            string columnString = "";
            foreach (string column in columns)
            {
                columnString += column + ";";
            }

            values["tablenames"] = tableNameString;
            values["filterexpressions"] = filterString;
            values["columns"] = columnString;


            string PHP_Directory = "";
            if (config.PHP_Directory != "") PHP_Directory = "/" + config.PHP_Directory;

            string url = "http://" + config.PHP_Server + PHP_Directory + "/get_tables.php";


            string responseString = HTTP.POST(url, values);

            if (responseString != null)
            {
                var tables = new List<DataTable>();

                string[] tableDatas = responseString.Split(TABLE_DELIMITER_START.ToCharArray());

                foreach (string tableData in tableDatas)
                {
                    if (!String.IsNullOrEmpty(tableData))
                    {
                        var delimiter = tableData.IndexOf(TABLE_DELIMITER_END);
                        if (delimiter > 0)
                        {
                            string tablename = tableData.Substring(0, delimiter);
                            string tabledata = tableData.Substring(delimiter + TABLE_DELIMITER_END.Length);

                            DataTable dt = JSON.ToTable(tabledata);
                            if (dt != null)
                            {
                                dt.TableName = tablename;
                                tables.Add(dt);
                            }
                        }
                    }
                }

                result = tables.ToArray();
            }

            return result;
        }

    }
}
