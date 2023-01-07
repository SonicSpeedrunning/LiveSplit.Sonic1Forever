using System.Xml;
using System.Windows.Forms;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;

namespace LiveSplit.Sonic1Forever
{
    partial class Sonic1ForeverComponent : LogicComponent
    {
        public override string ComponentName => "Sonic 1 Forever - Autosplitter";
        private readonly Settings settings = new();
        private readonly Watchers watchers = new();
        private readonly TimerModel timer;

        public Sonic1ForeverComponent(LiveSplitState state)
        {
            timer = new TimerModel { CurrentState = state };
        }

        public override void Dispose()
        {
            settings.Dispose();
            watchers.Dispose();
        }

        public override XmlNode GetSettings(XmlDocument document) => this.settings.GetSettings(document);

        public override Control GetSettingsControl(LayoutMode mode) => this.settings;

        public override void SetSettings(XmlNode settings) => this.settings.SetSettings(settings);

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            // If LiveSplit is not connected to the game, of course there's no point in going further
            if (!watchers.Init()) return;

            // Main update logic is inside the watcher class in order to avoid exposing unneded stuff to the outside
            watchers.Update();

            if (timer.CurrentState.CurrentPhase == TimerPhase.Running || timer.CurrentState.CurrentPhase == TimerPhase.Paused)
            {
                timer.CurrentState.IsGameTimePaused = IsLoading();
                if (GameTime() != null) timer.CurrentState.SetGameTime(GameTime());
                if (Reset()) timer.Reset();
                else if (Split()) timer.Split();
            }

            if (timer.CurrentState.CurrentPhase == TimerPhase.NotRunning)
            {
                if (Start()) timer.Start();
            }
        }
    }
}
