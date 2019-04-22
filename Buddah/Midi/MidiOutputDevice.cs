using Commons.Music.Midi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eidetic.Buddah.Midi
{
    public class MidiOutputDevice
    {
        IMidiOutput MidiOutput;
        
        public MidiOutputDevice(IMidiOutput midiOutput)
        {
            MidiOutput = midiOutput;
        }

        public async void Send(byte[] midiData, int start, int length, long timestamp) {
            if (await MidiManager.EnsureOutputReady(MidiOutput.Details.Name))
                MidiOutput.Send(midiData, start, length, timestamp);
        }

        public async void Close() => await MidiOutput.CloseAsync();
    }
}
