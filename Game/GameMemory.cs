using System;
using System.Linq;
using System.Reflection;
using LiveSplit.ComponentUtil;

namespace LiveSplit.Sonic1Forever
{
    partial class Watchers
    {
        // Game version
        public GameVersion GameVersion { get; private set; }

        // Watchers
        private MemoryWatcher<byte> LevelID { get; set; }
        public MemoryWatcher<byte> State { get; private set; }
        public MemoryWatcher<byte> ZoneSelectOnGameComplete { get; private set; }
        public MemoryWatcher<ZoneIndicator> ZoneIndicator { get; private set; }

        // Fake watchers
        public FakeMemoryWatcher<Acts> Act { get; private set; }


        public Watchers()
        {
            Act = new FakeMemoryWatcher<Acts>(() => ZoneIndicator.Current == Sonic1Forever.ZoneIndicator.Ending ? (Acts)(-1) : ZoneIndicator.Current == Sonic1Forever.ZoneIndicator.Zones ? (Acts)LevelID.Current : Act.Old);
            GameProcess = new ProcessHook("SonicForever");
        }

        public void Update()
        {
            WatcherList.UpdateAll(game);
            Act.Update();
        }

        /// <summary>
        /// This function is essentially equivalent of the init descriptor in script-based autosplitters.
        /// Everything you want to be executed when the game gets hooked needs to be put here.
        /// The main purpose of this function is to perform sigscanning and get memory addresses and offsets
        /// needed by the autosplitter.
        /// </summary>
        private void GetAddresses()
        {
            GameVersion = GetGameVersion(game);
            var Scanner = game.SigScanner();

            IntPtr ptr = IntPtr.Zero, lea = IntPtr.Zero;
            Func<int, int, int, bool, IntPtr> pointerPath = (int offset1, int offset2, int offset3, bool absolute) =>
            {
                switch (game.Is64Bit())
                {
                    case true:
                        if (offset1 == 0) return lea + offset3;
                        int tempOffset = game.ReadValue<int>(ptr + offset1);
                        IntPtr tempOffset2 = game.MainModuleWow64Safe().BaseAddress + tempOffset + offset2;
                        if (absolute) return game.MainModuleWow64Safe().BaseAddress + game.ReadValue<int>(tempOffset2) + offset3;
                        else return tempOffset2 + 0x4 + game.ReadValue<int>(tempOffset2) + offset3;
                    default:
                        return (IntPtr)new DeepPointer(ptr + offset1, offset2).Deref<int>(game) + offset3;
                }
            };

            switch (GameVersion)
            {
                case GameVersion.Below1_5_0:
                    if (game.Is64Bit())
                    {
                        ptr = Scanner.ScanOrThrow(new SigScanTarget(16, "81 F9 ???????? 0F 87 ???????? 41 8B 8C") { OnFound = (p, s, addr) => game.MainModuleWow64Safe().BaseAddress + p.ReadValue<int>(addr) });
                        lea = Scanner.ScanOrThrow(new SigScanTarget(3, "48 8D 05 ???????? 49 63 F8 4C") { OnFound = (p, s, addr) => addr + 0x4 + p.ReadValue<int>(addr) });
                        State = new MemoryWatcher<byte>(pointerPath(0x4 * 0, 0, 0x9EC, false));
                        LevelID = new MemoryWatcher<byte>(pointerPath(0x4 * 123, 2, 0, false));
                        ptr = Scanner.ScanOrThrow(new SigScanTarget(2, "C6 05 ???????? ?? E9 ???????? 48 8D 0D ????????") { OnFound = (p, s, addr) => addr + 0x5 + p.ReadValue<int>(addr) });
                        ZoneIndicator = new MemoryWatcher<ZoneIndicator>(ptr);
                    } else {
                        ptr = Scanner.ScanOrThrow(new SigScanTarget(14, "3D ???????? 0F 87 ???????? FF 24 85 ???????? A1") { OnFound = (p, s, addr) => p.ReadPointer(addr) });
                        State = new MemoryWatcher<byte>(pointerPath(0x4 * 73, 8, 0x9D8, true));
                        LevelID = new MemoryWatcher<byte>(pointerPath(0x4 * 123, 1, 0, true));
                        ptr = Scanner.Scan(new SigScanTarget(7, "69 F8 ???????? B8 ????????") { OnFound = (p, s, addr) => p.ReadPointer(addr) });
                        ZoneIndicator = new MemoryWatcher<ZoneIndicator>(ptr);
                    }
                    break;

                case GameVersion.V1_5_0_or_higher:
                default:
                    if (game.Is64Bit())
                        throw new NotImplementedException();

                    ptr = Scanner.ScanOrThrow(new SigScanTarget(14, "3D ???????? 0F 87 ???????? FF 24 85 ???????? A1") { OnFound = (p, s, addr) => p.ReadPointer(addr) });
                    State = new MemoryWatcher<byte>(pointerPath(0x4 * 30, 8, 0x9D8, true));
                    LevelID = new MemoryWatcher<byte>(pointerPath(0x4 * 123, 1, 0, true));
                    ZoneSelectOnGameComplete = new MemoryWatcher<byte>(pointerPath(0x4 * 18, 3, 4, true));
                    ptr = Scanner.ScanOrThrow(new SigScanTarget(7, "69 F8 ???????? B8 ????????") { OnFound = (p, s, addr) => p.ReadPointer(addr) });
                    ZoneIndicator = new MemoryWatcher<ZoneIndicator>(ptr);
                    break;
            }

            WatcherList = new MemoryWatcherList();
            
            WatcherList
                .AddRange(GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(p => !p.GetIndexParameters().Any())
                .Select(p => p.GetValue(this, null) as MemoryWatcher)
                .Where(p => p != null));
        }
    }
}