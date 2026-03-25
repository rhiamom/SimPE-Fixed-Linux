// WinForms-compatibility shims for Wizard and WizardStepPanel.
// The designer-generated code in ToolBoxWorkshops uses WinForms properties on these
// Avalonia controls. These stubs satisfy the compiler; actual Avalonia wiring is in Wizard.cs.

using System.Windows.Forms;

namespace SimPe.Wizards
{
    partial class Wizard
    {
        public System.Drawing.Color BackColor { get; set; }
        public ImageLayout BackgroundImageLayout { get; set; }
        public ControlCollection Controls { get; } = new ControlCollection();
        // Dock uses object to avoid ambiguous DockStyle when both simpe.wizardbase and
        // simpe.workspace.plugin are referenced (both define System.Windows.Forms.DockStyle)
        public object Dock { get; set; }
        public System.Drawing.Point Location { get; set; }
        public System.Drawing.Size Size { get; set; }
        public DockPaddingEdges DockPadding { get; } = new DockPaddingEdges();
        public void SuspendLayout() { }
        public void ResumeLayout(bool performLayout = false) { }

        // Allow passing Wizard where a WinForms Control is expected (designer code)
        public static implicit operator System.Windows.Forms.Control(Wizard w) => null;
    }

    partial class WizardStepPanel
    {
        public System.Drawing.Color BackColor { get; set; }
        public ControlCollection Controls { get; } = new ControlCollection();
        public object Dock { get; set; }
        public System.Drawing.Point Location { get; set; }
        public System.Drawing.Size Size { get; set; }
        public void SuspendLayout() { }
        public void ResumeLayout(bool performLayout = false) { }

        // Allow passing WizardStepPanel where a WinForms Control is expected (designer code)
        public static implicit operator System.Windows.Forms.Control(WizardStepPanel p) => null;
    }
}
