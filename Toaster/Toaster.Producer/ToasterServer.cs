using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.AllJoyn;
using com.microsoft.sample;

namespace Toaster.Producer
{
	class ToasterServer
	{
		private toasterProducer _toasterProducer;
		private AllJoynBusAttachment toasterBusAttachment;

		public ToasterServer()
		{
			_toasterProducer = null;
			toasterBusAttachment = new AllJoynBusAttachment();
			StartService();
		}

		private void StartService()
		{
			_toasterProducer = new toasterProducer(toasterBusAttachment);

			// Initialize an object of SecureInterfaceService - the implementation of ISecureInterfaceService that will handle the concatenation method calls.
			_toasterProducer.Service = new toasterService(_toasterProducer.Signals);
			
			// Create interface as defined in the introspect xml, create AllJoyn bus object
			// and announce the about interface.
			_toasterProducer.Start();
		}

		public void ToastDoneSignal()
		{
			_toasterProducer.Signals.ToastDone(AllJoynStatus.Ok);
		}
	}
}
