using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using org.alljoyn.example.Toaster;
using Windows.Devices.AllJoyn;
using Windows.Foundation;

namespace Toaster.Producer
{
	class ToasterService : IToasterService
	{
		byte darknessLevel;
		UInt16 toasterVersion;
		bool toasting;

		CancellationTokenSource source;
		CancellationToken token;

		public ToasterService()
		{
			darknessLevel = 5;
			toasterVersion = 1;
			toasting = false;
			GetNewToken();
		}

		public UInt16 ToasterVersion
		{
			get
			{
				return toasterVersion;
			}
		}

		private void GetNewToken()
		{
			source = new CancellationTokenSource();
			token = source.Token;
		}

		public event EventHandler ToastBurnt;

		protected virtual void OnToastBurnt(EventArgs e)
		{
			EventHandler handler = ToastBurnt;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		public event EventHandler StartedToasting;

		protected virtual void OnStartedToasting(EventArgs e)
		{
			EventHandler handler = StartedToasting;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		public event EventHandler StoppedToasting;

		protected virtual void OnStoppedToasting(EventArgs e)
		{
			EventHandler handler = StoppedToasting;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		public event EventHandler DarknessChanged;

		protected virtual void OnDarknessChanged(EventArgs e)
		{
			EventHandler handler = DarknessChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		public byte DarknessLevel
		{
			get
			{
				return darknessLevel;
			}
			set
			{
				darknessLevel = value;
			}
		}
		
		private async void ToastTask()
		{
			StartToasting();
			
			byte i;
			for (i = 0; i < Math.Min(darknessLevel, (byte)10); i++)
			{
				await Task.Delay(1000);

				if (i == 7)
				{
					OnToastBurnt(EventArgs.Empty);
				}
			}
			StopToasting();
		}

		private void StartToasting()
		{
			toasting = true;
			OnStartedToasting(EventArgs.Empty);
		}

		private void StopToasting()
		{
			toasting = false;
			OnStoppedToasting(EventArgs.Empty);			
		}


		public IAsyncOperation<ToasterGetDarknessLevelResult> GetDarknessLevelAsync(AllJoynMessageInfo info)
		{
			Task<ToasterGetDarknessLevelResult> task = new Task<ToasterGetDarknessLevelResult>(() =>
			{
				return ToasterGetDarknessLevelResult.CreateSuccessResult(darknessLevel);
			});
			task.Start();

			return task.AsAsyncOperation();
		}

		public IAsyncOperation<ToasterGetVersionResult> GetVersionAsync(AllJoynMessageInfo info)
		{
			Task<ToasterGetVersionResult> task = new Task<ToasterGetVersionResult>(() =>
			{
				return ToasterGetVersionResult.CreateSuccessResult(toasterVersion);
            });
			task.Start();

			return task.AsAsyncOperation();
		}

		public IAsyncOperation<ToasterSetDarknessLevelResult> SetDarknessLevelAsync(AllJoynMessageInfo info, byte value)
		{
			Task<ToasterSetDarknessLevelResult> task = new Task<ToasterSetDarknessLevelResult>(() =>
			{				
				darknessLevel = value;
				OnDarknessChanged(EventArgs.Empty);
				return ToasterSetDarknessLevelResult.CreateSuccessResult();
            });
			task.Start();

			return task.AsAsyncOperation();
		}

		public IAsyncOperation<ToasterStartToastingResult> StartToastingAsync(AllJoynMessageInfo info)
		{
			Task<ToasterStartToastingResult> task = new Task<ToasterStartToastingResult>(() =>
			{
				Task toast = Task.Factory.StartNew(ToastTask, token);

				return ToasterStartToastingResult.CreateSuccessResult();
			});
			task.Start();

			return task.AsAsyncOperation();
		}

		public IAsyncOperation<ToasterStopToastingResult> StopToastingAsync(AllJoynMessageInfo info)
		{
			Task<ToasterStopToastingResult> task = new Task<ToasterStopToastingResult>(() =>
			{
				source.Cancel();
				StopToasting();

				GetNewToken();

				return ToasterStopToastingResult.CreateSuccessResult();
            });

			task.Start();

			return task.AsAsyncOperation();
		}		
	}
}
