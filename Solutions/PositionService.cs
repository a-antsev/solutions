using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Solutions.Controls;

namespace Solutions
{
    class PositionService
    {

        private readonly GraphCanvas _graphCanvas;

        public PositionService(GraphCanvas graphCanvas)
        {
            _graphCanvas = graphCanvas;
            _graphCanvas.LayoutUpdated += LayoutUpdated;
        }

        private void LayoutUpdated(object sender, EventArgs e)
        {
            foreach (var group in _graphCanvas.Children.OfType<Group>())
            {             
                Canvas.SetTop(group, 20);
                Canvas.SetLeft(group, (group.Id - 1) * 200 + 50 * group.Id);
            }

            //foreach (var node in _graphCanvas.Children.OfType<Node>())
            //{
            //    Canvas.SetLeft(node, node);
            //}
        }
    }
}
