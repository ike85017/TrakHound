﻿// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MTConnect.Application.Components;

using TH_Global.Functions;
using TH_Plugins;
using TH_Plugins.Server;
using TH_UserManagement.Management;

namespace TH_Cycles.ConfigurationPage
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

        public string Title { get { return "Cycles"; } }

        private BitmapImage _image;
        public ImageSource Image
        {
            get
            {
                if (_image == null)
                {
                    _image = new BitmapImage(new Uri("pack://application:,,,/TH_Cycles;component/Resources/Cycle_01.png"));
                    _image.Freeze();
                }

                return _image;
            }
        }

        public bool Loaded { get; set; }

        public event SettingChanged_Handler SettingChanged;


        public event SendData_Handler SendData;

        public void GetSentData(EventData data)
        {
            GetProbeData(data);
        }


        public void LoadConfiguration(DataTable dt)
        {
            configurationTable = dt;

            LoadGeneratedEventItems(dt);

            GetProductionTypeValues();
            LoadProductionTypes(dt);
            LoadOverrideLinks(dt);

            ClearData();
            LoadData(dt);
        }

        void ClearData()
        {
            ProductionTypes.Clear();
        }

        void LoadData(DataTable dt)
        {
            LoadCycleEventName(dt);
            LoadStoppedEventValueName(dt);
            LoadCycleNameLink(dt);
            LoadProductionTypes(dt);
            LoadOverrideLinks(dt);

            if (!Loaded) LoadCollectedItems(probeData);
        }

        public void SaveConfiguration(DataTable dt)
        {
            DataTable_Functions.TrakHound.DeleteRows(prefix + "*", "address", dt);

            SaveCycleEventName(dt);
            SaveStoppedEventValueName(dt);
            SaveCycleNameLink(dt);
            SaveProductionTypes(dt);
            SaveOverrideLinks(dt);
        }

        DataTable configurationTable;

        const string prefix = "/Cycles/";


        const System.Windows.Threading.DispatcherPriority priority = System.Windows.Threading.DispatcherPriority.Background;


        #region "MTC Probe Data"

        List_Functions.ObservableCollectionEx<CollectedItem> _collectedItems;
        public List_Functions.ObservableCollectionEx<CollectedItem> CollectedItems
        {
            get
            {
                if (_collectedItems == null)
                    _collectedItems = new List_Functions.ObservableCollectionEx<CollectedItem>();
                return _collectedItems;
            }

            set
            {
                _collectedItems = value;
            }
        }

        private List<DataItem> probeData = new List<DataItem>();

        public class CollectedItem : IComparable
        {
            public CollectedItem() { }

            public CollectedItem(DataItem dataItem)
            {
                Id = dataItem.Id;
                Name = dataItem.Name;

                if (Name != null) Display = Id + " : " + Name;
                else Display = Id;
            }

            public string Id { get; set; }
            public string Name { get; set; }

            public string Display { get; set; }

            public override string ToString()
            {
                return Display;
            }

            public CollectedItem Copy()
            {
                var copy = new CollectedItem();
                copy.Id = Id;
                copy.Name = Name;
                copy.Display = Display;

                return copy;
            }

            public int CompareTo(object obj)
            {
                if (obj == null) return 1;

                var i = obj as CollectedItem;
                if (i != null)
                {
                    return Display.CompareTo(i.Display);
                }
                else return 1;
            }
        }

        void GetProbeData(EventData data)
        {
            if (data != null && data.Id != null && data.Data02 != null)
            {
                if (data.Id.ToLower() == "mtconnect_probe_dataitems")
                {
                    var dataItems = (List<DataItem>)data.Data02;
                    probeData = dataItems;
                    if (Loaded) LoadCollectedItems(dataItems);
                }
            }
        }

        private void LoadCollectedItems(List<DataItem> dataItems)
        {
            var newItems = new List<CollectedItem>();

            foreach (var dataItem in dataItems)
            {
                var item = new CollectedItem(dataItem);
                newItems.Add(item.Copy());
            }

            foreach (var newItem in newItems)
            {
                if (!CollectedItems.ToList().Exists(x => x.Id == newItem.Id)) CollectedItems.Add(newItem);
            }

            foreach (var item in CollectedItems.ToList())
            {
                if (!newItems.Exists(x => x.Id == item.Id)) CollectedItems.Remove(item);
            }

            CollectedItems.SupressNotification = true;
            CollectedItems.Sort();
            CollectedItems.SupressNotification = false;
        }

        #endregion

        #region "Generated Events"

        ObservableCollection<GeneratedEventItem> _generatedEventItems;
        public ObservableCollection<GeneratedEventItem> GeneratedEventItems
        {
            get
            {
                if (_generatedEventItems == null)
                    _generatedEventItems = new ObservableCollection<GeneratedEventItem>();
                return _generatedEventItems;
            }

            set
            {
                _generatedEventItems = value;
            }
        }

        public class GeneratedEventItem
        {
            public GeneratedEventItem(TH_GeneratedData.GeneratedEvents.ConfigurationPage.Page.Event e)
            {
                Id = e.name;
                Name = String_Functions.UppercaseFirst(e.name.Replace('_', ' ').ToLower());
                Event = e;
            }

            public string Id { get; set; }
            public string Name { get; set; }

            public TH_GeneratedData.GeneratedEvents.ConfigurationPage.Page.Event Event { get; set; }
        }

        private void LoadGeneratedEventItems(DataTable dt)
        {
            GeneratedEventItems.Clear();

            var events = TH_GeneratedData.GeneratedEvents.ConfigurationPage.Page.GetGeneratedEvents(dt);
            foreach (var e in events)
            {
                GeneratedEventItems.Add(new GeneratedEventItem(e));
            }
        }


        ObservableCollection<object> _generatedeventvalues;
        public ObservableCollection<object> GeneratedEventValues
        {
            get
            {
                if (_generatedeventvalues == null)
                    _generatedeventvalues = new ObservableCollection<object>();
                return _generatedeventvalues;
            }

            set
            {
                _generatedeventvalues = value;
            }
        }

        void GetGeneratedEventValues(string Id)
        {
            GeneratedEventValues.Clear();

            if (GeneratedEventItems != null)
            {
                var e = GeneratedEventItems.ToList().Find(x => x.Id == Id);
                if (e != null)
                {
                    // Add each Value
                    foreach (var value in e.Event.values)
                    {
                        GeneratedEventValues.Add(value.result.value);
                    }

                    // Add Default Value
                    if (e.Event.Default != null)
                    {
                        GeneratedEventValues.Add(e.Event.Default.value);
                    }
                }
            }
        }

        #endregion

        #region "Cycle Event Name"

        public object SelectedCycleEventName
        {
            get { return (object)GetValue(SelectedCycleEventNameProperty); }
            set { SetValue(SelectedCycleEventNameProperty, value); }
        }

        public static readonly DependencyProperty SelectedCycleEventNameProperty =
            DependencyProperty.Register("SelectedCycleEventName", typeof(object), typeof(Page), new PropertyMetadata(null));

        private void CycleEventName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmbox = (ComboBox)sender;
            if (cmbox.SelectedItem != null)
            {
                var item = (GeneratedEventItem)cmbox.SelectedItem;
                GetGeneratedEventValues(item.Id);
            }

            if (cmbox.IsKeyboardFocused || cmbox.IsMouseCaptured)
            {
                LoadProductionTypes();

                if (SettingChanged != null) SettingChanged("Cycle Event Name", null, null);
            }
        }

        void LoadCycleEventName(DataTable dt)
        {
            string query = "Address = '" + prefix + "CycleEventName'";

            var rows = dt.Select(query);
            if (rows != null)
            {
                foreach (var row in rows)
                {
                    string val = DataTable_Functions.GetRowValue("Value", row);
                    if (val != null) SelectedCycleEventName = val;
                }
            }
        }

        void SaveCycleEventName(DataTable dt)
        {
            string val = "";
            if (SelectedCycleEventName != null) val = SelectedCycleEventName.ToString();

            if (val != null) Table_Functions.UpdateTableValue(val.Replace(' ', '_').ToLower(), prefix + "CycleEventName", dt);
        }

        #endregion

        #region "Stopped Event Value"

        public object SelectedStoppedEventValue
        {
            get { return (object)GetValue(SelectedStoppedEventValueProperty); }
            set { SetValue(SelectedStoppedEventValueProperty, value); }
        }

        public static readonly DependencyProperty SelectedStoppedEventValueProperty =
            DependencyProperty.Register("SelectedStoppedEventValue", typeof(object), typeof(Page), new PropertyMetadata(null));


        private void stoppedValue_COMBO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmbox = (ComboBox)sender;
            if (cmbox.IsKeyboardFocused || cmbox.IsMouseCaptured) if (SettingChanged != null) SettingChanged("Stopped Event Value", null, null);
        }

        void LoadStoppedEventValueName(DataTable dt)
        {
            string query = "Address = '" + prefix + "StoppedEventValue'";

            var rows = dt.Select(query);
            if (rows != null)
            {
                foreach (var row in rows)
                {
                    SelectedStoppedEventValue = DataTable_Functions.GetRowValue("Value", row);
                }
            }
        }

        void SaveStoppedEventValueName(DataTable dt)
        {
            string val = "";
            if (SelectedStoppedEventValue != null) val = SelectedStoppedEventValue.ToString();

            if (val != null) Table_Functions.UpdateTableValue(val, prefix + "StoppedEventValue", dt);
        }

        #endregion

        #region "Cycle Name Link"

        public object SelectedCycleNameLink
        {
            get { return (object)GetValue(SelectedCycleNameLinkProperty); }
            set { SetValue(SelectedCycleNameLinkProperty, value); }
        }

        public static readonly DependencyProperty SelectedCycleNameLinkProperty =
            DependencyProperty.Register("SelectedCycleNameLink", typeof(object), typeof(Page), new PropertyMetadata(null));

        private void cyclenamelink_COMBO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmbox = (ComboBox)sender;
            if (cmbox.IsKeyboardFocused || cmbox.IsMouseCaptured) if (SettingChanged != null) SettingChanged("Cycle Name Link", null, null);
        }

        void LoadCycleNameLink(DataTable dt)
        {
            string query = "Address = '" + prefix + "CycleNameLink'";

            var rows = dt.Select(query);
            if (rows != null)
            {
                foreach (var row in rows)
                {
                    string val = DataTable_Functions.GetRowValue("Value", row);
                    SelectedCycleNameLink = val;
                }
            }
        }

        void SaveCycleNameLink(DataTable dt)
        {
            string val = null;
            if (SelectedCycleNameLink != null) val = SelectedCycleNameLink.ToString();

            if (val != null) Table_Functions.UpdateTableValue(val, prefix + "CycleNameLink", dt);
        }

        #endregion

        #region "Production Types"

        ObservableCollection<object> productionTypes;
        public ObservableCollection<object> ProductionTypes
        {
            get
            {
                if (productionTypes == null)
                    productionTypes = new ObservableCollection<object>();
                return productionTypes;
            }

            set
            {
                productionTypes = value;
            }
        }

        ObservableCollection<object> productionTypeValues;
        public ObservableCollection<object> ProductionTypeValues
        {
            get
            {
                if (productionTypeValues == null)
                    productionTypeValues = new ObservableCollection<object>();
                return productionTypeValues;
            }

            set
            {
                productionTypeValues = value;
            }
        }


        void GetProductionTypeValues()
        {
            ProductionTypeValues.Clear();

            var values = Enum.GetValues(typeof(TH_Cycles.CycleData.CycleProductionType));
            foreach (var value in values)
            {
                ProductionTypeValues.Add(value);
            }
        }

        void LoadProductionTypes()
        {
            ProductionTypes.Clear();

            foreach (var value in GeneratedEventValues)
            {
                var item = new Controls.ProductionTypeItem();
                item.ParentPage = this;
                item.ValueName = value.ToString();
                item.SettingChanged += ProductionTypeItem_SettingChanged;
                ProductionTypes.Add(item);
            }
        }

        void LoadProductionTypes(DataTable dt)
        {
            ProductionTypes.Clear();

            string query = "Address LIKE '" + prefix + "ProductionTypes/*'";

            var rows = dt.Select(query);
            if (rows != null)
            {
                foreach (var row in rows)
                {
                    var item = new Controls.ProductionTypeItem();
                    item.ParentPage = this;
                    item.ValueName = Table_Functions.GetAttribute("name", row);
                    item.productionType_COMBO.Text = Table_Functions.GetAttribute("type", row);
                    item.SettingChanged += ProductionTypeItem_SettingChanged;
                    ProductionTypes.Add(item);
                }
            }
        }

        void ProductionTypeItem_SettingChanged()
        {
            if (SettingChanged != null) SettingChanged("Production Type", null, null);
        }

        void SaveProductionTypes(DataTable dt)
        {
            foreach (var productionType in ProductionTypes)
            {
                var item = (Controls.ProductionTypeItem)productionType;

                int id = DataTable_Functions.TrakHound.GetUnusedAddressId(prefix + "ProductionTypes/Value", dt);
                string adr = prefix + "ProductionTypes/Value||" + id.ToString("00");

                string attr = "";
                attr += "id||" + id.ToString("00") + ";";
                attr += "name||" + item.ValueName + ";";
                attr += "type||" + item.productionType_COMBO.Text + ";";

                Table_Functions.UpdateTableValue(null, attr, adr, dt);
            }
        }

        #endregion

        #region "Override Links"

        ObservableCollection<object> overrideLinks;
        public ObservableCollection<object> OverrideLinks
        {
            get
            {
                if (overrideLinks == null)
                    overrideLinks = new ObservableCollection<object>();
                return overrideLinks;
            }

            set
            {
                overrideLinks = value;
            }
        }

        private void AddOverrideLink_Clicked(TH_WPF.Button bt)
        {
            var item = new Controls.OverrideLinkItem();
            item.ParentPage = this;
            item.RemoveClicked += OverrideLinkItem_RemoveClicked;
            item.SettingChanged += OverrideLinkItem_SettingChanged;
            OverrideLinks.Add(item);
        }

        void LoadOverrideLinks(DataTable dt)
        {
            OverrideLinks.Clear();

            string query = "Address LIKE '" + prefix + "OverrideLinks/*'";

            var rows = dt.Select(query);
            if (rows != null)
            {
                foreach (var row in rows)
                {
                    var item = new Controls.OverrideLinkItem();
                    item.ParentPage = this;
                    item.SelectedId = DataTable_Functions.GetRowValue("Value", row);
                    item.RemoveClicked += OverrideLinkItem_RemoveClicked;
                    item.SettingChanged += OverrideLinkItem_SettingChanged;
                    OverrideLinks.Add(item);
                }
            }
        }

        void OverrideLinkItem_SettingChanged()
        {
            if (SettingChanged != null) SettingChanged("Override Link", null, null);
        }

        void OverrideLinkItem_RemoveClicked(Controls.OverrideLinkItem item)
        {
            if (OverrideLinks.Contains(item)) OverrideLinks.Remove(item);
        }

        void SaveOverrideLinks(DataTable dt)
        {
            foreach (var overrideLink in OverrideLinks)
            {
                var item = (Controls.OverrideLinkItem)overrideLink;

                int id = DataTable_Functions.TrakHound.GetUnusedAddressId(prefix + "OverrideLinks/Value", dt);
                string adr = prefix + "OverrideLinks/Value||" + id.ToString("00");

                string val = null;
                if (item.SelectedId != null) val = item.SelectedId.ToString();

                string attr = "";
                attr += "id||" + id.ToString("00") + ";";

                Table_Functions.UpdateTableValue(val, attr, adr, dt);
            }
        }

        #endregion

        private void Help_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender.GetType() == typeof(Rectangle))
            {
                Rectangle rect = (Rectangle)sender;

                if (rect.ToolTip != null)
                {
                    if (rect.ToolTip.GetType() == typeof(ToolTip))
                    {
                        ToolTip tt = (ToolTip)rect.ToolTip;
                        tt.IsOpen = true;
                    }
                }
            }
        }

        private void Help_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender.GetType() == typeof(Rectangle))
            {
                Rectangle rect = (Rectangle)sender;

                if (rect.ToolTip != null)
                {
                    if (rect.ToolTip.GetType() == typeof(ToolTip))
                    {
                        ToolTip tt = (ToolTip)rect.ToolTip;
                        tt.IsOpen = true;
                    }
                }
            }
        }

        private void Help_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender.GetType() == typeof(Rectangle))
            {
                Rectangle rect = (Rectangle)sender;

                if (rect.ToolTip != null)
                {
                    if (rect.ToolTip.GetType() == typeof(ToolTip))
                    {
                        ToolTip tt = (ToolTip)rect.ToolTip;
                        tt.IsOpen = false;
                    }
                }
            }
        }

    }
}
