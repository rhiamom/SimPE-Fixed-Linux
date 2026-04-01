// Minimal stubs for WinForms dialog/app APIs used in ToolboxScanner.
// These replace System.Windows.Forms types that have no Avalonia equivalent
// but whose actual dialog functionality is not needed on Mac (file dialogs
// handled by native Avalonia pickers when needed).

namespace SimPe.Plugin
{
    internal enum DialogResult { None, OK, Cancel, Yes, No }

    internal class FolderBrowserDialogStub
    {
        public string SelectedPath { get; set; } = "";
        public bool ShowNewFolderButton { get; set; }
        public DialogResult ShowDialog() => DialogResult.Cancel;
    }

    internal class SaveFileDialogStub
    {
        public string Filter { get; set; }
        public string FileName { get; set; } = "";
        public string Title { get; set; }
        public string InitialDirectory { get; set; }
        public DialogResult ShowDialog() => DialogResult.Cancel;
    }

    internal static class MessageBoxStub
    {
        public static DialogResult Show(string text) => DialogResult.OK;
        public static DialogResult Show(string text, string caption, object buttons) => DialogResult.Yes;
    }
}
