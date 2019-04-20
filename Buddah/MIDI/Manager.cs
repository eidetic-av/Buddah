using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Commons.Music.Midi;
using System.Threading.Tasks;

namespace Eidetic.Buddah.Midi
{
    public static class MidiManager
    {
        public static IMidiAccess AccessManager { get; private set; }

        public static List<IMidiPortDetails> AvailableInputs => AccessManager.Inputs.ToList();
        public static List<IMidiPortDetails> AvailableOutputs => AccessManager.Outputs.ToList();

        public static Dictionary<string, IMidiInput> ActiveInputs { get; private set; } = new Dictionary<string, IMidiInput>();
        public static Dictionary<string, IMidiOutput> ActiveOutputs { get; private set; } = new Dictionary<string, IMidiOutput>();

        static MidiManager()
        {
            AccessManager = MidiAccessManager.Default;
            Logger.WriteLine("Available MIDI Inputs ({0}): ", AccessManager.Inputs.Count());
            AvailableInputs.ForEach(i => Logger.WriteLine("    " + i.Name));
            Logger.WriteLine();
            Logger.WriteLine("Available MIDI Outputs: ");
            AvailableOutputs.ForEach(o => Logger.WriteLine("    " + o.Name));
            Logger.WriteLine();
        }

        public static async Task<bool> OpenInput(string inputDeviceName)
        {
            var inputInfo = AvailableInputs.SingleOrDefault(i => i.Name.ToLower() == inputDeviceName.ToLower());
            if (inputInfo == default) return false;
            ActiveInputs[inputDeviceName] = await AccessManager.OpenInputAsync(inputInfo.Id);
            Logger.WriteLine("Successfully opened input device {0}", inputDeviceName);
            return true;
        }

        public static async Task<bool> OpenOutput(string outputDeviceName)
        {
            var outputInfo = AvailableOutputs.SingleOrDefault(o => o.Name.ToLower() == outputDeviceName.ToLower());
            if (outputInfo == default) return false;
            ActiveOutputs[outputDeviceName] = await AccessManager.OpenOutputAsync(outputInfo.Id);
            Logger.WriteLine("Successfully opened output device {0}", outputDeviceName);
            return true;
        }

        public static async Task<bool> EnsureInputReady(string inputDeviceName)
        {
            if (ActiveInputs.ContainsKey(inputDeviceName)) return true;
            return await OpenInput(inputDeviceName);
        }

        public static async Task<bool> EnsureOutputReady(string outputDeviceName)
        {
            if (ActiveOutputs.ContainsKey(outputDeviceName)) return true;
            return await OpenOutput(outputDeviceName);
        }

        public static void Close()
        {
            Logger.WriteLine("Closing Midi Devices");
            foreach (var input in ActiveInputs) input.Value.CloseAsync();
            foreach (var output in ActiveOutputs) output.Value.CloseAsync();
        }
    }
}
