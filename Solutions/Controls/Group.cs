using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace Solutions.Controls
{
    public class Group : Control
    {
        static Group()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (Group), new FrameworkPropertyMetadata(typeof (Group)));
        }

        public Group()
        {
            NodesCount = 0;
            Nodes = new List<Node>();
            Loaded += OnLoaded;
        }

        private string _newExcelPath;

        public List<Node> Nodes { get; set; }

        public Button ExcelButton { get; set; }
        public Button OkButton { get; set; }
        public Button AddButton { get; set; }
        public Button DeleteButton { get; set; }
        public Button ChangeButton { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            AddButton = GetTemplateChild("PART_AddNode") as Button;
            if (AddButton != null) AddButton.Click += AddButtonOnClick;
            DeleteButton = GetTemplateChild("PART_DeleteNode") as Button;
            if (DeleteButton != null) DeleteButton.Click += DeleteButtonOnClick;
            ChangeButton = GetTemplateChild("PART_ChangeNode") as Button;
            if (ChangeButton != null) ChangeButton.Click += ChangeButtonOnClick;
            OkButton = GetTemplateChild("PART_OkButton") as Button;
            if (OkButton != null) OkButton.Click += OkButtonOnClick;
            ExcelButton = GetTemplateChild("PART_GetExcelButton") as Button;
            if (ExcelButton != null) ExcelButton.Click += ExcelButtonOnClick;

        }

        private void ExcelButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                _newExcelPath = openFileDialog.FileName;
        }

        private void OkButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var canvas = VisualTreeHelper.GetParent(this) as GraphCanvas;
            var nodeName = GetTemplateChild("PART_NodeName") as TextBox;
            if (canvas != null)
            {
                if (canvas.NodeService.SourceNode != null)
                {
                    if (_newExcelPath != null)
                    {
                        canvas.NodeService.SourceNode.ExcelPath = _newExcelPath;
                    }              
                    if (nodeName != null) canvas.NodeService.SourceNode.NodeName = nodeName.Text;
                    AddButton.Visibility = Visibility.Visible;
                    OkButton.Visibility = Visibility.Hidden;
                    ExcelButton.Visibility = Visibility.Hidden;
                    if (nodeName != null) nodeName.Visibility = Visibility.Hidden;
                    Height -= 50;
                    return;
                }
            }

            if (_newExcelPath == null)
            {
                MessageBox.Show("Не указана таблица Excel", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NodesCount++;
            Node node = new Node(Id);
            Canvas.SetTop(node, (NodesCount - 1) * 50 + 50 * NodesCount);
            double groupLeft = Canvas.GetLeft(this);
            Canvas.SetLeft(node, groupLeft + 25);
            if (nodeName != null) node.NodeName = nodeName.Text;
            if (!string.IsNullOrEmpty(_newExcelPath))
            {
                node.ExcelPath = _newExcelPath;
                _newExcelPath = null;
            }
            node.SelectionPropertyChanged += NodeOnSelectionPropertyChanged;
            Height += 100;
            if (canvas != null)
            {
                canvas.Children.Add(node);
            }
            Height -= 120;
            AddButton.Visibility = Visibility.Visible;
            OkButton.Visibility = Visibility.Hidden;
            ExcelButton.Visibility = Visibility.Hidden;
            if (nodeName != null) nodeName.Visibility = Visibility.Hidden;
        }

        private void ChangeButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var canvas = VisualTreeHelper.GetParent(this) as GraphCanvas;
            if (canvas != null)
            {
                canvas.NodeService.SourceNode.IsEditing = true;
            }
            AddButton.Visibility = Visibility.Hidden;
            OkButton.Visibility = Visibility.Visible;
            ExcelButton.Visibility = Visibility.Visible;
            var nodeName = GetTemplateChild("PART_NodeName") as TextBox;
            if (nodeName != null) nodeName.Visibility = Visibility.Visible;
            Height += 50;

        }

        private void UpdateNodesPosition()
        {
            var canvas = VisualTreeHelper.GetParent(this) as GraphCanvas;

            List<Node> groupNodes = new List<Node>();

            if (canvas != null)
            {
                var nodes = canvas.Children.OfType<Node>();
                var enumerable = nodes as IList<Node> ?? nodes.ToList();
                foreach (var node in enumerable)
                {
                    
                    if (node.GroupId == Id)
                    {
                       groupNodes.Add(node);                       
                    }
                }
                int nodesCount = 1;
                foreach (var node in groupNodes)
                {

                    Canvas.SetTop(node, (nodesCount - 1) * 50 + 50 * nodesCount);
                    node.UpdatePosition();
                    nodesCount++;
                }
            }
        }

        private void DeleteButtonOnClick(object sender, RoutedEventArgs e)
        {
            Height -= 100;
            NodesCount--;
            var canvas = VisualTreeHelper.GetParent(this) as GraphCanvas;

            if (canvas != null)
            {
                Node node = canvas.NodeService.SourceNode;
                var connections = canvas.Children.OfType<Connection>();
                var enumerable = connections as IList<Connection> ?? connections.ToList();
                foreach (var connection in enumerable)
                {
                    if (connection.SourceNode.Equals(node) || connection.SinkNode.Equals(node))
                    {
                        canvas.Children.Remove(connection);
                    }
                }
                canvas.Children.Remove(node);
            }
            UpdateNodesPosition();

        }

        private void AddButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            AddButton.Visibility = Visibility.Hidden;
            OkButton.Visibility = Visibility.Visible;
            ExcelButton.Visibility = Visibility.Visible;
            var nodeName = GetTemplateChild("PART_NodeName") as TextBox;
            if (nodeName != null) nodeName.Visibility = Visibility.Visible;
            Height += 120;
        }

        private void NodeOnSelectionPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (AddButton.Visibility == Visibility.Hidden)
            {
                Height -= 80;
                AddButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Hidden;
                ChangeButton.Visibility = Visibility.Hidden;
            }
            else
            {
                Height += 80;
                AddButton.Visibility = Visibility.Hidden;
                DeleteButton.Visibility = Visibility.Visible;
                ChangeButton.Visibility = Visibility.Visible;
            }
        }

        public Guid Guid { get; set; }

        public int Id { get; set; }

        public int NodesCount { get; set; }

    }
}
