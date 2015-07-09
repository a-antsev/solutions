using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Solutions.Controls;

namespace Solutions
{
    class NodeService
    {

        private readonly GraphCanvas _graphCanvas;

        public NodeService(GraphCanvas graphCanvas)
        {
            _graphCanvas = graphCanvas;
        }

        private Node _sourceNode;

        public Node SourceNode
        {
            get { return _sourceNode; }
            private set
            {
                _sourceNode = value;
                if (value != null)
                {
                    //SourceSet();
                }
            }
        }

        //private void SourceNull()
        //{
        //    Group currentGroup = null;
        //    foreach (var group in _graphCanvas.Children.OfType<Group>())
        //    {
        //        if (group.Id == _sourceNode.GroupId)
        //        {
        //            currentGroup = group;
        //        }
        //    }

        //    if (currentGroup != null)
        //    {
        //        currentGroup.AddButton.Visibility = Visibility.Visible;
        //        currentGroup.DeleteButton.Visibility = Visibility.Hidden;
        //    }
        //}

        //private void SourceSet()
        //{
        //    Group currentGroup = null;
        //    foreach (var group in _graphCanvas.Children.OfType<Group>())
        //    {
        //        if (group.Id == _sourceNode.GroupId)
        //        {
        //            currentGroup = group;
        //        }
        //    }

        //    if (currentGroup != null)
        //    {
        //        currentGroup.AddButton.Visibility = Visibility.Hidden;
        //        currentGroup.DeleteButton.Visibility = Visibility.Visible;
        //    }
        //}

        public Connection SelectedConnection { get; set; }

        public void ClearSelection()
        {
            if (SelectedConnection != null)
            {
                Panel.SetZIndex(SelectedConnection, 0);
                SelectedConnection.IsSelected = false;
                SelectedConnection = null;
            }

            if (SourceNode != null && SourceNode.IsEditing == false)
            {
                SourceNode.IsSelected = false;
                //SourceNull();
                SourceNode = null;    
                
            }      
        }

        public void SelectConnection(Connection connection)
        {
            SelectedConnection = null; 
            SelectedConnection = connection;
            SelectedConnection.IsSelected = true;
            Panel.SetZIndex(SelectedConnection, 1);
          
        }

        public void Connect(Node node)
        {

            if (SourceNode == null)
            {
                SourceNode = node;
                SourceNode.IsSelected = true;
            }
            else
            {
                if (!SourceNode.Equals(node) && SourceNode.GroupId < node.GroupId && SourceNode.GroupId + 1 == node.GroupId)
                {                 
                    Connection connection = new Connection(SourceNode,node);
                    _graphCanvas.Children.Add(connection);
                    SourceNode.Children.Add(node);      
                    ClearSelection();
                }
                else
                {
                    if (SourceNode.GroupId == node.GroupId && !SourceNode.Equals(node))
                    {
                        ClearSelection();
                        SourceNode = node;
                        SourceNode.IsSelected = true;
                    }
                    else
                    {
                        ClearSelection();
                    }
                }
            }
        }
    }
}
