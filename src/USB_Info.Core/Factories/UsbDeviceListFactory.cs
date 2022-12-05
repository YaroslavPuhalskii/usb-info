using Microsoft.Win32.SafeHandles;
using USB_Info.Core.Abstractions;
using USB_Info.Core.Models;
using USB_Info.Core.Natives;
using USB_Info.Core.Natives.Types;
using static USB_Info.Core.Natives.NativeMethods;

namespace USB_Info.Core.Factories;

internal static class UsbDeviceListFactory
{
    internal static IList<IUsbDevice> Create(string hubDevicePath, ushort hubIndex)
    {
        using var hubHandle = CreateFile(
            hubDevicePath, GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
        return Create(hubHandle, hubIndex);
    }

    private static IList<IUsbDevice> Create(SafeFileHandle hubHandle, ushort hubIndex)
    {
        var nodeInformation = DeviceIoControlInvoker
            .Invoke(hubHandle, IOCTL_USB_GET_NODE_INFORMATION, new USB_NODE_INFORMATION { NodeType = 0 });

        var usbDevices = new List<IUsbDevice>();
        var portNumber = nodeInformation.UsbNodeUnion.HubInformation.HubDescriptor.bNumberOfPorts;
        for (uint portIndex = 1; portIndex <= portNumber; portIndex++)
        {
            var usbDeviceFactory = AbstractUsbDeviceFactory.CreateFactory(hubHandle, portIndex, hubIndex);
            usbDevices.Add(usbDeviceFactory.Create());
        }

        return usbDevices;
    }


    private static ushort _hostIndex = 0;

    private static bool _endOfSearch = true;

    private static readonly ICollection<HostController> hostControllers = new List<HostController>();

    internal static IEnumerable<HostController> Create()
    {
        do
        {
            try
            {
                var hostController = HostControllerFactory.CreateHostController(_hostIndex++);

                hostControllers.Add(hostController);
            }
            catch (Exception ex)
            {
                _endOfSearch = false;
            }

        }
        while (_endOfSearch);

        return hostControllers;
    }
}