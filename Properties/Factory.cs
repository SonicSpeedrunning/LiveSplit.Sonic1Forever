using System;
using System.Reflection;
using LiveSplit.Model;
using LiveSplit.UI.Components;
using LiveSplit.Sonic1Forever;

[assembly: ComponentFactory(typeof(Sonic3Din2DFactory))]

namespace LiveSplit.Sonic1Forever
{
    public class Sonic3Din2DFactory : IComponentFactory
    {
        public string ComponentName => "Sonic 1 Forever - Autosplitter";
        public string Description => "Autosplitter";
        public ComponentCategory Category => ComponentCategory.Control;
        public string UpdateName => this.ComponentName;
        public string UpdateURL => "https://raw.githubusercontent.com/SonicSpeedrunning/LiveSplit.Sonic1Forever/master/";
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public string XMLURL => this.UpdateURL + "Components/LiveSplit.Sonic1Forever.xml";
        public IComponent Create(LiveSplitState state) => new Sonic1ForeverComponent(state);
    }
}
