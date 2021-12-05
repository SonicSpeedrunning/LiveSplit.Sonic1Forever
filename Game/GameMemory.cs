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

            var scanner = new SignatureScanner(game, game.MainModuleWow64Safe().BaseAddress, game.MainModuleWow64Safe().ModuleMemorySize);
            IntPtr ptr;

            switch (game.Is64Bit())
            {
                case false:
                    switch (game.MainModuleWow64Safe().ModuleMemorySize)
                    {
                        default:  // For 1.3.4 and over
                            ptr = scanner.Scan(new SigScanTarget(2,
                                "03 05 ????????",         // add eax,[SonicForever.exe+BF4E6C]
                                "69 C8 C1000000"));       // imul ecx,eax,000000C1
                            if (ptr == IntPtr.Zero) throw new Exception("Couldn't find address for the following variables: LevelID, ZoneIndicator");
                            this.LevelID = new MemoryWatcher<byte>(new DeepPointer((IntPtr)game.ReadValue<int>(ptr)));
                            this.ZoneIndicator = new MemoryWatcher<uint>(new DeepPointer((IntPtr)game.ReadValue<int>(ptr) + 4));

                            ptr = scanner.Scan(new SigScanTarget(2,
                                "8B 80 ????????",      // mov eax,[eax+SonicForever.exe+90FAAC]
                                "89 04 95 ????????",   // mov [edx*4+SonicForever.exe+1234F00],eax
                                "E9 070E0000"));       // jmp SonicForever.exe+2AAA6
                            if (ptr == IntPtr.Zero) throw new Exception("Couldn't find address for the following variable: State");
                            this.State = new MemoryWatcher<byte>(new DeepPointer((IntPtr)game.ReadValue<int>(ptr) + 0x9D8));
                            break;

                        case 0x362B000: // 1.2.1 32bit and below
                            this.LevelID = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x5C7FEC));
                            this.ZoneIndicator = new MemoryWatcher<uint>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x7A2970));
                            this.State = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x48867C));
                            break;
                    }
                    break;

                case true:
                    switch (game.MainModuleWow64Safe().ModuleMemorySize)
                    {
                        default: // For 1.3.4 and over
                            ptr = scanner.Scan(new SigScanTarget(3,
                                "48 63 05 ????????",   // movsxd rax,dword ptr [SonicForever.exe+7C6C74]  <----
                                "48 C1 E1 08"));       // shl rcx,08
                            if (ptr == IntPtr.Zero) throw new Exception("Couldn't find address for the following variables: LevelID");
                            this.LevelID = new MemoryWatcher<byte>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr)));

                            ptr = scanner.Scan(new SigScanTarget(2,
                                "8B 0D ????????",     // mov ecx,[SonicForever.exe+5EC0A8]  <----
                                "74 59"));            // je SonicForever.exe+30648
                            if (ptr == IntPtr.Zero) throw new Exception("Couldn't find address for the following variables: ZoneIndicator");
                            this.ZoneIndicator = new MemoryWatcher<uint>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr) + 0x8));

                            ptr = scanner.Scan(new SigScanTarget(3,
                                "4C 8D 25 ????????",  // lea r12,[SonicForever.exe+37C730]   <----
                                "44 8B 35 ????????",  // mov r14d,[SonicForever.exe+E07A94]
                                "33 DB"));            // xor ebx,ebx
                            if (ptr == IntPtr.Zero) throw new Exception("Could not find address!");
                            this.State = new MemoryWatcher<byte>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr) + 0x9D8 + 0x14));
                            break;

                        case 0x364B000:   // 1.2.1 64bit and below
                            this.LevelID = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x79E04C));
                            this.ZoneIndicator = new MemoryWatcher<uint>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x5E5C90));
                            this.State = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x376CFC));
                            break;
                    }
                    break;
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
