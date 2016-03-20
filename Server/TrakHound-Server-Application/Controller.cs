﻿// Copyright (c) 2015 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Windows.Forms;
using System.Diagnostics;

using TH_Configuration;
using TH_Global;
using TH_Global.Functions;
using TH_UserManagement.Management;

using TrakHound_Server_Core;

namespace TrakHound_Server
{
    public class Controller : ApplicationContext
    {
        private System.ComponentModel.IContainer mComponents;   //List of components
        private NotifyIcon mNotifyIcon;
        private ContextMenuStrip mContextMenu;

        private ToolStripMenuItem mLogin;
        private ToolStripMenuItem mLogout;

        private ToolStripMenuItem mDeviceManager;
        private ToolStripMenuItem mConsole;
        private ToolStripMenuItem mStartService;
        private ToolStripMenuItem mStopService;
        private ToolStripMenuItem mExitApplication;

        private Label mUsernameLBL;
        private ToolStripControlHost mLoggedIn;
        ToolStripSeparator seperator1;

        public Database_Settings userDatabaseSettings;

        private ServerGroup serverGroup;

        public Controller(ServerGroup group)
        {
            InitializeComponents();

            serverGroup = group;

            group.Server.CurrentUserChanged += s_CurrentUserChanged;
            group.Server.Started += s_Started;
            group.Server.Stopped += s_Stopped;
        }

        protected override void OnMainFormClosed(object sender, EventArgs e)
        {
            if (serverGroup != null) serverGroup.Close();

            if (mNotifyIcon != null) mNotifyIcon.Visible = false;
            mNotifyIcon.Dispose();

            base.OnMainFormClosed(sender, e);
        }

        void InitializeComponents()
        {
            mComponents = new System.ComponentModel.Container();

            mNotifyIcon = new NotifyIcon(this.mComponents);
            mNotifyIcon.Icon = Properties.Resources.TrakHound_Logo_Initials_10;
            mNotifyIcon.Text = "TrakHound Server";
            mNotifyIcon.Visible = true;

            mContextMenu = new ContextMenuStrip();
            mNotifyIcon.ContextMenuStrip = mContextMenu;

            // Add Username label
            mContextMenu.Items.Add(CreateLoggedInLabel());

            seperator1 = new System.Windows.Forms.ToolStripSeparator();
            seperator1.Visible = false;
            mContextMenu.Items.Add(seperator1);

            mLogin = new ToolStripMenuItem();
            mLogout = new ToolStripMenuItem();

            mDeviceManager = new ToolStripMenuItem();
            mConsole = new ToolStripMenuItem();
            mStartService = new ToolStripMenuItem();
            mStopService = new ToolStripMenuItem();
            mExitApplication = new ToolStripMenuItem();

            mLogin.Text = "Login";
            mLogin.Click += mLogin_Click;
            mContextMenu.Items.Add(mLogin);

            mLogout.Text = "Logout";
            mLogout.Click += mSignOut_Click;
            mContextMenu.Items.Add(mLogout);
            mLogout.Enabled = false;

            ToolStripSeparator seperator = new System.Windows.Forms.ToolStripSeparator();
            mContextMenu.Items.Add(seperator);

            mDeviceManager.Text = "Device Manager";
            mDeviceManager.Image = Properties.Resources.Root;
            mDeviceManager.Click += mDeviceManager_Click;
            mContextMenu.Items.Add(mDeviceManager);

            mConsole.Text = "Open Console";
            mConsole.Click += mConsole_Click;
            mContextMenu.Items.Add(mConsole);

            seperator = new System.Windows.Forms.ToolStripSeparator();
            mContextMenu.Items.Add(seperator);

            mStartService.Text = "Start Server";
            mStartService.Click += mStartService_Click;
            mContextMenu.Items.Add(mStartService);

            mStopService.Text = "Stop Server";
            mStopService.Click += mStopService_Click;
            mContextMenu.Items.Add(mStopService);
            mStopService.Enabled = false;

            seperator = new System.Windows.Forms.ToolStripSeparator();
            mContextMenu.Items.Add(seperator);

            mExitApplication.Text = "Exit";
            mExitApplication.Image = Properties.Resources.Power_01_Red;
            mExitApplication.Click += mExitApplication_Click;
            mContextMenu.Items.Add(mExitApplication);
        }

       ToolStripControlHost CreateLoggedInLabel()
        {
            var mPanel = new FlowLayoutPanel();
            mPanel.FlowDirection = FlowDirection.TopDown;
            mPanel.BackColor = System.Drawing.Color.Transparent;

            var mLabel = new Label();
            mLabel.BackColor = System.Drawing.Color.Transparent;
            mLabel.ForeColor = System.Drawing.Color.FromArgb(136, 136, 136);
            System.Drawing.Font font1 = new System.Drawing.Font("segoe", 8);
            mLabel.Font = font1;
            mLabel.Margin = new Padding(0, 10, 0, 2);
            mLabel.Height = 15;
            mLabel.Text = "Logged in as";
            mLabel.Parent = mPanel;

            mUsernameLBL = new Label();
            mUsernameLBL.BackColor = System.Drawing.Color.Transparent;
            mUsernameLBL.ForeColor = System.Drawing.Color.FromArgb(0, 128, 255);
            System.Drawing.Font font2 = new System.Drawing.Font("segoe", 10, System.Drawing.FontStyle.Bold);
            mUsernameLBL.Font = font2;
            mUsernameLBL.Margin = new Padding(0, 0, 0, 0);
            mUsernameLBL.Text = "";
            mUsernameLBL.Parent = mPanel;

            mLoggedIn = new ToolStripControlHost(mPanel);
            mLoggedIn.Visible = false;
            return mLoggedIn;
        }

        #region "Server Events"

        void s_Started()
        {
            mStartService.Enabled = false;
            mStopService.Enabled = true;

            mNotifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            mNotifyIcon.BalloonTipTitle = "TrakHound Server";
            mNotifyIcon.BalloonTipText = "Server Started Sucessfully!";
            mNotifyIcon.ShowBalloonTip(3000);
        }

        void s_Stopped()
        {
            mStartService.Enabled = true;
            mStopService.Enabled = false;

            mNotifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            mNotifyIcon.BalloonTipTitle = "TrakHound Server";
            mNotifyIcon.BalloonTipText = "Server Stopped";
            mNotifyIcon.ShowBalloonTip(3000);
        }

        void s_CurrentUserChanged(TH_UserManagement.Management.UserConfiguration userConfig)
        {
            if (userConfig != null)
            {
                mUsernameLBL.Text = String_Functions.UppercaseFirst(userConfig.username);
                mLoggedIn.Visible = true;
                seperator1.Visible = true;

                mLogin.Enabled = false;
                mLogout.Enabled = true;

                mNotifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                mNotifyIcon.BalloonTipTitle = "TrakHound Server";
                mNotifyIcon.BalloonTipText = String_Functions.UppercaseFirst(userConfig.username) + " Logged in Successfully!";
                mNotifyIcon.ShowBalloonTip(3000);
            }
            else
            {
                mLoggedIn.Visible = false;
                seperator1.Visible = false;

                mLogin.Enabled = true;
                mLogout.Enabled = false;

                mNotifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                mNotifyIcon.BalloonTipTitle = "TrakHound Server";
                mNotifyIcon.BalloonTipText = "User Logged Out";
                mNotifyIcon.ShowBalloonTip(3000);
            }
        }

        #endregion

        #region "Menu Options"

        void mLogin_Click(object sender, EventArgs e) { if (serverGroup != null) serverGroup.Login(); }

        void mSignOut_Click(object sender, EventArgs e) { if (serverGroup != null) serverGroup.Logout(); }

        void mDeviceManager_Click(object sender, EventArgs e) { if (serverGroup != null) serverGroup.OpenDeviceManager(); }

        void mConsole_Click(object sender, EventArgs e) { if (serverGroup != null) serverGroup.OpenOutputConsole(); }

        void mStartService_Click(object sender, EventArgs e) { if (serverGroup != null) serverGroup.StartServer(); }

        void mStopService_Click(object sender, EventArgs e) { if (serverGroup != null) serverGroup.StopServer(); }


        void mExitApplication_Click(object sender, EventArgs e)
        {
            if (serverGroup != null) serverGroup.StopServer();

            ExitThreadCore();
        }

        protected override void ExitThreadCore()
        {
            base.ExitThreadCore();
        }

        #endregion
    }
}