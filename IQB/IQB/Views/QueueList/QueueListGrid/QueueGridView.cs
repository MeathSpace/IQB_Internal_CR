namespace IQB.Views.QueueList.QueueListGrid
{
    using EntityLayer.Common;
    using EntityLayer.QueueELS;
    using IQBServices.QueueServices;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using ViewModel.QueueListManagement;
    using Xamarin.Forms;

    public class QueueGridView : ContentView
    {
        private AbsoluteLayout absolute;
        //private Grid queueGrid;
        private QueuelistViewModel viewModel;
        private List<QueueEL> queueList;
        private List<QueueEL> tempQlistNew;
        StackLayout mainLayout;
        bool IsRecordExist;
        //int PageNo = 1;

        public QueueGridView(QueuelistViewModel viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = this.viewModel;

            queueList = new List<QueueEL>();

            //Set absolute layout
            //SetAbsoluteLayout();
            //Set absolute layout

            mainLayout = new StackLayout() { Spacing = 0, BackgroundColor = Color.Black };

            //initialize queue grid and create headers
            SetQueueGridView();
            //initialize queue grid and create headers

            //Set the grid view data
            //SetGridQueueListData();
            //Set the grid view data

            //Append the grid to absolute view
            //appendGridToAbsoluteLayout();
            //Append the grid to absolute view


            //mainLayout.Children.Add(queueGrid);

            ScrollView scroll = new ScrollView();
            scroll.BackgroundColor = Color.Black;
            scroll.Content = mainLayout;
            Content = scroll;
            //scroll.Scrolled += Scroll_Scrolled;
        }

        private void Scroll_Scrolled(object sender, ScrolledEventArgs e)
        {
            App.Current.MainPage.DisplayAlert("Scroll Finished", "Scrolled", "OK");
        }

        #region Main Absolute Layout & Grid Set

        private void SetAbsoluteLayout()
        {
            absolute = new AbsoluteLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
        }

        private void SetQueueGridView()
        {
            //int RecordCount = queueList.Count();
            Grid queueGrid = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                HeightRequest = 40,
                BackgroundColor = Color.FromHex("#0DA5B7"),
                Padding = new Thickness(15, 20, 0, 0)
                //VerticalOptions = LayoutOptions.CenterAndExpand,
                //HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            queueGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });

            queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //     queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.01, GridUnitType.Star) });
            queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //      queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.01, GridUnitType.Star) });
            queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Label col0Text1 = new Label()
            {
                Text = "Q-Position",
                TextColor = Color.White,
                //HorizontalTextAlignment = TextAlignment.Center,
                //VerticalTextAlignment = TextAlignment.Center,
            };

            StackLayout col0Layout1 = new StackLayout()
            {
                BackgroundColor = Color.Transparent,
                // BackgroundColor = CommonEL.ListHeaderCellBackgroundColor,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            col0Layout1.Children.Add(col0Text1);

            Label col1Text1 = new Label()
            {
                Text = "Name",
                TextColor = Color.White,
                //HorizontalTextAlignment = TextAlignment.Center,
                //VerticalTextAlignment = TextAlignment.Center
            };

            StackLayout col1Layout1 = new StackLayout()
            {
                BackgroundColor = Color.Transparent,
                //      BackgroundColor = CommonEL.ListHeaderCellBackgroundColor,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(15, 0, 0, 0),
            };

            col1Layout1.Children.Add(col1Text1);

            Label col2Text1 = new Label()
            {
                Text = "Barber",
                TextColor = Color.White,
                //HorizontalTextAlignment = TextAlignment.Center,
                //VerticalTextAlignment = TextAlignment.Center,
            };

            StackLayout col2Layout1 = new StackLayout()
            {
                BackgroundColor = Color.Transparent,
                //      BackgroundColor = CommonEL.ListHeaderCellBackgroundColor,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
                //Padding = new Thickness(0, 7, 0, 7),
                //Margin = new Thickness(0, 0, 0, 2)
            };

            col2Layout1.Children.Add(col2Text1);

            //queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.05, BackgroundColor = CommonEL.WhiteHorizontalSeperatorColor, Margin = new Thickness(0, 0, 0, 2) }, 0, 0);
            queueGrid.Children.Add(col0Layout1, 0, 0);
            //   queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = CommonEL.WhiteHorizontalSeperatorColor, Margin = new Thickness(0, 0, 0, 2) }, 1, 0);
            queueGrid.Children.Add(col1Layout1, 1, 0);
            //      queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = CommonEL.WhiteHorizontalSeperatorColor, Margin = new Thickness(0, 0, 0, 2) }, 3, 0);
            queueGrid.Children.Add(col2Layout1, 2, 0);


            Frame FrameSample = new Frame()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(0),
                CornerRadius = 8
            };

            FrameSample.Content = queueGrid;

            mainLayout.Children.Add(FrameSample);
        }

        //private void appendGridToAbsoluteLayout()
        //{
        //    StackLayout stackLayout = new StackLayout()
        //    {
        //        Spacing = 0,
        //        Children =
        //        {
        //            queueGrid
        //        }
        //    };

        //    // Position the pageLayout to fill the entire screen.
        //    // Manage positioning of child elements on the page by editing the pageLayout.
        //    AbsoluteLayout.SetLayoutFlags(stackLayout, AbsoluteLayoutFlags.All);
        //    AbsoluteLayout.SetLayoutBounds(stackLayout, new Rectangle(0f, 0f, 1f, 1f));
        //    absolute.Children.Add(stackLayout);
        //}

        #endregion Main Absolute Layout & Grid Set

        #region Public methods


        #region Old Code

        //    public void SetGrid()
        //    {
        //        var plainButton = new Style(typeof(Button))
        //        {
        //            Setters = {
        //  new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex ("#eee") },
        //  new Setter { Property = Button.TextColorProperty, Value = Color.Black },
        //  new Setter { Property = Button.BorderRadiusProperty, Value = 0 },
        //  new Setter { Property = Button.FontSizeProperty, Value = 40 }
        //}
        //        };
        //        var darkerButton = new Style(typeof(Button))
        //        {
        //            Setters = {
        //  new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex ("#ddd") },
        //  new Setter { Property = Button.TextColorProperty, Value = Color.Black },
        //  new Setter { Property = Button.BorderRadiusProperty, Value = 0 },
        //  new Setter { Property = Button.FontSizeProperty, Value = 40 }
        //}
        //        };
        //        var orangeButton = new Style(typeof(Button))
        //        {
        //            Setters = {
        //  new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex ("#E8AD00") },
        //  new Setter { Property = Button.TextColorProperty, Value = Color.White },
        //  new Setter { Property = Button.BorderRadiusProperty, Value = 0 },
        //  new Setter { Property = Button.FontSizeProperty, Value = 40 }
        //}
        //        };

        //        var controlGrid = new Grid { RowSpacing = 1, ColumnSpacing = 1 };
        //        controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) });
        //        controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        //        controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        //        controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        //        controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        //        controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        //        controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        //        controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        //        controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        //        controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        //        var label = new Label
        //        {
        //            Text = "0",
        //            HorizontalTextAlignment = TextAlignment.End,
        //            VerticalTextAlignment = TextAlignment.End,
        //            TextColor = Color.White,
        //            FontSize = 60
        //        };
        //        controlGrid.Children.Add(label, 0, 0);

        //        Grid.SetColumnSpan(label, 4);

        //        controlGrid.Children.Add(new Button { Text = "C", Style = darkerButton }, 0, 1);
        //        controlGrid.Children.Add(new Button { Text = "+/-", Style = darkerButton }, 1, 1);
        //        controlGrid.Children.Add(new Button { Text = "%", Style = darkerButton }, 2, 1);
        //        controlGrid.Children.Add(new Button { Text = "div", Style = orangeButton }, 3, 1);
        //        controlGrid.Children.Add(new Button { Text = "7", Style = plainButton }, 0, 2);
        //        controlGrid.Children.Add(new Button { Text = "8", Style = plainButton }, 1, 2);
        //        controlGrid.Children.Add(new Button { Text = "9", Style = plainButton }, 2, 2);
        //        controlGrid.Children.Add(new Button { Text = "X", Style = orangeButton }, 3, 2);
        //        controlGrid.Children.Add(new Button { Text = "4", Style = plainButton }, 0, 3);
        //        controlGrid.Children.Add(new Button { Text = "5", Style = plainButton }, 1, 3);
        //        controlGrid.Children.Add(new Button { Text = "6", Style = plainButton }, 2, 3);
        //        controlGrid.Children.Add(new Button { Text = "-", Style = orangeButton }, 3, 3);
        //        controlGrid.Children.Add(new Button { Text = "1", Style = plainButton }, 0, 4);
        //        controlGrid.Children.Add(new Button { Text = "2", Style = plainButton }, 1, 4);
        //        controlGrid.Children.Add(new Button { Text = "3", Style = plainButton }, 2, 4);
        //        controlGrid.Children.Add(new Button { Text = "+", Style = orangeButton }, 3, 4);
        //        controlGrid.Children.Add(new Button { Text = ".", Style = plainButton }, 2, 5);
        //        controlGrid.Children.Add(new Button { Text = "=", Style = orangeButton }, 3, 5);

        //        var zeroButton = new Button { Text = "0", Style = plainButton };
        //        controlGrid.Children.Add(zeroButton, 0, 5);
        //        Grid.SetColumnSpan(zeroButton, 2);

        //        StackLayout stackLayout = new StackLayout()
        //        {
        //            Padding = new Thickness(20, 20, 20, 20),
        //            Spacing = 0,
        //            Children =
        //            {
        //                queueGrid
        //            }
        //        };

        //        // Position the pageLayout to fill the entire screen.
        //        // Manage positioning of child elements on the page by editing the pageLayout.
        //        AbsoluteLayout.SetLayoutFlags(stackLayout, AbsoluteLayoutFlags.All);
        //        AbsoluteLayout.SetLayoutBounds(stackLayout, new Rectangle(0f, 0f, 1f, 1f));
        //        absolute.Children.Add(stackLayout);
        //    }

        //    public void SetGridQueueList()
        //    {
        //        Label col0Text1 = new Label()
        //        {
        //            Text = "Q-Postion",
        //            TextColor = Color.FromHex("#ffffff"),
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center,
        //        };

        //        StackLayout col0Layout1 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("#252931"),
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand,
        //            Padding = new Thickness(0, 5, 0, 0),
        //            Margin = new Thickness(0, 0, 0, 2)
        //        };

        //        col0Layout1.Children.Add(col0Text1);

        //        Label col1Text1 = new Label()
        //        {
        //            Text = "Name",
        //            TextColor = Color.FromHex("#ffffff"),
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center,
        //            Margin = new Thickness(0, 0, 0, 2)
        //        };

        //        StackLayout col1Layout1 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("#252931"),
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand,
        //            Padding = new Thickness(0, 5, 0, 0),
        //            Margin = new Thickness(0, 0, 0, 2)
        //        };

        //        col1Layout1.Children.Add(col1Text1);

        //        Label col2Text1 = new Label()
        //        {
        //            Text = "Barber",
        //            TextColor = Color.FromHex("#ffffff"),
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center,
        //        };

        //        StackLayout col2Layout1 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("#252931"),
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand,
        //            Padding = new Thickness(0, 5, 0, 0),
        //            Margin = new Thickness(0, 0, 0, 2)
        //        };

        //        col2Layout1.Children.Add(col2Text1);

        //        Grid controlGrid = new Grid
        //        {
        //            RowSpacing = 1,
        //            ColumnSpacing = 1,
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand
        //        };

        //        controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30, GridUnitType.Star) });
        //        controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        //        controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30, GridUnitType.Auto) });
        //        controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
        //        controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30, GridUnitType.Auto) });

        //        controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.05, GridUnitType.Star) });
        //        controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        //        controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.01, GridUnitType.Star) });
        //        controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        //        controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.01, GridUnitType.Star) });
        //        controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        //        controlGrid.Children.Add(new StackLayout() { WidthRequest = 0.05, BackgroundColor = Color.FromHex("020203"), Margin = new Thickness(0, 0, 0, 2) }, 0, 0);
        //        controlGrid.Children.Add(col0Layout1, 1, 0);
        //        controlGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = Color.FromHex("797e86"), Margin = new Thickness(0, 0, 0, 2) }, 2, 0);
        //        controlGrid.Children.Add(col1Layout1, 3, 0);
        //        controlGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = Color.FromHex("797e86"), Margin = new Thickness(0, 0, 0, 2) }, 4, 0);
        //        controlGrid.Children.Add(col2Layout1, 5, 0);

        //        StackLayout lineSeparator = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("797e86"),
        //            HeightRequest = 1
        //        };

        //        controlGrid.Children.Add(new StackLayout() { BackgroundColor = Color.FromHex("020203"), HeightRequest = 1 }, 0, 1);
        //        controlGrid.Children.Add(lineSeparator, 1, 1);
        //        Grid.SetColumnSpan(lineSeparator, 5);

        //        //Content

        //        Label col0Text2 = new Label()
        //        {
        //            Text = "1",
        //            TextColor = Color.FromHex("#797e86"),
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center,
        //        };

        //        StackLayout col0Layout2 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("#252931"),
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand,
        //            Margin = new Thickness(0, 2, 0, 2)
        //        };

        //        col0Layout2.Children.Add(col0Text2);

        //        Label col1Text2 = new Label()
        //        {
        //            Text = "Carlos Antonio Dhar Carlos Antonio Dhar",
        //            TextColor = Color.FromHex("#797e86"),
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center,
        //        };

        //        StackLayout col1Layout2 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("#252931"),
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand,
        //            Margin = new Thickness(0, 2, 0, 2)
        //        };

        //        col1Layout2.Children.Add(col1Text2);

        //        Label col2Text2 = new Label()
        //        {
        //            Text = "Barber Name",
        //            TextColor = Color.FromHex("#797e86"),
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center,
        //        };

        //        StackLayout col2Layout2 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("#252931"),
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand,
        //            Margin = new Thickness(0, 2, 0, 2)
        //        };

        //        col2Layout2.Children.Add(col2Text2);

        //        controlGrid.Children.Add(new StackLayout() { WidthRequest = 0.05, BackgroundColor = Color.FromHex("020203"), Margin = new Thickness(0, 2, 0, 2) }, 0, 2);
        //        controlGrid.Children.Add(col0Layout2, 1, 2);
        //        controlGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = Color.FromHex("797e86"), Margin = new Thickness(0, 2, 0, 2) }, 2, 2);
        //        controlGrid.Children.Add(col1Layout2, 3, 2);
        //        controlGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = Color.FromHex("797e86"), Margin = new Thickness(0, 2, 0, 2) }, 4, 2);
        //        controlGrid.Children.Add(col2Layout2, 5, 2);

        //        StackLayout lSeparator1 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("797e86"),
        //            HeightRequest = 1
        //        };

        //        controlGrid.Children.Add(new StackLayout() { BackgroundColor = Color.FromHex("020203"), HeightRequest = 1 }, 0, 3);
        //        controlGrid.Children.Add(lSeparator1, 1, 3);
        //        Grid.SetColumnSpan(lSeparator1, 5);

        //        Label col0Text3 = new Label()
        //        {
        //            Text = "1",
        //            TextColor = Color.FromHex("#ef3430"),
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center,
        //        };

        //        StackLayout col0Layout3 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("#252931"),
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand,
        //            Margin = new Thickness(0, 2, 0, 0)
        //        };

        //        col0Layout3.Children.Add(col0Text3);

        //        Label col1Text3 = new Label()
        //        {
        //            Text = "Carlos Antonio",
        //            TextColor = Color.FromHex("#ef3430"),
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center,
        //        };

        //        StackLayout col1Layout3 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("#252931"),
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand,
        //            Margin = new Thickness(0, 2, 0, 0)
        //        };

        //        col1Layout3.Children.Add(col1Text3);

        //        Label col2Text3 = new Label()
        //        {
        //            Text = "Barber Name",
        //            TextColor = Color.FromHex("#ef3430"),
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center,
        //        };

        //        StackLayout col2Layout3 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("#252931"),
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand,
        //            Margin = new Thickness(0, 2, 0, 0)
        //        };

        //        col2Layout3.Children.Add(col2Text3);

        //        controlGrid.Children.Add(new StackLayout() { WidthRequest = 0.05, BackgroundColor = Color.FromHex("ef3430"), Margin = new Thickness(0, 0, 0, 0) }, 0, 4);
        //        controlGrid.Children.Add(col0Layout3, 1, 4);
        //        controlGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = Color.FromHex("797e86"), Margin = new Thickness(0, 2, 0, 0) }, 2, 4);
        //        controlGrid.Children.Add(col1Layout3, 3, 4);
        //        controlGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = Color.FromHex("797e86"), Margin = new Thickness(0, 2, 0, 0) }, 4, 4);
        //        controlGrid.Children.Add(col2Layout3, 5, 4);

        //        StackLayout stackLayout = new StackLayout()
        //        {
        //            Padding = new Thickness(20, 20, 20, 20),
        //            Spacing = 0,
        //            Children =
        //            {
        //                queueGrid
        //            }
        //        };

        //        // Position the pageLayout to fill the entire screen.
        //        // Manage positioning of child elements on the page by editing the pageLayout.
        //        AbsoluteLayout.SetLayoutFlags(stackLayout, AbsoluteLayoutFlags.All);
        //        AbsoluteLayout.SetLayoutBounds(stackLayout, new Rectangle(0f, 0f, 1f, 1f));
        //        absolute.Children.Add(stackLayout);
        //    }

        //    public void SetGridQueueListV1()
        //    {
        //        StackLayout lineSeparator = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("797e86"),
        //            HeightRequest = 1
        //        };

        //        queueGrid.Children.Add(new StackLayout() { BackgroundColor = Color.FromHex("020203"), HeightRequest = 1 }, 0, 1);
        //        queueGrid.Children.Add(lineSeparator, 1, 1);
        //        Grid.SetColumnSpan(lineSeparator, 5);

        //        //Content

        //        Label col0Text2 = new Label()
        //        {
        //            Text = "1",
        //            TextColor = Color.FromHex("#797e86"),
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center,
        //        };

        //        StackLayout col0Layout2 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("#252931"),
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand,
        //            Margin = new Thickness(0, 2, 0, 2)
        //        };

        //        col0Layout2.Children.Add(col0Text2);

        //        Label col1Text2 = new Label()
        //        {
        //            Text = "Carlos Antonio Dhar Carlos Antonio Dhar",
        //            TextColor = Color.FromHex("#797e86"),
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center,
        //        };

        //        StackLayout col1Layout2 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("#252931"),
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand,
        //            Margin = new Thickness(0, 2, 0, 2)
        //        };

        //        col1Layout2.Children.Add(col1Text2);

        //        Label col2Text2 = new Label()
        //        {
        //            Text = "Barber Name",
        //            TextColor = Color.FromHex("#797e86"),
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center,
        //        };

        //        StackLayout col2Layout2 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("#252931"),
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand,
        //            Margin = new Thickness(0, 2, 0, 2)
        //        };

        //        col2Layout2.Children.Add(col2Text2);

        //        queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.05, BackgroundColor = Color.FromHex("020203"), Margin = new Thickness(0, 2, 0, 2) }, 0, 2);
        //        queueGrid.Children.Add(col0Layout2, 1, 2);
        //        queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = Color.FromHex("797e86"), Margin = new Thickness(0, 2, 0, 2) }, 2, 2);
        //        queueGrid.Children.Add(col1Layout2, 3, 2);
        //        queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = Color.FromHex("797e86"), Margin = new Thickness(0, 2, 0, 2) }, 4, 2);
        //        queueGrid.Children.Add(col2Layout2, 5, 2);

        //        StackLayout lSeparator1 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("797e86"),
        //            HeightRequest = 1
        //        };

        //        queueGrid.Children.Add(new StackLayout() { BackgroundColor = Color.FromHex("020203"), HeightRequest = 1 }, 0, 3);
        //        queueGrid.Children.Add(lSeparator1, 1, 3);
        //        Grid.SetColumnSpan(lSeparator1, 5);

        //        Label col0Text3 = new Label()
        //        {
        //            Text = "1",
        //            TextColor = Color.FromHex("#ef3430"),
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center,
        //        };

        //        StackLayout col0Layout3 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("#252931"),
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand,
        //            Margin = new Thickness(0, 2, 0, 0)
        //        };

        //        col0Layout3.Children.Add(col0Text3);

        //        Label col1Text3 = new Label()
        //        {
        //            Text = "Carlos Antonio",
        //            TextColor = Color.FromHex("#ef3430"),
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center,
        //        };

        //        StackLayout col1Layout3 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("#252931"),
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand,
        //            Margin = new Thickness(0, 2, 0, 0)
        //        };

        //        col1Layout3.Children.Add(col1Text3);

        //        Label col2Text3 = new Label()
        //        {
        //            Text = "Barber Name",
        //            TextColor = Color.FromHex("#ef3430"),
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center,
        //        };

        //        StackLayout col2Layout3 = new StackLayout()
        //        {
        //            BackgroundColor = Color.FromHex("#252931"),
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand,
        //            Margin = new Thickness(0, 2, 0, 0)
        //        };

        //        col2Layout3.Children.Add(col2Text3);

        //        queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.05, BackgroundColor = Color.FromHex("ef3430"), Margin = new Thickness(0, 0, 0, 0) }, 0, 4);
        //        queueGrid.Children.Add(col0Layout3, 1, 4);
        //        queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = Color.FromHex("797e86"), Margin = new Thickness(0, 2, 0, 0) }, 2, 4);
        //        queueGrid.Children.Add(col1Layout3, 3, 4);
        //        queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = Color.FromHex("797e86"), Margin = new Thickness(0, 2, 0, 0) }, 4, 4);
        //        queueGrid.Children.Add(col2Layout3, 5, 4);
        //    }

        //public async void SetGridQueueListData()
        //{

        //    await SetQueueList();

        //    int RecordCount = queueList.Count();

        //    Grid queueGrid = new Grid
        //    {
        //        RowSpacing = 0,
        //        ColumnSpacing = 0,
        //        VerticalOptions = LayoutOptions.FillAndExpand,
        //        HorizontalOptions = LayoutOptions.FillAndExpand
        //    };

        //    if (RecordCount > 0)
        //    {
        //        for (int i = 0; i < RecordCount; i++)
        //        {
        //            queueGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
        //            queueGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30, GridUnitType.Auto) });
        //        }
        //    }
        //    else
        //    {
        //        //queueGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Star) });
        //        queueGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30, GridUnitType.Auto) });
        //    }

        //    if (queueList != null && queueList.Count() > 0)
        //    {
        //        int RowNumber = 0;
        //        for (int i = 0; i < queueList.Count(); i++)
        //        {
        //            bool IsLastRecord = (i == (queueList.Count() - 1)) ? true : false;
        //            RowNumber++;

        //            //StackLayout lineSeparator = new StackLayout()
        //            //{
        //            //    BackgroundColor = CommonEL.LightHorizontalSeperatorColor,
        //            //    HeightRequest = 1
        //            //};

        //            //queueGrid.Children.Add(new StackLayout() { BackgroundColor = CommonEL.WhiteHorizontalSeperatorColor, HeightRequest = 1 }, 0, RowNumber);
        //            //queueGrid.Children.Add(lineSeparator, 0, RowNumber);
        //            //Grid.SetColumnSpan(lineSeparator, 4);

        //            RowNumber++;

        //            Label col0Text = new Label()
        //            {
        //                Text = Convert.ToString(queueList[i].Position),
        //                TextColor = queueList[i].IsCurrent ? CommonEL.RedTextColor : CommonEL.LightBlackTextColor,
        //                HorizontalTextAlignment = TextAlignment.Center,
        //                VerticalTextAlignment = TextAlignment.Center
        //            };

        //            StackLayout col0Layout = new StackLayout()
        //            {
        //                BackgroundColor = CommonEL.ListDataCellBackgroundColor,
        //                VerticalOptions = LayoutOptions.FillAndExpand,
        //                HorizontalOptions = LayoutOptions.FillAndExpand,
        //                Margin = IsLastRecord ? new Thickness(0, 0.01, 0, 0) : new Thickness(0, 0.01, 0, 2),
        //                Padding = new Thickness(0, 5, 0, 5)
        //            };

        //            col0Layout.Children.Add(col0Text);

        //            Label col1Text = new Label()
        //            {
        //                Text = Convert.ToString(queueList[i].Name),
        //                TextColor = queueList[i].IsCurrent ? CommonEL.RedTextColor : CommonEL.LightBlackTextColor,
        //                HorizontalTextAlignment = TextAlignment.Center,
        //                VerticalTextAlignment = TextAlignment.Center,
        //            };

        //            StackLayout col1Layout = new StackLayout()
        //            {
        //                BackgroundColor = CommonEL.ListDataCellBackgroundColor,
        //                VerticalOptions = LayoutOptions.FillAndExpand,
        //                HorizontalOptions = LayoutOptions.FillAndExpand,
        //                Margin = IsLastRecord ? new Thickness(0, 0.01, 0, 0) : new Thickness(0, 0.01, 0, 2),
        //                Padding = new Thickness(0, 5, 0, 5)
        //            };

        //            col1Layout.Children.Add(col1Text);

        //            Label col2Text = new Label()
        //            {
        //                Text = Convert.ToString(queueList[i].BarberName),
        //                TextColor = queueList[i].IsCurrent ? CommonEL.RedTextColor : CommonEL.LightBlackTextColor,
        //                HorizontalTextAlignment = TextAlignment.Center,
        //                VerticalTextAlignment = TextAlignment.Center,
        //            };

        //            StackLayout col2Layout = new StackLayout()
        //            {
        //                BackgroundColor = CommonEL.ListDataCellBackgroundColor,
        //                VerticalOptions = LayoutOptions.FillAndExpand,
        //                HorizontalOptions = LayoutOptions.FillAndExpand,
        //                Margin = IsLastRecord ? new Thickness(0, 0.01, 0, 0) : new Thickness(0, 0.01, 0, 2),
        //                Padding = new Thickness(0, 5, 0, 5)
        //            };

        //            col2Layout.Children.Add(col2Text);

        //            //queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.05, BackgroundColor = queueList[i].IsCurrent ? CommonEL.RedVerticalSeperatorColor : CommonEL.LightBackgroundColor, Margin = (queueList[i].IsCurrent || IsLastRecord) ? new Thickness(0, 0, 0, 0) : new Thickness(0, 2, 0, 2) }, 0, RowNumber);
        //            queueGrid.Children.Add(col0Layout, 0, RowNumber);
        //            queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = CommonEL.WhiteHorizontalSeperatorColor, Margin = IsLastRecord ? new Thickness(0, 0.01, 0, 0) : new Thickness(0, 0.01, 0, 2) }, 1, RowNumber);
        //            queueGrid.Children.Add(col1Layout, 2, RowNumber);
        //            queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = CommonEL.WhiteHorizontalSeperatorColor, Margin = IsLastRecord ? new Thickness(0, 0.01, 0, 0) : new Thickness(0, 0.01, 0, 2) }, 3, RowNumber);
        //            queueGrid.Children.Add(col2Layout, 4, RowNumber);
        //        }
        //    }
        //    else
        //    {
        //        //StackLayout lineSeparator = new StackLayout()
        //        //{
        //        //    BackgroundColor = CommonEL.LightHorizontalSeperatorColor,
        //        //    HeightRequest = 1
        //        //};

        //        //queueGrid.Children.Add(new StackLayout() { BackgroundColor = CommonEL.WhiteHorizontalSeperatorColor, HeightRequest = 1 }, 0, 1);
        //        //queueGrid.Children.Add(lineSeparator, 0, 2);
        //        //Grid.SetColumnSpan(lineSeparator, 5);

        //        Label lblText = new Label()
        //        {
        //            Text = "No queue.",
        //            TextColor = CommonEL.LightBlackTextColor,
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center
        //        };

        //        StackLayout stck = new StackLayout()
        //        {
        //            BackgroundColor = CommonEL.ListDataCellBackgroundColor,
        //            VerticalOptions = LayoutOptions.FillAndExpand,
        //            HorizontalOptions = LayoutOptions.FillAndExpand,
        //            Margin = new Thickness(0, 0.01, 0, 0),
        //            Padding = new Thickness(0, 5, 0, 5)
        //        };

        //        stck.Children.Add(lblText);

        //        // queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.05, BackgroundColor = CommonEL.LightBackgroundColor, Margin = new Thickness(0, 0, 0, 0) }, 0, 2);
        //        queueGrid.Children.Add(stck, 0, 1);
        //        Grid.SetColumnSpan(stck, 5);
        //    }
        //}

        #endregion

        public async Task SetQueueList(int PageNo)
        {
            using (QueueListService obj = new QueueListService())
            {
                List<QueueListApiModel> list = await obj.GetQueueList(viewModel.SalonId, PageNo);

                bool InQueueExist = false;

                //queueList = new List<QueueEL>();

                List<QueueEL> tempqueueList = new List<QueueEL>();

                if (list != null && list.Count() > 0)
                {
                    if (list.Where(t => t.Username == Convert.ToString(viewModel.CurrentUserId)).Count() > 0)
                    {
                        InQueueExist = true;
                    }

                    string QgCode = string.Empty;

                    string CurrentUserId1 = Convert.ToString(viewModel.CurrentUserId) + "-1";
                    string CurrentUserId2 = Convert.ToString(viewModel.CurrentUserId) + "-2";
                    string CurrentUserId3 = Convert.ToString(viewModel.CurrentUserId) + "-3";
                    string CurrentUserId4 = Convert.ToString(viewModel.CurrentUserId) + "-4";
                    string CurrentUserId5 = Convert.ToString(viewModel.CurrentUserId) + "-5";
                    string CurrentUserId6 = Convert.ToString(viewModel.CurrentUserId) + "-6";
                    string CurrentUserId7 = Convert.ToString(viewModel.CurrentUserId) + "-7";
                    string CurrentUserId8 = Convert.ToString(viewModel.CurrentUserId) + "-8";
                    string CurrentUserId9 = Convert.ToString(viewModel.CurrentUserId) + "-9";
                    string CurrentUserId10 = Convert.ToString(viewModel.CurrentUserId) + "-10";

                    if (list.Where(t => (t.Username == Convert.ToString(viewModel.CurrentUserId)) || (t.Username == CurrentUserId1) || (t.Username == CurrentUserId2) || (t.Username == CurrentUserId3) || (t.Username == CurrentUserId4) || (t.Username == CurrentUserId5) || (t.Username == CurrentUserId6) || (t.Username == CurrentUserId7) || (t.Username == CurrentUserId8) || (t.Username == CurrentUserId9) || (t.Username == CurrentUserId10)).Count() > 0)
                    {
                        QgCode = list.Where(t => (t.Username == Convert.ToString(viewModel.CurrentUserId)) || (t.Username == CurrentUserId1) || (t.Username == CurrentUserId2) || (t.Username == CurrentUserId3) || (t.Username == CurrentUserId4) || (t.Username == CurrentUserId5) || (t.Username == CurrentUserId6) || (t.Username == CurrentUserId7) || (t.Username == CurrentUserId8) || (t.Username == CurrentUserId9) || (t.Username == CurrentUserId10)).FirstOrDefault().QGCode;
                    }

                    if (!string.IsNullOrWhiteSpace(QgCode))
                    {
                        if (QgCode.ToLower() == "n/a")
                        {
                            QgCode = string.Empty;
                        }
                    }

                    if (PageNo == 1)
                    {
                        queueList = new List<QueueEL>();
                        tempQlistNew = new List<QueueEL>();
                    }

                    foreach (QueueListApiModel item in list)
                    {
                        QueueEL temp = new QueueEL()
                        {
                            BarberName = item.BarberName,
                            Position = item.QPosition
                        };

                        if (string.IsNullOrWhiteSpace(QgCode))
                        {
                            if (Convert.ToInt32(Application.Current.Properties["UserLevel"]) == 0)
                                temp.Name = "Client";
                            else
                                temp.Name = item.FirstLastName;

                            if (item.Username == Convert.ToString(viewModel.CurrentUserId))
                            {
                                temp.IsCurrent = true;
                                temp.Name = item.FirstLastName;
                            }
                            else
                            {
                                temp.IsCurrent = false;
                                //temp.Name = "Client";
                            }

                        }
                        else
                        {
                            if (item.QGCode == QgCode)
                            {
                                temp.IsCurrent = true;
                                temp.Name = item.FirstLastName;
                            }
                            else
                            {
                                temp.IsCurrent = false;

                                if (Convert.ToInt32(Application.Current.Properties["UserLevel"]) == 0)
                                    temp.Name = "Client";
                                else
                                    temp.Name = item.FirstLastName;
                                //     temp.Name = "Client";
                                //     temp.Name = item.FirstLastName;
                            }
                        }

                        tempqueueList.Add(temp);
                        tempQlistNew.Add(temp);
                    }


                    //if (tempqueueList != null && tempqueueList.Count() > 0)
                    //{
                    //    if (tempqueueList.Where(t => t.IsCurrent == true).Count() == 1)//In case of single joined record show only the records with that barber only
                    //    {
                    //        queueList = tempqueueList.Where(t => t.BarberName == (tempqueueList.Where(x => x.IsCurrent == true).FirstOrDefault().BarberName)).OrderBy(t => t.Position).ToList();
                    //    }
                    //    else
                    //    {
                    //        queueList = tempqueueList.OrderBy(t => t.Position).ToList();
                    //    }
                    //}

                    if (tempqueueList == null || tempqueueList.Count() == 0)
                    {
                        if (tempQlistNew.Where(t => t.IsCurrent == true).Count() == 1)//In case of single joined record show only the records with that barber only
                        {
                            queueList = tempQlistNew.Where(t => t.BarberName == (tempQlistNew.Where(x => x.IsCurrent == true).FirstOrDefault().BarberName)).OrderBy(t => t.Position).ToList();
                        }
                        else
                        {
                            queueList = tempQlistNew.OrderBy(t => t.Position).ToList();
                        }
                        IsRecordExist = false;
                    }
                    else
                    {
                        IsRecordExist = true;
                    }
                }

                else
                {
                    if (tempQlistNew != null && tempQlistNew.Count() != 0) // Added this validation on 29/10/2019 for loading always on in q list
                    {
                        if (tempQlistNew.Where(t => t.IsCurrent == true).Count() == 1)//In case of single joined record show only the records with that barber only
                        {
                            queueList = tempQlistNew.Where(t => t.BarberName == (tempQlistNew.Where(x => x.IsCurrent == true).FirstOrDefault().BarberName)).OrderBy(t => t.Position).ToList();
                        }
                        else
                        {
                            queueList = tempQlistNew.OrderBy(t => t.Position).ToList();
                        }
                    }
                    IsRecordExist = false;
                }
            }
        }

        public void AddLoader()
        {
            Grid queueGrid = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.01, GridUnitType.Star) });
            queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.01, GridUnitType.Star) });
            queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            queueGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30, GridUnitType.Auto) });

            StackLayout stckLoader = new StackLayout()
            {
                BackgroundColor = CommonEL.ListDataCellBackgroundColor,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 0.01, 0, 0),
                Padding = new Thickness(0, 5, 0, 5),
            };

            stckLoader.Children.Add(new Label()
            {
                Text = "Loading...",
                //TextColor = CommonEL.LightBlackTextColor,
                TextColor = CommonEL.BlackTextColor,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            });

            queueGrid.Children.Add(stckLoader, 0, 1);
            Grid.SetColumnSpan(stckLoader, 5);

            mainLayout.Children.Add(queueGrid);
        }

        public async Task<bool> GetGridQueueListData(int PageNo)
        {


            IsRecordExist = false;

            await SetQueueList(PageNo);

            //if (queueList != null && queueList.Count() > 0)
            //{
            //    IsRecordExist = true;
            //}

            return IsRecordExist;

            //    mainLayout.Children.RemoveAt(mainLayout.Children.Count() - 1);

            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 2 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 3 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 4 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 5 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 6 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 7 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 8 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 9 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 10 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 11 });

            //  if (queueList != null && queueList.Count() > 0)
            //  {
            //      IsRecordExist = true;
            //      int RecordCount = queueList.Count();


            //      Grid queueGrid = new Grid
            //      {
            //          RowSpacing = 10,
            //          ColumnSpacing = 0,
            //          VerticalOptions = LayoutOptions.Center,
            //          HorizontalOptions = LayoutOptions.FillAndExpand
            //      };

            //      queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            // //     queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.01, GridUnitType.Star) });
            //      queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.2, GridUnitType.Star) });
            ////      queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.01, GridUnitType.Star) });
            //      queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            //      for (int i = 0; i < RecordCount; i++)
            //      {
            //          queueGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
            //          queueGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
            //      }

            //      int RowNumber = 0;
            //      for (int i = 0; i < queueList.Count(); i++)
            //      {
            //          bool IsLastRecord = (i == (queueList.Count() - 1)) ? true : false;
            //          RowNumber++;


            //          if(i != 0 || PageNo != 1)
            //          {
            //              StackLayout lineSeparator = new StackLayout()
            //              {
            //                  BackgroundColor = Color.Gray,
            //                  HorizontalOptions = LayoutOptions.FillAndExpand,
            //                  VerticalOptions = LayoutOptions.FillAndExpand,
            //                  HeightRequest = 1
            //              };

            //              //queueGrid.Children.Add(new StackLayout() { BackgroundColor = CommonEL.WhiteHorizontalSeperatorColor, HeightRequest = 1 }, 0, RowNumber);
            //              queueGrid.Children.Add(lineSeparator, 0, RowNumber);
            //              Grid.SetColumnSpan(lineSeparator, 3);
            //          }

            //          RowNumber++;

            //          Label col0Text = new Label()
            //          {
            //              Text = Convert.ToString(queueList[i].Position),
            //              //TextColor = queueList[i].IsCurrent ? CommonEL.RedTextColor : CommonEL.LightBlackTextColor,
            //              TextColor = queueList[i].IsCurrent ? CommonEL.WhiteTextColor : CommonEL.DarkGrayColor,
            //              HorizontalTextAlignment = TextAlignment.Center,
            //              VerticalTextAlignment = TextAlignment.Center,
            //              FontSize = 14,
            //              //FontAttributes = FontAttributes.Bold,
            //              FontAttributes = queueList[i].IsCurrent ? FontAttributes.Bold : FontAttributes.None,
            //              //FontFamily = "Helvetica-Bold"
            //              //FontFamily = queueList[i].IsCurrent ? "Helvetica-Bold" : "Helvetica-Bold"
            //          };

            //          //switch (Device.RuntimePlatform)
            //          //{
            //          //    case Device.iOS:
            //          //        col0Text.FontFamily = queueList[i].IsCurrent ? "Helvetica-Bold" : "Helvetica-Light";
            //          //        break;
            //          //    case Device.Android:
            //          //        col0Text.FontFamily = "Roboto-Light";
            //          //        break;
            //          //    default:
            //          //        col0Text.FontFamily = "Roboto-Light";
            //          //        break;

            //          //}

            //          StackLayout col0Layout = new StackLayout()
            //          {
            //              HeightRequest = 30,
            //              BackgroundColor = Color.Transparent,
            //       //       BackgroundColor = CommonEL.ListDataCellBackgroundColor,
            //              VerticalOptions = LayoutOptions.FillAndExpand,
            //              HorizontalOptions = LayoutOptions.FillAndExpand,
            //              //Margin = IsLastRecord ? new Thickness(0, 0.01, 0, 0) : new Thickness(0, 0.01, 0, 2),
            //              //Padding = new Thickness(0, 5, 0, 5)
            //          };

            //          if (i == (queueList.Count() - 1))
            //          {
            //              col0Layout.Margin = new Thickness(0, 0, 0, 2);
            //          }

            //          col0Layout.Children.Add(col0Text);

            //          Label col1Text = new Label()
            //          {
            //              Text = Convert.ToString(queueList[i].Name),
            //              //TextColor = queueList[i].IsCurrent ? CommonEL.RedTextColor : CommonEL.LightBlackTextColor,
            //              TextColor = queueList[i].IsCurrent ? CommonEL.WhiteTextColor : CommonEL.DarkGrayColor,
            //              HorizontalTextAlignment = TextAlignment.Center,
            //              VerticalTextAlignment = TextAlignment.Center,
            //              FontSize = 14,
            //              // FontAttributes = FontAttributes.Bold,
            //              FontAttributes = queueList[i].IsCurrent ? FontAttributes.Bold : FontAttributes.None,
            //              //FontFamily = "Helvetica-Bold"
            //              //FontFamily = queueList[i].IsCurrent ? "Helvetica-Bold" : "Helvetica-Bold"

            //          };

            //          //switch (Device.RuntimePlatform)
            //          //{
            //          //    case Device.iOS:
            //          //        col1Text.FontFamily = queueList[i].IsCurrent ? "Helvetica-Bold" : "Helvetica-Light";
            //          //        break;
            //          //    case Device.Android:
            //          //        col1Text.FontFamily = "Roboto-Light";
            //          //        break;
            //          //    default:
            //          //        col1Text.FontFamily = "Roboto-Light";
            //          //        break;

            //          //}

            //          StackLayout col1Layout = new StackLayout()
            //          {
            //              HeightRequest = 30,
            //              BackgroundColor = Color.Transparent,
            //   //           BackgroundColor = CommonEL.ListDataCellBackgroundColor,
            //              VerticalOptions = LayoutOptions.FillAndExpand,
            //              HorizontalOptions = LayoutOptions.FillAndExpand,
            //              //Margin = IsLastRecord ? new Thickness(0, 0.01, 0, 0) : new Thickness(0, 0.01, 0, 2),
            //              //Padding = new Thickness(0, 5, 0, 5)
            //          };

            //          if (i == (queueList.Count() - 1))
            //          {
            //              col1Layout.Margin = new Thickness(0, 0, 0, 2);
            //          }

            //          col1Layout.Children.Add(col1Text);

            //          Label col2Text = new Label()
            //          {
            //              Text = Convert.ToString(queueList[i].BarberName),
            //              //TextColor = queueList[i].IsCurrent ? CommonEL.RedTextColor : CommonEL.LightBlackTextColor,
            //              TextColor = queueList[i].IsCurrent ? CommonEL.WhiteTextColor : CommonEL.DarkGrayColor,
            //              HorizontalTextAlignment = TextAlignment.Center,
            //              VerticalTextAlignment = TextAlignment.Center,
            //              FontSize = 14,
            //              //FontAttributes = FontAttributes.Bold,
            //              FontAttributes = queueList[i].IsCurrent ? FontAttributes.Bold : FontAttributes.None,
            //              //FontFamily = "Helvetica-Bold"
            //              // FontFamily = queueList[i].IsCurrent ? "Helvetica-Bold" : "Helvetica-Bold"
            //          };

            //          //switch (Device.RuntimePlatform)
            //          //{
            //          //    case Device.iOS:
            //          //        col2Text.FontFamily = queueList[i].IsCurrent ? "Helvetica-Bold" : "Helvetica-Light";
            //          //        break;
            //          //    case Device.Android:
            //          //        col2Text.FontFamily = "Roboto-Light";
            //          //        break;
            //          //    default:
            //          //        col2Text.FontFamily = "Roboto-Light";
            //          //        break;

            //          //}

            //          StackLayout col2Layout = new StackLayout()
            //          {
            //              HeightRequest = 30,
            //              BackgroundColor = Color.Transparent,
            //      //      BackgroundColor = CommonEL.ListDataCellBackgroundColor,
            //              VerticalOptions = LayoutOptions.FillAndExpand,
            //              HorizontalOptions = LayoutOptions.FillAndExpand,
            //              //Margin = IsLastRecord ? new Thickness(0, 0.01, 0, 0) : new Thickness(0, 0.01, 0, 2),
            //              //Padding = new Thickness(0, 5, 0, 5)
            //          };

            //          if (i == (queueList.Count() - 1))
            //          {
            //              col2Layout.Margin = new Thickness(0, 0, 0, 2);
            //          }

            //          col2Layout.Children.Add(col2Text);

            //          //queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.05, BackgroundColor = queueList[i].IsCurrent ? CommonEL.RedVerticalSeperatorColor : CommonEL.LightBackgroundColor, Margin = (queueList[i].IsCurrent || IsLastRecord) ? new Thickness(0, 0, 0, 0) : new Thickness(0, 2, 0, 2) }, 0, RowNumber);
            //          queueGrid.Children.Add(col0Layout, 0, RowNumber);
            //   //       queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = CommonEL.WhiteHorizontalSeperatorColor, Margin = IsLastRecord ? new Thickness(0, 0.01, 0, 0) : new Thickness(0, 0.01, 0, 2) }, 1, RowNumber);
            //          queueGrid.Children.Add(col1Layout, 1, RowNumber);
            //  //        queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = CommonEL.WhiteHorizontalSeperatorColor, Margin = IsLastRecord ? new Thickness(0, 0.01, 0, 0) : new Thickness(0, 0.01, 0, 2) }, 3, RowNumber);
            //          queueGrid.Children.Add(col2Layout, 2, RowNumber);
            //      }

            //      mainLayout.Children.Add(queueGrid);
            //  }

            //  else if (PageNo == 1)
            //  {
            //      Grid queueGrid = new Grid
            //      {
            //          RowSpacing = 0,
            //          ColumnSpacing = 0,
            //          VerticalOptions = LayoutOptions.FillAndExpand,
            //          HorizontalOptions = LayoutOptions.FillAndExpand
            //      };

            //      queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //      queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.01, GridUnitType.Star) });
            //      queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //      queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.01, GridUnitType.Star) });
            //      queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            //      queueGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30, GridUnitType.Auto) });

            //      Label lblText = new Label()
            //      {
            //          Text = "No queue.",
            //          TextColor = CommonEL.DarkGrayColor,
            //          HorizontalTextAlignment = TextAlignment.Center,
            //          VerticalTextAlignment = TextAlignment.Center
            //      };

            //      StackLayout stck = new StackLayout()
            //      {
            //          BackgroundColor = Color.Transparent,
            //    //      BackgroundColor = CommonEL.ListDataCellBackgroundColor,
            //          VerticalOptions = LayoutOptions.FillAndExpand,
            //          HorizontalOptions = LayoutOptions.FillAndExpand,
            //          Margin = new Thickness(0, 0.01, 0, 0),
            //          Padding = new Thickness(0, 5, 0, 5)
            //      };

            //      stck.Children.Add(lblText);

            //      // queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.05, BackgroundColor = CommonEL.LightBackgroundColor, Margin = new Thickness(0, 0, 0, 0) }, 0, 2);
            //      queueGrid.Children.Add(stck, 0, 1);
            //      Grid.SetColumnSpan(stck, 5);

            //      mainLayout.Children.Add(queueGrid);
            //  }

            //if (queueList != null && queueList.Count() > 0)
            //{
            //    IsRecordExist = true;
            //}

            //return IsRecordExist;
        }


        public async Task SetListViewNew(int PageNo)
        {
            mainLayout.Children.RemoveAt(mainLayout.Children.Count() - 1);

            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 2 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 3 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 4 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 5 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 6 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 7 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 8 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 9 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 10 });
            //queueList.Add(new QueueEL() { BarberName = "abc", IsCurrent = true, Name = "he", Position = 11 });

            //   if (queueList != null && queueList.Count() > 0)
            if (queueList?.Count() > 0)
            {
                //IsRecordExist = true;
                int RecordCount = queueList.Count();


                Grid queueGrid = new Grid
                {
                    RowSpacing = 10,
                    ColumnSpacing = 0,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                //     queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.01, GridUnitType.Star) });
                queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.2, GridUnitType.Star) });
                //      queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.01, GridUnitType.Star) });
                queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                for (int i = 0; i < RecordCount; i++)
                {
                    queueGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
                    queueGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
                }

                int RowNumber = 0;
                for (int i = 0; i < queueList.Count(); i++)
                {
                    bool IsLastRecord = (i == (queueList.Count() - 1)) ? true : false;
                    RowNumber++;


                    if (i != 0)
                    {
                        StackLayout lineSeparator = new StackLayout()
                        {
                            BackgroundColor = Color.Gray,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HeightRequest = 1
                        };

                        //queueGrid.Children.Add(new StackLayout() { BackgroundColor = CommonEL.WhiteHorizontalSeperatorColor, HeightRequest = 1 }, 0, RowNumber);
                        queueGrid.Children.Add(lineSeparator, 0, RowNumber);
                        Grid.SetColumnSpan(lineSeparator, 3);
                    }

                    RowNumber++;

                    Label col0Text = new Label()
                    {
                        Text = Convert.ToString(queueList[i].Position),
                        //TextColor = queueList[i].IsCurrent ? CommonEL.RedTextColor : CommonEL.LightBlackTextColor,
                        TextColor = queueList[i].IsCurrent ? CommonEL.WhiteTextColor : CommonEL.DarkGrayColor,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        FontSize = 14,
                        //FontAttributes = FontAttributes.Bold,
                        FontAttributes = queueList[i].IsCurrent ? FontAttributes.Bold : FontAttributes.None,
                        //FontFamily = "Helvetica-Bold"
                        //FontFamily = queueList[i].IsCurrent ? "Helvetica-Bold" : "Helvetica-Bold"
                    };

                    //switch (Device.RuntimePlatform)
                    //{
                    //    case Device.iOS:
                    //        col0Text.FontFamily = queueList[i].IsCurrent ? "Helvetica-Bold" : "Helvetica-Light";
                    //        break;
                    //    case Device.Android:
                    //        col0Text.FontFamily = "Roboto-Light";
                    //        break;
                    //    default:
                    //        col0Text.FontFamily = "Roboto-Light";
                    //        break;

                    //}

                    StackLayout col0Layout = new StackLayout()
                    {
                        HeightRequest = 30,
                        BackgroundColor = Color.Transparent,
                        //       BackgroundColor = CommonEL.ListDataCellBackgroundColor,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        //Margin = IsLastRecord ? new Thickness(0, 0.01, 0, 0) : new Thickness(0, 0.01, 0, 2),
                        //Padding = new Thickness(0, 5, 0, 5)
                    };

                    if (i == (queueList.Count() - 1))
                    {
                        col0Layout.Margin = new Thickness(0, 0, 0, 2);
                    }

                    col0Layout.Children.Add(col0Text);

                    Label col1Text = new Label()
                    {
                        Text = Convert.ToString(queueList[i].Name),
                        //TextColor = queueList[i].IsCurrent ? CommonEL.RedTextColor : CommonEL.LightBlackTextColor,
                        TextColor = queueList[i].IsCurrent ? CommonEL.WhiteTextColor : CommonEL.DarkGrayColor,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        FontSize = 14,
                        // FontAttributes = FontAttributes.Bold,
                        FontAttributes = queueList[i].IsCurrent ? FontAttributes.Bold : FontAttributes.None,
                        //FontFamily = "Helvetica-Bold"
                        //FontFamily = queueList[i].IsCurrent ? "Helvetica-Bold" : "Helvetica-Bold"

                    };

                    //switch (Device.RuntimePlatform)
                    //{
                    //    case Device.iOS:
                    //        col1Text.FontFamily = queueList[i].IsCurrent ? "Helvetica-Bold" : "Helvetica-Light";
                    //        break;
                    //    case Device.Android:
                    //        col1Text.FontFamily = "Roboto-Light";
                    //        break;
                    //    default:
                    //        col1Text.FontFamily = "Roboto-Light";
                    //        break;

                    //}

                    StackLayout col1Layout = new StackLayout()
                    {
                        HeightRequest = 30,
                        BackgroundColor = Color.Transparent,
                        //           BackgroundColor = CommonEL.ListDataCellBackgroundColor,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        //Margin = IsLastRecord ? new Thickness(0, 0.01, 0, 0) : new Thickness(0, 0.01, 0, 2),
                        //Padding = new Thickness(0, 5, 0, 5)
                    };

                    if (i == (queueList.Count() - 1))
                    {
                        col1Layout.Margin = new Thickness(0, 0, 0, 2);
                    }

                    col1Layout.Children.Add(col1Text);

                    Label col2Text = new Label()
                    {
                        Text = Convert.ToString(queueList[i].BarberName),
                        //TextColor = queueList[i].IsCurrent ? CommonEL.RedTextColor : CommonEL.LightBlackTextColor,
                        TextColor = queueList[i].IsCurrent ? CommonEL.WhiteTextColor : CommonEL.DarkGrayColor,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        FontSize = 14,
                        //FontAttributes = FontAttributes.Bold,
                        FontAttributes = queueList[i].IsCurrent ? FontAttributes.Bold : FontAttributes.None,
                        //FontFamily = "Helvetica-Bold"
                        // FontFamily = queueList[i].IsCurrent ? "Helvetica-Bold" : "Helvetica-Bold"
                    };

                    //switch (Device.RuntimePlatform)
                    //{
                    //    case Device.iOS:
                    //        col2Text.FontFamily = queueList[i].IsCurrent ? "Helvetica-Bold" : "Helvetica-Light";
                    //        break;
                    //    case Device.Android:
                    //        col2Text.FontFamily = "Roboto-Light";
                    //        break;
                    //    default:
                    //        col2Text.FontFamily = "Roboto-Light";
                    //        break;

                    //}

                    StackLayout col2Layout = new StackLayout()
                    {
                        HeightRequest = 30,
                        BackgroundColor = Color.Transparent,
                        //      BackgroundColor = CommonEL.ListDataCellBackgroundColor,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        //Margin = IsLastRecord ? new Thickness(0, 0.01, 0, 0) : new Thickness(0, 0.01, 0, 2),
                        //Padding = new Thickness(0, 5, 0, 5)
                    };

                    if (i == (queueList.Count() - 1))
                    {
                        col2Layout.Margin = new Thickness(0, 0, 0, 2);
                    }

                    col2Layout.Children.Add(col2Text);

                    //queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.05, BackgroundColor = queueList[i].IsCurrent ? CommonEL.RedVerticalSeperatorColor : CommonEL.LightBackgroundColor, Margin = (queueList[i].IsCurrent || IsLastRecord) ? new Thickness(0, 0, 0, 0) : new Thickness(0, 2, 0, 2) }, 0, RowNumber);
                    queueGrid.Children.Add(col0Layout, 0, RowNumber);
                    //       queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = CommonEL.WhiteHorizontalSeperatorColor, Margin = IsLastRecord ? new Thickness(0, 0.01, 0, 0) : new Thickness(0, 0.01, 0, 2) }, 1, RowNumber);
                    queueGrid.Children.Add(col1Layout, 1, RowNumber);
                    //        queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.01, BackgroundColor = CommonEL.WhiteHorizontalSeperatorColor, Margin = IsLastRecord ? new Thickness(0, 0.01, 0, 0) : new Thickness(0, 0.01, 0, 2) }, 3, RowNumber);
                    queueGrid.Children.Add(col2Layout, 2, RowNumber);
                }

                mainLayout.Children.Add(queueGrid);
            }

            else if (PageNo == 2) /// Changed PageNo == 1 to show "No queue". As pageNo that is passed is always 2 in this case
            {
                Grid queueGrid = new Grid
                {
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.01, GridUnitType.Star) });
                queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.01, GridUnitType.Star) });
                queueGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                queueGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30, GridUnitType.Auto) });

                Label lblText = new Label()
                {
                    Text = "No queue.",
                    TextColor = CommonEL.DarkGrayColor,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                };

                StackLayout stck = new StackLayout()
                {
                    BackgroundColor = Color.Transparent,
                    //      BackgroundColor = CommonEL.ListDataCellBackgroundColor,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Margin = new Thickness(0, 0.01, 0, 0),
                    Padding = new Thickness(0, 5, 0, 5)
                };

                stck.Children.Add(lblText);

                // queueGrid.Children.Add(new StackLayout() { WidthRequest = 0.05, BackgroundColor = CommonEL.LightBackgroundColor, Margin = new Thickness(0, 0, 0, 0) }, 0, 2);
                queueGrid.Children.Add(stck, 0, 1);
                Grid.SetColumnSpan(stck, 5);

                mainLayout.Children.Add(queueGrid);
            }

        }

        #endregion Public methods
    }
}