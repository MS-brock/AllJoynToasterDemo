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
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Toaster.Consumer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		ToasterClient toasterClient;
		Windows.UI.Core.CoreDispatcher dispatcher;

		public MainPage()
        {
			dispatcher = Window.Current.CoreWindow.Dispatcher;

            ToasterFinder toasterFinder = new ToasterFinder();
            toasterFinder.ToasterFound += ToasterFinder_ToasterFound;
            toasterFinder.Start();

            this.InitializeComponent();            

            

			DarknessLevelSlider.IsEnabled = false;
			StartToastButton.IsEnabled = false;
			StopToastButton.IsEnabled = false;
        }

        private async void ToasterFinder_ToasterFound(object sender, ToasterFoundEventArgs args)
        {
            toasterClient = new ToasterClient(args.Consumer);

            toasterClient.Consumer.DarknessLevelChanged += Consumer_DarknessLevelChanged;
            toasterClient.Consumer.Signals.ToastBurntReceived += Signals_ToastBurntReceived;

            string versionText = toasterClient.ToasterVersion.ToString();

            await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                EnableUI(versionText);
            });

            UpdateSliderValue();
        }

		private void Signals_ToastBurntReceived(org.alljoyn.example.Toaster.ToasterSignals sender, org.alljoyn.example.Toaster.ToasterToastBurntReceivedEventArgs args)
		{
			ToastTemplateType toastTemplate = ToastTemplateType.ToastText01;
			XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

			XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
			toastTextElements[0].AppendChild(toastXml.CreateTextNode("Your toast was burnt!"));

			ToastNotification toast = new ToastNotification(toastXml);

			ToastNotificationManager.CreateToastNotifier().Show(toast);
		}

		private void Consumer_DarknessLevelChanged(org.alljoyn.example.Toaster.ToasterConsumer sender, object args)
		{
			UpdateSliderValue();
		}

		private async void UpdateSliderValue()
		{
			byte darknessLevel = toasterClient.DarknessLevel;

			await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				if (DarknessLevelSlider.Value != darknessLevel)
				{
					DarknessLevelSlider.Value = darknessLevel;
				}
			});
		}

		private void EnableUI(string versionText)
		{			
			DarknessLevelSlider.IsEnabled = true;
			StartToastButton.IsEnabled = true;
			StopToastButton.IsEnabled = true;

			ToasterVersionText.Text = versionText;
		}

		private void StopToastButton_Click(object sender, RoutedEventArgs e)
		{
            if (toasterClient != null)
            {
                toasterClient.StopToasting();
            }
		}

		private void StartToastButton_Click(object sender, RoutedEventArgs e)
		{
            if (toasterClient != null)
            {
                toasterClient.StartToasting();
            }
		}

		private void DarknessLevelSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
		{
            if (toasterClient != null)
            {
                toasterClient.DarknessLevel = Convert.ToByte(DarknessLevelSlider.Value);
            }
		}
	}
}
