using System;
using System.Collections.Generic;
using System.Text;
using Ambertation.Windows.Forms;

namespace SimPe.Plugin
{
    partial class TreesPackedFileUI
    {

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btMove = new System.Windows.Forms.ButtonCompat();
            this.btRemove = new System.Windows.Forms.ButtonCompat();
            this.pnhidim = new System.Windows.Forms.Panel();
            this.label19 = new System.Windows.Forms.LabelCompat();
            this.label18 = new System.Windows.Forms.LabelCompat();
            this.tbheader = new System.Windows.Forms.TextBoxCompat();
            this.tbunk5 = new System.Windows.Forms.TextBoxCompat();
            this.tbunk0 = new System.Windows.Forms.TextBoxCompat();
            this.label13 = new System.Windows.Forms.LabelCompat();
            this.label16 = new System.Windows.Forms.LabelCompat();
            this.tbunk4 = new System.Windows.Forms.TextBoxCompat();
            this.tbunk1 = new System.Windows.Forms.TextBoxCompat();
            this.tbunk3 = new System.Windows.Forms.TextBoxCompat();
            this.label15 = new System.Windows.Forms.LabelCompat();
            this.label17 = new System.Windows.Forms.LabelCompat();
            this.tbunk2 = new System.Windows.Forms.TextBoxCompat();
            this.label14 = new System.Windows.Forms.LabelCompat();
            this.btAdder = new System.Windows.Forms.ButtonCompat();
            this.tbversion = new System.Windows.Forms.TextBoxCompat();
            this.label2 = new System.Windows.Forms.LabelCompat();
            this.tbcount = new System.Windows.Forms.TextBoxCompat();
            this.label1 = new System.Windows.Forms.LabelCompat();
            this.lbfilename = new System.Windows.Forms.LabelCompat();
            this.tbfilename = new System.Windows.Forms.TextBoxCompat();
            this.listList = new System.Windows.Forms.ListView();
            this.Comment = new System.Windows.Forms.ColumnHeader();
            this.Zero2 = new System.Windows.Forms.ColumnHeader();
            this.Block1 = new System.Windows.Forms.ColumnHeader();
            this.Block2 = new System.Windows.Forms.ColumnHeader();
            this.Block3 = new System.Windows.Forms.ColumnHeader();
            this.Block4 = new System.Windows.Forms.ColumnHeader();
            this.Block5 = new System.Windows.Forms.ColumnHeader();
            this.Block6 = new System.Windows.Forms.ColumnHeader();
            this.Block7 = new System.Windows.Forms.ColumnHeader();
            this.Block8 = new System.Windows.Forms.ColumnHeader();
            this.Block9 = new System.Windows.Forms.ColumnHeader();
            this.listLast = new System.Windows.Forms.ListView();
            this.Indecks = new System.Windows.Forms.ColumnHeader();
            this.Comment2 = new System.Windows.Forms.ColumnHeader();
            this.taskBox1 = new XPTaskBoxSimple();
            this.textBox11 = new System.Windows.Forms.TextBoxCompat();
            this.textBox10 = new System.Windows.Forms.TextBoxCompat();
            this.textBox9 = new System.Windows.Forms.TextBoxCompat();
            this.textBox8 = new System.Windows.Forms.TextBoxCompat();
            this.textBox7 = new System.Windows.Forms.TextBoxCompat();
            this.textBox6 = new System.Windows.Forms.TextBoxCompat();
            this.textBox5 = new System.Windows.Forms.TextBoxCompat();
            this.textBox4 = new System.Windows.Forms.TextBoxCompat();
            this.textBox3 = new System.Windows.Forms.TextBoxCompat();
            this.textBox2 = new System.Windows.Forms.TextBoxCompat();
            this.label12 = new System.Windows.Forms.LabelCompat();
            this.label11 = new System.Windows.Forms.LabelCompat();
            this.label10 = new System.Windows.Forms.LabelCompat();
            this.label9 = new System.Windows.Forms.LabelCompat();
            this.label8 = new System.Windows.Forms.LabelCompat();
            this.label7 = new System.Windows.Forms.LabelCompat();
            this.label6 = new System.Windows.Forms.LabelCompat();
            this.label5 = new System.Windows.Forms.LabelCompat();
            this.label4 = new System.Windows.Forms.LabelCompat();
            this.label3 = new System.Windows.Forms.LabelCompat();
            this.tbComment = new System.Windows.Forms.TextBoxCompat();
            this.lbComment = new System.Windows.Forms.LabelCompat();
            this.taskBox2 = new XPTaskBoxSimple();
            this.tbComment2 = new System.Windows.Forms.TextBoxCompat();
            this.lbComment2 = new System.Windows.Forms.LabelCompat();
            this.btDown = new System.Windows.Forms.ButtonCompat();
            this.btBhave = new System.Windows.Forms.ButtonCompat();
            // 
            // panel1
            // 
            
            this.panel1.Controls.Add(this.btBhave);
            this.panel1.Controls.Add(this.btDown);
            this.panel1.Controls.Add(this.btMove);
            this.panel1.Controls.Add(this.btRemove);
            this.panel1.Controls.Add(this.pnhidim);
            this.panel1.Controls.Add(this.btAdder);
            this.panel1.Controls.Add(this.tbversion);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tbcount);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lbfilename);
            this.panel1.Controls.Add(this.tbfilename);
            this.panel1.Controls.Add(this.listList);
            this.panel1.Controls.Add(this.listLast);
            this.panel1.Controls.Add(this.taskBox1);
            this.panel1.Controls.Add(this.taskBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            
            
            this.panel1.Name = "panel1";
            
            // 
            // btMove
            // 
            this.btMove.Name = "btMove";
            this.btMove.Text = "Move Up";
            this.btMove.UseVisualStyleBackColor = true;
            this.btMove.Click += (s, e) => this.btMove_Click(s, e);
            // 
            // btRemove
            // 
            this.btRemove.Name = "btRemove";
            this.btRemove.Text = "Delete a Line";
            this.btRemove.UseVisualStyleBackColor = true;
            this.btRemove.Click += (s, e) => this.btRemove_Click(s, e);
            // 
            // pnhidim
            // 
            this.pnhidim.Controls.Add(this.label19);
            this.pnhidim.Controls.Add(this.label18);
            this.pnhidim.Controls.Add(this.tbheader);
            this.pnhidim.Controls.Add(this.tbunk5);
            this.pnhidim.Controls.Add(this.tbunk0);
            this.pnhidim.Controls.Add(this.label13);
            this.pnhidim.Controls.Add(this.label16);
            this.pnhidim.Controls.Add(this.tbunk4);
            this.pnhidim.Controls.Add(this.tbunk1);
            this.pnhidim.Controls.Add(this.tbunk3);
            this.pnhidim.Controls.Add(this.label15);
            this.pnhidim.Controls.Add(this.label17);
            this.pnhidim.Controls.Add(this.tbunk2);
            this.pnhidim.Controls.Add(this.label14);
            this.pnhidim.Name = "pnhidim";
            // 
            // label19
            // 
            this.label19.Name = "label19";
            this.label19.Text = "U0 :";
            // 
            // label18
            // 
            this.label18.Name = "label18";
            this.label18.Text = "U1 :";
            // 
            // tbheader
            // 
            this.tbheader.Name = "tbheader";
            this.tbheader.IsReadOnly = true;
            // 
            // tbunk5
            // 
            this.tbunk5.Name = "tbunk5";
            this.tbunk5.IsReadOnly = true;
            // 
            // tbunk0
            // 
            this.tbunk0.Name = "tbunk0";
            this.tbunk0.IsReadOnly = true;
            // 
            // label13
            // 
            this.label13.Name = "label13";
            this.label13.Text = "Header :";
            // 
            // label16
            // 
            this.label16.Name = "label16";
            this.label16.Text = "U5 :";
            // 
            // tbunk4
            // 
            this.tbunk4.Name = "tbunk4";
            this.tbunk4.IsReadOnly = true;
            // 
            // tbunk1
            // 
            this.tbunk1.Name = "tbunk1";
            this.tbunk1.IsReadOnly = true;
            // 
            // tbunk3
            // 
            this.tbunk3.Name = "tbunk3";
            this.tbunk3.IsReadOnly = true;
            // 
            // label15
            // 
            this.label15.Name = "label15";
            this.label15.Text = "U4 :";
            // 
            // label17
            // 
            this.label17.Name = "label17";
            this.label17.Text = "U2 :";
            // 
            // tbunk2
            // 
            this.tbunk2.Name = "tbunk2";
            this.tbunk2.IsReadOnly = true;
            // 
            // label14
            // 
            this.label14.Name = "label14";
            this.label14.Text = "U3 :";
            // 
            // btAdder
            // 
            this.btAdder.Name = "btAdder";
            this.btAdder.Text = "Add a Line";
            this.btAdder.UseVisualStyleBackColor = true;
            this.btAdder.Click += (s, e) => this.btAdder_Click(s, e);
            // 
            // tbversion
            // 
            this.tbversion.Name = "tbversion";
            this.tbversion.IsReadOnly = true;
            // 
            // label2
            // 
            this.label2.Name = "label2";
            this.label2.Text = "Version :";
            // 
            // tbcount
            // 
            this.tbcount.Name = "tbcount";
            this.tbcount.IsReadOnly = true;
            // 
            // label1
            // 
            this.label1.Name = "label1";
            this.label1.Text = "Count :";
            // 
            // lbfilename
            // 
            this.lbfilename.Name = "lbfilename";
            this.lbfilename.Text = "FileName :";
            // 
            // tbfilename
            // 
            this.tbfilename.MaxLength = 40;
            this.tbfilename.Name = "tbfilename";
            this.tbfilename.TextChanged += (s, e) => this.tbfilename_TextChanged(s, e);
            // 
            // listList
            // 
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Comment,
            this.Zero2,
            this.Block1,
            this.Block2,
            this.Block3,
            this.Block4,
            this.Block5,
            this.Block6,
            this.Block7,
            this.Block8,
            this.Block9});
            this.listList.FullRowSelect = true;
            this.listList.GridLines = true;
            this.listList.MultiSelect = false;
            this.listList.Name = "listList";
            this.listList.UseCompatibleStateImageBehavior = false;
            this.listList.View = System.Windows.Forms.View.Details;
            this.listList.SelectedIndexChanged += (s, e) => this.listList_SelectedIndexChanged(s, e);
            // 
            // Comment
            // 
            this.Comment.Text = "Comment";
            this.Comment.Width = 400;
            // 
            // Zero2
            // 
            this.Zero2.Text = "Zero2";
            this.Zero2.Width = 88;
            // 
            // Block1
            // 
            this.Block1.Text = "Block1";
            // 
            // Block2
            // 
            this.Block2.Text = "Block2";
            // 
            // Block3
            // 
            this.Block3.Text = "Block3";
            // 
            // Block4
            // 
            this.Block4.Text = "Block4";
            // 
            // Block5
            // 
            this.Block5.Text = "Block5";
            // 
            // Block6
            // 
            this.Block6.Text = "Block6";
            // 
            // Block7
            // 
            this.Block7.Text = "Block7";
            // 
            // Block8
            // 
            this.Block8.Text = "Block8";
            // 
            // Block9
            // 
            this.Block9.Text = "Block9";
            // 
            // listLast
            // 
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listLast.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Indecks,
            this.Comment2});
            this.listLast.FullRowSelect = true;
            this.listLast.GridLines = true;
            this.listLast.MultiSelect = false;
            this.listLast.Name = "listLast";
            this.listLast.UseCompatibleStateImageBehavior = false;
            this.listLast.View = System.Windows.Forms.View.Details;
            this.listLast.Visible = false;
            this.listLast.SelectedIndexChanged += (s, e) => this.listLast_SelectedIndexChanged(s, e);
            // 
            // Indecks
            // 
            this.Indecks.Text = "Index";
            this.Indecks.Width = 100;
            // 
            // Comment2
            // 
            this.Comment2.Text = "Comment";
            this.Comment2.Width = 810;
            // 
            // taskBox1
            // 
            this.taskBox1.Controls.Add(this.textBox11);
            this.taskBox1.Controls.Add(this.textBox10);
            this.taskBox1.Controls.Add(this.textBox9);
            this.taskBox1.Controls.Add(this.textBox8);
            this.taskBox1.Controls.Add(this.textBox7);
            this.taskBox1.Controls.Add(this.textBox6);
            this.taskBox1.Controls.Add(this.textBox5);
            this.taskBox1.Controls.Add(this.textBox4);
            this.taskBox1.Controls.Add(this.textBox3);
            this.taskBox1.Controls.Add(this.textBox2);
            this.taskBox1.Controls.Add(this.label12);
            this.taskBox1.Controls.Add(this.label11);
            this.taskBox1.Controls.Add(this.label10);
            this.taskBox1.Controls.Add(this.label9);
            this.taskBox1.Controls.Add(this.label8);
            this.taskBox1.Controls.Add(this.label7);
            this.taskBox1.Controls.Add(this.label6);
            this.taskBox1.Controls.Add(this.label5);
            this.taskBox1.Controls.Add(this.label4);
            this.taskBox1.Controls.Add(this.label3);
            this.taskBox1.Controls.Add(this.tbComment);
            this.taskBox1.Controls.Add(this.lbComment);
            this.taskBox1.HeaderText = "Editer";
            this.taskBox1.IconLocation = new System.Drawing.Point(4, 12);
            this.taskBox1.IconSize = new System.Drawing.Size(32, 32);
            this.taskBox1.Name = "taskBox1";
            // 
            // textBox11
            // 
            this.textBox11.Name = "textBox11";
            this.textBox11.TextChanged += (s, e) => this.textbox_TextChanged(s, e);
            // 
            // textBox10
            // 
            this.textBox10.Name = "textBox10";
            this.textBox10.TextChanged += (s, e) => this.textbox_TextChanged(s, e);
            // 
            // textBox9
            // 
            this.textBox9.Name = "textBox9";
            this.textBox9.TextChanged += (s, e) => this.textbox_TextChanged(s, e);
            // 
            // textBox8
            // 
            this.textBox8.Name = "textBox8";
            this.textBox8.TextChanged += (s, e) => this.textbox_TextChanged(s, e);
            // 
            // textBox7
            // 
            this.textBox7.Name = "textBox7";
            this.textBox7.TextChanged += (s, e) => this.textbox_TextChanged(s, e);
            // 
            // textBox6
            // 
            this.textBox6.Name = "textBox6";
            this.textBox6.TextChanged += (s, e) => this.textbox_TextChanged(s, e);
            // 
            // textBox5
            // 
            this.textBox5.Name = "textBox5";
            this.textBox5.TextChanged += (s, e) => this.textbox_TextChanged(s, e);
            // 
            // textBox4
            // 
            this.textBox4.Name = "textBox4";
            this.textBox4.TextChanged += (s, e) => this.textbox_TextChanged(s, e);
            // 
            // textBox3
            // 
            this.textBox3.Name = "textBox3";
            this.textBox3.TextChanged += (s, e) => this.textbox_TextChanged(s, e);
            // 
            // textBox2
            // 
            this.textBox2.Name = "textBox2";
            this.textBox2.TextChanged += (s, e) => this.textbox_TextChanged(s, e);
            // 
            // label12
            // 
            this.label12.Name = "label12";
            this.label12.Text = "Block9";
            // 
            // label11
            // 
            this.label11.Name = "label11";
            this.label11.Text = "Block8";
            // 
            // label10
            // 
            this.label10.Name = "label10";
            this.label10.Text = "Block7";
            // 
            // label9
            // 
            this.label9.Name = "label9";
            this.label9.Text = "Block6";
            // 
            // label8
            // 
            this.label8.Name = "label8";
            this.label8.Text = "Block5";
            // 
            // label7
            // 
            this.label7.Name = "label7";
            this.label7.Text = "Block4";
            // 
            // label6
            // 
            this.label6.Name = "label6";
            this.label6.Text = "Block3";
            // 
            // label5
            // 
            this.label5.Name = "label5";
            this.label5.Text = "Block2";
            // 
            // label4
            // 
            this.label4.Name = "label4";
            this.label4.Text = "Block1";
            // 
            // label3
            // 
            this.label3.Name = "label3";
            this.label3.Text = "Zero2";
            // 
            // tbComment
            // 
            this.tbComment.Multiline = true;
            this.tbComment.Name = "tbComment";
            this.tbComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbComment.TextChanged += (s, e) => this.textbox_TextChanged(s, e);
            // 
            // lbComment
            // 
            this.lbComment.Name = "lbComment";
            this.lbComment.Text = "Comment";
            // 
            // taskBox2
            // 
            this.taskBox2.Controls.Add(this.tbComment2);
            this.taskBox2.Controls.Add(this.lbComment2);
            this.taskBox2.HeaderText = "Editer";
            this.taskBox2.IconLocation = new System.Drawing.Point(4, 12);
            this.taskBox2.IconSize = new System.Drawing.Size(32, 32);
            this.taskBox2.Name = "taskBox2";
            this.taskBox2.Visible = false;
            // 
            // tbComment2
            // 
            this.tbComment2.Multiline = true;
            this.tbComment2.Name = "tbComment2";
            this.tbComment2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbComment2.TextChanged += (s, e) => this.textbox2_TextChanged(s, e);
            // 
            // lbComment2
            // 
            this.lbComment2.Name = "lbComment2";
            this.lbComment2.Text = "Comment:";
            this.tbComment2.IsReadOnly = true;            
            // 
            // btBhave
            // 
            this.btBhave.Name = "btBhave";
            this.btBhave.Text = "BHAV";
            this.btBhave.UseVisualStyleBackColor = true;
            this.btBhave.Click += (s, e) => this.btBhave_Click(s, e);
            this.btBhave.Visible = false;
            // 
            // btDown
            // 
            this.btDown.Name = "btDown";
            this.btDown.Text = "Move Down";
            this.btDown.UseVisualStyleBackColor = true;
            this.btDown.Click += (s, e) => this.btDown_Click(s, e);
            // 
            // TreesPackedFileUI
            // 
            this.CanCommit = false;
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.HeaderText = "Op-Code Comments";
            this.Name = "TreesPackedFileUI";
            this.Controls.SetChildIndex(this.panel1, 0);
        }

        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.ListView listList;
        internal System.Windows.Forms.ListView listLast;
        private System.Windows.Forms.ColumnHeader Zero2;
        private System.Windows.Forms.ColumnHeader Block1;
        private System.Windows.Forms.ColumnHeader Block2;
        private System.Windows.Forms.ColumnHeader Block3;
        private System.Windows.Forms.ColumnHeader Block4;
        private System.Windows.Forms.ColumnHeader Block5;
        private System.Windows.Forms.ColumnHeader Block6;
        private System.Windows.Forms.ColumnHeader Block7;
        private System.Windows.Forms.ColumnHeader Block8;
        private System.Windows.Forms.ColumnHeader Block9;
        private System.Windows.Forms.ColumnHeader Comment;        
        private System.Windows.Forms.ColumnHeader Comment2;
        private System.Windows.Forms.ColumnHeader Indecks;        
        private XPTaskBoxSimple taskBox1;
        private XPTaskBoxSimple taskBox2;
        private System.Windows.Forms.LabelCompat lbComment;
        private System.Windows.Forms.LabelCompat lbComment2;
        private System.Windows.Forms.LabelCompat label12;
        private System.Windows.Forms.LabelCompat label11;
        private System.Windows.Forms.LabelCompat label10;
        private System.Windows.Forms.LabelCompat label9;
        private System.Windows.Forms.LabelCompat label8;
        private System.Windows.Forms.LabelCompat label7;
        private System.Windows.Forms.LabelCompat label6;
        private System.Windows.Forms.LabelCompat label5;
        private System.Windows.Forms.LabelCompat label4;
        private System.Windows.Forms.LabelCompat label3;
        private System.Windows.Forms.TextBoxCompat tbComment;
        private System.Windows.Forms.TextBoxCompat tbComment2;
        private System.Windows.Forms.TextBoxCompat textBox11;
        private System.Windows.Forms.TextBoxCompat textBox10;
        private System.Windows.Forms.TextBoxCompat textBox9;
        private System.Windows.Forms.TextBoxCompat textBox8;
        private System.Windows.Forms.TextBoxCompat textBox7;
        private System.Windows.Forms.TextBoxCompat textBox6;
        private System.Windows.Forms.TextBoxCompat textBox5;
        private System.Windows.Forms.TextBoxCompat textBox4;
        private System.Windows.Forms.TextBoxCompat textBox3;
        private System.Windows.Forms.TextBoxCompat textBox2;
        private System.Windows.Forms.LabelCompat lbfilename;
        private System.Windows.Forms.TextBoxCompat tbfilename;
        private System.Windows.Forms.TextBoxCompat tbcount;
        private System.Windows.Forms.LabelCompat label1;
        private System.Windows.Forms.TextBoxCompat tbheader;
        private System.Windows.Forms.TextBoxCompat tbversion;
        private System.Windows.Forms.LabelCompat label13;
        private System.Windows.Forms.LabelCompat label2;
        private System.Windows.Forms.TextBoxCompat tbunk5;
        private System.Windows.Forms.TextBoxCompat tbunk4;
        private System.Windows.Forms.TextBoxCompat tbunk3;
        private System.Windows.Forms.TextBoxCompat tbunk2;
        private System.Windows.Forms.TextBoxCompat tbunk1;
        private System.Windows.Forms.LabelCompat label18;
        private System.Windows.Forms.LabelCompat label17;
        private System.Windows.Forms.LabelCompat label16;
        private System.Windows.Forms.LabelCompat label15;
        private System.Windows.Forms.LabelCompat label14;
        private System.Windows.Forms.TextBoxCompat tbunk0;
        private System.Windows.Forms.LabelCompat label19;
        private System.Windows.Forms.ButtonCompat btAdder;
        private System.Windows.Forms.Panel pnhidim;
        private System.Windows.Forms.ButtonCompat btMove;
        private System.Windows.Forms.ButtonCompat btRemove;
        private System.Windows.Forms.ButtonCompat btDown;
        private System.Windows.Forms.ButtonCompat btBhave;
    }
}
