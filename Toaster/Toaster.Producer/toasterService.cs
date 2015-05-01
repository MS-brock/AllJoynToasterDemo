using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.microsoft.sample;
using Windows.Foundation;
using Windows.Devices.AllJoyn;

namespace Toaster.Producer
{
	class toasterService : ItoasterService
	{
		private uint darkness;
		toasterSignals _toasterSignals;

		public toasterService(toasterSignals toasterSignals)
		{
			darkness = (uint)5;
			_toasterSignals = toasterSignals;
		}


		// Implement this function to handle calls to the startToasting method.
		public IAsyncOperation<toasterStartToastingResult> StartToastingAsync(AllJoynMessageInfo info)
		{
			Task<toasterStartToastingResult> task = new Task<toasterStartToastingResult>(() =>
			{
				_toasterSignals.ToastDone(AllJoynStatus.Ok);
				return toasterStartToastingResult.CreateSuccessResult();
			});

			task.Start();
			return task.AsAsyncOperation();
		}

		// Implement this function to handle calls to the stopToasting method.
		public IAsyncOperation<toasterStopToastingResult> StopToastingAsync(AllJoynMessageInfo info)
		{
			Task<toasterStopToastingResult> task = new Task<toasterStopToastingResult>(() =>
			{
				return toasterStopToastingResult.CreateSuccessResult();
			});

			task.Start();
			return task.AsAsyncOperation();
		}

		// Implement this function to handle requests for the value of the Darkness property.
		//
		// Currently, info will always be null, because no information is available about the requestor.
		public IAsyncOperation<toasterGetDarknessResult> GetDarknessAsync(AllJoynMessageInfo info)
		{
			Task<toasterGetDarknessResult> task = new Task<toasterGetDarknessResult>(() =>
			{
				return toasterGetDarknessResult.CreateSuccessResult(darkness);
			});

			task.Start();
			return task.AsAsyncOperation();
		}

		// Implement this function to handle requests to set the Darkness property.
		//
		// Currently, info will always be null, because no information is available about the requestor.
		public IAsyncOperation<int> SetDarknessAsync(AllJoynMessageInfo info, uint value)
		{
			darkness = value;
			

			Task<int> task = new Task<int>(() =>
			{
				return AllJoynStatus.Ok;
			});

			task.Start();
			return task.AsAsyncOperation();
		}
	}
}
