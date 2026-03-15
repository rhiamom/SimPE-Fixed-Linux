using System.Drawing;

namespace Ambertation.Windows.Forms;

public interface IRenderDockHints : IControlRenderer
{
	Size HintSize { get; }

	Rectangle LeftRectangle { get; }

	Rectangle TopRectangle { get; }

	Rectangle RightRectangle { get; }

	Rectangle BottomRectangle { get; }

	Rectangle CenterRectangle { get; }

	void InitHints(bool l, bool t, bool r, bool b, bool c);

	void RenderHint(Graphics g, bool l, bool t, bool r, bool b, bool c, SelectedHint sel);
}
