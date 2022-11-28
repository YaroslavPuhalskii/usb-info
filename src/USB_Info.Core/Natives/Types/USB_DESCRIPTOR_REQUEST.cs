using System.Runtime.InteropServices;

namespace USB_Info.Core.Natives.Types;

[StructLayout(LayoutKind.Sequential)]
internal struct USB_DESCRIPTOR_REQUEST
{
    internal uint ConnectionIndex;

    internal USB_SETUP_PACKET SetupPacket;

    internal USB_STRING_DESCRIPTOR StringDescriptor;
}