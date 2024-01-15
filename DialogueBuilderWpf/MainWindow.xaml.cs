using DialogueBuilderWpf.src;
using DialogueBuilderWpf.src.serializer;
using DialogueBuilderWpf.ui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace DialogueBuilderWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Flag to identify when nodeEditor box is dragged.
        bool isDragging = false;

        DataService? _dataService;
        NodeEditorViewModel _viewModel;
        string selectedNodeId = "";

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new NodeEditorViewModel();
           
            property_updateBtn.Click += Property_updateBtn_Click;
            property_addChildBtn.Click += Property_addChildBtn_Click;
            property_deleteBtn.Click += Property_deleteBtn_Click;
        }

        /// <summary>
        /// Handle delete button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Property_deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_dataService == null || string.IsNullOrEmpty(selectedNodeId))
            {
                MessageBox.Show("No project or active node selected!", "Add failed", MessageBoxButton.OK);
                return;
            }

            Node? node = _dataService.FindNodeById(selectedNodeId);

            if(node == null)
            {
                MessageBox.Show("Cannot remove root node.", "Error", MessageBoxButton.OK);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Warning!\nThis will remove all the following nodes.\n{node.ChildrenListAsString()}", "Warning", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _dataService.DeleteNodeBranch(selectedNodeId);
            }
        }

        /// <summary>
        /// Handle add children button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Property_addChildBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_dataService == null || string.IsNullOrEmpty(selectedNodeId))
            {
                MessageBox.Show("No project or active node selected!", "Add failed", MessageBoxButton.OK);
                return;
            }

            _dataService!.AddNewNodeToParent(selectedNodeId);
        }

        /// <summary>
        /// Handle update button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Property_updateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_dataService == null || string.IsNullOrEmpty(selectedNodeId))
                {
                    MessageBox.Show("No project selected!", "Update failed", MessageBoxButton.OK);
                    return;
                }
                // Collect values
                string id = string.IsNullOrEmpty(UI_uiID.Text) ? "" : UI_uiID.Text;
                string npcTxt = string.IsNullOrEmpty(UI_npcText.Text) ? "" : UI_npcText.Text;
                string tooltipTxt = string.IsNullOrEmpty(UI_tooltipText.Text) ? "" : UI_tooltipText.Text;
                string effect = string.IsNullOrEmpty(UI_effectOnSkill.Text) ? "" : UI_effectOnSkill.Text;
                string skillId = string.IsNullOrEmpty(UI_skillID.Text) ? "" : UI_skillID.Text;
                bool invokeActivity = (UI_invokeActivity.IsChecked == true);

                if(!selectedNodeId.Equals(id))
                {
                    _viewModel.RenameKey(selectedNodeId, id);
                }
                // Update values
                bool success = _dataService!.UpdateNodeValues(selectedNodeId, id, npcTxt, tooltipTxt, effect, skillId, invokeActivity);
                if (!success)
                {
                    MessageBox.Show("Failed to update values. Chek input!", "Update failed" ,MessageBoxButton.OK);
                    return;
                }

                selectedNodeId = id;
            }
            catch (Exception excception)
            {
                MessageBox.Show($"Failed to update values.\n{excception}", "Update failed", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Mouse down event in node editor canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nodeEditor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = e.Handled = true;
            if (sender is TextBlock t)
            {
                selectedNodeId = t.Name;
                UpdatePropertiesWindow(_dataService!.FindNodeById(selectedNodeId));
            }
        }

        /// <summary>
        /// Mouse up event in node editor canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nodeEditor_MouseUp(object sender, MouseButtonEventArgs e) 
        {
            isDragging = false;
            if(sender is TextBlock t)
            {
                System.Drawing.Point position = new System.Drawing.Point((int)Canvas.GetLeft(t), (int)Canvas.GetTop(t));
                _viewModel.UpdateEntryPosition(t.Name, position);
                ReDrawNodeEditor();
            }
        }

        /// <summary>
        /// Mouse position move in canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nodeEditor_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && sender is TextBlock t)
            {
                var screenPosition = Mouse.GetPosition(nodeEditor);
                Canvas.SetTop(t, screenPosition.Y - t.Height / 2);
                Canvas.SetLeft(t, screenPosition.X - t.Width / 2);
            }
        }

        /// <summary>
        /// Main entry to intialize program logic. Initializes needed components.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Open(object sender, RoutedEventArgs e)
        {
            try
            {
                var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                folderBrowserDialog.Description = "Select project directory";
                folderBrowserDialog.UseDescriptionForTitle = true;
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _dataService = new DataService();
                    _dataService.DataChangeEvent += _dataService_DataChangeEvent;
                    _dataService.InitializeFromFile(folderBrowserDialog.SelectedPath);
                    _viewModel.Load(_dataService);
                    _dataService_DataChangeEvent();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error opening project!", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Main entry to intialize program logic. Initializes needed components.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Create(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.Description = "Select project directory";
            folderBrowserDialog.UseDescriptionForTitle = true;
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _dataService = new DataService();
                _dataService.DataChangeEvent += _dataService_DataChangeEvent;
                _dataService.InitializeNewProject(folderBrowserDialog.SelectedPath);
            }
        }

        /// <summary>
        /// Handle DataService data change events.
        /// </summary>
        private void _dataService_DataChangeEvent()
        {
            _viewModel.UpdateUiState(_dataService!.Data!.Root);
            ReDrawNodeEditor();
        }

        /// <summary>
        /// Project save button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Save(object sender, RoutedEventArgs e)
        {
            if (_dataService == null) return;
            try
            {
                _dataService.Save(new MJsonSerializer());
                _viewModel.Save(_dataService);
                MessageBox.Show("Saved succesfully", "Saved!", MessageBoxButton.OK);
            }
            catch (System.Exception exception)
            {
                MessageBox.Show(exception.Message, "Failed to save data.", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Export to unreal engine compatible CSV button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_ExportUnrealCSV(object sender, RoutedEventArgs e)
        {
            if (_dataService == null) return;
            try
            {
                _dataService.Save(new UeCsvSerilizer());
                MessageBox.Show("Exported succesfully", "Exported!", MessageBoxButton.OK);
            }
            catch (System.Exception exception)
            {
                MessageBox.Show(exception.Message,  "Failed to save data.", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Normal CSV export button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_ExportCsv(object sender, RoutedEventArgs e)
        {
            if (_dataService == null) return;
            try
            {
                _dataService.Save(new CsvSerilizer());
                MessageBox.Show("Exported succesfully", "Exported!", MessageBoxButton.OK);
            }
            catch (System.Exception exception)
            {
                MessageBox.Show(exception.Message, "Failed to save data.", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Quit program and save state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_QuitSave(object sender, RoutedEventArgs e)
        {
            if (_dataService == null) return;
            try
            {
                _dataService.Save(new MJsonSerializer());
                _viewModel.Save(_dataService);
                Application.Current.Shutdown();
            }
            catch (System.Exception exception)
            {
                MessageBox.Show(exception.Message, "Failed to save data.", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Quit program without saving.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Quit(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        /// <summary>
        /// Info about program button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Info(object sender, RoutedEventArgs e)
        {

            string info =
                $"""
                Info about project can be found at Github repo.
                
                https://github.com/linre-90/DialogueBuilder

                """;
            MessageBox.Show(info, "Info", MessageBoxButton.OK);
        }

        /// <summary>
        /// License button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_License(object sender, RoutedEventArgs e)
        {
            try
            {
                string lis = File.ReadAllText(@"License.txt");
                MessageBox.Show(lis, "License", MessageBoxButton.OK);
            }
            catch (Exception)
            {
                MessageBox.Show("Error reading license file.", "Error", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Redraws node editor canvas.
        /// </summary>
        private void ReDrawNodeEditor()
        {
            nodeEditor.Children.Clear();
            NodeEditorNodeBuilder editorNodeBuilder = new NodeEditorNodeBuilder();
            foreach (KeyValuePair<string, System.Drawing.Point> entry in _viewModel.NodeTreeNodes)
            {
                TextBlock block = editorNodeBuilder.BuildBlock(entry.Key);
                block.MouseMove += nodeEditor_MouseMove;
                block.MouseLeftButtonDown += nodeEditor_MouseDown;
                block.MouseLeftButtonUp += nodeEditor_MouseUp;
                Canvas.SetTop(block, entry.Value.Y);
                Canvas.SetLeft(block, entry.Value.X);
                nodeEditor.Children.Add(block);
                // Draw connection lines
                foreach (var item in _dataService!.GetChildrenUiIds(entry.Key)!)
                {
                    Line line = editorNodeBuilder.BuildLineToChild(entry.Value, _viewModel.NodeTreeNodes[item]);
                    nodeEditor.Children.Add(line);
                }
            }
        }

        /// <summary>
        /// Updates properties window values.
        /// </summary>
        /// <param name="node"></param>
        private void UpdatePropertiesWindow(Node? node)
        {
            if (node != null)
            {
                UI_uiID.Text = node.UiID;
                UI_npcText.Text = node.NpcText;
                UI_tooltipText.Text = node.TooltipText;
                UI_skillID.Text = node.SkillID.ToString();
                UI_effectOnSkill.Text = node.Effect.ToString();
                UI_invokeActivity.IsChecked = node.InvokeActivity;
            }
        }
    }
}
