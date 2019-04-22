using System;
using System.Threading.Tasks;
using Eidetic.Buddah.Midi;

class Program
{
    static bool Running = true;

    static async Task Main(string[] args)
    {
        AppDomain.CurrentDomain.ProcessExit += CloseApp;
        Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) => Environment.Exit(0);

        Console.WriteLine("Buddah Audio and Midi Manager by Eidetic");
        Console.WriteLine();

        await MidiRouter.Forward("Engine MIDI", "OP-1 Midi Device");
        await MidiRouter.Forward("Engine MIDI", "Moog Minitaur");
        await MidiRouter.Forward("Engine MIDI", "loopMIDI Port");
        await MidiRouter.Forward("OP-1 Midi Device", "loopMIDI Port");
        await MidiRouter.Forward("Moog Minitaur", "loopMIDI Port");

        //MidiManager.ActiveInputDevices["Moog Minitaur"].ControlChange += (int ccNumber, int value) => Console.WriteLine("ey: " + value);

        while (Running)
        {
            Console.Read();
        }
    }

    private static void CloseApp(object sender, EventArgs e)
    {
        MidiManager.Close();
        Running = false;
    }
}