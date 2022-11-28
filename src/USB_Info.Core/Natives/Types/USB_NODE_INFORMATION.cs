using System.Runtime.InteropServices;

namespace USB_Info.Core.Natives.Types;

[StructLayout(LayoutKind.Sequential)]
internal struct USB_NODE_INFORMATION
{
    public int NodeType;

    public USB_NODE_UNION UsbNodeUnion;
}