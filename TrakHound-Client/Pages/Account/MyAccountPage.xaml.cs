﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Collections.Specialized;

using TH_Global;
using TH_Global.Functions;
using TH_Global.TrakHound.Users;
using TH_Global.Web;
//using TH_UserManagement.Management;
using TH_WPF;


namespace TrakHound_Client.Pages.Account
{
    /// <summary>
    /// Interaction logic for Page.xaml
    /// </summary>
    public partial class Page : UserControl, IPage
    {
        const System.Windows.Threading.DispatcherPriority priority = System.Windows.Threading.DispatcherPriority.ContextIdle;

        public Page()
        {
            InitializeComponent();
            DataContext = this;

            // Fill Country List
            foreach (string country in Countries.Names) CountryList.Add(country);

            // Fill State List
            foreach (string state in States.Abbreviations) StateList.Add(state);

            SetPageType(CurrentUser);
        }

        //public string PageName { get; set; }

        //public ImageSource Image { get; set; }


        public void Opened() { }
        public bool Opening() { return true; }

        public void Closed() { }
        public bool Closing() { return true; }



        public UserConfiguration CurrentUser
        {
            get { return (UserConfiguration)GetValue(CurrentUserProperty); }
            set 
            {               
                SetValue(CurrentUserProperty, value);
            }
        }

        public static readonly DependencyProperty CurrentUserProperty =
           DependencyProperty.Register("CurrentUser", typeof(UserConfiguration), typeof(Page), new PropertyMetadata(null));


        public delegate void UserChanged_Handler(UserConfiguration userConfig);
        public event UserChanged_Handler UserChanged;



        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Page), new PropertyMetadata(null));


        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(Page), new PropertyMetadata(null));


        public void LoadUserConfiguration(UserConfiguration userConfig)
        {
            CurrentUser = userConfig;

            profileImageLoaded = false;

            SetPageType(userConfig);

            if (userConfig != null)
            {
                SetDependencyProperty(UsernameVerifiedProperty, true);

                SetDependencyProperty(FirstNameProperty, String_Functions.UppercaseFirst(userConfig.FirstName));
                SetDependencyProperty(LastNameProperty, String_Functions.UppercaseFirst(userConfig.LastName));

                SetDependencyProperty(UsernameDisplayProperty, String_Functions.UppercaseFirst(userConfig.Username));
                SetDependencyProperty(EmailProperty, userConfig.Email);

                SetDependencyProperty(CompanyProperty, String_Functions.UppercaseFirst(userConfig.Company));
                SetDependencyProperty(PhoneProperty, userConfig.Phone);
                SetDependencyProperty(Address1Property, userConfig.Address1);
                SetDependencyProperty(Address2Property, userConfig.Address2);
                SetDependencyProperty(CityProperty, userConfig.City);

                int country = CountryList.ToList().FindIndex(x => x == String_Functions.UppercaseFirst(userConfig.Country));
                if (country >= 0) country_COMBO.SelectedIndex = country;

                int state = CountryList.ToList().FindIndex(x => x == String_Functions.UppercaseFirst(userConfig.State));
                if (state >= 0) state_COMBO.SelectedIndex = state;

                SetDependencyProperty(Page.ZipCodeProperty, userConfig.Zipcode);

                //if (this.IsLoaded) LoadProfileImage(userConfig);
                LoadProfileImage(userConfig);
            }
            else
            {
                CleanForm();
            }
        }

        void SetDependencyProperty(DependencyProperty dp, object value)
        {
            this.Dispatcher.BeginInvoke(new Action<DependencyProperty, object>(SetDependencyProperty_GUI), priority, new object[] { dp, value });
        }

        void SetDependencyProperty_GUI(DependencyProperty dp, object value)
        {
            SetValue(dp, value);
        }


        void SetPageType(UserConfiguration userConfig)
        {
            if (userConfig != null)
            {
                Title = "Edit Account";
                Image = new BitmapImage(new Uri("pack://application:,,,/TH_UserManagement;component/Resources/blank_profile_01_sm.png"));
            }
            else
            {
                Title = "Create Account";
                Image = new BitmapImage(new Uri("pack://application:,,,/TH_UserManagement;component/Resources/AddUser_01.png"));
            }

            //if (UserManagementSettings.Database != null) Title = Title + " (Local)";
        }

        public void CleanForm()
        {
            UsernameVerified = false;
            PasswordEntered = false;
            ConfirmPasswordEntered = false;

            password_TXT.PasswordText = null;
            confirmpassword_TXT.PasswordText = null;

            SetDependencyProperty(Page.FirstNameProperty, null);
            SetDependencyProperty(Page.LastNameProperty, null);

            SetDependencyProperty(Page.UsernameProperty, null);
            SetDependencyProperty(Page.EmailProperty, null);

            SetDependencyProperty(Page.CompanyProperty, null);
            SetDependencyProperty(Page.PhoneProperty, null);
            SetDependencyProperty(Page.Address1Property, null);
            SetDependencyProperty(Page.Address2Property, null);
            SetDependencyProperty(Page.CityProperty, null);

            SetDependencyProperty(Page.ZipCodeProperty, null);

            int US = CountryList.ToList().FindIndex(x => x == "United States");
            if (US >= 0) country_COMBO.SelectedIndex = US;

            state_COMBO.SelectedIndex = -1;

            ClearProfileImage();
        }

        #region "Form Properties"

        public string FirstName
        {
            get { return (string)GetValue(FirstNameProperty); }
            set { SetValue(FirstNameProperty, value); }
        }

        public static readonly DependencyProperty FirstNameProperty =
            DependencyProperty.Register("FirstName", typeof(string), typeof(Page), new PropertyMetadata(null));


        public string LastName
        {
            get { return (string)GetValue(LastNameProperty); }
            set { SetValue(LastNameProperty, value); }
        }

        public static readonly DependencyProperty LastNameProperty =
            DependencyProperty.Register("LastName", typeof(string), typeof(Page), new PropertyMetadata(null));


        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register("Username", typeof(string), typeof(Page), new PropertyMetadata(null));


        public string UsernameDisplay
        {
            get { return (string)GetValue(UsernameDisplayProperty); }
            set { SetValue(UsernameDisplayProperty, value); }
        }

        public static readonly DependencyProperty UsernameDisplayProperty =
            DependencyProperty.Register("UsernameDisplay", typeof(string), typeof(Page), new PropertyMetadata(null));




        public string Company
        {
            get { return (string)GetValue(CompanyProperty); }
            set { SetValue(CompanyProperty, value); }
        }

        public static readonly DependencyProperty CompanyProperty =
            DependencyProperty.Register("Company", typeof(string), typeof(Page), new PropertyMetadata(null));


        public string Email
        {
            get { return (string)GetValue(EmailProperty); }
            set { SetValue(EmailProperty, value); }
        }

        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register("Email", typeof(string), typeof(Page), new PropertyMetadata(null));


        public string Phone
        {
            get { return (string)GetValue(PhoneProperty); }
            set { SetValue(PhoneProperty, value); }
        }

        public static readonly DependencyProperty PhoneProperty =
            DependencyProperty.Register("Phone", typeof(string), typeof(Page), new PropertyMetadata(null));


        public string Address1
        {
            get { return (string)GetValue(Address1Property); }
            set { SetValue(Address1Property, value); }
        }

        public static readonly DependencyProperty Address1Property =
            DependencyProperty.Register("Address1", typeof(string), typeof(Page), new PropertyMetadata(null));


        public string Address2
        {
            get { return (string)GetValue(Address2Property); }
            set { SetValue(Address2Property, value); }
        }

        public static readonly DependencyProperty Address2Property =
            DependencyProperty.Register("Address2", typeof(string), typeof(Page), new PropertyMetadata(null));


        public string City
        {
            get { return (string)GetValue(CityProperty); }
            set { SetValue(CityProperty, value); }
        }

        public static readonly DependencyProperty CityProperty =
            DependencyProperty.Register("City", typeof(string), typeof(Page), new PropertyMetadata(null));


        public string ZipCode
        {
            get { return (string)GetValue(ZipCodeProperty); }
            set { SetValue(ZipCodeProperty, value); }
        }

        public static readonly DependencyProperty ZipCodeProperty =
            DependencyProperty.Register("ZipCode", typeof(string), typeof(Page), new PropertyMetadata(null));


        ObservableCollection<string> countrylist;
        public ObservableCollection<string> CountryList
        {
            get
            {
                if (countrylist == null)
                    countrylist = new ObservableCollection<string>();
                return countrylist;
            }

            set
            {
                countrylist = value;
            }
        }

        ObservableCollection<string> statelist;
        public ObservableCollection<string> StateList
        {
            get
            {
                if (statelist == null)
                    statelist = new ObservableCollection<string>();
                return statelist;
            }

            set
            {
                statelist = value;
            }
        }

        public bool ShowStates
        {
            get { return (bool)GetValue(ShowStatesProperty); }
            set { SetValue(ShowStatesProperty, value); }
        }

        public static readonly DependencyProperty ShowStatesProperty =
            DependencyProperty.Register("ShowStates", typeof(bool), typeof(Page), new PropertyMetadata(false));


        private void country_COMBO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender.GetType() == typeof(ComboBox))
            {
                ComboBox cb = (ComboBox)sender;

                if (cb.SelectedItem != null)
                {
                    if (cb.SelectedItem.ToString() == "United States") ShowStates = true;
                    else ShowStates = false;
                }
            }
        }

        #region "Password"

        public bool PasswordEntered
        {
            get { return (bool)GetValue(PasswordEnteredProperty); }
            set { SetValue(PasswordEnteredProperty, value); }
        }

        public static readonly DependencyProperty PasswordEnteredProperty =
            DependencyProperty.Register("PasswordEntered", typeof(bool), typeof(Page), new PropertyMetadata(false));


        private void password_TXT_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (password_TXT.PasswordText != "") PasswordEntered = true;
            else PasswordEntered = false;

            PasswordShort = false;
            PasswordLong = false;

            VerifyPassword();
            ConfirmPassword();
        }

        #endregion

        #region "Confirm Password"

        public bool ConfirmPasswordEntered
        {
            get { return (bool)GetValue(ConfirmPasswordEnteredProperty); }
            set { SetValue(ConfirmPasswordEnteredProperty, value); }
        }

        public static readonly DependencyProperty ConfirmPasswordEnteredProperty =
            DependencyProperty.Register("ConfirmPasswordEntered", typeof(bool), typeof(Page), new PropertyMetadata(false));

        private void confirmpassword_TXT_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (confirmpassword_TXT.PasswordText != "") ConfirmPasswordEntered = true;
            else ConfirmPasswordEntered = false;

            ConfirmPassword();
        }

        #endregion

        #endregion

        #region "Apply / Create"

        public bool Saving
        {
            get { return (bool)GetValue(SavingProperty); }
            set { SetValue(SavingProperty, value); }
        }

        public static readonly DependencyProperty SavingProperty =
            DependencyProperty.Register("Saving", typeof(bool), typeof(Page), new PropertyMetadata(false));
      

        private void Apply_Clicked(TH_WPF.Button bt)
        {
            if (CurrentUser != null) EditUser(NewEditUserInfo());
            else CreateUser(NewCreateUserInfo());
        }

        CreateUserInfo NewCreateUserInfo()
        {
            var info = new CreateUserInfo();

            info.FirstName = FirstName;
            info.LastName = LastName;

            info.Username = Username;
            info.Password = password_TXT.PasswordText;

            info.Company = Company;

            info.Email = Email;
            info.Phone = Phone;

            info.Address1 = Address1;
            info.Address2 = Address2;
            info.City = City;

            if (country_COMBO.SelectedItem != null) info.Country = country_COMBO.SelectedItem.ToString();
            if (state_COMBO.SelectedItem != null) info.State = state_COMBO.SelectedItem.ToString();

            info.Zipcode = ZipCode;

            return info;
        }

        EditUserInfo NewEditUserInfo()
        {
            var info = new EditUserInfo();

            info.SessionToken = CurrentUser.SessionToken;

            info.FirstName = FirstName;
            info.LastName = LastName;

            if (ShowChangePassword) info.Password = password_TXT.PasswordText;

            info.Company = Company;

            info.Email = Email;
            info.Phone = Phone;

            info.Address1 = Address1;
            info.Address2 = Address2;
            info.City = City;

            if (country_COMBO.SelectedItem != null) info.Country = country_COMBO.SelectedItem.ToString();
            if (state_COMBO.SelectedItem != null) info.State = state_COMBO.SelectedItem.ToString();

            info.Zipcode = ZipCode;

            return info;
        }

        //UserConfiguration CreateUserConfiguration()
        //{
        //    // Create new UserConfiguration object with new data
        //    UserConfiguration userConfig = new UserConfiguration();

        //    userConfig.FirstName = FirstName;
        //    userConfig.LastName = LastName;

        //    userConfig.Username = Username;

        //    userConfig.Company = Company;

        //    userConfig.Email = Email;
        //    userConfig.Phone = Phone;

        //    userConfig.Address1 = Address1;
        //    userConfig.Address2 = Address2;
        //    userConfig.City = City;

        //    if (country_COMBO.SelectedItem != null) userConfig.Country = country_COMBO.SelectedItem.ToString();
        //    if (state_COMBO.SelectedItem != null) userConfig.State = state_COMBO.SelectedItem.ToString();

        //    userConfig.Zipcode = ZipCode;

        //    return userConfig;
        //}


        //class UpdateUser_Info
        //{
        //    public UserConfiguration userConfig { get; set; }
        //    public string password { get; set; }
        //}

        //class UpdateUser_Return
        //{
        //    public bool success { get; set; }
        //    public UserConfiguration userConfig { get; set; }
        //}

        //class UpdateUser_Info
        //{
        //    public CreateUserInfo Info { get; set; }
        //}

        class UpdateUser_Return
        {
            public bool Success { get; set; }
            public UserConfiguration Info { get; set; }
        }

        void CreateUser(CreateUserInfo info)
        {
            Saving = true;

            ThreadPool.QueueUserWorkItem(new WaitCallback(CreateUser_Worker), info);
        }

        void CreateUser_Worker(object o)
        {
            var result = new UpdateUser_Return();

            if (o != null)
            {
                var info = (CreateUserInfo)o;

                var userConfig = UserManagement.CreateUser(info, "TrakHound Client Create User");

                // Upload Profile Image
                if (userConfig != null && profileImageChanged)
                {
                    if (profileImageFilename != null)
                    {
                        userConfig = UploadProfileImage(userConfig, profileImageFilename);
                    }
                }

                result.Info = userConfig;
            }

            Dispatcher.BeginInvoke(new Action<UpdateUser_Return>(CreateUser_GUI), priority, new object[] { result });
        }

        void CreateUser_GUI(UpdateUser_Return result)
        {
            if (result.Info != null)
            {
                if (UserChanged != null) UserChanged(result.Info);
            }
            else
            {
                TH_WPF.MessageBox.Show("Error during User Creation! Try Again.");
            }

            Saving = false;
        }


        void EditUser(EditUserInfo info)
        {
            Saving = true;

            ThreadPool.QueueUserWorkItem(new WaitCallback(EditUser_Worker), info);
        }

        void EditUser_Worker(object o)
        {
            var result = new UpdateUser_Return();

            if (o != null)
            {
                var info = (EditUserInfo)o;

                var userConfig = UserManagement.EditUser(info, "TrakHound Client Edit User");

                // Upload Profile Image
                if (userConfig != null && profileImageChanged)
                {
                    if (profileImageFilename != null)
                    {
                        userConfig = UploadProfileImage(userConfig, profileImageFilename);
                    }
                }

                result.Info = userConfig;
            }

            Dispatcher.BeginInvoke(new Action<UpdateUser_Return>(EditUser_GUI), priority, new object[] { result });
        }

        void EditUser_GUI(UpdateUser_Return result)
        {
            if (result.Info != null)
            {
                if (UserChanged != null) UserChanged(result.Info);
            }
            else
            {
                TH_WPF.MessageBox.Show("Error during User Edit! Try Again.");
            }

            Saving = false;
        }

        #endregion


        #region "Username Verification"

        public bool UsernameVerified
        {
            get { return (bool)GetValue(UsernameVerifiedProperty); }
            set { SetValue(UsernameVerifiedProperty, value); }
        }

        public static readonly DependencyProperty UsernameVerifiedProperty =
            DependencyProperty.Register("UsernameVerified", typeof(bool), typeof(Page), new PropertyMetadata(false));

        public string UsernameMessage
        {
            get { return (string)GetValue(UsernameMessageProperty); }
            set { SetValue(UsernameMessageProperty, value); }
        }

        public static readonly DependencyProperty UsernameMessageProperty =
            DependencyProperty.Register("UsernameMessage", typeof(string), typeof(Page), new PropertyMetadata(null));
    

        System.Timers.Timer username_TIMER;

        string previousUsernameText = null;

        private void username_TXT_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Username != previousUsernameText)
            {
                if (username_TIMER != null) username_TIMER.Enabled = false;

                username_TIMER = new System.Timers.Timer();
                username_TIMER.Interval = 500;
                username_TIMER.Elapsed += username_TIMER_Elapsed;
                username_TIMER.Enabled = true;

                previousUsernameText = Username;
            }
        }

        void username_TIMER_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            username_TIMER.Enabled = false;

            Dispatcher.BeginInvoke(new Action(VerifyUsername));
        }

        void VerifyUsername()
        {
            // If no userconfiguration database configuration found then use default TrakHound User Database
            var usernameReturn = TH_UserManagement.Management.Users.VerifyUsername(Username);
            if (usernameReturn != null)
            {
                UsernameVerified = usernameReturn.available;
                UsernameMessage = usernameReturn.message;
            }
            else
            {
                UsernameVerified = false;
                UsernameMessage = null;
            }
        }

        #endregion

        #region "Password"

        public bool ShowChangePassword
        {
            get { return (bool)GetValue(ShowChangePasswordProperty); }
            set { SetValue(ShowChangePasswordProperty, value); }
        }

        public static readonly DependencyProperty ShowChangePasswordProperty =
            DependencyProperty.Register("ShowChangePassword", typeof(bool), typeof(Page), new PropertyMetadata(false));

        private void ChangePassword_Clicked(TH_WPF.Button bt)
        {
            ShowChangePassword = true;
        }

        public bool PasswordVerified
        {
            get { return (bool)GetValue(PasswordVerifiedProperty); }
            set { SetValue(PasswordVerifiedProperty, value); }
        }

        public static readonly DependencyProperty PasswordVerifiedProperty =
            DependencyProperty.Register("PasswordVerified", typeof(bool), typeof(Page), new PropertyMetadata(false));


        public bool PasswordShort
        {
            get { return (bool)GetValue(PasswordShortProperty); }
            set { SetValue(PasswordShortProperty, value); }
        }

        public static readonly DependencyProperty PasswordShortProperty =
            DependencyProperty.Register("PasswordShort", typeof(bool), typeof(Page), new PropertyMetadata(false));


        public bool PasswordLong
        {
            get { return (bool)GetValue(PasswordLongProperty); }
            set { SetValue(PasswordLongProperty, value); }
        }

        public static readonly DependencyProperty PasswordLongProperty =
            DependencyProperty.Register("PasswordLong", typeof(bool), typeof(Page), new PropertyMetadata(false));

        System.Timers.Timer password_TIMER;

        void VerifyPassword()
        {
            if (password_TIMER != null) password_TIMER.Enabled = false;

            password_TIMER = new System.Timers.Timer();
            password_TIMER.Interval = 500;
            password_TIMER.Elapsed += password_TIMER_Elapsed;
            password_TIMER.Enabled = true;
        }

        void password_TIMER_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            password_TIMER.Enabled = false;

            this.Dispatcher.BeginInvoke(new Action(VerifyPassword_GUI));
        }

        void VerifyPassword_GUI()
        {
            System.Security.SecureString pwd = password_TXT.SecurePassword;

            if (!TH_UserManagement.Management.Security_Functions.VerifyPasswordMinimum(pwd)) PasswordShort = true;
            else if (!TH_UserManagement.Management.Security_Functions.VerifyPasswordMaximum(pwd)) PasswordLong = true;
        }

        System.Timers.Timer confirmpassword_TIMER;

        void ConfirmPassword()
        {
            if (confirmpassword_TIMER != null) confirmpassword_TIMER.Enabled = false;

            confirmpassword_TIMER = new System.Timers.Timer();
            confirmpassword_TIMER.Interval = 500;
            confirmpassword_TIMER.Elapsed += confirmpassword_TIMER_Elapsed;
            confirmpassword_TIMER.Enabled = true;
        }

        void confirmpassword_TIMER_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            confirmpassword_TIMER.Enabled = false;

            this.Dispatcher.BeginInvoke(new Action(ConfirmPassword_GUI));
        }

        void ConfirmPassword_GUI()
        {
            if (PasswordEntered && ConfirmPasswordEntered)
            {
                if (password_TXT.PasswordText == confirmpassword_TXT.PasswordText) PasswordVerified = true;
                else PasswordVerified = false;
            }
        }

        #endregion


        #region "Profile Image"

        public ImageSource ProfileImage
        {
            get { return (ImageSource)GetValue(ProfileImageProperty); }
            set { SetValue(ProfileImageProperty, value); }
        }

        public static readonly DependencyProperty ProfileImageProperty =
            DependencyProperty.Register("ProfileImage", typeof(ImageSource), typeof(Page), new PropertyMetadata(null));


        public bool ProfileImageSet
        {
            get { return (bool)GetValue(ProfileImageSetProperty); }
            set { SetValue(ProfileImageSetProperty, value); }
        }

        public static readonly DependencyProperty ProfileImageSetProperty =
            DependencyProperty.Register("ProfileImageSet", typeof(bool), typeof(Page), new PropertyMetadata(false));


        public bool ProfileImageLoading
        {
            get { return (bool)GetValue(ProfileImageLoadingProperty); }
            set { SetValue(ProfileImageLoadingProperty, value); }
        }

        public static readonly DependencyProperty ProfileImageLoadingProperty =
            DependencyProperty.Register("ProfileImageLoading", typeof(bool), typeof(Page), new PropertyMetadata(false));

        //Thread profileimage_THREAD;

        void LoadProfileImage(UserConfiguration userConfig)
        {
            ProfileImageLoading = true;
            ProfileImageSet = false;
            ProfileImage = null;

            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadProfileImage_Worker), userConfig);

            //if (profileimage_THREAD != null) profileimage_THREAD.Abort();

            //profileimage_THREAD = new Thread(new ParameterizedThreadStart(LoadProfileImage_Worker));
            //profileimage_THREAD.Start(userConfig);
        }

        void LoadProfileImage_Worker(object o)
        {
            if (o != null)
            {
                var userConfig = (UserConfiguration)o;

                if (userConfig != null && !string.IsNullOrEmpty(userConfig.ImageUrl))
                {
                    var img = UserManagement.ProfileImage.Get(userConfig.ImageUrl);
                    if (img != null)
                    {
                        System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img);

                        IntPtr bmpPt = bmp.GetHbitmap();
                        BitmapSource bmpSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmpPt, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        bmpSource = Image_Functions.SetImageSize(bmpSource, 200, 200);
                        bmpSource.Freeze();

                        Dispatcher.BeginInvoke(new Action<BitmapSource>(LoadProfileImage_GUI), priority, new object[] { bmpSource });
                    }
                }

                Dispatcher.BeginInvoke(new Action(LoadProfileImage_Finished), priority, new object[] { });
            }
        }

        void LoadProfileImage_GUI(BitmapSource src)
        {
            if (src != null)
            {
                ProfileImage = src;
                ProfileImageSet = true;
            }

            profileImageLoaded = true;
        }

        void LoadProfileImage_Finished()
        {
            ProfileImageLoading = false;
        }

        string profileImageFilename;

        System.Drawing.Image profileImage;

        bool profileImageChanged = false;

        bool profileImageLoaded = false;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //if (!profileImageLoaded && CurrentUser != null) LoadProfileImage(CurrentUser);
        }


        void SetProfileImage()
        {
            // Show OpenFileDialog for selecting new Profile Image
            string imagePath = OpenImageBrowse();
            if (imagePath != null)
            {
                // Crop and Resize image
                System.Drawing.Image img = ProcessImage(imagePath);
                if (img != null)
                {
                    profileImageFilename = imagePath;
                    profileImageChanged = true;

                    img = TH_Global.Functions.Image_Functions.CropImageToCenter(img);

                    profileImage = img;

                    ProfileImage = TH_Global.Functions.Image_Functions.SourceFromImage(img);

                    if (ProfileImage != null) ProfileImageSet = true;
                    else ProfileImageSet = false;
                }
            }
        }

        UserConfiguration UploadProfileImage(UserConfiguration userConfig, string path)
        {
            UserConfiguration result = null;

            if (path != null)
            {
                // Crop and Resize image
                System.Drawing.Image img = ProcessImage(path);
                if (img != null)
                {
                    string newFilename = String_Functions.RandomString(20);

                    profileImageFilename = newFilename;

                    string tempdir = FileLocations.TrakHound + @"\temp";
                    if (!Directory.Exists(tempdir)) Directory.CreateDirectory(tempdir);

                    string localPath = tempdir + @"\" + newFilename;

                    img.Save(localPath);

                    result = UserManagement.ProfileImage.Set(userConfig.SessionToken, localPath);
                }
            }

            return result;
        }

        //bool UploadProfileImage(System.Drawing.Image profileImg)
        //{
        //    bool result = false;

        //    if (profileImg != null)
        //    {
        //        // Crop and Resize image
        //        System.Drawing.Image img = ProcessImage(profileImg);
        //        if (img != null)
        //        {
        //            string newFilename = String_Functions.RandomString(20);

        //            profileImageFilename = newFilename;

        //            string tempdir = FileLocations.TrakHound + @"\temp";
        //            if (!Directory.Exists(tempdir)) Directory.CreateDirectory(tempdir);

        //            string localPath = tempdir + @"\" + newFilename;

        //            img.Save(localPath);

        //            result = UploadProfileImage(newFilename, localPath);
        //        }
        //    }

        //    return result;
        //}

        void ClearProfileImage()
        {
            ProfileImage = null;
            ProfileImageSet = false;
            ProfileImageLoading = false;

            profileImage = null;
            profileImageFilename = null;
        }

        private void ProfileImage_UploadClicked(ImageBox sender)
        {
            SetProfileImage();
        }

        private void ProfileImage_ClearClicked(ImageBox sender)
        {
            profileImageChanged = true;

            ClearProfileImage();
        }

        //void ChangeProfileImage()
        //{
        //    if (LoggedIn && CurrentUser != null)
        //    {

        //        // Show OpenFileDialog for selecting new Profile Image
        //        string imagePath = OpenImageBrowse();
        //        if (imagePath != null)
        //        {
        //            // Crop and Resize image
        //            System.Drawing.Image img = ProcessImage(imagePath);
        //            if (img != null)
        //            {
        //                string filename = String_Functions.RandomString(20);

        //                string tempdir = FileLocations.TrakHound + @"\temp";
        //                if (!Directory.Exists(tempdir)) Directory.CreateDirectory(tempdir);

        //                string localPath = tempdir + @"\" + filename;

        //                img.Save(localPath);

        //                if (UploadProfileImage(filename, localPath))
        //                {
        //                    UserManagement.SetProfileImage(CurrentUser.ImageUrl, filename);
        //                    //Users.UpdateImageURL(filename, CurrentUser);

        //                    LoadProfileImage(CurrentUser);

        //                    CurrentUser = CurrentUser;
        //                }
        //            }
        //        }
        //    }
        //}

        public static string OpenImageBrowse()
        {
            string result = null;

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.InitialDirectory = FileLocations.TrakHound;
            dlg.Multiselect = false;
            dlg.Title = "Browse for Profile Image";
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            Nullable<bool> dialogResult = dlg.ShowDialog();

            if (dialogResult == true)
            {
                if (dlg.FileName != null) result = dlg.FileName;
            }

            return result;
        }

        public static System.Drawing.Image ProcessImage(string path)
        {
            System.Drawing.Image result = null;

            if (File.Exists(path))
            {
                System.Drawing.Image img = Image_Functions.CropImageToCenter(System.Drawing.Image.FromFile(path));

                result = Image_Functions.SetImageSize(img, 200, 200);
            }

            return result;
        }

        //public static bool UploadProfileImage(string filename, string localpath)
        //{
        //    bool result = false;

        //    NameValueCollection nvc = new NameValueCollection();
        //    if (HTTP.UploadFile("https://www.feenux.com/php/users/uploadprofileimage.php", localpath, "file", "image/jpeg", nvc))
        //    {
        //        result = true;
        //    }

        //    return result;
        //}


        #endregion

        private void PrivacyPolicy_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.Uri.ToString());
            }
            catch (Exception ex) { TH_WPF.MessageBox.Show("Error Opening Privacy Policy. Please try again or open a browser and navigate to 'http://www.feenux.com/trakhound/docs/privacypolicy_desktopapp.html'"); }
        }


    }
}
