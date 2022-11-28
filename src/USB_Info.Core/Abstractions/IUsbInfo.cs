using USB_Info.Core.Models;

namespace USB_Info.Core.Abstractions;

public interface IUsbInfo
{
    IEnumerable<UsbRootHub> RootHubs();
}
