namespace USB_Info.Core.Helpers;

internal static class PathHelper
{
    public static string HostPath = @"\\.\HCD";

    public static string UsbRegistryPath = @"SYSTEM\CurrentControlSet\Enum\USB\";

    public static string HidRegistryPath = @"SYSTEM\CurrentControlSet\Enum\HID\";

    public static string EnumRegistryPath = @"SYSTEM\CurrentControlSet\Enum\";
}