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
            Console.CancelKeyPress += CloseApp;

            Console.WriteLine("Buddah Audio and Midi Manager by Eidetic");
            Console.WriteLine();

            await Router.Forward("Engine MIDI", "OP-1 Midi Device");
            await Router.Forward("Engine MIDI", "Moog Minitaur");

            while (Running)
            {
                Console.Read();
            }

            Environment.Exit(0);
        }

        private static void CloseApp(object sender, EventArgs e)
        {
            Manager.Close();
            Running = false;
        }
    }
}
