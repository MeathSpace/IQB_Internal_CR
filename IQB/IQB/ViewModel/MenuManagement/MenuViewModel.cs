using IQB.Views.ApplicationManagement;
using IQB.Views.LoginManagement;
using IQB.Views.Settings;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace IQB.ViewModel
{
    public class MenuViewModel
    {
        public List<MasterPageItems> menuList { get; set; }

        public MenuViewModel()
        {
            menuList = new List<MasterPageItems>();

            var HomePage = new MasterPageItems() { Title = "Home", Icon = "home.png", TargetType = typeof(MyProfile), TextColor = Color.FromHex("#797e86") };
            var QueuingList = new MasterPageItems() { Title = "Queuing List", Icon = "queinglist.png", TargetType = typeof(MyProfile), TextColor = Color.FromHex("#797e86") };
            var MyProfile = new MasterPageItems() { Title = "MyProfile", Icon = "myprofile.png", TargetType = typeof(MyProfile), TextColor = Color.FromHex("#797e86") };
            var Settings = new MasterPageItems() { Title = "Settings", Icon = "settings.png", TargetType = typeof(Settings), TextColor = Color.FromHex("#797e86") };
            var AdminSettings = new MasterPageItems() { Title = "AdminSettings", Icon = "settings.png", TargetType = typeof(Settings), TextColor = Color.FromHex("#797e86") };
            var About = new MasterPageItems() { Title = "About", Icon = "about.png", TargetType = typeof(MyProfile), TextColor = Color.FromHex("#797e86") };
            //var Appointment = new MasterPageItems() { Title = "Appointment", Icon = "about.png", TargetType = typeof(MyProfile), TextColor = Color.FromHex("#797e86") };
            var Logout = new MasterPageItems() { Title = "Logout", Icon = "logout.png", TargetType = typeof(Logout), TextColor = Color.FromHex("#797e86") };
            
            menuList.Add(HomePage);
            menuList.Add(QueuingList);
            menuList.Add(MyProfile);
            menuList.Add(Settings);
            menuList.Add(AdminSettings);
            menuList.Add(About);
           // menuList.Add(Appointment);
            menuList.Add(Logout);
           
        }
    }

    public class MasterPageItems
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type TargetType { get; set; }
        public Color TextColor { get; set; }
    }
}