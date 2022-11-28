using System.Runtime.InteropServices;

namespace USB_Info.Core.Natives.Types;

[StructLayout(LayoutKind.Sequential)]
internal struct USB_STRING_DESCRIPTOR
{
    internal byte bLength;

    internal byte bDescriptorType;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
    internal string bString;
}