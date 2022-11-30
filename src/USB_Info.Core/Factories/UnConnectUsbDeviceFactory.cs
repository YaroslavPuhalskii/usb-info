using USB_Info.Core.Abstractions;
using USB_Info.Core.Extensions;
using USB_Info.Core.Models;
using USB_Info.Core.Natives.Types;
using static USB_Info.Core.Natives.NativeMethods;

namespace USB_Info.Core.Factories;

internal class UnConnectUsbDeviceFactory : IUsbDeviceFactory
{
    private readonly USB_NODE_CONNECTION_INFORMATION_EX _usbConnectInformation;

    private readonly uint _portIndex;

    private readonly ushort _hubIndex;

    public UnConnectUsbDeviceFactory(USB_NODE_CONNECTION_INFORMATION_EX usbConnectInformation,
        uint portIndex,
        ushort hubIndex)
    {
        _usbConnectInformation = usbConnectInformation;
        _portIndex = portIndex;
        _hubIndex = hubIndex;
    }

    public IUsbDevice Create()
        => new UnConnectUsbDevice($"Hub{_hubIndex.HubPortNumber()}Port{_portIndex.PortNumber()}",
            GetDeviceConnectionStatus(_usbConnectInformation.ConnectionStatus),
            _hubIndex,
            _portIndex);
}