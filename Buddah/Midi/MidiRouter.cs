using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Commons.Music.Midi;

namespace Eidetic.Buddah.Midi
{
    public static class MidiRouter
    {
        public static async Task<bool> Forward(string inputDeviceName, string outputDeviceName)
        {
            if (!await MidiManager.EnsureInputReady(inputDeviceName)) return false;
            if (!await MidiManager.EnsureOutputReady(outputDeviceName)) return false;

            // Forward all messages received to the output device
            MidiManager.ActiveInputDevices[inputDeviceName].MessageReceived += (MidiReceivedEventArgs e) =>
                MidiManager.ActiveOutputDevices[outputDeviceName].Send(e.Data, e.Start, e.Length, e.Timestamp);

            Logger.WriteLine("Successfully attached {0} to {1} for message forwarding.", inputDeviceName, outputDeviceName);

            return true;
        }
    }
}
