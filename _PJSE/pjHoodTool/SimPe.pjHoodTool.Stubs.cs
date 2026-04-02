using Avalonia.Controls;
using SimPe.Scenegraph.Compat;
using CheckBox   = SimPe.Scenegraph.Compat.CheckBoxCompat2;
using Label      = SimPe.Scenegraph.Compat.LabelCompat;
using Panel      = SimPe.Scenegraph.Compat.PanelCompat;
using Button     = SimPe.Scenegraph.Compat.ButtonCompat;
using RadioButton = Avalonia.Controls.RadioButton;
using PictureBox = SimPe.Scenegraph.Compat.PictureBox;

namespace pjHoodTool
{
    partial class Settims
    {
        private System.ComponentModel.IContainer components = null;

        private Panel       background        = new Panel();
        private CheckBox    cbshowbasic       = new CheckBox();
        private CheckBox    cbshowuniversity  = new CheckBox();
        private CheckBox    cbshowskills      = new CheckBox();
        private CheckBox    cbshowcharacter   = new CheckBox();
        private CheckBox    cbshowinterests   = new CheckBox();
        private CheckBox    cbshowapartments  = new CheckBox();
        private CheckBox    cbshowfreetime    = new CheckBox();
        private CheckBox    cbshownpcs        = new CheckBox();
        private CheckBox    cbshowdesc        = new CheckBox();
        private CheckBox    cbshowpets        = new CheckBox();
        private CheckBox    cbExcludeLots     = new CheckBox();
        private Label       lbheader          = new Label();
        private Button      btdoned           = new Button();
        private RadioButton rbcsv             = new RadioButton();
        private RadioButton rbtext            = new RadioButton();
        private Label       lbtypy            = new Label();
        private CheckBox    cbshowbusi        = new CheckBox();
        private PictureBox  simLogo           = new PictureBox();

        private void InitializeComponent() { }
    }
}
