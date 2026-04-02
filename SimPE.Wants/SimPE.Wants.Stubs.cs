using Avalonia.Controls;
using SimPe.Scenegraph.Compat;
using Button          = SimPe.Scenegraph.Compat.ButtonCompat;
using CheckBox        = SimPe.Scenegraph.Compat.CheckBoxCompat2;
using ComboBox        = SimPe.Scenegraph.Compat.ComboBoxCompat;
using TextBox         = SimPe.Scenegraph.Compat.TextBoxCompat;
using Label           = SimPe.Scenegraph.Compat.LabelCompat;
using Panel           = SimPe.Scenegraph.Compat.PanelCompat;
using ListView        = SimPe.Scenegraph.Compat.ListView;
using ColumnHeader    = SimPe.Scenegraph.Compat.ColumnHeader;
using FlowLayoutPanel = SimPe.Scenegraph.Compat.FlowLayoutPanel;
using LinkLabel       = SimPe.Scenegraph.Compat.LinkLabel;
using SplitContainer  = SimPe.Scenegraph.Compat.SplitContainer;

namespace SimPe.Wants
{
    partial class SWAFEditor
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent() { }

        private Panel           pnSWAFEditor    = new Panel();
        private Label           label1          = new Label();
        private CheckBox        ckbIncWants     = new CheckBox();
        private ComboBox        cbFileVersion   = new ComboBox();
        private Label           label4          = new Label();
        private CheckBox        ckbIncFears     = new CheckBox();
        private Label           label2          = new Label();
        private CheckBox        ckbIncLTWants   = new CheckBox();
        private TextBox         tbUnknown1      = new TextBox();
        private CheckBox        ckbIncHistory   = new CheckBox();
        private Button          btnAddWant      = new Button();
        private Label           label5          = new Label();
        private TextBox         tbMaxWants      = new TextBox();
        private TextBox         tbUnknown2      = new TextBox();
        private Button          btnAddFear      = new Button();
        private Label           label3          = new Label();
        private TextBox         tbMaxFears      = new TextBox();
        private Label           label6          = new Label();
        private TextBox         tbUnknown3      = new TextBox();
        private Button          btnAddLTWant    = new Button();
        private TextBox         tbUnknown4      = new TextBox();
        private Button          btnAddHistory   = new Button();
        private Button          btnDelete       = new Button();
        private Label           label7          = new Label();
        private Button          btnCommit       = new Button();
        private SplitContainer  splitContainer1 = new SplitContainer();
        private ListView        lvItems         = new ListView();
        private ColumnHeader    chSIItemType    = new ColumnHeader();
        private ColumnHeader    chSIWantID      = new ColumnHeader();
        private ColumnHeader    chSIArg         = new ColumnHeader();
        private ColumnHeader    chSIArg2        = new ColumnHeader();
        private ColumnHeader    chSICounter     = new ColumnHeader();
        private ColumnHeader    chSIScore       = new ColumnHeader();
        private ColumnHeader    chSIInfluence   = new ColumnHeader();
        private ColumnHeader    chSIFlags       = new ColumnHeader();
        private ColumnHeader    chSIVersion     = new ColumnHeader();
        private ColumnHeader    chSISimID       = new ColumnHeader();
        private ColumnHeader    chSIArgType     = new ColumnHeader();
        private Panel           gbSelectedItem  = new Panel();
        private Panel           pnArg           = new Panel();
        private Button          btnSim2         = new Button();
        private TextBox         tbSISimID2      = new TextBox();
        private ComboBox        cbSISkill       = new ComboBox();
        private SimPe.Plugin.GUIDChooser gcSICareer   = new SimPe.Plugin.GUIDChooser();
        private SimPe.Plugin.GUIDChooser gcSIObject   = new SimPe.Plugin.GUIDChooser();
        private LinkLabel       llSREL          = new LinkLabel();
        private Label           lbArg           = new Label();
        private SimPe.Plugin.GUIDChooser gcSICategory = new SimPe.Plugin.GUIDChooser();
        private LinkLabel       llSimName2      = new LinkLabel();
        private SimPe.Plugin.GUIDChooser gcSIBadge    = new SimPe.Plugin.GUIDChooser();
        private Label           lbTimes         = new Label();
        private Label           lbXWNTIntMult   = new Label();
        private ComboBox        cbSIArgType     = new ComboBox();
        private Label           label12         = new Label();
        private Label           label19         = new Label();
        private CheckBox        ckbFlag8        = new CheckBox();
        private TextBox         tbSIArg2        = new TextBox();
        private Label           lbXWNTType      = new Label();
        private Label           label13         = new Label();
        private Label           label8          = new Label();
        private Label           lbXWNTIntOp     = new Label();
        private Label           lbSIItemType    = new Label();
        private CheckBox        ckbFlag7        = new CheckBox();
        private Label           label9          = new Label();
        private LinkLabel       llXWNT          = new LinkLabel();
        private CheckBox        ckbFlag6        = new CheckBox();
        private ComboBox        cbSIVersion     = new ComboBox();
        private Label           label18         = new Label();
        private CheckBox        ckbFlag5        = new CheckBox();
        private Label           label10         = new Label();
        private CheckBox        ckbFlag4        = new CheckBox();
        private TextBox         tbSISimID       = new TextBox();
        private CheckBox        ckbFlag3        = new CheckBox();
        private SimPe.Plugin.GUIDChooser gcSIWant     = new SimPe.Plugin.GUIDChooser();
        private CheckBox        ckbFlag2        = new CheckBox();
        private Button          btnSim          = new Button();
        private CheckBox        ckbFlag1        = new CheckBox();
        private LinkLabel       llSimName       = new LinkLabel();
        private Label           label11         = new Label();
        private Label           label15         = new Label();
        private TextBox         tbSICounter     = new TextBox();
        private Label           label16         = new Label();
        private TextBox         tbSIScore       = new TextBox();
        private TextBox         tbSIInfluence   = new TextBox();
        private Label           label17         = new Label();
        private pjse.pjse_banner pjse_banner1   = new pjse.pjse_banner();
    }

    partial class XWNTEditor
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent() { }

        private Panel           pnXWNTEditor    = new Panel();
        private pjse.pjse_banner pjse_banner1   = new pjse.pjse_banner();
        private Button          btnCommit       = new Button();
        private ListView        lvWants         = new ListView();
        private ColumnHeader    chKey           = new ColumnHeader();
        private ColumnHeader    chType          = new ColumnHeader();
        private ColumnHeader    chValue         = new ColumnHeader();
        private FlowLayoutPanel flowLayoutPanel1 = new FlowLayoutPanel();
        private Label           label1          = new Label();
        private Label           lbWant          = new Label();
        private Label           label4          = new Label();
        private ComboBox        cbVersion       = new ComboBox();
        private Label           label2          = new Label();
        private ComboBox        cbProperty      = new ComboBox();
        private Label           label3          = new Label();
        private ComboBox        cbValue         = new ComboBox();
        private TextBox         tbValue         = new TextBox();
        private Button          btnDelete       = new Button();
        private Button          btnAdd          = new Button();
    }
}
