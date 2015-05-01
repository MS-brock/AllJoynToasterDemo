using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.AllJoyn;
using com.microsoft.sample;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

namespace Toaster.Consumer
{
	class ToasterClient
	{
		private toasterConsumer _toasterConsumer;
		private AllJoynBusAttachment toasterBusAttachment;
		private uint darkness;

		public ToasterClient()
		{
			_toasterConsumer = null;
			toasterBusAttachment = new AllJoynBusAttachment();
			StartWatcher();
		}

		private void StartWatcher()
		{
			toasterWatcher _toasterWatcher = new toasterWatcher(toasterBusAttachment);
			_toasterWatcher.Added += toasterWatcher_Added;
			_toasterWatcher.Start();
		}

		private async void toasterWatcher_Added(toasterWatcher sender, AllJoynServiceInfo args)
		{

			toasterJoinSessionResult joinResult = await toasterConsumer.JoinSessionAsync(args, sender);

			if (joinResult.Status == AllJoynStatus.Ok)
			{
				_toasterConsumer = joinResult.Consumer;
				_toasterConsumer.Signals.ToastDoneReceived += ToastDoneReceived_Signal;
				RetrieveDarkness();
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Joining the session went wrong");
			}
		}

        public void ToastDoneReceived_Signal(toasterSignals sender, toasterToastDoneReceivedEventArgs args)
		{
			//Show UI Toast
			ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;
			XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

			//Populate UI Toast
			XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
			toastTextElements[0].AppendChild(toastXml.CreateTextNode("Toast Done"));

			//Create and Send UI Toast
			ToastNotification toast = new ToastNotification(toastXml);
			ToastNotificationManager.CreateToastNotifier().Show(toast);
		}

		public async void StartToasting()
		{
			if (_toasterConsumer != null)
			{
				await _toasterConsumer.StartToastingAsync();
			}			
		}

		public async void StopToasting()
		{
			if (_toasterConsumer != null)
			{
				await _toasterConsumer.StopToastingAsync();
			}
		}

		public async void SetDarkness(int newDarkness)
		{
			if (_toasterConsumer != null)
			{
				darkness = uint.Parse(newDarkness.ToString());
				await _toasterConsumer.SetDarknessAsync(darkness);
			}
		}

		public async void RetrieveDarkness()
		{
			if (_toasterConsumer != null)
			{
				toasterGetDarknessResult darknessResult = await _toasterConsumer.GetDarknessAsync();
				darkness = darknessResult.Darkness;
			}
		}
	}
}
