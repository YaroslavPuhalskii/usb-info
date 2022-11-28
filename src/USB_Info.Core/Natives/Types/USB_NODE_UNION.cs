using System.Runtime.InteropServices;

namespace USB_Info.Core.Natives.Types;

[StructLayout(LayoutKind.Explicit)]
internal struct USB_NODE_UNION
{
    [FieldOffset(0)] public USB_HUB_INFORMATION HubInformation;

    [FieldOffset(0)] public USB_MI_PARENT_INFORMATION MiParentInformation;
}
