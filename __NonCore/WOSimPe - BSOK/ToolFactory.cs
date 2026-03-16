using System;
using SimPe.Interfaces;
using SimPe.Interfaces.Plugin;
using SimPe.Plugin;

namespace SimPe.Wizards
{
    public class BsokWizardTool : AbstractTool, ITool
    {
        public bool IsEnabled(SimPe.Interfaces.Files.IPackedFileDescriptor pfd, SimPe.Interfaces.Files.IPackageFile package)
            => true;

        public IToolResult ShowDialog(ref SimPe.Interfaces.Files.IPackedFileDescriptor pfd, ref SimPe.Interfaces.Files.IPackageFile package)
        {
            new BsokWizardForm().ShowDialog();
            return new ToolResult(false, false);
        }

        public override string ToString() => "Wizards\\BSOK Wizard...";
    }

    public class BsokToolFactory : AbstractWrapperFactory, IToolFactory
    {
        public IToolPlugin[] KnownTools => new IToolPlugin[] { new BsokWizardTool() };
    }
}
