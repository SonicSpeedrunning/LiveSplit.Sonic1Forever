using LiveSplit.ComponentUtil;
using System.Diagnostics;

namespace LiveSplit.Sonic1Forever
{
    partial class Watchers
    {
        private GameVersion GetGameVersion(Process process)
        {
            // Hacky way, for now, because I'm lazy

            if (process.Is64Bit())
                return GameVersion.Below1_5_0;
            else
            {
                if (process.MainModuleWow64Safe().ModuleMemorySize < (int)GameVersionSize.v_1_5_0_32)
                    return GameVersion.Below1_5_0;
                else return GameVersion.V1_5_0_or_higher;
            }
        }
    }

    enum GameVersion : int
    {
        Below1_5_0,
        V1_5_0_or_higher,
    }

    enum GameVersionSize : int
    {
        v_1_5_0_32 = 0x57F4000,
        v_1_3_4_32 = 0x3623000,
        v_1_2_1_32 = 0x362B000,
    }
}
