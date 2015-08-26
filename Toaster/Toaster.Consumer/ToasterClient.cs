using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.AllJoyn;
using org.alljoyn.example.Toaster;

namespace Toaster.Consumer
{
	class ToasterClient
	{
		UInt16 toasterVersion;
		byte darknessLevel;

		ToasterConsumer toasterConsumer;
		
		public ToasterClient()
		{
			toasterVersion = 0;
			darknessLevel = 0;			
		}

		public void FindToaster()
		{
			AllJoynBusAttachment toasterBusAttachment = new AllJoynBusAttachment();
			ToasterWatcher toasterWatcher = new ToasterWatcher(toasterBusAttachment);
			toasterWatcher.Added += ToasterWatcher_Added;
			toasterWatcher.Start();
		}

		public event EventHandler ToasterProducerFound;

		protected virtual void OnToasterProducerFound(EventArgs e)
		{
			EventHandler handler = ToasterProducerFound;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		private async void ToasterWatcher_Added(ToasterWatcher sender, AllJoynServiceInfo args)
		{
			ToasterJoinSessionResult toasterJoinSessionResult = await ToasterConsumer.JoinSessionAsync(args, sender);

			if (toasterJoinSessionResult.Status == AllJoynStatus.Ok)
			{
				toasterConsumer = toasterJoinSessionResult.Consumer;
				
				GetToasterVersion();
				GetToasterDarknessLevel();

				OnToasterProducerFound(EventArgs.Empty);
			}
		}

		public ToasterConsumer Consumer
		{
			get
			{
				if (toasterConsumer != null)
				{
					return toasterConsumer;
				}
				else
					return null;
			}
		}

		public UInt16 ToasterVersion
		{
			get
			{
				return toasterVersion;			
			}
		}

		public byte DarknessLevel
		{
			get
			{
				GetToasterDarknessLevel();
				return darknessLevel;
			}
			set
			{
				if (toasterConsumer != null)
				{
					darknessLevel = value;
					SetToasterDarknessLevel();
				}				
			}
		}

		private async void GetToasterDarknessLevel()
		{
			ToasterGetDarknessLevelResult toasterGetDarknessLevelResult = await toasterConsumer.GetDarknessLevelAsync();
			if (toasterGetDarknessLevelResult.Status == AllJoynStatus.Ok)
			{
				darknessLevel = toasterGetDarknessLevelResult.DarknessLevel;
			}
		}

		private async void SetToasterDarknessLevel()
		{
			ToasterSetDarknessLevelResult toasterSetDarknessLevelResult = await toasterConsumer.SetDarknessLevelAsync(darknessLevel);
			if (toasterSetDarknessLevelResult.Status == AllJoynStatus.Ok)
			{
				// success
			}
		}

		private async void GetToasterVersion()
		{
			ToasterGetVersionResult toasterGetVersionResult = await toasterConsumer.GetVersionAsync();
			if (toasterGetVersionResult.Status == AllJoynStatus.Ok)
			{
				toasterVersion = toasterGetVersionResult.Version;
			}
		}

		public async void StartToasting()
		{
			if (toasterConsumer != null)
			{
				ToasterStartToastingResult toasterStartToastingResult = await toasterConsumer.StartToastingAsync();
				if (toasterStartToastingResult.Status == AllJoynStatus.Ok)
				{
					// success
				}
			}
		}

		public async void StopToasting()
		{
			if (toasterConsumer != null)
			{
				ToasterStopToastingResult toasterStopToastingResult = await toasterConsumer.StopToastingAsync();
				if (toasterStopToastingResult.Status == AllJoynStatus.Ok)
				{
					// success
				}
			}
		}
	}
}
