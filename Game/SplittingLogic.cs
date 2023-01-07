using System;

namespace LiveSplit.Sonic1Forever
{
    partial class Sonic1ForeverComponent
    {
        private bool Start()
        {
            bool RunStartedSaveFile, RunStartedNoSaveFile, RunStartedNGP;

            switch (watchers.GameVersion)
            {
                case GameVersion.Below1_5_0:
                    RunStartedSaveFile = watchers.State.Changed && watchers.State.Current == 2 && watchers.ZoneIndicator.Current == ZoneIndicator.SaveSelect;
                    RunStartedNoSaveFile = watchers.State.Old == 6 && watchers.State.Current == 7;
                    RunStartedNGP = watchers.State.Old == 8 && watchers.State.Current == 9;
                    return (settings.StartCleanSave && (RunStartedSaveFile || RunStartedNoSaveFile)) || (settings.StartNewGamePlus && RunStartedNGP);

                case GameVersion.V1_5_0_or_higher:
                default:
                    RunStartedSaveFile = watchers.State.Old == 3 && watchers.State.Current == 7 && watchers.ZoneIndicator.Current == ZoneIndicator.SaveSelect;
                    RunStartedNoSaveFile = watchers.State.Old == 10 && watchers.State.Current == 11;
                    RunStartedNGP = watchers.State.Old == 2 && watchers.State.Current == 6 && watchers.ZoneSelectOnGameComplete.Current == 0;
                    return (settings.StartCleanSave && (RunStartedSaveFile || RunStartedNoSaveFile)) || (settings.StartNewGamePlus && RunStartedNGP);
            }
        }

        private bool Split()
        {
            if (watchers.Act.Current == watchers.Act.Old + 1)
                return settings["c" + ((int)watchers.Act.Old + 1).ToString()];
            else if (watchers.Act.Current == (Acts)(-1) && watchers.Act.Current != watchers.Act.Old)
                return settings.c19;
            else return false;
        }

        bool Reset()
        {
            if (!settings.Reset)
                return false;

            switch (watchers.GameVersion)
            {
                case GameVersion.Below1_5_0:
                    return watchers.State.Current == 201 && watchers.State.Old == 200 && watchers.ZoneIndicator.Current == ZoneIndicator.SaveSelect;
                case GameVersion.V1_5_0_or_higher:
                default:
                    return watchers.State.Current == 14 && watchers.State.Old == 13 && watchers.ZoneIndicator.Current == ZoneIndicator.SaveSelect;
            }
        }

        bool IsLoading()
        {
            return false;
        }

        private TimeSpan? GameTime()
        {
            return null;
        }
    }
}