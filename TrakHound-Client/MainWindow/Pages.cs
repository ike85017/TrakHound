﻿// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;

using TH_Configuration;
using TH_DeviceManager;
using TH_Global;
using TH_Global.TrakHound.Users;
//using TH_UserManagement.Management;

using TrakHound_Client.Controls;

namespace TrakHound_Client
{
    public partial class MainWindow
    {

        ObservableCollection<TabPage> _tabPages;
        public ObservableCollection<TabPage> TabPages
        {
            get
            {
                if (_tabPages == null) _tabPages = new ObservableCollection<TabPage>();
                return _tabPages;
            }
            set
            {
                _tabPages = value;
            }
        }

        ObservableCollection<TabHeader> _tabHeaders;
        public ObservableCollection<TabHeader> TabHeaders
        {
            get
            {
                if (_tabHeaders == null) _tabHeaders = new ObservableCollection<TabHeader>();
                return _tabHeaders;
            }
            set
            {
                _tabHeaders = value;
            }
        }

        TabHeader SelectedTab { get; set; }

        public TabPage CurrentPage
        {
            get { return (TabPage)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(TabPage), typeof(MainWindow), new PropertyMetadata(null));


        public void AddTab(IPage page, string name = null, ImageSource image = null, string tag = null)
        {
            string txt = page.Title;
            ImageSource img = page.Image;

            if (name != null) txt = name;
            if (image != null) img = image;

            TabHeader header = FindTab(page, txt, tag);
            if (header == null)
            {
                header = new TabHeader();
                header.Text = txt;
                header.Image = img;
                header.Tag = tag;
                header.Page = new TabPage(page);

                header.Clicked += TabHeader_Clicked;
                header.CloseClicked += TabHeader_CloseClicked;
                header.Opened += TabHeader_Opened;
                header.Closed += TabHeader_Closed;

                TabHeaders.Add(header);

                header.Open(TabHeaders.Count == 1);
            }

            SelectTab(header);
        }

        private void TabHeader_Opened(object sender, EventArgs e)
        {
            SetTabWidths();
        }

        private void TabHeader_Closed(object sender, EventArgs e)
        {
            var tab = (TabHeader)sender;
            if (TabHeaders.Contains(tab)) TabHeaders.Remove(tab);

            tab.Clicked -= TabHeader_Clicked;
            tab.CloseClicked -= TabHeader_CloseClicked;
            tab.Opened -= TabHeader_Opened;
            tab.Closed -= TabHeader_Closed;

            SetTabWidths();
        }

        private void SetTabWidths()
        {
            if (TabHeaders.Count > 0)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    double windowWidth = TabPanel.ActualWidth;
                    double tabsWidth = GetTabPanelWidth();

                    if (tabsWidth > windowWidth)
                    {
                        var tabs = TabHeaders.ToList().OrderByDescending(x => x.ActualWidth).ToList();
                        double widestTab = tabs[0].ActualWidth;

                        tabs = tabs.FindAll(x => x.ActualWidth >= widestTab);

                        foreach (var tab in tabs)
                        {
                            double increment = (tabsWidth - windowWidth) / tabs.Count;

                            int index = TabHeaders.ToList().FindIndex(x => x.Id == tab.Id);
                            if (index >= 0)
                            {
                                var t = TabHeaders[index];
                                t.MaxWidth = Math.Max(TabHeader.MIN_WIDTH, t.ActualWidth - increment);
                            }

                            tabsWidth = GetTabPanelWidth();
                        }
                    }
                    else if (tabsWidth < windowWidth)
                    {
                        var tabs = TabHeaders.ToList().OrderByDescending(x => x.ActualWidth).ToList();
                        double widestTab = tabs[0].ActualWidth;

                        tabs = tabs.FindAll(x => x.ActualWidth >= widestTab);

                        foreach (var tab in tabs)
                        {
                            double increment = (windowWidth - tabsWidth) / tabs.Count;

                            int index = TabHeaders.ToList().FindIndex(x => x.Id == tab.Id);
                            if (index >= 0)
                            {
                                var t = TabHeaders[index];

                                double width = t.MaxWidth + increment;
                                width = Math.Max(width, TabHeader.MIN_WIDTH);
                                width = Math.Min(width, TabHeader.MAX_WIDTH);

                                t.MaxWidth = width;
                            }

                            tabsWidth = GetTabPanelWidth();
                        }
                    }
                }), MainWindow.PRIORITY_CONTEXT_IDLE, new object[] { });
            }
        }

        private double GetTabPanelWidth()
        {
            double tabsWidth = 0;
            foreach (var tab in TabHeaders) tabsWidth += tab.ActualWidth;
            return tabsWidth;
        }

        #region "Select"

        public void SelectTab(string name, string tag = null)
        {
            var tab = FindTab(name, tag);
            if (tab != null) SelectTab(tab);
        }

        public void SelectTab(IPage page, string tag = null)
        {
            SelectTab(page.Title, tag);
        }

        public void SelectTab(TabHeader header)
        {
            // Unselect other tabs
            foreach (var oheader in TabHeaders) if (oheader != header) oheader.IsSelected = false;

            header.IsSelected = true;

            // Fade in if not previously set
            if (CurrentPage == null) AnimateTabPageOpen();

            CurrentPage = header.Page;
            SelectedTab = header;
        }

        public void SelectTab(int index)
        {
            if (index >= 0 && index <= TabHeaders.Count - 1)
            {
                SelectTab(TabHeaders[index]);
            }
        }


        const double TAB_PAGE_OPEN_ANIMATION_TIME = 300;
        const double TAB_PAGE_CLOSE_ANIMATION_TIME = 300;

        private void AnimateTabPageOpen()
        {
            var animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 1;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(TAB_PAGE_OPEN_ANIMATION_TIME));

            var ease = new CubicEase();
            ease.EasingMode = EasingMode.EaseIn;

            animation.EasingFunction = ease;
            CurrentPage_CONTENT.BeginAnimation(OpacityProperty, animation);
        }

        private void AnimateTabPageClose()
        {
            var animation = new DoubleAnimation();
            animation.From = 1;
            animation.To = 0;
            animation.Completed += AnimateTabPageClose_Completed;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(TAB_PAGE_CLOSE_ANIMATION_TIME));

            var ease = new CubicEase();
            ease.EasingMode = EasingMode.EaseOut;

            animation.EasingFunction = ease;
            CurrentPage_CONTENT.BeginAnimation(OpacityProperty, animation);
        }

        private void AnimateTabPageClose_Completed(object sender, EventArgs e)
        {
            CurrentPage = null;
        }

        #endregion

        #region "Find"

        public TabHeader FindTab(string name, string tag = null)
        {
            int index = TabHeaders.ToList().FindIndex(x => x.Text == name && x.Tag == tag);
            if (index >= 0)
            {
                return TabHeaders[index];
            }
            return null;
        }

        public TabHeader FindTab(IPage page, string name = null, string tag = null)
        {
            string title = page.Title;

            if (name != null) title = name;

            int index = TabHeaders.ToList().FindIndex(x => x.Text == title && x.Tag == tag);
            if (index >= 0)
            {
                return TabHeaders[index];
            }
            return null;
        }

        public TabHeader FindTab(int index)
        {
            if (index >= 0 && index <= TabHeaders.Count - 1)
            {
                return TabHeaders[index];
            }
            return null;
        }


        public int FindTabIndex(string name, string tag = null)
        {
            int index = TabHeaders.ToList().FindIndex(x => x.Text == name && x.Tag == tag);
            return index;
        }

        public int FindTabIndex(TabHeader header)
        {
            int index = TabHeaders.ToList().FindIndex(x => x.Text == header.Text);
            return index;
        }

        #endregion

        #region "Close"

        public void CloseTab(string name, string tag = null)
        {
            var tab = FindTab(name, tag);
            if (tab != null)
            {
                bool cancel = CheckCancel(tab);
                if (!cancel)
                {
                    tab.Close();

                    // If current tab then switch to tab to the left
                    if (CurrentPage == tab.Page)
                    {
                        int tabIndex = FindTabIndex(name, tag);
                        if (tabIndex >= 0)
                        {
                            if (tabIndex > 0) SelectTab(tabIndex - 1);
                            else if (TabHeaders.Count > 1) SelectTab(tabIndex + 1);
                            else AnimateTabPageClose();
                        }
                    }

                    if (tab.Page != null && tab.Page.PageContent != null)
                    {
                        Dispatcher.BeginInvoke(new Action(() => { tab.Page.PageContent.Closed(); }));
                    }
                }
            }
        }

        private bool CheckCancel(TabHeader header)
        {
            bool result = false;

            if (header.Page != null && header.Page.PageContent != null)
            {
                result = !header.Page.PageContent.Closing();
            }

            return result;
        }

        #endregion

        #region "TabHeader Event Handlers"

        void TabHeader_Clicked(TabHeader header) { SelectTab(header.Text, header.Tag); }

        void TabHeader_CloseClicked(TabHeader header) { CloseTab(header.Text, header.Tag); }

        #endregion

        void ChangePage_Forward()
        {
            if (TabHeaders.Count > 1)
            {
                if (SelectedTab != null)
                {
                    int index = FindTabIndex(SelectedTab);
                    if (index < TabHeaders.Count - 1) SelectTab(index + 1);
                    else SelectTab(0);
                }
            }
        }

        void ChangePage_Backward()
        {
            if (TabHeaders.Count > 1)
            {
                if (SelectedTab != null)
                {
                    int index = FindTabIndex(SelectedTab);
                    if (index > 0) SelectTab(index - 1);
                    else SelectTab(TabHeaders.Count - 1);
                }
            }
        }

        #region "Zoom"

        public double ZoomLevel
        {
            get { return (double)GetValue(ZoomLevelProperty); }
            set
            {
                SetValue(ZoomLevelProperty, value);

                //if (Pages_TABCONTROL.SelectedIndex >= 0)
                //{
                //    TH_TabItem tab = (TH_TabItem)Pages_TABCONTROL.Items[Pages_TABCONTROL.SelectedIndex];

                //    Controls.Page page = (Controls.Page)tab.Content;
                //    page.ZoomLevel = value;

                //    ZoomLevelDisplay = value.ToString("P0");

                //    if (ZoomLevelChanged != null) ZoomLevelChanged(value);
                //}

            }
        }

        public static readonly DependencyProperty ZoomLevelProperty =
            DependencyProperty.Register("ZoomLevel", typeof(double), typeof(MainWindow), new PropertyMetadata(1D));


        public string ZoomLevelDisplay
        {
            get { return (string)GetValue(ZoomLevelDisplayProperty); }
            set
            {
                SetValue(ZoomLevelDisplayProperty, value);
            }
        }

        public static readonly DependencyProperty ZoomLevelDisplayProperty =
            DependencyProperty.Register("ZoomLevelDisplay", typeof(string), typeof(MainWindow), new PropertyMetadata("100%"));

        public delegate void ZoomLevelChanged_Handler(double zoomlevel);
        public event ZoomLevelChanged_Handler ZoomLevelChanged;

        #endregion

        #region "About"

        public PageManager aboutManager;

        void About_Initialize()
        {
            if (aboutManager == null)
            {
                aboutManager = new PageManager();
                aboutManager.TabTitle = "About";
                aboutManager.TabImage = new BitmapImage(new Uri("pack://application:,,,/TrakHound-Client;component/Resources/About_01.png"));
                aboutManager.AddPage(new Pages.About.Information.Page());
                aboutManager.AddPage(new Pages.About.License.Page());
            }
        }

        public void About_Open()
        {
            About_Initialize();

            AddTab(aboutManager);
        }

        #endregion

        #region "Device Manager"

        #region "Device List"

        public DeviceList deviceListPage;

        void DeviceManager_DeviceList_Initialize()
        {
            if (deviceListPage == null)
            {
                deviceListPage = new DeviceList();
                deviceListPage.DeviceManager = DeviceManager;

                deviceListPage.PageClosed += DeviceListPage_PageClosed;
                deviceListPage.AddDeviceSelected += DeviceManager_DeviceList_AddDeviceSelected;
                deviceListPage.EditSelected += DeviceManager_DeviceList_DeviceEditSelected;
                deviceListPage.EditTableSelected += DeviceManager_DeviceList_DeviceEditTableSelected;
            }
        }

        private void DeviceListPage_PageClosed()
        {
            deviceListPage.PageClosed -= DeviceListPage_PageClosed;
            deviceListPage.AddDeviceSelected -= DeviceManager_DeviceList_AddDeviceSelected;
            deviceListPage.EditSelected -= DeviceManager_DeviceList_DeviceEditSelected;
            deviceListPage.EditTableSelected -= DeviceManager_DeviceList_DeviceEditTableSelected;

            deviceListPage = null;
        }

        public void DeviceManager_DeviceList_Open()
        {
            DeviceManager_DeviceList_Initialize();

            if (FindTab(deviceListPage) == null) AddTab(deviceListPage);
            else SelectTab(deviceListPage);
        }

        private void DeviceManager_DeviceList_DeviceManagerListSelected() { DeviceManager_DeviceList_Open(); }

        private void DeviceManager_DeviceList_AddDeviceSelected() { DeviceManager_AddDevice_Open(); }

        private void DeviceManager_DeviceList_DeviceEditSelected(DeviceConfiguration config) { DeviceManager_EditDevice_Open(config); }

        private void DeviceManager_DeviceList_DeviceEditTableSelected(DeviceConfiguration config) { }

        #endregion

        #region "Add Device"

        public TH_DeviceManager.AddDevice.Page addDevicePage;

        public void DeviceManager_AddDevice_Initialize()
        {
            if (addDevicePage == null)
            {
                addDevicePage = new TH_DeviceManager.AddDevice.Page();
                addDevicePage.DeviceManager = DeviceManager;
                addDevicePage.ShowAutoDetect();

                addDevicePage.DeviceListSelected += DeviceManager_AddDevice_DeviceListSelected;
                addDevicePage.EditTableSelected += DeviceManager_AddDevice_EditTableSelected;
                addDevicePage.PageClosed += AddDevicePage_PageClosed;
            }
        }

        private void AddDevicePage_PageClosed()
        {
            addDevicePage.DeviceListSelected -= DeviceManager_AddDevice_DeviceListSelected;
            addDevicePage.EditTableSelected -= DeviceManager_AddDevice_EditTableSelected;
            addDevicePage.PageClosed -= AddDevicePage_PageClosed;

            addDevicePage = null;
        }

        public void DeviceManager_AddDevice_Open()
        {
            DeviceManager_AddDevice_Initialize();

            if (FindTab(addDevicePage) == null) AddTab(addDevicePage);
            else SelectTab(addDevicePage);
        }

        private void DeviceManager_AddDevice_DeviceListSelected() { DeviceManager_DeviceList_Open(); }

        private void DeviceManager_AddDevice_EditTableSelected(DeviceConfiguration config) { }

        #endregion

        public void DeviceManager_EditDevice_Open(DeviceConfiguration config)
        {
            string title = "Edit Device - " + config.Description.Description;
            if (config.Description.Device_ID != null) title += " (" + config.Description.Device_ID + ")";

            string tag = config.UniqueId;

            var tab = FindTab(title, tag);
            if (tab == null)
            {
                var page = new EditPage(config);
                page.DeviceManager = DeviceManager;

                page.DeviceListSelected += DeviceManager_EditDevice_DeviceListSelected;

                AddTab(page, title, page.Image, tag);
            }
            else
            {
                SelectTab(tab);
            }
        }

        private void DeviceManager_EditDevice_DeviceListSelected() { DeviceManager_DeviceList_Open(); }

        public void DeviceManager_EditDeviceTable_Open(DeviceConfiguration config)
        {
            //string title = "Edit Device Table - " + config.Description.Description;
            //if (config.Description.Device_ID != null) title += " (" + config.Description.Device_ID + ")";

            //var tab = FindTab(title);
            //if (tab == null)
            //{
            //    var page = new EditTablePage(config, ManagementType.Client);
            //    page.DeviceManager = DeviceManager;

            //    AddTab(page, title);
            //}
            //else
            //{
            //    SelectTab(tab);
            //}
        }

        #endregion

        #region "Account Manager"

        public PageManager accountManager;

        Pages.Account.Page accountpage;

        void AccountManager_Initialize()
        {
            if (accountManager == null)
            {
                accountpage = new Pages.Account.Page();
                accountpage.LoadUserConfiguration(CurrentUser);
                accountpage.UserChanged += accountpage_UserChanged;

                accountManager = new PageManager();
                accountManager.TabTitle = "Account Management";
                accountManager.TabImage = new BitmapImage(new Uri("pack://application:,,,/TrakHound-Client;component/Resources/blank_profile_01_sm.png"));
                accountManager.AddPage(accountpage);
            }
        }

        void accountpage_UserChanged(UserConfiguration userConfig)
        {
            if (LoginMenu != null) LoginMenu.LoadUserConfiguration(userConfig);
        }

        public void AccountManager_Open()
        {
            AccountManager_Initialize();

            AddTab(accountManager);
        }

        #endregion

        #region "Options"

        public PageManager optionsManager;

        void Options_Initialize()
        {
            if (optionsManager == null)
            {
                optionsManager = new PageManager();
                optionsManager.TabTitle = "Options";
                optionsManager.TabImage = new BitmapImage(new Uri("pack://application:,,,/TrakHound-Client;component/Resources/options_gear_30px.png"));
                optionsManager.AddPage(new Pages.Options.Updates.Page());
                optionsManager.AddPage(new Pages.Options.Logger.Page());
            }
        }

        public void Options_AddPage(IPage page)
        {
            Options_Initialize();
            optionsManager.AddPage(page);
        }

        public void Options_Open()
        {
            Options_Initialize();

            AddTab(optionsManager);
        }

        #endregion

        #region "Plugins"

        PageManager pluginsManager;

        Pages.Plugins.Installed.Page pluginsPage;

        void Plugins_Initialize()
        {
            if (pluginsManager == null)
            {
                pluginsManager = new PageManager();
                pluginsManager.TabTitle = "Plugins";
                pluginsManager.TabImage = new BitmapImage(new Uri("pack://application:,,,/TrakHound-Client;component/Resources/Rocket_02.png"));
                pluginsPage = new Pages.Plugins.Installed.Page();
                pluginsManager.AddPage(pluginsPage);

                if (PluginConfigurations != null) Plugins_AddItems(PluginConfigurations);
            }
        }

        private void Plugins_AddItems(List<TH_Plugins.Client.PluginConfiguration> configs)
        {
            pluginsPage.ClearInstalledItems();

            configs.Sort((a, b) => a.Name.CompareTo(b.Name));

            foreach (var config in configs)
            {
                pluginsPage.AddPlugin(config);
            }
        }

        public void Plugins_Open()
        {
            Plugins_Initialize();

            AddTab(pluginsManager);
        }

        #endregion



    }
}
