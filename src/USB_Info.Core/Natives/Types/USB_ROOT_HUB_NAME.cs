using System.Runtime.InteropServices;

namespace USB_Info.Core.Natives.Types;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
internal struct USB_ROOT_HUB_NAME
{
    public readonly uint ActualLength;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NativeMethods.BUFFER_SIZE)]
    public readonly string RootHubName;
}