namespace SimPe.Plugin.Tool.Dockable
{
    partial class SimpleObjectPreview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleObjectPreview));
            this.lbVert = new Avalonia.Controls.TextBlock();
            this.cbCat = new Avalonia.Controls.ComboBox();
            this.label1 = new Avalonia.Controls.TextBlock();
            this.label2 = new Avalonia.Controls.TextBlock();
            this.lbEPs = new Avalonia.Controls.TextBlock();
            this.label3 = new Avalonia.Controls.TextBlock();
            this.lbName = new Avalonia.Controls.TextBlock();
            this.lbPrice = new Avalonia.Controls.TextBlock();
            this.lbCat = new Avalonia.Controls.TextBlock();
            this.label7 = new Avalonia.Controls.TextBlock();
            this.lbAbout = new Avalonia.Controls.TextBlock();
            this.lbEPList = new Avalonia.Controls.TextBlock();
            this.pb = new SimPe.Scenegraph.Compat.PictureBox();
            this.label4 = new Avalonia.Controls.TextBlock();
            // 
            // lbVert
            // 
            this.lbVert.Name = "lbVert";
            // 
            // cbCat
            // 
            this.cbCat.Name = "cbCat";
            // 
            // label1
            // 
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.Name = "label2";
            // 
            // lbEPs
            // 
            this.lbEPs.Name = "lbEPs";
            // 
            // label3
            // 
            this.label3.Name = "label3";
            // 
            // lbName
            // 
            this.lbName.Name = "lbName";
            // 
            // lbPrice
            // 
            this.lbPrice.Name = "lbPrice";
            // 
            // lbCat
            // 
            this.lbCat.Name = "lbCat";
            // 
            // label7
            // 
            this.label7.Name = "label7";
            // 
            // lbAbout
            // 
            this.lbAbout.MaximumSize = new System.Drawing.Size(600, 0);
            this.lbAbout.MinimumSize = new System.Drawing.Size(0, 22);
            this.lbAbout.Name = "lbAbout";
            // 
            // lbEPList
            // 
            this.lbEPList.Name = "lbEPList";
            // 
            // pb
            // 
            this.pb.BackColor = System.Drawing.Color.Transparent;
            this.pb.Name = "pb";
            // 
            // label4
            // 
            this.label4.Name = "label4";
            // 
            // SimpleObjectPreview
            // 

        }

        #endregion

        private Avalonia.Controls.TextBlock label1;
        private Avalonia.Controls.TextBlock label2;
        private Avalonia.Controls.TextBlock label4;
        private Avalonia.Controls.TextBlock label3;
        protected SimPe.Scenegraph.Compat.PictureBox pb;
        protected Avalonia.Controls.TextBlock lbName;
        protected Avalonia.Controls.TextBlock lbAbout;
        protected Avalonia.Controls.TextBlock lbPrice;
        protected Avalonia.Controls.TextBlock lbCat;
        protected Avalonia.Controls.ComboBox cbCat;
        protected Avalonia.Controls.TextBlock lbVert;
        private Avalonia.Controls.TextBlock label7;
        protected Avalonia.Controls.TextBlock lbEPs;
        protected Avalonia.Controls.TextBlock lbEPList;
    }
}
