using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Microsoft.UI.Xaml.Media;
using Microsoft.Web.WebView2.Core;
using Windows.UI;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Diagnostics;

namespace App1.ViewModels
{
    [Bindable]
    public class SettingsViewModel : ViewModelBase
    {
        public string Version
        {
            get => Get<string>();
            set => Set(value);
        }

        public string IpAddress
        {
            get => Get<string>();
            set => Set(value);
        }

        public string Id
        {
            get => Get<string>();
            set => Set(value);
        }

        public string Name
        {
            get => Get<string>();
            set => Set(value);
        }

        public string Site
        {
            get => Get<string>();
            set => Set(value);
        }

        public string Building
        {
            get => Get<string>();
            set => Set(value);
        }

        public string Room
        {
            get => Get<string>();
            set => Set(value);
        }

        public List<string> Users

        {
            get => Get<List<string>>();
            set => Set(value);
        }

        public List<string> Complans

        {
            get => Get<List<string>>();
            set => Set(value);
        }

        public SettingsViewModel()
        {
            IpAddress = "192.168.1.2:1111";
            Version = "1.0.1";
            Id = Guid.NewGuid().ToString();
            Name = "Main Room Console";
            Site = "Site 1";
            Building = "Building A";
            Room = "Room 789";

            Users = new List<string>()
            {
                "Exercise",
                "Role Players"
            };

            Complans = new List<string>()
            {
                "Plan 1",
                "Plan 2",
                "Comm Plan"
            };
        }
    }

    [Bindable]
    public class ViewModelDataTemplateLink
    {
        public ViewModelBase ViewModel { get; set; }
        public DataTemplate DataTemplate { get; set; }
        public Page Page { get; set; }
    }

    [Bindable]
    public class GeometryToggleViewModel : ViewModelBase
    {
        protected readonly string c_OffGeometry;
        protected readonly string c_OnGeometry;
        protected readonly string c_OffForeground;
        protected readonly string c_OnForeground;

        public bool State
        {
            get => Get<bool>();
            set
            {
                Set(value);

                if (value)
                {
                    Geometry = c_OnGeometry;
                    ForegroundColor = c_OnForeground;
                }
                else
                {
                    Geometry = c_OffGeometry;
                    ForegroundColor = c_OffForeground;
                }
            }
        }

        public object ForegroundColor
        {
            get => Get<string>();
            set => Set(value);
        }

        public string Geometry
        {
            get => Get<string>();
            set => Set(value);
        }

        public GeometryToggleViewModel(string offGeometry, string onGeometry, string offForeground = "", string onForeground = "", bool initialState = false)
        {
            c_OffGeometry = offGeometry;
            c_OnGeometry = onGeometry;
            c_OffForeground = offForeground;
            c_OnForeground = onForeground;
            State = initialState;
        }
    }

    [Bindable]
    public class GeometryBackgroundToggleViewModel : GeometryToggleViewModel
    {
        private readonly string c_OnColor;
        private readonly string c_OffColor;

        public object BackgroundColor
        {
            get => Get<object>();
            set => Set(value);
        }

        public ICommand CommandToggle { get; private set; }

        public GeometryBackgroundToggleViewModel(string offGeometry, string onGeometry, string offBackgroundColor, string onBackgroundColor, string offForeground, string onForeground, bool initialState = false) : base(offGeometry, onGeometry, offForeground, onForeground, initialState)
        {
            c_OffColor = offBackgroundColor;
            c_OnColor = onBackgroundColor;

            State = initialState;
            //Geometry = State ? c_OnGeometry : c_OffGeometry;
            BackgroundColor = State ? c_OnColor : c_OffColor;

            CommandToggle = new Command((o) =>
            {
                State = !State;
                //Geometry = State ? c_OnGeometry : c_OffGeometry;
                BackgroundColor = State ? c_OnColor : c_OffColor;
            });
        }
    }

    public class InteractiveTimer
    {
        private System.Timers.Timer m_Timer;
        private Action m_TimerElapsed;

        public InteractiveTimer(int delayInMs, Action timerElapsed)
        {
            m_TimerElapsed = timerElapsed;
            m_Timer = new System.Timers.Timer();
            m_Timer.Elapsed += Timer_Elapsed;
            m_Timer.Interval = delayInMs;
            //m_Timer.Enabled = true;
        }

        public void Restart()
        {
            m_Timer.Stop();
            m_Timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Debug.WriteLine("Timer elapsed");
            m_TimerElapsed?.Invoke();
        }

        internal void Stop()
        {
            m_Timer.Stop();
        }
    }

    [Bindable]
    public class MainViewModel : ViewModelBase
    {
        private InteractiveTimer m_SliderTimer;
        private Dictionary<int, ViewModelDataTemplateLink> m_ViewsByIndex = new Dictionary<int, ViewModelDataTemplateLink>();

        public GeometryBackgroundToggleViewModel PttToggleObject
        {
            get => Get<GeometryBackgroundToggleViewModel>();
            set => Set(value);
        }

        public GeometryBackgroundToggleViewModel SpeakerToggleObject
        {
            get => Get<GeometryBackgroundToggleViewModel>();
            set => Set(value);
        }

        public int SelectedNavIndex
        {
            get => Get<int>();
            set
            {
                Set(value);

                //UpdateCurrentView();

                SelectedNavIndexChanged(value);
            }
        }

        public Visibility SettingsVisibility
        {
            get => Get<Visibility>();
            set => Set(value);
        }

        public Visibility ChannelsVisibility
        {
            get => Get<Visibility>();
            set => Set(value);
        }

        public Visibility SliderVisibility
        {
            get => Get<Visibility>();
            set => Set(value);
        }

        public ViewModelDataTemplateLink CurrentViewDataTemplateLink
        {
            get => Get<ViewModelDataTemplateLink>();
            set => Set(value);
        }

        public object CurrentView
        {
            get => Get<object>();
            set => Set(value);
        }

        public SettingsViewModel Settings
        {
            get => Get<SettingsViewModel>();
            set => Set(value);
        }

        public MainViewModel()
        {
            Settings = new SettingsViewModel();
            SliderVisibility = Visibility.Collapsed;

            //m_ViewsByIndex.Add(0, new ViewModelDataTemplateLink
            //{
            //    ViewModel = new ChannelListViewModel(),
            //    DataTemplate = (DataTemplate)App.Current.Resources[nameof(ChannelListViewModel)],
            //    Page = new ControlsPage()
            //});
            //m_ViewsByIndex.Add(1, new ViewModelDataTemplateLink
            //{
            //    ViewModel = new SettingsViewModel(),
            //    DataTemplate = (DataTemplate)App.Current.Resources[nameof(SettingsViewModel)],
            //    Page = new SettingsPage()
            //});
            //m_ViewsByIndex.Add(2, new ViewModelDataTemplateLink
            //{
            //    ViewModel = new ChannelList02ViewModel(),
            //    DataTemplate = (DataTemplate)App.Current.Resources[nameof(ChannelList02ViewModel)],
            //    Page = new CardPage02()
            //});

            //m_ViewsByIndex.Add(0, new ViewModelDataTemplateLink
            //{
            //    ViewModel = new ChannelList03ViewModel(),
            //    DataTemplate = (DataTemplate)App.Current.Resources[nameof(ChannelList03ViewModel)],
            //    Page = new ChannelsPage03()
            //});

            //m_ViewsByIndex.Add(4, new ViewModelDataTemplateLink
            //{
            //    ViewModel = new SettingsViewModel(),
            //    DataTemplate = (DataTemplate)App.Current.Resources["SettingsFixed"],
            //    Page = new SettingsPageFixed()
            //});

            SelectedNavIndex = 0;

            PttToggleObject = new GeometryBackgroundToggleViewModel(
                "M19,11C19,12.19 18.66,13.3 18.1,14.28L16.87,13.05C17.14,12.43 17.3,11.74 17.3,11H19M15,11.16L9,5.18V5A3,3 0 0,1 12,2A3,3 0 0,1 15,5V11L15,11.16M4.27,3L21,19.73L19.73,21L15.54,16.81C14.77,17.27 13.91,17.58 13,17.72V21H11V17.72C7.72,17.23 5,14.41 5,11H6.7C6.7,14 9.24,16.1 12,16.1C12.81,16.1 13.6,15.91 14.31,15.58L12.65,13.92L12,14A3,3 0 0,1 9,11V10.28L3,4.27L4.27,3Z",
                "M12,2A3,3 0 0,1 15,5V11A3,3 0 0,1 12,14A3,3 0 0,1 9,11V5A3,3 0 0,1 12,2M19,11C19,14.53 16.39,17.44 13,17.93V21H11V17.93C7.61,17.44 5,14.53 5,11H7A5,5 0 0,0 12,16A5,5 0 0,0 17,11H19Z",
                "#8A0B11",
                "#FF0000",
                "",
                "");

            SpeakerToggleObject = new GeometryBackgroundToggleViewModel(
                  "M2,5.27L3.28,4L21,21.72L19.73,23L18.27,21.54C17.93,21.83 17.5,22 17,22H7C5.89,22 5,21.1 5,20V8.27L2,5.27M12,18A3,3 0 0,1 9,15C9,14.24 9.28,13.54 9.75,13L8.33,11.6C7.5,12.5 7,13.69 7,15A5,5 0 0,0 12,20C13.31,20 14.5,19.5 15.4,18.67L14,17.25C13.45,17.72 12.76,18 12,18M17,15A5,5 0 0,0 12,10H11.82L5.12,3.3C5.41,2.54 6.14,2 7,2H17A2,2 0 0,1 19,4V17.18L17,15.17V15M12,4C10.89,4 10,4.89 10,6A2,2 0 0,0 12,8A2,2 0 0,0 14,6C14,4.89 13.1,4 12,4Z",
                  "M12,12A3,3 0 0,0 9,15A3,3 0 0,0 12,18A3,3 0 0,0 15,15A3,3 0 0,0 12,12M12,20A5,5 0 0,1 7,15A5,5 0 0,1 12,10A5,5 0 0,1 17,15A5,5 0 0,1 12,20M12,4A2,2 0 0,1 14,6A2,2 0 0,1 12,8C10.89,8 10,7.1 10,6C10,4.89 10.89,4 12,4M17,2H7C5.89,2 5,2.89 5,4V20A2,2 0 0,0 7,22H17A2,2 0 0,0 19,20V4C19,2.89 18.1,2 17,2Z",
                  "#003307",
                  "#00A116",
                  "",
                  "");
        }

        private void SelectedNavIndexChanged(int selectedNavIndex)
        {
            if (selectedNavIndex == 0)
            {
                SettingsVisibility = Visibility.Collapsed;
                ChannelsVisibility = Visibility.Visible;
            }
            else

            {
                SettingsVisibility = Visibility.Visible;
                ChannelsVisibility = Visibility.Collapsed;
            }
        }

        private void UpdateCurrentView()
        {
            if (m_ViewsByIndex.TryGetValue(SelectedNavIndex, out ViewModelDataTemplateLink current))
            {
                Serilog.Log.Debug($"Selected Nav Index: {SelectedNavIndex}, View Model: {current.ViewModel.ToString()}, DT null: {current.DataTemplate == null}, page: {current.Page.ToString()}");
                CurrentViewDataTemplateLink = current;
                CurrentView = current.Page;
            }
        }

        public void ShowSlider()
        {
            SliderVisibility = Visibility.Visible;

            if (m_SliderTimer == null)
            {
                m_SliderTimer = new InteractiveTimer(1000, () =>
                {
                    SliderVisibility = Visibility.Collapsed;
                    m_SliderTimer.Stop();
                });
            }

            //m_SliderTimer.Restart();
        }

        public void RestartSliderTimer()
        {
            m_SliderTimer?.Restart();
        }

        internal void StopSliderTimer()
        {
            m_SliderTimer?.Stop();
        }
    }
}