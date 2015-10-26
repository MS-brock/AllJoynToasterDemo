using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.AllJoyn;
using org.alljoyn.example.Toaster;

namespace Toaster.Consumer
{
    class ToasterFinder
    {
        public ToasterFinder()
        {

        }

        public delegate void ToasterFoundEventHandler(object sender, ToasterFoundEventArgs args);

        public event ToasterFoundEventHandler ToasterFound;

        public void Start()
        {
            StartWatcher();
        }

        protected virtual void OnToasterFound(ToasterFoundEventArgs args)
        {
            ToasterFoundEventHandler handler = ToasterFound;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void StartWatcher()
        {
            AllJoynBusAttachment toasterBusAttachment = new AllJoynBusAttachment();
            ToasterWatcher toasterWatcher = new ToasterWatcher(toasterBusAttachment);
            toasterWatcher.Added += ToasterWatcher_Added;

            toasterWatcher.Start();
        }

        private async void ToasterWatcher_Added(ToasterWatcher sender, AllJoynServiceInfo args)
        {
            ToasterJoinSessionResult toasterJoinSessionResult = await ToasterConsumer.JoinSessionAsync(args, sender);

            if (toasterJoinSessionResult.Status == AllJoynStatus.Ok)
            {
                ToasterFoundEventArgs toasterFoundArgs = new ToasterFoundEventArgs(toasterJoinSessionResult.Consumer);
                OnToasterFound(toasterFoundArgs);
            }
            else
            {

            }
        }
    }
}
