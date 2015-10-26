using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using org.alljoyn.example.Toaster;

namespace Toaster.Consumer
{
    class ToasterFoundEventArgs
    {
        private ToasterConsumer consumer;

        public ToasterFoundEventArgs(ToasterConsumer toasterConsumer)
        {
            consumer = toasterConsumer;
        }

        public ToasterConsumer Consumer
        {
            get
            {
                return consumer;
            }
        }
    }
}
