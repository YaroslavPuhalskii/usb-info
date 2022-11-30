using USB_Info.Core.Abstractions;
using USB_Info.Core.Extensions;
using USB_Info.Core.Models;
using USB_Info.Core.Natives;
using USB_Info.Core.Natives.Types;
using static USB_Info.Core.Natives.NativeMethods;

namespace USB_Info.Core.Factories
{
    internal static class UsbHubFactory
    {
        private static ICollection<IUsbDevice>? Devices;

        private static UsbHub? _usbHub;

        public static UsbHub Create(USB_ROOT_HUB_NAME usbRootHubName, ushort hubIndex)
        {
            Devices = new List<IUsbDevice>();

            _usbHub = new UsbHub
            {
                ConnectionName = $"Hub{hubIndex.HubPortNumber()}Port000",
                ConnectionStatus = GetDeviceConnectionStatus(1),
                Flag = FLAG_ROOT,
                HubIndex = hubIndex,
            };

            _usbHub.Devices = EnumerateHubDevices(usbRootHubName, hubIndex);


            return _usbHub;
        }

        private static ICollection<IUsbDevice> EnumerateHubDevices(USB_ROOT_HUB_NAME usbRootHubName, ushort hubIndex)
        {
            using (var handle = CreateFile(usbRootHubName.DevicePath(), GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero))
            {
                var nodeInformation = DeviceIoControlInvoker.Invoke<USB_NODE_INFORMATION>(handle, IOCTL_USB_GET_NODE_INFORMATION);

                _usbHub!.HubPorts = nodeInformation.UsbNodeUnion.HubInformation.HubDescriptor.bNumberOfPorts;

                for (uint portIndex = 1; portIndex <= _usbHub.HubPorts; portIndex++)
                {
                    var device = AbstractUsbDeviceFactory.CreateFactory(handle, portIndex, hubIndex);

                    // Devices.Add(device);
                }
            }

            return Devices!;
        }
    }
}