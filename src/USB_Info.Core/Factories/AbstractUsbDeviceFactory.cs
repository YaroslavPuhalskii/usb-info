using Microsoft.Win32.SafeHandles;
using USB_Info.Core.Abstractions;
using USB_Info.Core.Natives;
using USB_Info.Core.Natives.Types;

namespace USB_Info.Core.Factories;

internal class AbstractUsbDeviceFactory
{
    internal static IUsbDeviceFactory CreateFactory(SafeFileHandle hubHandle, uint portIndex, ushort hubIndex)
    {
        var connectInfomation = DeviceIoControlInvoker.Invoke(
            hubHandle, NativeMethods.IOCTL_USB_GET_NODE_CONNECTION_INFORMATION_EX,
            new USB_NODE_CONNECTION_INFORMATION_EX { ConnectionIndex = portIndex });

        if (connectInfomation.ConnectionStatus == 0)
        {
            return new UnConnectUsbDeviceFactory(connectInfomation, portIndex, hubIndex);
        }

        if (Convert.ToBoolean(connectInfomation.DeviceIsHub))
        {
            return new UsbDeviceFactory(hubHandle, connectInfomation, portIndex, hubIndex);
        }

        return new UsbDeviceFactory(hubHandle, connectInfomation, portIndex, hubIndex);
    }
}