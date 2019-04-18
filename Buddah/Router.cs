using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Commons.Music.Midi;

namespace Buddah
{
    public class Router
    {
        static Router instance;
        public static Router Instance
        {
            get
            {
                if (instance == null)
                    instance = new Router();
                return instance;
            }
        }

        static Dictionary<string, IMidiInput> Inputs = new Dictionary<string, IMidiInput>();
        static Dictionary<string, IMidiOutput> Outputs = new Dictionary<string, IMidiOutput>();

        IMidiAccess Manager;

        private Router()
        {
            Manager = MidiAccessManager.Default;
            Console.WriteLine("Available MIDI Inputs: ");
            Manager.Inputs.ToList().ForEach(i => Console.WriteLine("    > " + i.Name));
            Console.WriteLine();
            Console.WriteLine("Available MIDI Outputs: ");
            Manager.Outputs.ToList().ForEach(o => Console.WriteLine("    > " + o.Name));
            Console.WriteLine();
        }

        public static async void Forward(string inputDeviceName, string outputDeviceName)
        {
            // Get the input device
            if (!Inputs.ContainsKey(inputDeviceName))
            {
                var inputInfo = Instance.Manager.Inputs.SingleOrDefault(i => i.Name.ToLower() == inputDeviceName.ToLower());
                if (inputInfo == default) return;
                Inputs[inputDeviceName] = await Instance.Manager.OpenInputAsync(inputInfo.Id);
            }

            // Get the output device
            if (!Outputs.ContainsKey(outputDeviceName))
            {
                var outputInfo = Instance.Manager.Outputs.SingleOrDefault(i => i.Name.ToLower() == outputDeviceName.ToLower());
                if (outputInfo == default) return;
                Outputs[outputDeviceName] = await Instance.Manager.OpenOutputAsync(outputInfo.Id);
            }

            // Forward all messages received to the output device
            Inputs[inputDeviceName].MessageReceived += (object s, MidiReceivedEventArgs e) => Outputs[outputDeviceName].Send(e.Data, e.Start, e.Length, e.Timestamp);

            Console.WriteLine(System.DateTime.Now.ToLongTimeString() + " - Successfully attached {0} to {1} for forwarding messages.", inputDeviceName, outputDeviceName);
        }

        public static void Stop()
        {
            Console.WriteLine("Closing Midi Devices");
            foreach (var input in Inputs) input.Value.CloseAsync();
            foreach (var output in Outputs) output.Value.CloseAsync();
        }
    }
}
