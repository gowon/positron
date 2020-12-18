using System;
using System.Runtime.InteropServices;

namespace WpfHybridApp.Services
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class SampleService : ISampleService
    {
        // Sample property.
        public string Prop { get; set; } = "BridgeAddRemoteObject.Prop";

        public string GetCurrentDate()
        {
            return DateTime.Now.ToString("F");
        }
    }
}