using System.Drawing;

namespace pjse
{
    partial class pjse_banner
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnFloat = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTree = new System.Windows.Forms.Button();
            this.btnSibling = new System.Windows.Forms.Button();
            this.btnExtract = new System.Windows.Forms.Button();
            this.btnRefreshFT = new System.Windows.Forms.Button();
            this.lbLabel = new System.Windows.Forms.Label();
            // 
            // btnHelp
            // 
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnHelp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += (s, e) => this.btnHelp_Click(s, e);
            // 
            // btnFloat
            // 
            this.btnFloat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFloat.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFloat.Name = "btnFloat";
            this.btnFloat.Text = "Float";
            this.btnFloat.Visible = false;
            this.btnFloat.Click += (s, e) => this.btnFloat_Click(s, e);
            // 
            // btnView
            // 
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnView.Name = "btnView";
            this.btnView.Text = "View";
            this.btnView.Visible = false;
            this.btnView.Click += (s, e) => this.btnView_Click(s, e);
            // 
            // flpButtons
            // 
            this.flpButtons.Controls.Add(this.btnTree);
            this.flpButtons.Controls.Add(this.btnSibling);
            this.flpButtons.Controls.Add(this.btnView);
            this.flpButtons.Controls.Add(this.btnFloat);
            this.flpButtons.Controls.Add(this.btnExtract);
            this.flpButtons.Controls.Add(this.btnRefreshFT);
            this.flpButtons.Controls.Add(this.btnHelp);
            this.flpButtons.Name = "flpButtons";
            // 
            // btnTree
            // 
            this.btnTree.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnTree.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTree.Name = "btnTree";
            this.btnTree.Text = "Comments";
            this.btnTree.Visible = false;
            this.btnTree.Click += (s, e) => this.btnTree_Click(s, e);
            // 
            // btnSibling
            // 
            this.btnSibling.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSibling.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSibling.Name = "btnSibling";
            this.btnSibling.Text = "{Type}";
            this.btnSibling.Visible = false;
            this.btnSibling.Click += (s, e) => this.btnSibling_Click(s, e);
            // 
            // btnExtract
            // 
            this.btnExtract.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExtract.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Text = "Extract";
            this.btnExtract.Visible = false;
            this.btnExtract.Click += (s, e) => this.btnExtract_Click(s, e);
            // 
            // btnRefreshFT
            // 
            this.btnRefreshFT.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRefreshFT.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRefreshFT.Name = "btnRefreshFT";
            this.btnRefreshFT.Text = "RFT";
            this.btnRefreshFT.Visible = false;
            // 
            // lbLabel
            // 
            this.lbLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbLabel.Name = "lbLabel";
            this.lbLabel.Text = "PJSE: file type Editor";
            // 
            // pjse_banner
            // 
            this.Controls.Add(this.flpButtons);
            this.Controls.Add(this.lbLabel);
            this.Name = "pjse_banner";

        }

        #endregion

        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnFloat;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.Label lbLabel;
        private System.Windows.Forms.Button btnSibling;
        private System.Windows.Forms.Button btnTree;
        private System.Windows.Forms.Button btnRefreshFT;
    }
}
