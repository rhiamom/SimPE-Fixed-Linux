using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Ambertation.Windows.Forms;

[Designer(typeof(DockContainerDesigner))]
[ToolboxBitmap(typeof(DockManager), "Floaters.dockimg.png")]
[ToolboxItem(true)]
public class TabControl : BaseDockManager
{
	public override bool OneChild => false;

	protected override bool MeAsCenterDock => true;

	public TabControl()
		: base(ManagerSingelton.Global.TabRenderer)
	{
	}

	protected override void CleanUp()
	{
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		TabPage tabPage = e.Control as TabPage;
		if (tabPage == null)
		{
			throw new Exception("You can only add TabPage Controls to a TabControl! (tried to add " + e.Control.GetType().Name + ")");
		}
		base.OnControlAdded(e);
	}

	internal override void StartDockMode(DockPanel dock)
	{
	}

	internal override void StopDockMode(DockPanel dock)
	{
		AddPage(dock as TabPage);
	}

	internal override void MouseMoved(Point scrpt)
	{
	}

	public void AddPage(TabPage tp)
	{
		DockPanelInt(tp, DockStyle.Fill);
	}

	public override void Collapse(bool animated)
	{
	}

	public override void Expand(bool animated)
	{
	}
}
