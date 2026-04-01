namespace pjse
{
    partial class LabelledDataOwner
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbLabel = new System.Windows.Forms.Label();
            this.flpValue = new System.Windows.Forms.FlowLayoutPanel();
            this.cbPicker = new System.Windows.Forms.ComboBox();
            this.tbVal = new System.Windows.Forms.TextBox();
            this.lbInstance = new System.Windows.Forms.Label();
            this.cbDataOwner = new System.Windows.Forms.ComboBox();
            this.flpCheckBoxes = new System.Windows.Forms.FlowLayoutPanel();
            this.ckbDecimal = new System.Windows.Forms.CheckBox();
            this.ckbUseInstancePicker = new System.Windows.Forms.CheckBox();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lbLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flpValue, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbInstance, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbDataOwner, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flpCheckBoxes, 1, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            // 
            // lbLabel
            // 
            this.lbLabel.Name = "lbLabel";
            this.lbLabel.Text = "label1";
            // 
            // flpValue
            // 
            this.flpValue.Controls.Add(this.cbPicker);
            this.flpValue.Controls.Add(this.tbVal);
            this.flpValue.Name = "flpValue";
            // 
            // cbPicker
            // 
            this.cbPicker.Name = "cbPicker";
            this.cbPicker.Visible = false;
            // 
            // tbVal
            // 
            this.tbVal.Name = "tbVal";
            this.tbVal.Text = "0x0000";
            // 
            // lbConst
            // 
            this.lbInstance.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbInstance.Name = "lbConst";
            this.lbInstance.Text = "Const value";
            // 
            // cbDataOwner
            // 
            this.cbDataOwner.Name = "cbDataOwner";
            // 
            // flpCheckBoxes
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.flpCheckBoxes, 2);
            this.flpCheckBoxes.Controls.Add(this.ckbDecimal);
            this.flpCheckBoxes.Controls.Add(this.ckbUseInstancePicker);
            this.flpCheckBoxes.Name = "flpCheckBoxes";
            // 
            // ckbDecimal
            // 
            this.ckbDecimal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ckbDecimal.Name = "ckbDecimal";
            this.ckbDecimal.Text = "Decimal (except Consts)";
            // 
            // ckbUseAttrPicker
            // 
            this.ckbUseInstancePicker.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ckbUseInstancePicker.Name = "ckbUseAttrPicker";
            this.ckbUseInstancePicker.Text = "use Instance Picker";
            // 
            // LabelledDataOwner
            // 
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LabelledDataOwner";

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbLabel;
        private System.Windows.Forms.Label lbInstance;
        private System.Windows.Forms.ComboBox cbPicker;
        private System.Windows.Forms.TextBox tbVal;
        private System.Windows.Forms.ComboBox cbDataOwner;
        private System.Windows.Forms.CheckBox ckbDecimal;
        private System.Windows.Forms.CheckBox ckbUseInstancePicker;
        private System.Windows.Forms.FlowLayoutPanel flpValue;
        private System.Windows.Forms.FlowLayoutPanel flpCheckBoxes;
    }
}
