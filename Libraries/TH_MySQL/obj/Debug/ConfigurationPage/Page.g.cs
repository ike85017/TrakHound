﻿#pragma checksum "..\..\..\ConfigurationPage\Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9F0B40AFDCF4AD39FA9E7E0FEEA2CCED"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using TH_WPF;


namespace TH_MySQL.ConfigurationPage {
    
    
    /// <summary>
    /// Page
    /// </summary>
    public partial class Page : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 161 "..\..\..\ConfigurationPage\Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal TH_WPF.PasswordBox password_TXT;
        
        #line default
        #line hidden
        
        
        #line 279 "..\..\..\ConfigurationPage\Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal TH_WPF.PasswordBox confirmpassword_TXT;
        
        #line default
        #line hidden
        
        
        #line 315 "..\..\..\ConfigurationPage\Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox usephp_CHK;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TH_MySQL;component/configurationpage/page.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ConfigurationPage\Page.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 34 "..\..\..\ConfigurationPage\Page.xaml"
            ((TH_WPF.TextBox)(target)).TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TXT_TextChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 41 "..\..\..\ConfigurationPage\Page.xaml"
            ((TH_WPF.TextBox)(target)).TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TXT_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 48 "..\..\..\ConfigurationPage\Page.xaml"
            ((TH_WPF.TextBox)(target)).TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TXT_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 55 "..\..\..\ConfigurationPage\Page.xaml"
            ((TH_WPF.TextBox)(target)).TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TXT_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.password_TXT = ((TH_WPF.PasswordBox)(target));
            
            #line 161 "..\..\..\ConfigurationPage\Page.xaml"
            this.password_TXT.PasswordChanged += new System.Windows.RoutedEventHandler(this.password_TXT_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.confirmpassword_TXT = ((TH_WPF.PasswordBox)(target));
            
            #line 279 "..\..\..\ConfigurationPage\Page.xaml"
            this.confirmpassword_TXT.PasswordChanged += new System.Windows.RoutedEventHandler(this.confirmpassword_TXT_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.usephp_CHK = ((System.Windows.Controls.CheckBox)(target));
            
            #line 315 "..\..\..\ConfigurationPage\Page.xaml"
            this.usephp_CHK.Checked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked);
            
            #line default
            #line hidden
            
            #line 315 "..\..\..\ConfigurationPage\Page.xaml"
            this.usephp_CHK.Unchecked += new System.Windows.RoutedEventHandler(this.CheckBox_Unchecked);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 324 "..\..\..\ConfigurationPage\Page.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Help_MouseDown);
            
            #line default
            #line hidden
            
            #line 324 "..\..\..\ConfigurationPage\Page.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.Help_MouseEnter);
            
            #line default
            #line hidden
            
            #line 324 "..\..\..\ConfigurationPage\Page.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.Help_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 398 "..\..\..\ConfigurationPage\Page.xaml"
            ((TH_WPF.TextBox)(target)).TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TXT_TextChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 499 "..\..\..\ConfigurationPage\Page.xaml"
            ((TH_WPF.TextBox)(target)).TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TXT_TextChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

