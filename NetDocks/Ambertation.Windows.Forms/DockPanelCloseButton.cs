using System.Drawing;

namespace Ambertation.Windows.Forms;

public class DockPanelCloseButton : DockPanelCaptionButton
{
	internal DockPanelCloseButton(DockPanel dp)
		: base(dp)
	{
	}

	protected override Rectangle GetBounds(Rectangle captionrect)
	{
		return base.Renderer.GetCloseButtonRect(base.Parent, captionrect);
	}

	protected override string GetImageName()
	{
		return "closeX";
	}

	protected override void OnClick()
	{
		base.Parent.Close();
	}
}
