using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using USB_Info.MVVM.Models;
using USB_Info.Services.Abstractions;

namespace USB_Info.MVVM.ViewModels
{
    internal partial class LogicalHIDViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<HidDevice>? devices;

        [ObservableProperty]
        public HidDevice? selectedDevice;

        [ObservableProperty]
        public ObservableCollection<Parameter>? parameters;

        private readonly ILogicalHIDService _logicalHIDDeviceService;

        public LogicalHIDViewModel(ILogicalHIDService logicalHIDDeviceService)
        {
            _logicalHIDDeviceService = logicalHIDDeviceService;

            Devices = _logicalHIDDeviceService.GetDevices();
            Parameters = new ObservableCollection<Parameter>();

            StartTimer();
        }

        private void StartTimer()
        {
            var dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            dispatcherTimer.Tick += new EventHandler(UpdateDevices!);
            dispatcherTimer.Start();
        }

        partial void OnSelectedDeviceChanged(HidDevice? value)
        {
            if (value == null)
                return;

            Parameters = _logicalHIDDeviceService.GetDeviceInformation(value!);
        }

        void UpdateDevices(object sender, EventArgs e)
        {
            if (_logicalHIDDeviceService.UpdateDevices(Devices!))
                Parameters!.Clear();
        }
    }
}
