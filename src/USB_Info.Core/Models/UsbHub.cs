namespace USB_Info.Core.Models
{
    public class UsbHub : IUsbHub
    {
        public string? ConnectionName { get; set; }

        public string? ConnectionStatus { get; set; }

        public int Flag { get; set; }

        public int HubIndex { get; set; }

        public int HubPorts { get; set; }

        public ICollection<IUsbDevice>? Devices { get; set; }
    }
}