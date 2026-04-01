using Avalonia.Controls;
using SimPe.Scenegraph.Compat;

namespace SimPe.Plugin
{
    partial class GUIDChooser
    {
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.flpMain = new FlowLayoutPanel();
            this.lbLabel = new Label();
            this.cbKnownObjects = new ComboBox();
            this.tbGUID = new TextBox();
            //
            // lbLabel
            //
            this.lbLabel.Content = "Label";
            //
            // cbKnownObjects
            //
            this.cbKnownObjects.SelectionChanged += (s, e) => this.cbKnownObjects_SelectedIndexChanged(s, e);
            //
            // tbGUID
            //
            this.tbGUID.Text = "0xDDDDDDDD";
            this.tbGUID.TextChanged += (s, e) => this.tbGUID_TextChanged(s, e);
            this.tbGUID.LostFocus += (s, e) => this.hex32_Validated(s, e);
        }

        #endregion

        private FlowLayoutPanel flpMain;
        private Label lbLabel;
        private ComboBox cbKnownObjects;
        private TextBox tbGUID;
    }
}
