using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Toaster.Producer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		ToasterServer toasterServer;
		Windows.UI.Core.CoreDispatcher dispatcher;

        public MainPage()
        {
			dispatcher = Window.Current.CoreWindow.Dispatcher;
			InitializeToaster();
            this.InitializeComponent();			
			ToastingIndicator.IsEnabled = false;
			SetVersionText();
        }

		private void InitializeToaster()
		{
			toasterServer = new ToasterServer();
			toasterServer.Service.StartedToasting += Service_StartedToasting;
			toasterServer.Service.StoppedToasting += Service_StoppedToasting;
			toasterServer.Service.DarknessChanged += Service_DarknessChanged;			
		}

		private async void SetVersionText()
		{
			await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				ToasterVersionText.Text = toasterServer.Service.ToasterVersion.ToString();
			});
		}

		private void Service_DarknessChanged(object sender, EventArgs e)
		{
			UpdateSliderValue(toasterServer.Service.DarknessLevel);
		}

		private async void UpdateSliderValue(byte darknessLevel)
		{
			await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				DarknessLevelSlider.Value = darknessLevel;
			});
		}

		private async void Service_StoppedToasting(object sender, EventArgs e)
		{
			await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,() =>
		    {
				ToastingIndicator.IsEnabled = true;
				ToastingIndicator.IsOn = false;
				ToastingIndicator.IsEnabled = false;
			});
			
		}

		private async void Service_StartedToasting(object sender, EventArgs e)
		{
			await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				ToastingIndicator.IsEnabled = true;
				ToastingIndicator.IsOn = true;
				ToastingIndicator.IsEnabled = false;
			});
		}
		
		private async void DarknessLevelSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
		{
			await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				toasterServer.DarknessChanged(Convert.ToByte(DarknessLevelSlider.Value));
			});
		}
	}
}
