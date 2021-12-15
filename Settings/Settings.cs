using System;
using System.Xml;
using System.Windows.Forms;

namespace LiveSplit.Sonic1Forever
{
    public partial class Sonic1ForeverSettings : UserControl
    {
        public bool RunStart { get; set; }
        public bool RunStartNGP { get; set; }
        public bool Reset { get; set; }
        public bool GH1 { get; set; }
        public bool GH2 { get; set; }
        public bool GH3 { get; set; }
        public bool MZ1 { get; set; }
        public bool MZ2 { get; set; }
        public bool MZ3 { get; set; }
        public bool SY1 { get; set; }
        public bool SY2 { get; set; }
        public bool SY3 { get; set; }
        public bool LZ1 { get; set; }
        public bool LZ2 { get; set; }
        public bool LZ3 { get; set; }
        public bool SL1 { get; set; }
        public bool SL2 { get; set; }
        public bool SL3 { get; set; }
        public bool SB1 { get; set; }
        public bool SB2 { get; set; }
        public bool SB3 { get; set; }
        public bool FZ { get; set; }

        // Event Handlers
        public event EventHandler OnbtnSetSplits_Click;
        void btnSetSplits_Click(object sender, EventArgs e) { this.OnbtnSetSplits_Click?.Invoke(this, EventArgs.Empty); }


        public Sonic1ForeverSettings()
        {
            InitializeComponent();

            // General settings
            this.chkRunStart.DataBindings.Add("Checked", this, "RunStart", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkNGPstart.DataBindings.Add("Checked", this, "RunStartNGP", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkReset.DataBindings.Add("Checked", this, "Reset", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkGH1.DataBindings.Add("Checked", this, "GH1", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkGH2.DataBindings.Add("Checked", this, "GH2", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkGH3.DataBindings.Add("Checked", this, "GH3", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkMZ1.DataBindings.Add("Checked", this, "MZ1", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkMZ2.DataBindings.Add("Checked", this, "MZ2", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkMZ3.DataBindings.Add("Checked", this, "MZ3", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSY1.DataBindings.Add("Checked", this, "SY1", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSY2.DataBindings.Add("Checked", this, "SY2", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSY3.DataBindings.Add("Checked", this, "SY3", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkLZ1.DataBindings.Add("Checked", this, "LZ1", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkLZ2.DataBindings.Add("Checked", this, "LZ2", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkLZ3.DataBindings.Add("Checked", this, "LZ3", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSL1.DataBindings.Add("Checked", this, "SL1", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSL2.DataBindings.Add("Checked", this, "SL2", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSL3.DataBindings.Add("Checked", this, "SL3", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSB1.DataBindings.Add("Checked", this, "SB1", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSB2.DataBindings.Add("Checked", this, "SB2", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSB3.DataBindings.Add("Checked", this, "SB3", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkFZ.DataBindings.Add("Checked", this, "FZ", false, DataSourceUpdateMode.OnPropertyChanged);

            // Default Values
            this.RunStart = true;
            this.RunStartNGP = true;
            this.Reset = true;
            GH1 = GH2 = GH3 = MZ1 = MZ2 = MZ3 = SY1 = SY2 = SY3 = LZ1 = LZ2 = LZ3 = SL1 = SL2 = SL3 = SB1 = SB2 = SB3 = FZ = true;
        }

        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settingsNode = doc.CreateElement("settings");
            settingsNode.AppendChild(ToElement(doc, "RunStart", this.RunStart));
            settingsNode.AppendChild(ToElement(doc, "RunStartNGP", this.RunStartNGP));
            settingsNode.AppendChild(ToElement(doc, "Reset", this.Reset));
            settingsNode.AppendChild(ToElement(doc, "GH1", this.GH1));
            settingsNode.AppendChild(ToElement(doc, "GH2", this.GH2));
            settingsNode.AppendChild(ToElement(doc, "GH3", this.GH3));
            settingsNode.AppendChild(ToElement(doc, "MZ1", this.MZ1));
            settingsNode.AppendChild(ToElement(doc, "MZ2", this.MZ2));
            settingsNode.AppendChild(ToElement(doc, "MZ3", this.MZ3));
            settingsNode.AppendChild(ToElement(doc, "SY1", this.SY1));
            settingsNode.AppendChild(ToElement(doc, "SY2", this.SY2));
            settingsNode.AppendChild(ToElement(doc, "SY3", this.SY3));
            settingsNode.AppendChild(ToElement(doc, "LZ1", this.LZ1));
            settingsNode.AppendChild(ToElement(doc, "LZ2", this.LZ2));
            settingsNode.AppendChild(ToElement(doc, "LZ3", this.LZ3));
            settingsNode.AppendChild(ToElement(doc, "SL1", this.SL1));
            settingsNode.AppendChild(ToElement(doc, "SL2", this.SL2));
            settingsNode.AppendChild(ToElement(doc, "SL3", this.SL3));
            settingsNode.AppendChild(ToElement(doc, "SB1", this.SB1));
            settingsNode.AppendChild(ToElement(doc, "SB2", this.SB2));
            settingsNode.AppendChild(ToElement(doc, "SB3", this.SB3));
            settingsNode.AppendChild(ToElement(doc, "FZ", this.FZ));
            return settingsNode;
        }

        public void SetSettings(XmlNode settings)
        {
            this.RunStart = ParseBool(settings, "RunStart", true);
            this.RunStartNGP = ParseBool(settings, "RunStartNGP", true);
            this.Reset = ParseBool(settings, "Reset", true);
            this.GH1 = ParseBool(settings, "GH1", true);
            this.GH2 = ParseBool(settings, "GH2", true);
            this.GH3 = ParseBool(settings, "GH3", true);
            this.MZ1 = ParseBool(settings, "MZ1", true);
            this.MZ2 = ParseBool(settings, "MZ2", true);
            this.MZ3 = ParseBool(settings, "MZ3", true);
            this.SY1 = ParseBool(settings, "SY1", true);
            this.SY2 = ParseBool(settings, "SY2", true);
            this.SY3 = ParseBool(settings, "SY3", true);
            this.LZ1 = ParseBool(settings, "LZ1", true);
            this.LZ2 = ParseBool(settings, "LZ2", true);
            this.LZ3 = ParseBool(settings, "LZ3", true);
            this.SY1 = ParseBool(settings, "SY1", true);
            this.SY2 = ParseBool(settings, "SY2", true);
            this.SY3 = ParseBool(settings, "SY3", true);
            this.SB1 = ParseBool(settings, "SB1", true);
            this.SB2 = ParseBool(settings, "SB2", true);
            this.SB3 = ParseBool(settings, "SB3", true);
            this.FZ = ParseBool(settings, "FZ", true);
        }

        static bool ParseBool(XmlNode settings, string setting, bool default_ = false)
        {
            bool val;
            return settings[setting] != null ? (Boolean.TryParse(settings[setting].InnerText, out val) ? val : default_) : default_;
        }

        static XmlElement ToElement<T>(XmlDocument document, string name, T value)
        {
            XmlElement str = document.CreateElement(name);
            str.InnerText = value.ToString();
            return str;
        }
    }
}
