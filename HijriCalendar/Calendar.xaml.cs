using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using System.Globalization;

namespace HijriCalendar
{
	public partial class Calendar : UserControl
	{
        private readonly int zero = 0x0660;
        public Calendar()
		{
			// Required to initialize variables
			InitializeComponent();
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            this.Loaded += new RoutedEventHandler(Calendar_Loaded);

            var dayNames = hijriGrid.Children.Where(c => c is System.Windows.Controls.TextBlock).Select(c => c as TextBlock).ToList<TextBlock>();
            if (currentCulture.Name == "id-ID")
            {
                for (int i = 0; i < dayNames.Count(); i++)
                {
                    dayNames[i].Text = currentCulture.DateTimeFormat.DayNames[i];
                }
            }
            else
            {
                for (int i = 0; i < dayNames.Count(); i++)
                {
                    dayNames[i].Text = currentCulture.DateTimeFormat.AbbreviatedDayNames[i];
                }
            }
		}

        void Calendar_Loaded(object sender, RoutedEventArgs e)
        {
            //var today = DateTime.Now;
            //RenderDate(today);
        }
        public void RenderDate(DateTime date)
        {
            System.Globalization.HijriCalendar hijriCalendar = new System.Globalization.HijriCalendar();
            System.Globalization.UmAlQuraCalendar umAlQuraCalendar = new System.Globalization.UmAlQuraCalendar();
            
            //var arabicCulture = new System.Globalization.CultureInfo("ar");

            //arabicCulture.DateTimeFormat = new System.Globalization.DateTimeFormatInfo
            //{
            //    AbbreviatedDayNames = new[] { "Aha", "Sen", "Sel", "Rab", "Kam", "Jum", "Sab" },
            //    DayNames = new[] { "Ahad", "Senin", "Selasa", "Rabu", "Kamis", "Jum'at", "Sabtu" },
            //    MonthNames = new[] { "Muharram", "Safar", "Rabi al-Awwal", "Rabi ath-Thani", "Jumada al-Ula", "Jumada ath-Thaniya", "Rajab", "Sha'ban", "Ramadan", "Shawwal", "Dhu al-Qa'da", "Dhu al-Hijja", "" }
            //};
            var firstDay = date.AddDays(-1 * (hijriCalendar.GetDayOfMonth(date) - 1));
            int dayOfWeek = (int)hijriCalendar.GetDayOfWeek(firstDay);
            var grids = hijriGrid.Children.Where(c => c is System.Windows.Controls.Grid).ToList();
            var gregorianDates = new List<DateTime>();
            for (var i = 0; i < grids.Count; i++)
            {
                var grid = grids[i] as System.Windows.Controls.Grid;
                if (grid != null)
                {
                    var day = firstDay.AddDays((-1 * dayOfWeek) + i);
                    gregorianDates.Add(day);
                    if (day.Date == DateTime.Today)
                    {
                        //var currentAccentBrush = (Brush)Application.Current.Resources["PhoneAccentBrush"];
                        var currentAccentStyle = (Style)Application.Current.Resources["PhoneTextAccentStyle"];
                        foreach (TextBlock textBlock in grid.Children)
                        {
                            textBlock.Style = currentAccentStyle;
                        }
                        //var gradientStops = new GradientStopCollection();
                        //gradientStops.Add(new GradientStop { Color = Color.FromArgb(127, 255, 255, 255), Offset = 0 });
                        //gradientStops.Add(new GradientStop { Color = System.Windows.Media.Colors.Transparent, Offset = 1 });
                        //grid.Background = currentAccentBrush; // new LinearGradientBrush(gradientStops, 90);
                        //var border = new Border { BorderThickness = new Thickness(2), BorderBrush = new SolidColorBrush(Colors.Red) };
                        //grid.Children.Add(border);
                    }
                    else
                    {
                        var currentAccentStyle = (Style)Application.Current.Resources["PhoneTextAccentStyle"];
                        foreach (TextBlock textBlock in grid.Children)
                        {
                            textBlock.ClearValue(TextBlock.StyleProperty);
                        }
                        //var background = grid.Background;
                        //if (background != null)
                        //{
                        //    grid.ClearValue(Grid.BackgroundProperty);
                        //}
                        //var border = grid.Children.Where(c => c is Border).FirstOrDefault();
                        //if (border != null)
                        //{
                        //    grid.Children.Remove(border);
                        //}
                    }
                    var hijriDayOfMonth = grid.Children.FirstOrDefault() as System.Windows.Controls.TextBlock;
                    if (hijriDayOfMonth != null)
                    {
                        var hijriDay = hijriCalendar.GetDayOfMonth(day);

                        hijriDayOfMonth.Text = hijriDay.ToString(CultureInfo.CurrentCulture);
                        if (hijriCalendar.GetMonth(date) != hijriCalendar.GetMonth(day))
                        {
                            hijriDayOfMonth.Foreground = new SolidColorBrush(Colors.Gray);
                        }
                    }
                    var gregorianDayOfMonth = grid.Children.Skip(1).Take(1).FirstOrDefault() as System.Windows.Controls.TextBlock;
                    if (gregorianDayOfMonth != null)
                    {
                        gregorianDayOfMonth.Text = day.ToString("MMM d", CultureInfo.CurrentCulture);
                    }
                }
            }
            HijriYear.Text = string.Format("{0} AH", hijriCalendar.GetYear(date).ToString(CultureInfo.CurrentCulture));
            GregorianMonths.Text = string.Join(", ", gregorianDates.OrderBy(c => c).Select(c => c.ToString("MMMM yyyy")).Distinct());
        }
        //private string ConvertToArabicNumber(int number)
        //{
        //    var arabicNumbers = System.Text.UTF8Encoding.UTF8.GetBytes(number.ToString()).Select(c => Convert.ToChar(zero + c - 0x30));
        //    return string.Join("", arabicNumbers);
        //}
       
	}
}