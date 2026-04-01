namespace pjse
{
    partial class PickANumber
    {
        private Avalonia.Controls.Grid tableLayoutPanel1;
        private Avalonia.Controls.TextBox textBox1;
        private Avalonia.Controls.RadioButton radioButton1;
        private Avalonia.Controls.Button btnCancel;
        private Avalonia.Controls.Button btnOK;
        private Avalonia.Controls.Label label1;

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new Avalonia.Controls.Grid();
            this.textBox1 = new Avalonia.Controls.TextBox();
            this.radioButton1 = new Avalonia.Controls.RadioButton();
            this.btnCancel = new Avalonia.Controls.Button { Content = "Cancel" };
            this.btnOK = new Avalonia.Controls.Button { Content = "OK" };
            this.label1 = new Avalonia.Controls.Label();

            this.radioButton1.IsCheckedChanged += (s, e) => this.radioButton1_CheckedChanged(s, e);
            this.btnOK.Click += (s, e) => { _dialogAccepted = true; Close(); };
            this.btnCancel.Click += (s, e) => { _dialogAccepted = false; Close(); };

            var mainPanel = new Avalonia.Controls.StackPanel();
            mainPanel.Children.Add(this.label1);
            var row1 = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            row1.Children.Add(this.radioButton1);
            row1.Children.Add(this.textBox1);
            mainPanel.Children.Add(row1);
            var btnRow = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            btnRow.Children.Add(this.btnOK);
            btnRow.Children.Add(this.btnCancel);
            mainPanel.Children.Add(btnRow);
            this.Content = mainPanel;
            this.Name = "PickANumber";
        }
    }
}
