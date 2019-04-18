using System;

namespace Buddah
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Buddah Audio and Midi Manager by Eidetic");
            Console.WriteLine();

            AppDomain.CurrentDomain.ProcessExit += CloseApp;

            Router.Forward("Engine MIDI", "OP-1 Midi Device");
            Router.Forward("Engine MIDI", "Moog Minitaur");

            Console.ReadKey();
        }

        private static void CloseApp(object sender, EventArgs e)
        {
            Router.Stop();
        }
    }
}
