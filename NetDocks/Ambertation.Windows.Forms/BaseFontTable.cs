using System.Drawing;

namespace Ambertation.Windows.Forms;

public class BaseFontTable : IFontTable
{
	private Font fnt;

	protected Font DefaultFont => fnt;

	public virtual Font ButtonFont => fnt;

	public virtual Font ButtonHighlightFont => fnt;

	public virtual Font CaptionFont => fnt;

	public BaseFontTable()
	{
		fnt = new Font("Arial", 8f, FontStyle.Regular);
	}
}
