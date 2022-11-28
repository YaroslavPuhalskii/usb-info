using System.Runtime.InteropServices;

namespace USB_Info.Core.Natives.Types;

[StructLayout(LayoutKind.Sequential)]
internal struct USB_SETUP_PACKET
{
    internal byte bmRequest;

    internal byte bRequest;

    internal short wValue;

    internal short wIndex;

    internal short wLength;
}