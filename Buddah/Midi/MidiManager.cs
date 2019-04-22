using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Commons.Music.Midi;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Eidetic.Buddah.Midi
{
    public static class MidiManager
    {
        public static IMidiAccess AccessManager { get; private set; }

        public static List<IMidiPortDetails> AvailableInputDevices => AccessManager.Inputs.ToList();
        public static List<IMidiPortDetails> AvailableOutputDevices => AccessManager.Outputs.ToList();

        public static Dictionary<string, MidiInputDevice> ActiveInputDevices { get; private set; } = new Dictionary<string, MidiInputDevice>();
        public static Dictionary<string, MidiOutputDevice> ActiveOutputDevices { get; private set; } = new Dictionary<string, MidiOutputDevice>();

        static MidiManager()
        {
            AccessManager = MidiAccessManager.Default;
            Logger.WriteLine("Available MIDI Inputs ({0}): ", AvailableInputDevices.Count());
            AvailableInputDevices.ForEach(i => Logger.WriteLine("    " + i.Name));
            Logger.WriteLine();
            Logger.WriteLine("Available MIDI Outputs: ({0}): ", AvailableOutputDevices.Count());
            AvailableOutputDevices.ForEach(o => Logger.WriteLine("    " + o.Name));
            Logger.WriteLine();

        }

        public static async Task<bool> OpenInput(string inputDeviceName)
        {
            var inputInfo = AvailableInputDevices.SingleOrDefault(i => i.Name.ToLower() == inputDeviceName.ToLower());
            if (inputInfo == default) return false;
            ActiveInputDevices[inputDeviceName] = new MidiInputDevice(await AccessManager.OpenInputAsync(inputInfo.Id));
            Logger.WriteLine("Successfully opened input device {0}", inputDeviceName);
            return true;
        }

        public static async Task<bool> OpenOutput(string outputDeviceName)
        {
            var outputInfo = AvailableOutputDevices.SingleOrDefault(o => o.Name.ToLower() == outputDeviceName.ToLower());
            if (outputInfo == default) return false;
            ActiveOutputDevices[outputDeviceName] = new MidiOutputDevice(await AccessManager.OpenOutputAsync(outputInfo.Id));
            Logger.WriteLine("Successfully opened output device {0}", outputDeviceName);
            return true;
        }

        public static async Task<bool> EnsureInputReady(string inputDeviceName)
        {
            if (ActiveInputDevices.ContainsKey(inputDeviceName)) return true;
            var opened = await OpenInput(inputDeviceName);
            if (!opened && ActiveInputDevices.ContainsKey(inputDeviceName))
                ActiveInputDevices.Remove(inputDeviceName);
            return opened;
        }

        public static async Task<bool> EnsureOutputReady(string outputDeviceName)
        {
            if (ActiveOutputDevices.ContainsKey(outputDeviceName)) return true;
            var opened = await OpenOutput(outputDeviceName);
            if (!opened && ActiveOutputDevices.ContainsKey(outputDeviceName))
                ActiveOutputDevices.Remove(outputDeviceName);
            return opened;
        }

        public static void Close()
        {
            Logger.WriteLine("Closing Midi Devices");
            foreach (var input in ActiveInputDevices) input.Value.Close();
            foreach (var output in ActiveOutputDevices) output.Value.Close();
        }
    }
}
