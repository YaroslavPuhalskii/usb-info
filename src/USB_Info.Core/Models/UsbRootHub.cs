using USB_Info.Core.Abstractions;
using USB_Info.Core.Factories;

namespace USB_Info.Core.Models;

public class UsbRootHub : IUsbRootHub
{
    public HostController? HostController { get; }

    public int PortNumber { get; }

    public IList<IUsbDevice>? ConnectedDevices { get; }

    public UsbRootHub(HostController hostController)
    {
        HostController = hostController;
        ConnectedDevices = UsbDeviceListFactory.Create(hostController.RootHubPath!, hostController.HubIndex);
        PortNumber = ConnectedDevices.Count;
    }
}
