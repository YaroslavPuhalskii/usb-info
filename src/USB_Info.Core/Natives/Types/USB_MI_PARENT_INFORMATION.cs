using System.Runtime.InteropServices;

namespace USB_Info.Core.Natives.Types;

[StructLayout(LayoutKind.Sequential)]
internal struct USB_MI_PARENT_INFORMATION
{
    public readonly int NumberOfInterfaces;
}
