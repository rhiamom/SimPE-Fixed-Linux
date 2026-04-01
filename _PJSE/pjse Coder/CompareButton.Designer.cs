namespace pjse
{
    partial class CompareButton
    {
        private System.Windows.Forms.ContextMenuStrip cmenuCompare;
        private System.Windows.Forms.ToolStripMenuItem currentObjectspackageToolStripMenuItem;
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompareButton));
            this.cmenuCompare = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.currentObjectspackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            // 
            // cmenuCompareBHAV
            // 
            this.cmenuCompare.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.currentObjectspackageToolStripMenuItem});
            this.cmenuCompare.Name = "cmenuCompareBHAV";
            this.cmenuCompare.Opening += new System.ComponentModel.CancelEventHandler(this.cmenuCompare_Opening);
            // 
            // currentObjectspackageToolStripMenuItem
            // 
            this.currentObjectspackageToolStripMenuItem.Name = "currentObjectspackageToolStripMenuItem";
            this.currentObjectspackageToolStripMenuItem.Click += (s, e) => this.tsmi_Click(s, e);
            // 
            // CompareButton
            // 
            this.Click += (s, e) => this.btnCompare_Click(s, e);

        }

        #endregion
    }
}
