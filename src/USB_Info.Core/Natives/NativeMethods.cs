using Microsoft.Win32.SafeHandles;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace USB_Info.Core.Natives;

internal static class NativeMethods
{
    internal const int IOCTL_USB_GET_NODE_CONNECTION_INFORMATION_EX = 0x00220448;
    internal const int IOCTL_USB_GET_NODE_INFORMATION = 0x00220408;
    internal const int IOCTL_USB_GET_ROOT_HUB_NAME = 0x00220408;
    internal const int IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME = 0x00220420;
    internal const int IOCTL_USB_GET_DESCRIPTOR_FROM_NODE_CONNECTION = 0x220410;
    internal const int IOCTL_GET_HCD_DRIVERKEY_NAME = 0x220424;

    internal const int GENERIC_WRITE = 0x4000_0000;
    internal const int FILE_SHARE_WRITE = 0x000_0002;
    internal const int OPEN_EXISTING = 0x0000_0003;

    internal const int FLAG_ZERO = 0x00;
    internal const int FLAG_DEVC = 0x10;
    internal const int FLAG_ROOT = 0x40;
    internal const int FLAG_HOST = 0x80;

    internal static ushort _hostIndex = 0;

    internal const int USB_STRING_DESCRIPTOR_TYPE = 0x3;

    internal const int BUFFER_SIZE = 2048;

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern SafeFileHandle CreateFile(
        string lpFileName,
        int dwDesiredAccess,
        int dwShareMode,
        IntPtr lpSecurityAttributes,
        int dwCreationDisposition,
        int dwFlagsAndAttributes,
        IntPtr hTemplateFile
    );

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern bool DeviceIoControl(
        SafeFileHandle hDevice,
        int dwIoControlCode,
        IntPtr lpInBuffer,
        int nInBufferSize,
        byte[] lpOutBuffer,
        int nOutBufferSize,
        out int lpBytesReturned,
        IntPtr lpOverlapped
    );

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern bool DeviceIoControl(
        SafeFileHandle hDevice,
        int dwIoControlCode,
        IntPtr lpInBuffer,
        int nInBufferSize,
        IntPtr lpOutBuffer,
        int nOutBufferSize,
        out int lpBytesReturned,
        IntPtr lpOverlapped
    );

    internal static void ThrowIfSetLastError(bool result)
    {
        if (!result)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    internal static IEnumerable<string> HostControllerPath()
    {
        var hostControllerPaths = new List<string>();

        for (ushort hostIndex = 0; hostIndex < ushort.MaxValue; hostIndex++)
        {
            var hostControllerPath = $"{Helpers.PathHelper.HostPath}{hostIndex}";

            using var hostHandle = CreateFile(hostControllerPath, GENERIC_WRITE,
                FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
            if (hostHandle.IsInvalid)
                return hostControllerPaths;
            else
                hostControllerPaths.Add(hostControllerPath);
        }

        return hostControllerPaths;
    }

    internal static string GetDeviceConnectionStatus(uint index)
        => index switch
        {
            0 => "Device not connected.",
            1 => "Device connected.",
            2 => "Enumeration of devices with failures.",
            3 => "General device failure.",
            4 => "Device caused by over current.",
            5 => "The device does not have enough power.",
            6 => "The device does not have enough bandwidth.",
            _ => "Null.",
        };
}