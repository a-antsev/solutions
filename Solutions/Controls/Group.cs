using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Solutions.Controls
{
    public class Group : Control
    {
        static Group()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Group), new FrameworkPropertyMetadata(typeof(Group)));
        }

        public Group()
        {
            NodesCount = 0;
            Nodes = new List<Node>();
            Loaded += OnLoaded;
        }

        public List<Node> Nodes { get; set; }

        public Button AddButton { get; set; }
        public Button DeleteButton { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            AddButton = GetTemplateChild("PART_AddNode") as Button;
            if (AddButton != null) AddButton.Click += AddButtonOnClick;
            DeleteButton = GetTemplateChild("PART_DeleteNode") as Button;
            if (DeleteButton != null) DeleteButton.Click += DeleteButtonOnClick;
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
            AddButton.Visibility = Visibility.Visible;
            DeleteButton.Visibility = Visibility.Hidden;
        }

        private void AddButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            NodesCount++;
            Node node = new Node(Id);
            Canvas.SetTop(node, (NodesCount - 1) * 50 + 50 * NodesCount);
            double groupLeft = Canvas.GetLeft(this);
            Canvas.SetLeft(node, groupLeft + 25);
            CreateNode createNode = new CreateNode();
            if (createNode.ShowDialog() == true)
            {
                node.NodeName = createNode.NodeName.Text;
                node.ExcelPath = createNode.ExcelPath.Text;
            }
            if (!string.IsNullOrEmpty(node.ExcelPath))
            {
                node.IsTableAdded = true;
            }
            Height += 100;
            var canvas = VisualTreeHelper.GetParent(this) as GraphCanvas;

            if (canvas != null)
            {
                canvas.Children.Add(node);
            }
        }

        public Guid Guid { get; set; }

        public int Id { get; set; }

        public int NodesCount { get; set; }

    }
}
