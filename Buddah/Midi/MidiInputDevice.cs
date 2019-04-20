using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Commons.Music.Midi;

namespace Eidetic.Buddah.Midi
{
    public class MidiInputDevice
    {
        IMidiInput MidiInput;

        public Action<MidiReceivedEventArgs> MessageReceived;
        public Action<int, int> NoteOn;
        public Action<int> NoteOff;
        public Action<int, int> ControlChange;

        public MidiInputDevice(IMidiInput midiInput)
        {
            MidiInput = midiInput;

            // Initialise these higher-level actions with empty callbacks
            MessageReceived = (MidiReceivedEventArgs e) => { };
            NoteOn = (int noteNumber, int velocity) => { };
            NoteOff = (int noteNumber) => { };
            ControlChange = (int ccNumber, int value) => { };

            // And add the message routing method to the base callback
            MidiInput.MessageReceived += ProcessMessageReceived;
        }

        public async void Close() => await MidiInput.CloseAsync();

        void ProcessMessageReceived(object sender, MidiReceivedEventArgs e)
        {
            MessageReceived.Invoke(e);

            // Extract the information from the MIDI byte array,
            // and invoke the respective callbacks

            switch ((MessageType)(e.Data[0] >> 4))
            {
                case MessageType.NoteOn:
                    {
                        var noteNumber = (int)e.Data[1];
                        var velocity = (int)e.Data[2];
                        if (velocity != 0) NoteOn.Invoke(noteNumber, velocity);
                        else NoteOff.Invoke(noteNumber);
                        break;
                    }
                case MessageType.NoteOff:
                    {
                        var noteNumber = (int)e.Data[1];
                        NoteOff.Invoke(noteNumber);
                        break;
                    }
                case MessageType.ControlChange:
                    {
                        var ccNumber = (int)e.Data[1];
                        var value = (int)e.Data[2];
                        ControlChange.Invoke(ccNumber, value);
                        break;
                    }
                default: break;
            }
        }

        enum MessageType
        {
            NoteOn = 9, NoteOff = 8, ControlChange = 11
        }
    }
}
