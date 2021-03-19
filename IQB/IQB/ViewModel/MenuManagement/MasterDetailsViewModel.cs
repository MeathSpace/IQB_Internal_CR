namespace IQB.ViewModel.MenuManagement
{
    using AuthViewModel;
    using IQB.Utility;
    using IQB.Views.AdminManagement;
    using Base;
    using EntityLayer.Common;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Views.ApplicationManagement;
    using Views.Home;
    using Views.LoginManagement;
    using Views.QueueList;
    using Views.Settings;
    using Xamarin.Forms;
    using IQB.Views.Appointment.Customer;
    using IQB.Views.Report;

    public class MasterDetailsViewModel : BaseViewModel
    {
        WriteErrorLog errLog = new WriteErrorLog();
        bool IsRedirectedToProfile = false;

        public MasterDetailsViewModel(bool IsRedirectedToProfile)
        {
            this.IsRedirectedToProfile = IsRedirectedToProfile;
            errLog.WinrtErrLogTest(Convert.ToString(IsRedirectedToProfile));
            MenuList = new ObservableCollection<MenuListModel>();
            SetDefaultMenuList();
            errLog.WinrtErrLogTest(" SetDefaultMenuList()");
            SetProfileData();
            errLog.WinrtErrLogTest("  SetProfileData()");
        }

        public void SetProfileData()
        {
            using (LoginViewModel obj = new LoginViewModel())
            {
                ProfileImage = obj.GetUserProfileImageName();
            }

            using (LoginViewModel obj = new LoginViewModel())
            {
                UserName = obj.GetName();
            }
        }
        public void SetProfileData1()
        {
            ProfileImage = CommonEL.ProfileImageLocation + "noimage.png";
        }

        public void SetDefaultMenuList()
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                //Home view
                MenuListModel HomePage = null;
                MenuListModel QueuingList = null;
                MenuListModel MyProfile = null;
                MenuListModel Settings = null;
                MenuListModel AdminSetting = null;
                MenuListModel About = null;
                MenuListModel Help = null;
                MenuListModel Appointment = null;
                MenuListModel ReportGraph = null;
                MenuListModel Logout = null;
                MenuListModel MyBarberProfile = null;

                int userLevel = Convert.ToInt32(Application.Current.Properties["UserLevel"]);
                var w = IsRedirectedToProfile;

                switch (userLevel)
                {
                    case 0:
                        if (!IsRedirectedToProfile)
                        {
                            HomePage = new MenuListModel()
                            {
                                Id = 1,
                                MainTitle = "Home",
                                DefaultIconSource = "homegray.png",
                                SelectedIconSource = "homewhite.png",
                                MainIconSource = "homewhite.png",
                                TargetType = typeof(Views.Home.Home),
                                //MainTextColor = Color.FromHex("#ffffff"),
                                //DefaultTextColor = Color.FromHex("#797e86"),
                                //SelectedTextColor = Color.FromHex("#ffffff"),
                                MainTextColor = Color.FromHex("#ffffff"),
                                DefaultTextColor = Color.FromHex("#000000"),
                                SelectedTextColor = Color.FromHex("#ffffff"),
                                //MainBackgroundColor = Color.FromHex("#1c2027"),
                                MainBackgroundColor = Color.FromHex("#000000"),
                                DefaultBackgroundColor = Color.FromHex("#ffffff"),
                                SelectedBackgroundColor = Color.FromHex("#000000"),

                                //DefaultBackgroundColor = Color.FromHex("#444D5E"),
                                //SelectedBackgroundColor = Color.FromHex("#1c2027"),
                                BorderBackgroundColor = Color.Black
                            };
                        }
                        else
                        {
                            HomePage = new MenuListModel()
                            {
                                Id = 1,
                                MainTitle = "Home",
                                DefaultIconSource = "homegray.png",
                                SelectedIconSource = "homewhite.png",
                                MainIconSource = "homegray.png",
                                TargetType = typeof(Views.Home.Home),
                                //MainTextColor = Color.FromHex("#797e86"),
                                //DefaultTextColor = Color.FromHex("#797e86"),
                                //SelectedTextColor = Color.FromHex("#ffffff"),
                                //MainBackgroundColor = Color.FromHex("#444D5E"),
                                //DefaultBackgroundColor = Color.FromHex("#444D5E"),
                                //SelectedBackgroundColor = Color.FromHex("#1c2027"),
                                //BorderBackgroundColor = Color.Gray
                                MainTextColor = Color.FromHex("#797e86"),
                                DefaultTextColor = Color.FromHex("#797e86"),
                                SelectedTextColor = Color.FromHex("#ffffff"),
                                MainBackgroundColor = Color.FromHex("#ffffff"),
                                DefaultBackgroundColor = Color.FromHex("#ffffff"),
                                SelectedBackgroundColor = Color.FromHex("#000000"),
                                BorderBackgroundColor = Color.Black
                            };
                        }

                        if (!IsRedirectedToProfile)
                        {
                            MyProfile = new MenuListModel()
                            {
                                Id = 3,
                                MainTitle = "My Profile",
                                DefaultIconSource = "myprofilegray.png",
                                SelectedIconSource = "myprofilewhite.png",
                                MainIconSource = "myprofilegray.png",
                                TargetType = typeof(MyProfile),
                                MainTextColor = Color.FromHex("#000000"),
                                DefaultTextColor = Color.FromHex("#000000"),
                                SelectedTextColor = Color.FromHex("#ffffff"),
                                MainBackgroundColor = Color.FromHex("#ffffff"),
                                DefaultBackgroundColor = Color.FromHex("#ffffff"),
                                SelectedBackgroundColor = Color.FromHex("#000000"),
                                BorderBackgroundColor = Color.Black
                            };
                        }
                        else
                        {
                            MyProfile = new MenuListModel()
                            {
                                Id = 3,
                                MainTitle = "My Profile",
                                DefaultIconSource = "myprofilegray.png",
                                SelectedIconSource = "myprofilewhite.png",
                                MainIconSource = "myprofilewhite.png",
                                TargetType = typeof(MyProfile),
                                MainTextColor = Color.FromHex("#000000"),
                                DefaultTextColor = Color.FromHex("#000000"),
                                SelectedTextColor = Color.FromHex("#ffffff"),
                                MainBackgroundColor = Color.FromHex("#ffffff"),
                                DefaultBackgroundColor = Color.FromHex("#ffffff"),
                                SelectedBackgroundColor = Color.FromHex("#000000"),
                                BorderBackgroundColor = Color.Black
                            };
                        }

                        //Queuing List
                        QueuingList = new MenuListModel()
                        {
                            Id = 2,
                            MainTitle = "Queueing List",
                            DefaultIconSource = "queuegray.png",
                            SelectedIconSource = "queuewhite.png",
                            MainIconSource = "queuegray.png",
                            TargetType = typeof(QueueingList),
                            //MainTextColor = Color.FromHex("#797e86"),
                            //DefaultTextColor = Color.FromHex("#797e86"),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        //Settings
                        Settings = new MenuListModel()
                        {
                            Id = 4,
                            MainTitle = "Settings",
                            DefaultIconSource = "settingsgray.png",
                            SelectedIconSource = "settingswhite.png",
                            MainIconSource = "settingsgray.png",
                            TargetType = typeof(Settings),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        //About
                        About = new MenuListModel()
                        {
                            Id = 6,
                            MainTitle = "About",
                            DefaultIconSource = "aboutgray.png",
                            SelectedIconSource = "aboutwhite.png",
                            MainIconSource = "aboutgray.png",
                            TargetType = typeof(About),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        //About
                        Help = new MenuListModel()
                        {
                            Id = 7,
                            MainTitle = "Help",
                            //DefaultIconSource = "helpgray.png",
                            DefaultIconSource = "helpblack.png",
                            SelectedIconSource = "helpwhite.png",
                            //MainIconSource = "helpgray.png",
                            MainIconSource = "helpblack.png",
                            TargetType = typeof(HelpScreen),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };


                        //Appoinrment
                        Appointment = new MenuListModel()
                        {
                            Id = 7,
                            MainTitle = "Appointment",
                            //DefaultIconSource = "helpgray.png",
                            DefaultIconSource = "helpblack.png",
                            SelectedIconSource = "helpwhite.png",
                            //MainIconSource = "helpgray.png",
                            MainIconSource = "helpblack.png",
                            TargetType = typeof(ManageAppointment),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };



                        //ReportGrah
                        ReportGraph = new MenuListModel()
                        {
                            Id = 7,
                            MainTitle = "Report",
                            //DefaultIconSource = "helpgray.png",
                            DefaultIconSource = "helpblack.png",
                            SelectedIconSource = "helpwhite.png",
                            //MainIconSource = "helpgray.png",
                            MainIconSource = "helpblack.png",
                            TargetType = typeof(ReportGraph),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        //Logout
                        Logout = new MenuListModel()
                        {
                            Id = 8,
                            MainTitle = "Logout",
                            DefaultIconSource = "logoutgray.png",
                            SelectedIconSource = "logoutwhite.png",
                            MainIconSource = "logoutgray.png",
                            TargetType = typeof(Logout),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        MenuList.Add(HomePage);
                        MenuList.Add(MyProfile);
                        MenuList.Add(QueuingList);
                        MenuList.Add(Settings);
                        MenuList.Add(About);
                        MenuList.Add(Help);
                        MenuList.Add(Appointment);
                        MenuList.Add(ReportGraph);
                        MenuList.Add(Logout);

                        break;


                    case 1:
                        if (!IsRedirectedToProfile)
                        {
                            HomePage = new MenuListModel()
                            {
                                Id = 1,
                                MainTitle = "Home",
                                DefaultIconSource = "homegray.png",
                                SelectedIconSource = "homewhite.png",
                                MainIconSource = "homewhite.png",
                                TargetType = typeof(Views.Home.Home),
                                //MainTextColor = Color.FromHex("#ffffff"),
                                //DefaultTextColor = Color.FromHex("#797e86"),
                                //SelectedTextColor = Color.FromHex("#ffffff"),
                                MainTextColor = Color.FromHex("#ffffff"),
                                DefaultTextColor = Color.FromHex("#000000"),
                                SelectedTextColor = Color.FromHex("#ffffff"),
                                //MainBackgroundColor = Color.FromHex("#1c2027"),
                                MainBackgroundColor = Color.FromHex("#000000"),
                                DefaultBackgroundColor = Color.FromHex("#ffffff"),
                                SelectedBackgroundColor = Color.FromHex("#000000"),

                                //DefaultBackgroundColor = Color.FromHex("#444D5E"),
                                //SelectedBackgroundColor = Color.FromHex("#1c2027"),
                                BorderBackgroundColor = Color.Black
                            };
                        }
                        else
                        {
                            HomePage = new MenuListModel()
                            {
                                Id = 1,
                                MainTitle = "Home",
                                DefaultIconSource = "homegray.png",
                                SelectedIconSource = "homewhite.png",
                                MainIconSource = "homegray.png",
                                TargetType = typeof(Views.Home.Home),
                                //MainTextColor = Color.FromHex("#797e86"),
                                //DefaultTextColor = Color.FromHex("#797e86"),
                                //SelectedTextColor = Color.FromHex("#ffffff"),
                                //MainBackgroundColor = Color.FromHex("#444D5E"),
                                //DefaultBackgroundColor = Color.FromHex("#444D5E"),
                                //SelectedBackgroundColor = Color.FromHex("#1c2027"),
                                //BorderBackgroundColor = Color.Gray
                                MainTextColor = Color.FromHex("#797e86"),
                                DefaultTextColor = Color.FromHex("#797e86"),
                                SelectedTextColor = Color.FromHex("#ffffff"),
                                MainBackgroundColor = Color.FromHex("#ffffff"),
                                DefaultBackgroundColor = Color.FromHex("#ffffff"),
                                SelectedBackgroundColor = Color.FromHex("#000000"),
                                BorderBackgroundColor = Color.Black
                            };
                        }

                        //My Profile
                        //  MenuListModel MyProfile = null;
                        if (!IsRedirectedToProfile)
                        {
                            MyProfile = new MenuListModel()
                            {
                                Id = 3,
                                MainTitle = "My Profile",
                                DefaultIconSource = "myprofilegray.png",
                                SelectedIconSource = "myprofilewhite.png",
                                MainIconSource = "myprofilegray.png",
                                TargetType = typeof(MyProfile),
                                MainTextColor = Color.FromHex("#000000"),
                                DefaultTextColor = Color.FromHex("#000000"),
                                SelectedTextColor = Color.FromHex("#ffffff"),
                                MainBackgroundColor = Color.FromHex("#ffffff"),
                                DefaultBackgroundColor = Color.FromHex("#ffffff"),
                                SelectedBackgroundColor = Color.FromHex("#000000"),
                                BorderBackgroundColor = Color.Black
                            };
                        }
                        else
                        {
                            MyProfile = new MenuListModel()
                            {
                                Id = 3,
                                MainTitle = "My Profile",
                                DefaultIconSource = "myprofilegray.png",
                                SelectedIconSource = "myprofilewhite.png",
                                MainIconSource = "myprofilewhite.png",
                                TargetType = typeof(MyProfile),
                                MainTextColor = Color.FromHex("#000000"),
                                DefaultTextColor = Color.FromHex("#000000"),
                                SelectedTextColor = Color.FromHex("#ffffff"),
                                MainBackgroundColor = Color.FromHex("#ffffff"),
                                DefaultBackgroundColor = Color.FromHex("#ffffff"),
                                SelectedBackgroundColor = Color.FromHex("#000000"),
                                BorderBackgroundColor = Color.Black
                            };
                        }


                        //My barber profile 
                        MyBarberProfile = new MenuListModel()
                        {
                            Id = 5,
                            MainTitle = "My Barber Profile",
                            DefaultIconSource = "myprofilegray.png",
                            SelectedIconSource = "myprofilewhite.png",
                            MainIconSource = "myprofilegray.png",
                            TargetType = typeof(AddStaffMember),
                            //MainTextColor = Color.FromHex("#797e86"),
                            //DefaultTextColor = Color.FromHex("#797e86"),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        //Queuing List
                        QueuingList = new MenuListModel()
                        {
                            Id = 2,
                            MainTitle = "Queueing List",
                            DefaultIconSource = "queuegray.png",
                            SelectedIconSource = "queuewhite.png",
                            MainIconSource = "queuegray.png",
                            TargetType = typeof(QueueingList),
                            //MainTextColor = Color.FromHex("#797e86"),
                            //DefaultTextColor = Color.FromHex("#797e86"),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        //Settings
                        Settings = new MenuListModel()
                        {
                            Id = 4,
                            MainTitle = "Settings",
                            DefaultIconSource = "settingsgray.png",
                            SelectedIconSource = "settingswhite.png",
                            MainIconSource = "settingsgray.png",
                            TargetType = typeof(Settings),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        //About
                        About = new MenuListModel()
                        {
                            Id = 6,
                            MainTitle = "About",
                            DefaultIconSource = "aboutgray.png",
                            SelectedIconSource = "aboutwhite.png",
                            MainIconSource = "aboutgray.png",
                            TargetType = typeof(About),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        //About
                        Help = new MenuListModel()
                        {
                            Id = 7,
                            MainTitle = "Help",
                            //DefaultIconSource = "helpgray.png",
                            DefaultIconSource = "helpblack.png",
                            SelectedIconSource = "helpwhite.png",
                            //MainIconSource = "helpgray.png",
                            MainIconSource = "helpblack.png",
                            TargetType = typeof(HelpScreen),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        //Logout
                        Logout = new MenuListModel()
                        {
                            Id = 8,
                            MainTitle = "Logout",
                            DefaultIconSource = "logoutgray.png",
                            SelectedIconSource = "logoutwhite.png",
                            MainIconSource = "logoutgray.png",
                            TargetType = typeof(Logout),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        MenuList.Add(HomePage);
                        MenuList.Add(MyProfile);
                        MenuList.Add(MyBarberProfile);
                        MenuList.Add(QueuingList);
                        MenuList.Add(Settings);
                        MenuList.Add(About);
                        MenuList.Add(Help);
                        MenuList.Add(Logout);

                        break;

                    case 2:
                        if (!IsRedirectedToProfile)
                        {
                            HomePage = new MenuListModel()
                            {
                                Id = 1,
                                MainTitle = "Home",
                                DefaultIconSource = "homegray.png",
                                SelectedIconSource = "homewhite.png",
                                MainIconSource = "homewhite.png",
                                TargetType = typeof(Views.Home.Home),
                                //MainTextColor = Color.FromHex("#ffffff"),
                                //DefaultTextColor = Color.FromHex("#797e86"),
                                //SelectedTextColor = Color.FromHex("#ffffff"),
                                MainTextColor = Color.FromHex("#ffffff"),
                                DefaultTextColor = Color.FromHex("#000000"),
                                SelectedTextColor = Color.FromHex("#ffffff"),
                                //MainBackgroundColor = Color.FromHex("#1c2027"),
                                MainBackgroundColor = Color.FromHex("#000000"),
                                DefaultBackgroundColor = Color.FromHex("#ffffff"),
                                SelectedBackgroundColor = Color.FromHex("#000000"),

                                //DefaultBackgroundColor = Color.FromHex("#444D5E"),
                                //SelectedBackgroundColor = Color.FromHex("#1c2027"),
                                BorderBackgroundColor = Color.Black
                            };
                        }
                        else
                        {
                            HomePage = new MenuListModel()
                            {
                                Id = 1,
                                MainTitle = "Home",
                                DefaultIconSource = "homegray.png",
                                SelectedIconSource = "homewhite.png",
                                MainIconSource = "homegray.png",
                                TargetType = typeof(Views.Home.Home),
                                //MainTextColor = Color.FromHex("#797e86"),
                                //DefaultTextColor = Color.FromHex("#797e86"),
                                //SelectedTextColor = Color.FromHex("#ffffff"),
                                //MainBackgroundColor = Color.FromHex("#444D5E"),
                                //DefaultBackgroundColor = Color.FromHex("#444D5E"),
                                //SelectedBackgroundColor = Color.FromHex("#1c2027"),
                                //BorderBackgroundColor = Color.Gray
                                MainTextColor = Color.FromHex("#797e86"),
                                DefaultTextColor = Color.FromHex("#797e86"),
                                SelectedTextColor = Color.FromHex("#ffffff"),
                                MainBackgroundColor = Color.FromHex("#ffffff"),
                                DefaultBackgroundColor = Color.FromHex("#ffffff"),
                                SelectedBackgroundColor = Color.FromHex("#000000"),
                                BorderBackgroundColor = Color.Black
                            };
                        }

                        if (!IsRedirectedToProfile)
                        {
                            MyProfile = new MenuListModel()
                            {
                                Id = 3,
                                MainTitle = "My Profile",
                                DefaultIconSource = "myprofilegray.png",
                                SelectedIconSource = "myprofilewhite.png",
                                MainIconSource = "myprofilegray.png",
                                TargetType = typeof(MyProfile),
                                MainTextColor = Color.FromHex("#000000"),
                                DefaultTextColor = Color.FromHex("#000000"),
                                SelectedTextColor = Color.FromHex("#ffffff"),
                                MainBackgroundColor = Color.FromHex("#ffffff"),
                                DefaultBackgroundColor = Color.FromHex("#ffffff"),
                                SelectedBackgroundColor = Color.FromHex("#000000"),
                                BorderBackgroundColor = Color.Black
                            };
                        }
                        else
                        {
                            MyProfile = new MenuListModel()
                            {
                                Id = 3,
                                MainTitle = "My Profile",
                                DefaultIconSource = "myprofilegray.png",
                                SelectedIconSource = "myprofilewhite.png",
                                MainIconSource = "myprofilegray.png",
                                TargetType = typeof(MyProfile),
                                MainTextColor = Color.FromHex("#000000"),
                                DefaultTextColor = Color.FromHex("#000000"),
                                SelectedTextColor = Color.FromHex("#ffffff"),
                                MainBackgroundColor = Color.FromHex("#ffffff"),
                                DefaultBackgroundColor = Color.FromHex("#ffffff"),
                                SelectedBackgroundColor = Color.FromHex("#000000"),
                                BorderBackgroundColor = Color.Black
                            };
                        }

                        //Queuing List
                        QueuingList = new MenuListModel()
                        {
                            Id = 2,
                            MainTitle = "Queueing List",
                            DefaultIconSource = "queuegray.png",
                            SelectedIconSource = "queuewhite.png",
                            MainIconSource = "queuegray.png",
                            TargetType = typeof(QueueingList),
                            //MainTextColor = Color.FromHex("#797e86"),
                            //DefaultTextColor = Color.FromHex("#797e86"),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        //Settings
                        Settings = new MenuListModel()
                        {
                            Id = 4,
                            MainTitle = "Settings",
                            DefaultIconSource = "settingsgray.png",
                            SelectedIconSource = "settingswhite.png",
                            MainIconSource = "settingsgray.png",
                            TargetType = typeof(Settings),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        //AdminSettings
                        AdminSetting = new MenuListModel()
                        {
                            Id = 9,
                            MainTitle = "Admin Settings",
                            DefaultIconSource = "settingsgray.png",
                            SelectedIconSource = "settingswhite.png",
                            MainIconSource = "settingsgray.png",
                            TargetType = typeof(AdminSettings),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        //About
                        About = new MenuListModel()
                        {
                            Id = 6,
                            MainTitle = "About",
                            DefaultIconSource = "aboutgray.png",
                            SelectedIconSource = "aboutwhite.png",
                            MainIconSource = "aboutgray.png",
                            TargetType = typeof(About),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        //Help
                        Help = new MenuListModel()
                        {
                            Id = 7,
                            MainTitle = "Help",
                            //DefaultIconSource = "helpgray.png",
                            DefaultIconSource = "helpblack.png",
                            SelectedIconSource = "helpwhite.png",
                            //MainIconSource = "helpgray.png",
                            MainIconSource = "helpblack.png",
                            TargetType = typeof(HelpScreen),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        //ReportGrah
                        ReportGraph = new MenuListModel()
                        {
                            Id = 7,
                            MainTitle = "Report",
                            //DefaultIconSource = "helpgray.png",
                            DefaultIconSource = "helpblack.png",
                            SelectedIconSource = "helpwhite.png",
                            //MainIconSource = "helpgray.png",
                            MainIconSource = "helpblack.png",
                            TargetType = typeof(ReportGraph),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        //Logout
                        Logout = new MenuListModel()
                        {
                            Id = 8,
                            MainTitle = "Logout",
                            DefaultIconSource = "logoutgray.png",
                            SelectedIconSource = "logoutwhite.png",
                            MainIconSource = "logoutgray.png",
                            TargetType = typeof(Logout),
                            MainTextColor = Color.FromHex("#000000"),
                            DefaultTextColor = Color.FromHex("#000000"),
                            SelectedTextColor = Color.FromHex("#ffffff"),
                            MainBackgroundColor = Color.FromHex("#ffffff"),
                            DefaultBackgroundColor = Color.FromHex("#ffffff"),
                            SelectedBackgroundColor = Color.FromHex("#000000"),
                            BorderBackgroundColor = Color.Black
                        };

                        MenuList.Add(HomePage);
                        MenuList.Add(MyProfile);
                        MenuList.Add(QueuingList);
                        MenuList.Add(Settings);
                        MenuList.Add(AdminSetting);
                        MenuList.Add(About);
                        MenuList.Add(Help);
                        MenuList.Add(ReportGraph);
                        MenuList.Add(Logout);

                        break;
                }



                //MenuList.Add(HomePage);
                //MenuList.Add(QueuingList);
                //MenuList.Add(MyProfile);
                //MenuList.Add(Settings);
                //MenuList.Add(AdminSetting);
                //MenuList.Add(About);
                //MenuList.Add(Help);
                //MenuList.Add(Logout);
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex, "testPage");
            }
        }

        public void ChangeMenuList(int id)
        {
            //if (Convert.ToString(Application.Current.Properties["ChangeColor"]) != "true")
            //   {
            //if (id > 0)
            //{
            for (int i = 0; i < MenuList.Count(); i++)
            {
                if (MenuList[i].Id == id)
                {
                    MenuList[i].MainTextColor = MenuList[i].SelectedTextColor;
                    MenuList[i].MainIconSource = MenuList[i].SelectedIconSource;
                    MenuList[i].MainBackgroundColor = MenuList[i].SelectedBackgroundColor;
                }
                else
                {
                    MenuList[i].MainTextColor = MenuList[i].DefaultTextColor;
                    MenuList[i].MainIconSource = MenuList[i].DefaultIconSource;
                    MenuList[i].MainBackgroundColor = MenuList[i].DefaultBackgroundColor;
                }
            }
            // }
            //}

            //else if(Convert.ToString(Application.Current.Properties["ChangeColor"]) == "0")
            //{
            //    for (int i = 0; i < MenuList.Count(); i++)
            //    {
            //        if (MenuList[i].Id == 1)
            //        {
            //            MenuList[i].MainTextColor = MenuList[i].SelectedTextColor;
            //            MenuList[i].MainIconSource = MenuList[i].SelectedIconSource;
            //            MenuList[i].MainBackgroundColor = MenuList[i].SelectedBackgroundColor;
            //        }
            //        else
            //        {
            //            MenuList[i].MainTextColor = MenuList[i].DefaultTextColor;
            //            MenuList[i].MainIconSource = MenuList[i].DefaultIconSource;
            //            MenuList[i].MainBackgroundColor = Color.FromHex("#ffffff");
            //        }
            //    }
            //}
            //if(Convert.ToString(Application.Current.Properties["ChangeColor"]) == "0")
            //{
            //    for (int i = 0; i < MenuList.Count(); i++)
            //    {
            //        if (MenuList[i].Id == 1)
            //        {
            //            MenuList[i].MainTextColor = MenuList[i].SelectedTextColor;
            //            MenuList[i].MainIconSource = MenuList[i].SelectedIconSource;
            //            MenuList[i].MainBackgroundColor = MenuList[i].SelectedBackgroundColor;
            //        }
            //        else
            //        {
            //            MenuList[i].MainTextColor = MenuList[i].DefaultTextColor;
            //            MenuList[i].MainIconSource = MenuList[i].DefaultIconSource;
            //            MenuList[i].MainBackgroundColor = Color.FromHex("#ffffff");
            //        }
            //    }
            //}
        }

        private string m_ProfileImage;

        public string ProfileImage
        {
            get { return m_ProfileImage; }
            set { m_ProfileImage = value; OnPropertyChanged("ProfileImage"); }
        }

        private string m_UserName;

        public string UserName
        {
            get { return m_UserName; }
            set { m_UserName = value; OnPropertyChanged("UserName"); }
        }

        private ObservableCollection<MenuListModel> m_MenuList;

        public ObservableCollection<MenuListModel> MenuList
        {
            get { return m_MenuList; }
            set { m_MenuList = value; OnPropertyChanged("MenuList"); }
        }
    }

    public class MenuListModel : BaseViewModel
    {
        //public int Id { get; set; }

        private int m_Id;

        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; OnPropertyChanged("Id"); }
        }

        //public string MainTitle { get; set; }

        private string m_MainTitle;

        public string MainTitle
        {
            get { return m_MainTitle; }
            set { m_MainTitle = value; OnPropertyChanged("MainTitle"); }
        }

        //public ImageSource DefaultIconSource { get; set; }

        private ImageSource m_DefaultIconSource;

        public ImageSource DefaultIconSource
        {
            get { return m_DefaultIconSource; }
            set { m_DefaultIconSource = value; OnPropertyChanged("DefaultIconSource"); }
        }

        //public ImageSource SelectedIconSource { get; set; }

        private ImageSource m_SelectedIconSource;

        public ImageSource SelectedIconSource
        {
            get { return m_SelectedIconSource; }
            set { m_SelectedIconSource = value; OnPropertyChanged("SelectedIconSource"); }
        }

        //public ImageSource MainIconSource { get; set; }

        private ImageSource m_MainIconSource;

        public ImageSource MainIconSource
        {
            get { return m_MainIconSource; }
            set { m_MainIconSource = value; OnPropertyChanged("MainIconSource"); }
        }

        //public Type TargetType { get; set; }

        private Type m_TargetType;

        public Type TargetType
        {
            get { return m_TargetType; }
            set { m_TargetType = value; OnPropertyChanged("TargetType"); }
        }

        //public Color DefaultTextColor { get; set; }

        private Color m_DefaultTextColor;

        public Color DefaultTextColor
        {
            get { return m_DefaultTextColor; }
            set { m_DefaultTextColor = value; OnPropertyChanged("DefaultTextColor"); }
        }

        //public Color SelectedTextColor { get; set; }

        private Color m_SelectedTextColor;

        public Color SelectedTextColor
        {
            get { return m_SelectedTextColor; }
            set { m_SelectedTextColor = value; OnPropertyChanged("SelectedTextColor"); }
        }

        //public Color MainTextColor { get; set; }

        private Color m_MainTextColor;

        public Color MainTextColor
        {
            get { return m_MainTextColor; }
            set { m_MainTextColor = value; OnPropertyChanged("MainTextColor"); }
        }

        //public Color MainBackgroundColor { get; set; }

        private Color m_MainBackgroundColor;

        public Color MainBackgroundColor
        {
            get { return m_MainBackgroundColor; }
            set { m_MainBackgroundColor = value; OnPropertyChanged("MainBackgroundColor"); }
        }

        //public Color DefaultBackgroundColor { get; set; }

        private Color m_DefaultBackgroundColor;

        public Color DefaultBackgroundColor
        {
            get { return m_DefaultBackgroundColor; }
            set { m_DefaultBackgroundColor = value; OnPropertyChanged("DefaultBackgroundColor"); }
        }

        //public Color SelectedBackgroundColor { get; set; }

        private Color m_SelectedBackgroundColor;

        public Color SelectedBackgroundColor
        {
            get { return m_SelectedBackgroundColor; }
            set { m_SelectedBackgroundColor = value; OnPropertyChanged("SelectedBackgroundColor"); }
        }

        private Color m_BorderBackgroundColor;

        public Color BorderBackgroundColor
        {
            get { return m_BorderBackgroundColor; }
            set { m_BorderBackgroundColor = value; OnPropertyChanged("BorderBackgroundColor"); }
        }
    }
}