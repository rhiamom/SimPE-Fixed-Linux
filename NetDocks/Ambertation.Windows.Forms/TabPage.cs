using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Ambertation.Windows.Forms;

public class TabPage : DockPanel
{
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ReadOnly(true)]
	[Browsable(false)]
	public new DockStyle Dock
	{
		get
		{
			return base.Dock;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ReadOnly(true)]
	public override bool ShowCollapseButton
	{
		get
		{
			return base.ShowCollapseButton;
		}
		set
		{
		}
	}

	public TabControl TabControl => base.Manager as TabControl;

	public override bool OnlyChild => false;

	[ReadOnly(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override string ButtonText
	{
		get
		{
			return base.CaptionText;
		}
		set
		{
			base.CaptionText = value;
		}
	}

	public TabPage()
		: this(null)
	{
	}

	public TabPage(TabControl tc)
		: base(tc)
	{
		base.Dock = DockStyle.Fill;
		base.ShowCollapseButton = false;
	}

	protected override void OnDockChanged(EventArgs e)
	{
		base.Dock = DockStyle.Fill;
		base.OnDockChanged(e);
	}
}
