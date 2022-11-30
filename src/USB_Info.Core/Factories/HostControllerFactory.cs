using USB_Info.Core.Extensions;
using USB_Info.Core.Models;
using USB_Info.Core.Natives;
using USB_Info.Core.Natives.Types;
using static USB_Info.Core.Natives.NativeMethods;

namespace USB_Info.Core.Factories;

internal static class HostControllerFactory
{
    // will be delete
    private static ushort _hubIndex = 1;

    internal static IEnumerable<HostController> Create()
    {
        return HostControllerPath().Select(Create);
    }

    private static HostController Create(string hostControllerPath)
    {
        using var hostHandle = CreateFile(hostControllerPath, GENERIC_WRITE,
            FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
        ThrowIfSetLastError(!hostHandle.IsInvalid);

        var usbRootHubName = DeviceIoControlInvoker.Invoke<USB_ROOT_HUB_NAME>(hostHandle, IOCTL_USB_GET_ROOT_HUB_NAME);

        return new HostController($"HOST CONTROLLER {hostControllerPath.Last()}",
            usbRootHubName.DevicePath(),
            hostControllerPath,
            _hubIndex++,
            1,
            GetDeviceConnectionStatus(1));
    }

    // will be delete
    public static HostController CreateHostController(int hostIndex)
    {
        using var hostHandle = CreateFile($"{Helpers.PathHelper.HostPath}{hostIndex}",
            GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

        ThrowIfSetLastError(!hostHandle.IsInvalid);
        var usbRootHubName = DeviceIoControlInvoker.Invoke<USB_ROOT_HUB_NAME>(hostHandle, IOCTL_USB_GET_ROOT_HUB_NAME);

        return new HostController()
        {
            Name = $"HOST CONTROLLER {hostIndex}",
            RootHubPath = usbRootHubName.DevicePath(),
            HostControllerPath = $"{Helpers.PathHelper.HostPath}{hostIndex}",
            HubPorts = 1,
            ConnectionStatus = GetDeviceConnectionStatus(1),
            Hub = UsbHubFactory.Create(usbRootHubName, _hubIndex++),
        };
    }
}