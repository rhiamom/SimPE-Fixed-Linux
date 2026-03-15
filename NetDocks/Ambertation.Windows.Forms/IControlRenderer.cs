namespace Ambertation.Windows.Forms;

public interface IControlRenderer
{
	BaseRenderer Parent { get; }

	IColorTable ColorTable { get; }
}
