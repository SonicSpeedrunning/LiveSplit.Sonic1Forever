using System.Xml;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

namespace LiveSplit.Sonic1Forever
{
    public partial class Settings : UserControl
    {
        // General
        public bool StartCleanSave { get; set; }
        public bool StartNewGamePlus { get; set; }
        public bool Reset { get; set; }
        public bool c1 { get; set; }
        public bool c2 { get; set; }
        public bool c3 { get; set; }
        public bool c4 { get; set; }
        public bool c5 { get; set; }
        public bool c6 { get; set; }
        public bool c7 { get; set; }
        public bool c8 { get; set; }
        public bool c9 { get; set; }
        public bool c10 { get; set; }
        public bool c11 { get; set; }
        public bool c12 { get; set; }
        public bool c13 { get; set; }
        public bool c14 { get; set; }
        public bool c15 { get; set; }
        public bool c16 { get; set; }
        public bool c17 { get; set; }
        public bool c18 { get; set; }
        public bool c19 { get; set; }


        public Settings()
        {
            InitializeComponent();
            autosplitterVersion.Text = "Autosplitter version: v" + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

            // General settings
            chkStartCleanSave.DataBindings.Add("Checked", this, "StartCleanSave", false, DataSourceUpdateMode.OnPropertyChanged);
            chkStartNewGamePlus.DataBindings.Add("Checked", this, "StartNewGamePlus", false, DataSourceUpdateMode.OnPropertyChanged);
            chkReset.DataBindings.Add("Checked", this, "Reset", false, DataSourceUpdateMode.OnPropertyChanged);
            chk1.DataBindings.Add("Checked", this, "c1", false, DataSourceUpdateMode.OnPropertyChanged);
            chk2.DataBindings.Add("Checked", this, "c2", false, DataSourceUpdateMode.OnPropertyChanged);
            chk3.DataBindings.Add("Checked", this, "c3", false, DataSourceUpdateMode.OnPropertyChanged);
            chk4.DataBindings.Add("Checked", this, "c4", false, DataSourceUpdateMode.OnPropertyChanged);
            chk5.DataBindings.Add("Checked", this, "c5", false, DataSourceUpdateMode.OnPropertyChanged);
            chk6.DataBindings.Add("Checked", this, "c6", false, DataSourceUpdateMode.OnPropertyChanged);
            chk7.DataBindings.Add("Checked", this, "c7", false, DataSourceUpdateMode.OnPropertyChanged);
            chk8.DataBindings.Add("Checked", this, "c8", false, DataSourceUpdateMode.OnPropertyChanged);
            chk9.DataBindings.Add("Checked", this, "c9", false, DataSourceUpdateMode.OnPropertyChanged);
            chk10.DataBindings.Add("Checked", this, "c10", false, DataSourceUpdateMode.OnPropertyChanged);
            chk11.DataBindings.Add("Checked", this, "c11", false, DataSourceUpdateMode.OnPropertyChanged);
            chk12.DataBindings.Add("Checked", this, "c12", false, DataSourceUpdateMode.OnPropertyChanged);
            chk13.DataBindings.Add("Checked", this, "c13", false, DataSourceUpdateMode.OnPropertyChanged);
            chk14.DataBindings.Add("Checked", this, "c14", false, DataSourceUpdateMode.OnPropertyChanged);
            chk15.DataBindings.Add("Checked", this, "c15", false, DataSourceUpdateMode.OnPropertyChanged);
            chk16.DataBindings.Add("Checked", this, "c16", false, DataSourceUpdateMode.OnPropertyChanged);
            chk17.DataBindings.Add("Checked", this, "c17", false, DataSourceUpdateMode.OnPropertyChanged);
            chk18.DataBindings.Add("Checked", this, "c18", false, DataSourceUpdateMode.OnPropertyChanged);
            chk19.DataBindings.Add("Checked", this, "c19", false, DataSourceUpdateMode.OnPropertyChanged);

            // Default Values
            StartCleanSave = StartNewGamePlus = Reset = true;
            c1 = c2 = c3 = c4 = c5 = c6 = c7 = c8 = c9 = c10 = c11 = c12 = c13 = c14 = c15 = c16 = c17 = c18 = c19 = true;
        }

        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settingsNode = doc.CreateElement("Settings");
            settingsNode.AppendChild(ToElement(doc, "StartCleanSave", StartCleanSave));
            settingsNode.AppendChild(ToElement(doc, "StartNewGamePlus", StartNewGamePlus));
            settingsNode.AppendChild(ToElement(doc, "Reset", Reset));
            settingsNode.AppendChild(ToElement(doc, "c1", c1));
            settingsNode.AppendChild(ToElement(doc, "c2", c2));
            settingsNode.AppendChild(ToElement(doc, "c3", c3));
            settingsNode.AppendChild(ToElement(doc, "c4", c4));
            settingsNode.AppendChild(ToElement(doc, "c5", c5));
            settingsNode.AppendChild(ToElement(doc, "c6", c6));
            settingsNode.AppendChild(ToElement(doc, "c7", c7));
            settingsNode.AppendChild(ToElement(doc, "c8", c8));
            settingsNode.AppendChild(ToElement(doc, "c9", c9));
            settingsNode.AppendChild(ToElement(doc, "c10", c10));
            settingsNode.AppendChild(ToElement(doc, "c11", c11));
            settingsNode.AppendChild(ToElement(doc, "c12", c12));
            settingsNode.AppendChild(ToElement(doc, "c13", c13));
            settingsNode.AppendChild(ToElement(doc, "c14", c14));
            settingsNode.AppendChild(ToElement(doc, "c15", c15));
            settingsNode.AppendChild(ToElement(doc, "c16", c16));
            settingsNode.AppendChild(ToElement(doc, "c17", c17));
            settingsNode.AppendChild(ToElement(doc, "c18", c18));
            settingsNode.AppendChild(ToElement(doc, "c19", c19));
            return settingsNode;
        }

        public void SetSettings(XmlNode settings)
        {
            StartCleanSave = ParseBool(settings, "StartCleanSave", true);
            StartNewGamePlus = ParseBool(settings, "StartNewGamePlus", true);
            Reset = ParseBool(settings, "Reset", true);
            c1 = ParseBool(settings, "c1", true);
            c2 = ParseBool(settings, "c2", true);
            c3 = ParseBool(settings, "c3", true);
            c4 = ParseBool(settings, "c4", true);
            c5 = ParseBool(settings, "c5", true);
            c6 = ParseBool(settings, "c6", true);
            c7 = ParseBool(settings, "c7", true);
            c8 = ParseBool(settings, "c8", true);
            c9 = ParseBool(settings, "c9", true);
            c10 = ParseBool(settings, "c10", true);
            c11 = ParseBool(settings, "c11", true);
            c12 = ParseBool(settings, "c12", true);
            c13 = ParseBool(settings, "c13", true);
            c14 = ParseBool(settings, "c14", true);
            c15 = ParseBool(settings, "c15", true);
            c15 = ParseBool(settings, "c16", true);
            c15 = ParseBool(settings, "c17", true);
            c15 = ParseBool(settings, "c18", true);
            c15 = ParseBool(settings, "c19", true);
        }

        static bool ParseBool(XmlNode settings, string setting, bool default_ = false)
        {
            return settings[setting] != null ? (bool.TryParse(settings[setting].InnerText, out bool val) ? val : default_) : default_;
        }

        static XmlElement ToElement<T>(XmlDocument document, string name, T value)
        {
            XmlElement str = document.CreateElement(name);
            str.InnerText = value.ToString();
            return str;
        }

        public bool this[string entry]
        {
            get
            {
                bool t;
                try { t = (bool)this.GetType().GetProperty(entry).GetValue(this, null); }
                catch { return false; }
                return t;
            }
        }
    }
}
