using System.Runtime.InteropServices;

namespace USB_Info.Core.Natives.Types;

[StructLayout(LayoutKind.Sequential)]
internal struct USB_HUB_INFORMATION
{
    public USB_HUB_DESCRIPTOR HubDescriptor;

    public readonly byte HubIsBusPowered;
}