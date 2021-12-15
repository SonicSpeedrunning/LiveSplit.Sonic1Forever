using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using LiveSplit.ComponentUtil;

namespace LiveSplit.Sonic1Forever
{
    class Watchers : MemoryWatcherList
    {
        // Game process
        private readonly Process game;
        public bool IsGameHooked => !(game == null || game.HasExited);

        // Imported game data
        private MemoryWatcher<byte> LevelID { get; }
        private MemoryWatcher<uint> ZoneIndicator { get; }
        private MemoryWatcher<byte> State { get; }

        // Fake MemoryWatchers: used to convert game data into more easily readable formats
        public FakeMemoryWatcher<Acts> Act = new FakeMemoryWatcher<Acts>(Acts.GreenHillAct1, Acts.GreenHillAct1);
        public FakeMemoryWatcher<bool> RunStartedSaveFile = new FakeMemoryWatcher<bool>(false, false);
        public FakeMemoryWatcher<bool> RunStartedNoSaveFile = new FakeMemoryWatcher<bool>(false, false);
        public FakeMemoryWatcher<bool> StartingNewGame = new FakeMemoryWatcher<bool>(false, false);
        public FakeMemoryWatcher<bool> RunStartedNGP = new FakeMemoryWatcher<bool>(false, false);

        public Watchers()
        {
            game = Process.GetProcessesByName("SonicForever").OrderByDescending(x => x.StartTime).FirstOrDefault(x => !x.HasExited);
            if (game == null) throw new Exception("Couldn't connect to the game!");
            IntPtr baseAddress = game.MainModuleWow64Safe().BaseAddress;

            switch ((GameVersion)game.MainModuleWow64Safe().ModuleMemorySize)
            {
                case GameVersion.x86_1_2_1_and_below:
                    this.LevelID = new MemoryWatcher<byte>(new DeepPointer(baseAddress + 0x5C7FEC));
                    this.ZoneIndicator = new MemoryWatcher<uint>(new DeepPointer(baseAddress + 0x7A2970));
                    this.State = new MemoryWatcher<byte>(new DeepPointer(baseAddress + 0x48867C));
                    break;

                case GameVersion.x86_1_3_4_and_over:
                    this.LevelID = new MemoryWatcher<byte>(new DeepPointer(baseAddress + 0x7A2D7C));
                    this.ZoneIndicator = new MemoryWatcher<uint>(new DeepPointer(baseAddress + 0x7A2D80));
                    this.State = new MemoryWatcher<byte>(new DeepPointer(baseAddress + 0x50F294));
                    break;

                case GameVersion.x86_1_4_0_and_over:
                    this.LevelID = new MemoryWatcher<byte>(new DeepPointer(baseAddress + 0x15F4454));
                    this.ZoneIndicator = new MemoryWatcher<uint>(new DeepPointer(baseAddress + 0x15F4458));
                    this.State = new MemoryWatcher<byte>(new DeepPointer(baseAddress + 0x130FA6C));
                    break;

                case GameVersion.x64_1_2_1_and_below:
                    this.LevelID = new MemoryWatcher<byte>(new DeepPointer(baseAddress + 0x79E04C));
                    this.ZoneIndicator = new MemoryWatcher<uint>(new DeepPointer(baseAddress + 0x5E5C90));
                    this.State = new MemoryWatcher<byte>(new DeepPointer(baseAddress + 0x376CFC));
                    break;

                case GameVersion.x64_1_3_4_and_over:
                    this.LevelID = new MemoryWatcher<byte>(new DeepPointer(baseAddress + 0x7C6C74));
                    this.ZoneIndicator = new MemoryWatcher<uint>(new DeepPointer(baseAddress + 0x5EC0B0));
                    this.State = new MemoryWatcher<byte>(new DeepPointer(baseAddress + 0x37D11C));
                    break;

                default: throw new Exception("Unsupported game version");
            }

            this.AddRange(this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(p => !p.GetIndexParameters().Any()).Select(p => p.GetValue(this, null) as MemoryWatcher).Where(p => p != null));
        }

        public void Update()
        {
            this.UpdateAll(game);

            this.RunStartedSaveFile.Old = this.RunStartedSaveFile.Current;
            this.RunStartedSaveFile.Current = this.State.Changed && this.State.Current == 2;

            this.RunStartedNoSaveFile.Old = this.RunStartedNoSaveFile.Current;
            this.RunStartedNoSaveFile.Current = this.State.Old == 6 && this.State.Current == 7;

            this.RunStartedNGP.Old = this.RunStartedNGP.Current;
            this.RunStartedNGP.Current = this.State.Old == 8 && this.State.Current == 9;

            this.StartingNewGame.Old = this.StartingNewGame.Current;
            this.StartingNewGame.Current = this.State.Current == 201 && this.State.Old == 200 && (ZoneIndicator)this.ZoneIndicator.Current == Sonic1Forever.ZoneIndicator.SaveSelect;

            this.Act.Old = this.Act.Current;
            this.Act.Current = (ZoneIndicator)this.ZoneIndicator.Current == Sonic1Forever.ZoneIndicator.Ending ? Acts.Ending : (ZoneIndicator)ZoneIndicator.Current == Sonic1Forever.ZoneIndicator.Zones ? (Acts)LevelID.Current : this.Act.Old;
        }
    }

    class FakeMemoryWatcher<T>
    {
        public T Current { get; set; }
        public T Old { get; set; }
        public bool Changed => !this.Old.Equals(this.Current);
        public FakeMemoryWatcher(T old, T current)
        {
            this.Old = old;
            this.Current = current;
        }
    }
}
