using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.AllJoyn;
using org.alljoyn.example.Toaster;

namespace Toaster.Producer
{
	class ToasterServer
	{
		ToasterProducer toasterProducer;
		ToasterService toasterService;

		public ToasterServer()
		{
			AllJoynBusAttachment toasterBusAttachment = new AllJoynBusAttachment();
			toasterProducer = new ToasterProducer(toasterBusAttachment);
			toasterService = new ToasterService();
			toasterService.ToastBurnt += ToasterService_ToastBurnt;
			toasterProducer.Service = toasterService;
			toasterProducer.Start();
		}

		private void ToasterService_ToastBurnt(object sender, EventArgs e)
		{
			toasterProducer.Signals.ToastBurnt();
		}
		
		public ToasterService Service
		{
			get
			{
				return toasterService;
			}
		}
		

		public void DarknessChanged(byte darkness)
		{
			if (darkness != toasterService.DarknessLevel)
			{
				toasterService.DarknessLevel = darkness;
				toasterProducer.EmitDarknessLevelChanged();
			}			
		}
	}
}
