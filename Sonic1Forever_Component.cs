using System;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;

namespace LiveSplit.Sonic1Forever
{
    class Sonic1ForeverComponent : LogicComponent
    {
        public override string ComponentName => "Sonic 1 Forever - Autosplitter";
        private Sonic1ForeverSettings Settings { get; set; }
        private readonly TimerModel timer;
        private readonly System.Timers.Timer update_timer;
        private readonly SplittingLogic SplittingLogic;

        public Sonic1ForeverComponent(LiveSplitState state)
        {
            timer = new TimerModel { CurrentState = state };
            Settings = new Sonic1ForeverSettings();
            Settings.OnbtnSetSplits_Click += OnSetSplits;

            SplittingLogic = new SplittingLogic();
            SplittingLogic.OnStartTrigger += OnStartTrigger;
            SplittingLogic.OnSplitTrigger += OnSplitTrigger;
            SplittingLogic.OnResetTrigger += OnResetTrigger;

            update_timer = new System.Timers.Timer() { Interval = 15, Enabled = true, AutoReset = false };
            update_timer.Elapsed += delegate { SplittingLogic.Update(); update_timer.Start(); };
        }

        void OnStartTrigger(object sender, StartTrigger type)
        {
            if (timer.CurrentState.CurrentPhase != TimerPhase.NotRunning) return;
            switch (type)
            {
                case StartTrigger.NewGame: if (Settings.RunStart) timer.Start(); break;
                case StartTrigger.NewGamePlus: if (Settings.RunStartNGP) timer.Start(); break;
            }
        }

        void OnSplitTrigger(object sender, Acts type)
        {
            if (timer.CurrentState.CurrentPhase != TimerPhase.Running) return;
            switch (type)
            {
                case Acts.GreenHillAct1: if (Settings.GH1) timer.Split(); break;
                case Acts.GreenHillAct2: if (Settings.GH2) timer.Split(); break;
                case Acts.GreenHillAct3: if (Settings.GH3) timer.Split(); break;
                case Acts.MarbleAct1: if (Settings.MZ1) timer.Split(); break;
                case Acts.MarbleAct2: if (Settings.MZ2) timer.Split(); break;
                case Acts.MarbleAct3: if (Settings.MZ3) timer.Split(); break;
                case Acts.SpringYardAct1: if (Settings.SY1) timer.Split(); break;
                case Acts.SpringYardAct2: if (Settings.SY2) timer.Split(); break;
                case Acts.SpringYardAct3: if (Settings.SY3) timer.Split(); break;
                case Acts.LabyrinthAct1: if (Settings.LZ1) timer.Split(); break;
                case Acts.LabyrinthAct2: if (Settings.LZ2) timer.Split(); break;
                case Acts.LabyrinthAct3: if (Settings.LZ3) timer.Split(); break;
                case Acts.StarLightAct1: if (Settings.SL1) timer.Split(); break;
                case Acts.StarLightAct2: if (Settings.SL2) timer.Split(); break;
                case Acts.StarLightAct3: if (Settings.SL3) timer.Split(); break;
                case Acts.ScrapBrainAct1: if (Settings.SB1) timer.Split(); break;
                case Acts.ScrapBrainAct2: if (Settings.SB2) timer.Split(); break;
                case Acts.ScrapBrainAct3: if (Settings.SB3) timer.Split(); break;
                case Acts.FinalZone: if (Settings.FZ) timer.Split(); break;
            }
        }

        void OnResetTrigger(object sender, EventArgs e)
        {
            if (timer.CurrentState.CurrentPhase == TimerPhase.Running && Settings.Reset) timer.Reset();
        }

        public override void Dispose()
        {
            Settings.Dispose();
            update_timer?.Dispose();
        }

        public void OnSetSplits(object sender, EventArgs e)
        {
            var question = MessageBox.Show("This will set up your splits according to your selected autosplitting options.\n" +
                                            "WARNING: Any existing PB recorded for the current layout will be deleted.\n\n" +
                                            "Do you want to continue?", "LiveSplit - Sonic 1 Forever", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (question == DialogResult.No) return;
            timer.CurrentState.Run.Clear();
            if (Settings.GH1) timer.CurrentState.Run.AddSegment("Green Hill - Act 1");
            if (Settings.GH2) timer.CurrentState.Run.AddSegment("Green Hill - Act 2");
            if (Settings.GH3) timer.CurrentState.Run.AddSegment("Green Hill - Act 3");
            if (Settings.MZ1) timer.CurrentState.Run.AddSegment("Marble Zone - Act 1");
            if (Settings.MZ2) timer.CurrentState.Run.AddSegment("Marble Zone - Act 2");
            if (Settings.MZ3) timer.CurrentState.Run.AddSegment("Marble Zone - Act 3");
            if (Settings.SY1) timer.CurrentState.Run.AddSegment("Spring Yard - Act 1");
            if (Settings.SY2) timer.CurrentState.Run.AddSegment("Spring Yard - Act 2");
            if (Settings.SY3) timer.CurrentState.Run.AddSegment("Spring Yard - Act 3");
            if (Settings.LZ1) timer.CurrentState.Run.AddSegment("Labyrinth - Act 1");
            if (Settings.LZ2) timer.CurrentState.Run.AddSegment("Labyrinth - Act 2");
            if (Settings.LZ3) timer.CurrentState.Run.AddSegment("Labyrinth - Act 3");
            if (Settings.SL1) timer.CurrentState.Run.AddSegment("Star Light - Act 1");
            if (Settings.SL2) timer.CurrentState.Run.AddSegment("Star Light - Act 2");
            if (Settings.SL3) timer.CurrentState.Run.AddSegment("Star Light - Act 3");
            if (Settings.SB1) timer.CurrentState.Run.AddSegment("Scrap Brain - Act 1");
            if (Settings.SB2) timer.CurrentState.Run.AddSegment("Scrap Brain - Act 2");
            if (Settings.SB3) timer.CurrentState.Run.AddSegment("Scrap Brain - Act 3");
            if (Settings.FZ) timer.CurrentState.Run.AddSegment("Final Zone");
            if (timer.CurrentState.Run.Count == 0) timer.CurrentState.Run.AddSegment("");
        }

        public override XmlNode GetSettings(XmlDocument document) { return this.Settings.GetSettings(document); }

        public override Control GetSettingsControl(LayoutMode mode) { return this.Settings; }

        public override void SetSettings(XmlNode settings) { this.Settings.SetSettings(settings); }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) { }
    }
}
