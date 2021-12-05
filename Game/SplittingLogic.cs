using System;
using System.Threading;

namespace LiveSplit.Sonic1Forever
{
    class SplittingLogic
    {
        private Watchers watchers;

        public event EventHandler<StartTrigger> OnStartTrigger;
        public event EventHandler<Acts> OnSplitTrigger;
        public event EventHandler OnResetTrigger;

        public void Update()
        {
            if (!VerifyOrHookGameProcess()) return;
            watchers.Update();
            Start();
            ResetLogic();
            Split();
        }

        void Start()
        {
            if (FlippedBool(watchers.RunStartedSaveFile) || FlippedBool(watchers.RunStartedNoSaveFile))
                this.OnStartTrigger?.Invoke(this, StartTrigger.NewGame);
            else if (FlippedBool(watchers.RunStartedNGP))
                this.OnStartTrigger?.Invoke(this, StartTrigger.NewGamePlus);
        }

        void ResetLogic()
        {
            if (FlippedBool(watchers.StartingNewGame)) this.OnResetTrigger?.Invoke(this, EventArgs.Empty);
        }

        void Split()
        {
            if (watchers.Act.Changed && watchers.Act.Current == Acts.Ending) { this.OnSplitTrigger?.Invoke(this, Acts.FinalZone); }
            else if (watchers.Act.Old == watchers.Act.Current - 1) { this.OnSplitTrigger?.Invoke(this, watchers.Act.Old); }
        }

        bool VerifyOrHookGameProcess()
        {
            if (watchers != null && watchers.IsGameHooked) return true;
            try { watchers = new Watchers(); } catch { Thread.Sleep(500); return false; }
            return true;
        }

        private bool FlippedBool(FakeMemoryWatcher<bool> boolean)
        {
            return boolean.Current && !boolean.Old;
        }
    }
}
