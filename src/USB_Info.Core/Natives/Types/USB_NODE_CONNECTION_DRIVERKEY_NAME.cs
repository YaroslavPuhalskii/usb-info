using System.Runtime.InteropServices;

namespace USB_Info.Core.Natives.Types;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
internal struct USB_NODE_CONNECTION_DRIVERKEY_NAME
{
    public uint ConnectionIndex;

    public readonly uint ActualLength;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NativeMethods.BUFFER_SIZE)]
    public readonly string DriverKeyName;
}
