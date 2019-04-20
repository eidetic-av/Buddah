using System;
using System.Threading.Tasks;

namespace Eidetic.Buddah
{
    class Program
    {
        static bool Running = true;

        static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += CloseApp;
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) => Environment.Exit(0);

            Console.WriteLine("Buddah Audio and Midi Manager by Eidetic");
            Console.WriteLine();

            await Midi.MidiRouter.Forward("Engine MIDI", "OP-1 Midi Device");
            await Midi.MidiRouter.Forward("Engine MIDI", "Moog Minitaur");
            await Midi.MidiRouter.Forward("Engine MIDI", "loopMIDI Port");
            await Midi.MidiRouter.Forward("OP-1 Midi Device", "loopMIDI Port");
            await Midi.MidiRouter.Forward("Moog Minitaur", "loopMIDI Port");

            while (Running)
            {
                Console.Read();
            }
        }

        private static void CloseApp(object sender, EventArgs e)
        {
            Midi.MidiManager.Close();
            Running = false;
        }
    }
}