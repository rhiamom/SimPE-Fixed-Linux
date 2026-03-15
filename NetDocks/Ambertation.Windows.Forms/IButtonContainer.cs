using System.Windows.Forms;

namespace Ambertation.Windows.Forms;

public interface IButtonContainer
{
	ButtonOrientation BestOrientation { get; }

	DockPanel Highlight { get; }

	DockButtonBar.DockPanelList GetButtons();

	Padding GetBorderSize(ButtonOrientation orient);
}
