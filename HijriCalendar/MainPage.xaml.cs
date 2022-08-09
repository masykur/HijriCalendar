using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace HijriCalendar
{
    public partial class MainPage : PhoneApplicationPage
    {
        DateTime activeDate;
        List<DateTime> dates = new List<DateTime>();
        private readonly int zero = 0x0660;
        private string[] monthNames = new[] { "Muharram", "Safar", "Rabi I", "Rabi II", "Jumada I", "Jumada II", "Rajab", "Sha'ban", "Ramadan", "Shawwal", "Dhu al-Qa'da", "Dhu al-Hijja", "" };

        System.Globalization.HijriCalendar hijriCalendar = new System.Globalization.HijriCalendar();
        //System.Globalization.CultureInfo arabicCulture = new System.Globalization.CultureInfo("ar");
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            //DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }
        
 
        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //if (!App.ViewModel.IsDataLoaded)
            //{
            //    App.ViewModel.LoadData();
            //}
            var today = DateTime.Now;
            var h = Application.Current.Host.Content.ActualHeight;

            activeDate = today;
            //arabicCulture.DateTimeFormat = new System.Globalization.DateTimeFormatInfo
            //{
            //    AbbreviatedDayNames = new[] { "Aha", "Sen", "Sel", "Rab", "Kam", "Jum", "Sab" },
            //    DayNames = new[] { "Ahad", "Senin", "Selasa", "Rabu", "Kamis", "Jum'at", "Sabtu" },
            //    MonthNames = new[] { "Muharram", "Safar", "Rabi I", "Rabi II", "Jumada I", "Jumada II", "Rajab", "Sha'ban", "Ramadan", "Shawwal", "Dhu al-Qa'da", "Dhu al-Hijja", "" }
            //};
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            if (currentCulture.Name == "id-ID")
            {
                monthNames = new[] { "Muharam", "Safar", "Rabiul Awal", "Rabiul Akhir", "Jumadil Awal", "Jumadil Akhir", "Rajab", "Sha'ban", "Ramadhan", "Syawal", "Dhul Qa'dah", "Dhul Hijjah", "" };
            }
            else
            {
                monthNames = new[] { "Muharram", "Safar", "Rabi I", "Rabi II", "Jumada I", "Jumada II", "Rajab", "Sha'ban", "Ramadan", "Shawwal", "Dhu al-Qa'da", "Dhu al-Hijja", "" };
            }
            dates.AddRange(new[] 
            { 
                hijriCalendar.AddMonths(today, 0),
                hijriCalendar.AddMonths(today, 1),
                hijriCalendar.AddMonths(today, 2),
                hijriCalendar.AddMonths(today, -1)
            });
            for (var i = 0; i < PivotCalendar.Items.Count; i++ )
            {
                var item = PivotCalendar.Items[i] as PivotItem;
                item.Header = monthNames[hijriCalendar.GetMonth(dates[i]) - 1];
                if (i == 0)
                {
                    (item.Content as Calendar).RenderDate(dates[i]);
                }
            }
            currentIndex = 0;
        }
        int currentIndex;
        private void PivotCalendar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PivotCalendar.SelectedIndex != currentIndex)
            {
                // backward
                var currentDate = dates[PivotCalendar.SelectedIndex];
                if (PivotCalendar.SelectedIndex == (currentIndex + 3) % 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        dates[(i + PivotCalendar.SelectedIndex) % 4] = hijriCalendar.AddMonths(currentDate, i);
                    }
                    //dates[(PivotCalendar.SelectedIndex + 3) % 4] = hijriCalendar.AddMonths(currentDate, -1);
                }
                else // forward
                {
                    for (int i = 0; i < 4; i++)
                    {
                        dates[(i + PivotCalendar.SelectedIndex) % 4] = hijriCalendar.AddMonths(currentDate, i);
                    }
                }
                dates[(PivotCalendar.SelectedIndex + 3) % 4] = hijriCalendar.AddMonths(currentDate, -1);
                for (var i = 0; i < PivotCalendar.Items.Count; i++)
                {
                    var item = PivotCalendar.Items[i] as PivotItem;
                    item.Header = monthNames[hijriCalendar.GetMonth(dates[i]) - 1];
                    if (i == PivotCalendar.SelectedIndex)
                    {
                        (item.Content as Calendar).RenderDate(dates[i]);
                    }
                }
            }
            currentIndex = PivotCalendar.SelectedIndex;
        }

        //private string ConvertToArabicNumber(int number)
        //{
        //    var arabicNumbers = System.Text.UTF8Encoding.UTF8.GetBytes(number.ToString()).Select(c => Convert.ToChar(zero + c - 0x30));
        //    return string.Join("", arabicNumbers);
        //}

        private void PivotItem_GotFocus(object sender, RoutedEventArgs e)
        {
            currentIndex = 0;
        }

        private void PinToStartMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StandardTileData LiveTile = new StandardTileData
            {
                BackgroundImage = new Uri("/Images/calendar-173-tile.png", UriKind.Relative),
                Title = "Hijri Calendar",
                //BackContent = string.Format(arabicCulture, "{0} AH", ConvertToArabicNumber(hijriCalendar.GetYear(DateTime.Today))),
                //BackTitle = string.Format(arabicCulture, "{0} {1}",
                //                ConvertToArabicNumber(hijriCalendar.GetDayOfMonth(DateTime.Today)), 
                //                arabicCulture.DateTimeFormat.MonthNames[hijriCalendar.GetMonth(DateTime.Today) - 1]),
                //Count = hijriCalendar.GetYear(DateTime.Today),
                BackBackgroundImage = new Uri("/Images/calendar-173-tile.png", UriKind.Relative)
            };
            ShellTile Tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("DefaultTitle=" + LiveTile.Title));            
            if (Tile == null) 
            { 
                try 
                {
                    ShellTile.Create(new Uri("/MainPage.xaml?DefaultTitle=" + LiveTile.Title, UriKind.Relative), LiveTile);
                } 
                catch (Exception) 
                { 
                    MessageBox.Show("I prefer not to be pinned"); 
                } 
            } 
            else 
            {
                MessageBox.Show("The tile is already pinned"); 
            }
        }

        private void AboutMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show(AppResources.AboutMessage, AppResources.AboutCaption, MessageBoxButton.OK);
        }
    }
}