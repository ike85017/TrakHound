﻿// Copyright (c) 2015 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using TH_Global.TrakHound.Users;
//using TH_UserManagement.Management;

namespace TrakHound_Client.Menus.Main
{
    /// <summary>
    /// Interaction logic for DropDown.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();

            DataContext = this;

            mw = Application.Current.MainWindow as MainWindow;

            if (mw != null)
            {
                mw.CurrentUserChanged += mw_CurrentUserChanged;
            }

            Root_GRID.Width = 0;
            Root_GRID.Height = 0;

            // 1st Row
            AddPlugins_MenuItem();
            AddDeviceManager_MenuItem();
            AddOptions_MenuItem();

            // 2nd Row
            AddFullscreen_MenuItem();
            AddReportBug_MenuItem();
            AddDeveloperConsole_MenuItem();
        }

        public TrakHound_Client.MainWindow mw
        {
            get { return (TrakHound_Client.MainWindow)GetValue(mwProperty); }
            set { SetValue(mwProperty, value); }
        }

        public static readonly DependencyProperty mwProperty =
            DependencyProperty.Register("mw", typeof(TrakHound_Client.MainWindow), typeof(Menu), new PropertyMetadata(null));


        public bool Shown
        {
            get { return (bool)GetValue(ShownProperty); }
            set
            {
                SetValue(ShownProperty, value);
                if (ShownChanged != null) ShownChanged(value);
            }
        }

        public static readonly DependencyProperty ShownProperty =
            DependencyProperty.Register("Shown", typeof(bool), typeof(Menu), new PropertyMetadata(false));


        public void Hide()
        {
            if (!IsMouseOver) Shown = false;
        }

        public delegate void ShownChanged_Handler(bool val);
        public event ShownChanged_Handler ShownChanged;

        public delegate void Clicked_Handler();

        #region "Menu Items"

        ObservableCollection<object> menuitems;
        public ObservableCollection<object> MenuItems
        {
            get
            {
                if (menuitems == null)
                    menuitems = new ObservableCollection<object>();
                return menuitems;
            }

            set
            {
                menuitems = value;
            }
        }

        #region "Device Manager"

        void AddDeviceManager_MenuItem()
        {
            MenuItem mi = new MenuItem();
            mi.Image = new BitmapImage(new Uri("pack://application:,,,/TrakHound-Client;component/Resources/Root.png"));
            mi.Text = "Device Manager";
            mi.Clicked += DeviceManager_Clicked;

            var bt = new TH_WPF.Button();
            bt.ButtonContent = mi;

            MenuItems.Add(bt);
        }

        void DeviceManager_Clicked(object obj)
        {
            Shown = false;
            if (mw != null) mw.DeviceManager_DeviceList_Open();
        }

        #endregion

        #region "Options"

        void AddOptions_MenuItem()
        {
            var mi = new MenuItem();
            mi.Image = new BitmapImage(new Uri("pack://application:,,,/TrakHound-Client;component/Resources/options_gear_30px.png"));
            mi.Text = "Options";
            mi.Clicked += Options_Clicked;

            var bt = new TH_WPF.Button();
            bt.ButtonContent = mi;

            MenuItems.Add(bt);
        }

        void Options_Clicked(object obj)
        {
            Shown = false;
            if (mw != null) mw.Options_Open();
        }

        #endregion

        #region "Plugins"

        void AddPlugins_MenuItem()
        {
            MenuItem mi = new MenuItem();
            mi.Image = new BitmapImage(new Uri("pack://application:,,,/TrakHound-Client;component/Resources/Rocket_02.png"));
            mi.Text = "Plugins";
            mi.Clicked += Plugins_Clicked;

            var bt = new TH_WPF.Button();
            bt.ButtonContent = mi;

            MenuItems.Add(bt);
        }

        void Plugins_Clicked(object obj)
        {
            Shown = false;
            if (mw != null) mw.Plugins_Open();
        }

        #endregion

        #region "Developer Console"

        void AddDeveloperConsole_MenuItem()
        {
            MenuItem mi = new MenuItem();
            mi.Image = new BitmapImage(new Uri("pack://application:,,,/TrakHound-Client;component/Resources/Developer_01.png"));
            mi.Text = "Developer Console";
            mi.Clicked += DeveloperConsole_Clicked;

            var bt = new TH_WPF.Button();
            bt.ButtonContent = mi;

            MenuItems.Add(bt);
        }

        void DeveloperConsole_Clicked(object obj)
        {
            if (mw != null) mw.developerConsole.Shown = !mw.developerConsole.Shown;
        }

        #endregion

        #region "My Account"

        void AddMyAccount_MenuItem()
        {
            MenuItem mi = new MenuItem();
            mi.Image = new BitmapImage(new Uri("pack://application:,,,/TrakHound-Client;component/Resources/blank_profile_01_sm.png"));
            mi.Text = "My Account";
            mi.Clicked += MyAccount_Clicked;

            var bt = new TH_WPF.Button();
            bt.ButtonContent = mi;

            if (MenuItems.OfType<TH_WPF.Button>().ToList().Find(x => CheckButtonText(x, mi)) == null) MenuItems.Add(bt);
        }

        bool CheckButtonText(TH_WPF.Button bt, MenuItem mi)
        {
            object obj = bt.ButtonContent;
            if (obj != null)
            {
                var content = (MenuItem)obj;
                return content.Text == mi.Text;
            }
            return false;
        }

        void RemoveMyAccount_MenuItem()
        {
            int index = MenuItems.OfType<TH_WPF.Button>().ToList().FindIndex(x => x.Text == "My Account");
            if (index >= 0)
            {
                MenuItems.RemoveAt(index);
            }
        }

        void MyAccount_Clicked(object obj)
        {
            Shown = false;
            if (mw != null) mw.AccountManager_Open();
        }

        void mw_CurrentUserChanged(UserConfiguration userConfig)
        {
            if (userConfig != null)
            {
                AddMyAccount_MenuItem();
            }
            else
            {
                RemoveMyAccount_MenuItem();
            }
        }

        #endregion

        #region "Fullscreen"

        void AddFullscreen_MenuItem()
        {
            MenuItem mi = new MenuItem();
            mi.Image = new BitmapImage(new Uri("pack://application:,,,/TrakHound-Client;component/Resources/FullScreen_02_30px.png"));
            mi.Text = "Fullscreen";
            mi.Clicked += Fullscreen_Clicked;

            var bt = new TH_WPF.Button();
            bt.ButtonContent = mi;

            if (MenuItems.OfType<TH_WPF.Button>().ToList().Find(x => CheckButtonText(x, mi)) == null) MenuItems.Add(bt);
        }

        private void Fullscreen_Clicked(object obj)
        {
            Shown = false;
            if (mw != null && mw.CurrentPage != null) mw.CurrentPage.FullScreen();
        }

        #endregion

        #region "Report Bug"

        void AddReportBug_MenuItem()
        {
            MenuItem mi = new MenuItem();
            mi.Image = new BitmapImage(new Uri("pack://application:,,,/TrakHound-Client;component/Resources/Bug_01.png"));
            mi.Text = "Report Bug";
            mi.Clicked += ReportBug_Clicked;

            var bt = new TH_WPF.Button();
            bt.ButtonContent = mi;

            MenuItems.Add(bt);
        }

        void ReportBug_Clicked(object obj)
        {
            Shown = false;
            if (mw != null) mw.OpenBugReport();
        }

        #endregion

        #endregion

        #region "Bottom Buttons"

        private void About_Clicked(TH_WPF.Button bt)
        {
            Shown = false;
            mw.About_Open(); 
        }

        private void Exit_BT_Clicked(TH_WPF.Button bt)
        {
            mw.Close();
        }

        #endregion

        #region "Zoom"

        private void ZoomOut_Clicked(TH_WPF.Button bt)
        {
            if (mw != null && mw.CurrentPage != null)
            {
                mw.CurrentPage.ZoomOut();
            }
        }

        private void ZoomIn_Clicked(TH_WPF.Button bt)
        {
            if (mw != null && mw.CurrentPage != null)
            {
                mw.CurrentPage.ZoomIn();
            }
        }

        private void Reset_Clicked(TH_WPF.Button bt)
        {
            if (mw != null && mw.CurrentPage != null)
            {
                mw.CurrentPage.SetZoom(1.0);
            }
        }

        #endregion

    }
}
