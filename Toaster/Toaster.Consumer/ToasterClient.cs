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
		
		public ToasterClient(ToasterConsumer consumer)
		{
            toasterConsumer = consumer;
			toasterVersion = 0;
			darknessLevel = 0;

            GetToasterVersion();
            GetToasterDarknessLevel();
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
