namespace Ambertation.Windows.Forms;

public class GlossyRenderer : BaseRenderer
{
	public GlossyRenderer(IColorTable ct, IFontTable ft)
		: base(ct, ft)
	{
	}

	public GlossyRenderer(IColorTable ct)
		: this(ct, new GlossyFontTable())
	{
	}

	public GlossyRenderer()
		: this(new GlossyColorTable())
	{
	}

	protected override void CreateDockRenderer(out IRenderDockHints rnd)
	{
		rnd = new GlossyRenderDockHints(this);
	}

	protected override void CreateDockPanelRenderer(out IDockPanelRenderer rnd)
	{
		rnd = new GlossyRenderDockPanel(this);
	}
}
