using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Platform.Storage;
using SimPe.Interfaces.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimPe
{
    public partial class MainWindow : Window
    {
        private SimPe.Packages.GeneratableFile _package;

        public MainWindow()
        {
            InitializeComponent();
            WireEvents();
        }

        // ─────────────────────────────────────────────────────────────
        // Event wiring
        // ─────────────────────────────────────────────────────────────

        private void WireEvents()
        {
            MenuFileOpen.Click  += async (_, _) => await OpenPackage();
            TbOpen.Click        += async (_, _) => await OpenPackage();

            MenuFileClose.Click += (_, _) => ClosePackage();
            TbClose.Click       += (_, _) => ClosePackage();

            MenuFileExit.Click  += (_, _) => Close();

            ResourceTree.SelectionChanged += OnTreeSelectionChanged;
        }

        // ─────────────────────────────────────────────────────────────
        // Open
        // ─────────────────────────────────────────────────────────────

        private async Task OpenPackage()
        {
            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title         = "Open Package",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Sims 2 Package")
                    {
                        Patterns = new[] { "*.package", "*.package.disabled" }
                    },
                    new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
                }
            });

            if (files.Count == 0) return;

            ClosePackage();

            string path = files[0].Path.LocalPath;
            try
            {
                StatusText.Text = "Loading…";
                _package = SimPe.Packages.File.LoadFromFile(path, true);
                PopulateTree();
                ShowAll();
                Title = $"SimPE — {System.IO.Path.GetFileName(path)}";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Error: {ex.Message}";
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Close
        // ─────────────────────────────────────────────────────────────

        private void ClosePackage()
        {
            if (_package == null) return;
            _package = null;
            ResourceTree.ItemsSource = null;
            ResourceList.ItemsSource = null;
            Title = "SimPE — Sims Package Editor";
            StatusText.Text = "Ready";
        }

        // ─────────────────────────────────────────────────────────────
        // Tree
        // ─────────────────────────────────────────────────────────────

        private void PopulateTree()
        {
            var nodes = _package.Index
                .GroupBy(pfd => pfd.TypeName?.Name ?? $"0x{pfd.Type:X8}")
                .OrderBy(g => g.Key)
                .Select(g => new ResourceTypeNode(g.Key, g.ToList()))
                .ToList();

            ResourceTree.ItemsSource = nodes;
        }

        private void OnTreeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResourceTree.SelectedItem is ResourceTypeNode node)
            {
                PopulateList(node.Descriptors);
                StatusText.Text = $"{node.Descriptors.Count} resources";
            }
        }

        // ─────────────────────────────────────────────────────────────
        // List
        // ─────────────────────────────────────────────────────────────

        // Shows every resource in the package (no tree filter active)
        private void ShowAll()
        {
            PopulateList(_package.Index);
            StatusText.Text = $"{_package.Index.Length} resources";
        }

        private void PopulateList(IEnumerable<IPackedFileDescriptor> descriptors)
        {
            ResourceList.ItemsSource = descriptors
                .Select(pfd => new ResourceListItem(pfd))
                .ToList();
        }
    }
}
