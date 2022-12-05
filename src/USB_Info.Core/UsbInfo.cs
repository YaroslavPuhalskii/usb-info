using USB_Info.Core.Abstractions;
using USB_Info.Core.Models;

namespace USB_Info.Core
{
    public class UsbInfo : IUsbInfo
    {
        public IEnumerable<UsbRootHub> RootHubs()
        {
            return new List<UsbRootHub>();
            //throw new NotImplementedException();
        }
    }
}
