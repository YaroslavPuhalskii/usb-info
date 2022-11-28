using USB_Info.Core.Natives.Types;

namespace USB_Info.Core.Extensions;

internal static class UsbRootHubNameExtensions
{
    internal static string DevicePath(this USB_ROOT_HUB_NAME rootHubName)
    {
        return @"\\.\" + rootHubName.RootHubName;
    }

    internal static string HubPortNumber(this ushort hubPort)
    {
        if (hubPort < 10)
            return $"00{hubPort}";
        else if (hubPort < 100)
            return $"0{hubPort}";
        else
            return $"{hubPort}";
    }

    internal static string PortNumber(this uint portIndex)
    {
        if (portIndex < 10)
            return $"00{portIndex}";
        else if (portIndex < 100)
            return $"0{portIndex}";
        else
            return $"{portIndex}";
    }
}