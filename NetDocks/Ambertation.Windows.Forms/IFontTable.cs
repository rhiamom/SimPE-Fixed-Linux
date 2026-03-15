using System.Drawing;

namespace Ambertation.Windows.Forms;

public interface IFontTable
{
	Font ButtonFont { get; }

	Font ButtonHighlightFont { get; }

	Font CaptionFont { get; }
}
