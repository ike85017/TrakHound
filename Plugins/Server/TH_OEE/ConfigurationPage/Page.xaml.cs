﻿// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using TH_Global.Functions;
using TH_Plugins;
using TH_Plugins.Server;
using TH_UserManagement.Management;

namespace TH_OEE.ConfigurationPage
{
    /// <summary>
    /// Interaction logic for Page.xaml
    /// </summary>
    public partial class Page : UserControl, IConfigurationPage
    {
        public Page()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string Title { get { return "OEE"; } }

        private BitmapImage _image;
        public ImageSource Image
        {
            get
            {
                if (_image == null)
                {
                    _image = new BitmapImage(new Uri("pack://application:,,,/TH_OEE;component/Resources/Chart_01.png"));
                    _image.Freeze();
                }

                return _image;
            }
        }

        public event SettingChanged_Handler SettingChanged;

        public bool Loaded { get; set; }

        public event SendData_Handler SendData;

        public void GetSentData(EventData data)
        {

        }


        public void LoadConfiguration(DataTable dt)
        {
            configurationTable = dt;

            GeneratedEvents.Clear();

            genEvents = GetGeneratedEvents(dt);

            if (genEvents != null)
            {
                foreach (Event e in genEvents)
                {
                    GeneratedEvents.Add(String_Functions.UppercaseFirst(e.name.Replace('_', ' ')));
                }
            }

            LoadOEEData(dt);
        }

        public void SaveConfiguration(DataTable dt)
        {
            if (SelectedAvailabilityEvent != null) Table_Functions.UpdateTableValue(SelectedAvailabilityEvent.ToString().Replace(' ','_'), "/OEE/Availability/Event", dt);

            if (SelectedAvailabilityValue != null) Table_Functions.UpdateTableValue(SelectedAvailabilityValue.ToString(), "/OEE/Availability/Value", dt);

            configurationTable = dt;
        }

        DataTable configurationTable;


        void LoadOEEData(DataTable dt)
        {
            LoadAvailabilityData(dt);
        }

        #region "Availability"

        void LoadAvailabilityData(DataTable dt)
        {
            string address = "/OEE/Availability/";

            string filter = "address LIKE '" + address + "*'";
            DataView dv = dt.AsDataView();
            dv.RowFilter = filter;
            DataTable temp_dt = dv.ToTable();
            temp_dt.PrimaryKey = new DataColumn[] { temp_dt.Columns["address"] };

            // Get Events
            foreach (DataRow row in temp_dt.Rows)
            {
                string node = Table_Functions.GetLastNode(row);
                switch (node.ToLower())
                {
                    case "event": SelectedAvailabilityEvent = String_Functions.UppercaseFirst(row["value"].ToString().Replace('_', ' ')); break;

                    case "value": SelectedAvailabilityValue = String_Functions.UppercaseFirst(row["value"].ToString().Replace('_', ' ')); break;
                }
            }
        }

        ObservableCollection<object> availabilityeventvalues;
        public ObservableCollection<object> AvailabilityEventValues
        {
            get
            {
                if (availabilityeventvalues == null)
                    availabilityeventvalues = new ObservableCollection<object>();
                return availabilityeventvalues;
            }

            set
            {
                availabilityeventvalues = value;
            }
        }


        public object SelectedAvailabilityEvent
        {
            get { return (object)GetValue(SelectedAvailabilityEventProperty); }
            set { SetValue(SelectedAvailabilityEventProperty, value); }
        }

        public static readonly DependencyProperty SelectedAvailabilityEventProperty =
            DependencyProperty.Register("SelectedAvailabilityEvent", typeof(object), typeof(Page), new PropertyMetadata(null));


        public object SelectedAvailabilityValue
        {
            get { return (object)GetValue(SelectedAvailabilityValueProperty); }
            set { SetValue(SelectedAvailabilityValueProperty, value); }
        }

        public static readonly DependencyProperty SelectedAvailabilityValueProperty =
            DependencyProperty.Register("SelectedAvailabilityValue", typeof(object), typeof(Page), new PropertyMetadata(null));


        private void AvailabilityEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = null;

            ComboBox cmbox = (ComboBox)sender;
            if (cmbox.SelectedItem != null) selectedItem = cmbox.SelectedItem.ToString();

            if (selectedItem != null)
            {
                AvailabilityEventValues.Clear();

                if (genEvents != null)
                {
                    Event ev = genEvents.Find(x => String_Functions.UppercaseFirst(x.name.Replace('_', ' ')).ToLower() == selectedItem.ToLower());
                    if (ev != null)
                    {
                        if (ev.values != null)
                        {
                            foreach (Value v in ev.values)
                            {
                                if (v.result != null)
                                {
                                    AvailabilityEventValues.Add(String_Functions.UppercaseFirst(v.result.value));
                                }
                            }
                        }

                        if (ev.Default != null)
                        {
                            if (ev.Default.value != null)
                            {
                                AvailabilityEventValues.Add(String_Functions.UppercaseFirst(ev.Default.value));
                            }
                        }
                    }
                }
            }

            if (cmbox.IsKeyboardFocused || cmbox.IsMouseCaptured) if (SettingChanged != null) SettingChanged("Availability Event", null, null);
        }

        private void AvailabilityValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmbox = (ComboBox)sender;

            if (cmbox.IsKeyboardFocused || cmbox.IsMouseCaptured) if (SettingChanged != null) SettingChanged("Availability Value", null, null);
        }

        #endregion

        #region "Generated Events"

        #region "Sub Classes"

        public class Event
        {
            public Event() { values = new List<Value>(); }

            public List<Value> values { get; set; }

            public int id { get; set; }
            public string name { get; set; }

            public Result Default { get; set; }

            public string description { get; set; }
        }

        public class Value
        {
            public Value() { triggers = new List<Trigger>(); }

            public List<Trigger> triggers { get; set; }

            public int id { get; set; }

            public Result result { get; set; }
        }

        public class Result
        {
            public int numval { get; set; }
            public string value { get; set; }
        }

        #endregion

        ObservableCollection<object> generatedevents;
        public ObservableCollection<object> GeneratedEvents
        {
            get
            {
                if (generatedevents == null)
                    generatedevents = new ObservableCollection<object>();
                return generatedevents;
            }

            set
            {
                generatedevents = value;
            }
        }

        List<Event> genEvents;

        List<Event> GetGeneratedEvents(DataTable dt)
        {
            List<Event> result = new List<Event>();

            string address = "/GeneratedData/GeneratedEvents/";

            string filter = "address LIKE '" + address + "*'";
            DataView dv = dt.AsDataView();
            dv.RowFilter = filter;
            DataTable temp_dt = dv.ToTable();
            temp_dt.PrimaryKey = new DataColumn[] { temp_dt.Columns["address"] };

            // Get Events
            foreach (DataRow row in temp_dt.Rows)
            {
                Event e = GetEventFromRow(result, row);
                if (e != null) result.Add(e);
            }

            // Get Values
            foreach (Event e in result)
            {
                string eventfilter = "address LIKE '" + address + "Event||" + e.id.ToString("00") + "/";
                dv = dt.AsDataView();
                dv.RowFilter = eventfilter + "*'";
                temp_dt = dv.ToTable();
                temp_dt.PrimaryKey = new DataColumn[] { temp_dt.Columns["address"] };

                foreach (DataRow row in temp_dt.Rows)
                {
                    Value v = GetValueFromRow(e, row);
                    if (v != null) e.values.Add(v);

                    GetDefaultFromRow(e, row);
                }

                foreach (Value v in e.values)
                {
                    filter = eventfilter + "Value||" + v.id.ToString("00") + "/" + "*'";
                    dv = dt.AsDataView();
                    dv.RowFilter = filter;
                    temp_dt = dv.ToTable();
                    temp_dt.PrimaryKey = new DataColumn[] { temp_dt.Columns["address"] };

                    foreach (DataRow row in temp_dt.Rows)
                    {
                        //Trigger t = GetTriggerFromRow(v, row);
                        //if (t != null) v.triggers.Add(t);

                        GetResultFromRow(v, row);
                    }
                }
            }

            return result;
        }

        Event GetEventFromRow(List<Event> genEvents, DataRow row)
        {
            Event result = null;

            string adr = row["address"].ToString();

            string lastNode = Table_Functions.GetLastNode(row);

            if (lastNode != null)
            {
                if (lastNode.ToLower() == "event")
                {
                    int eventIndex = adr.IndexOf("Event");
                    int slashIndex = adr.IndexOf('/', eventIndex) + 1;
                    int separatorIndex = adr.IndexOf("||", slashIndex);

                    if (separatorIndex > slashIndex)
                    {
                        string name = Table_Functions.GetAttribute("name", row);
                        string strId = adr.Substring(separatorIndex + 2, 2);

                        int id;
                        if (int.TryParse(strId, out id))
                        {
                            Event e = genEvents.Find(x => x.id == id);
                            if (e == null)
                            {
                                result = new Event();
                                result.id = id;
                                result.name = name;
                                result.description = Table_Functions.GetAttribute("description", row);
                            }
                        }
                    }
                }
            }

            return result;
        }

        Value GetValueFromRow(Event e, DataRow row)
        {
            Value result = null;

            string adr = row["address"].ToString();

            string lastNode = Table_Functions.GetLastNode(row);

            if (lastNode != null)
            {
                if (lastNode.ToLower() == "value")
                {
                    string strId = Table_Functions.GetAttribute("id", row);
                    if (strId != null)
                    {
                        int id = -1;
                        if (int.TryParse(strId, out id))
                        {
                            Value v = e.values.Find(x => x.id == id);
                            if (v == null)
                            {
                                result = new Value();
                                result.id = id;
                            }
                        }
                    }
                }

            }

            return result;
        }

        void GetDefaultFromRow(Event e, DataRow row)
        {
            string adr = row["address"].ToString();

            if (adr.Contains("Default"))
            {
                string n = Table_Functions.GetAttribute("numval", row);
                if (n != null)
                {
                    int numval;
                    if (int.TryParse(n, out numval))
                    {
                        Result r = new Result();
                        r.value = row["value"].ToString().Replace('_', ' '); ;
                        r.numval = numval;
                        e.Default = r;
                    }
                }
            }
        }

        void GetResultFromRow(Value v, DataRow row)
        {
            string adr = row["address"].ToString();

            if (adr.Contains("Result"))
            {
                string n = Table_Functions.GetAttribute("numval", row);
                if (n != null)
                {
                    int numval;
                    if (int.TryParse(n, out numval))
                    {
                        Result r = new Result();
                        r.value = row["value"].ToString();
                        r.numval = numval;
                        v.result = r;
                    }
                }
            }
        }

        #endregion
 
    }
}
