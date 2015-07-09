using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Solutions.Annotations;

namespace Solutions.Controls
{
    public class Connection : Control, INotifyPropertyChanged
    {
        static Connection()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Connection), new FrameworkPropertyMetadata(typeof(Connection)));
        }

        public Connection()
        {
            
        }

        public Connection(Node sourceNode, Node sinkNode)
        {
            SourceNode = sourceNode;
            SinkNode = sinkNode;
            UpdatePathGeometry();
            //LayoutUpdated += OnLayoutUpdated;
            Loaded += OnLoaded;
            SourceNode.PositionPropertyChanged += PositionPropertyChanged;
            SinkNode.PositionPropertyChanged += PositionPropertyChanged;
        }

        void PositionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdatePathGeometry();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Button deleteButton = GetTemplateChild("DeleteButton") as Button;
            if (deleteButton != null) deleteButton.Click += DeleteButtonOnClick;
        }

        private void DeleteButtonOnClick(object sender, RoutedEventArgs e)
        {
            GraphCanvas canvas = VisualTreeHelper.GetParent(this) as GraphCanvas;
            if (canvas != null)
            {
                SourceNode.Children.Remove(SinkNode);
                canvas.Children.Remove(this);
            }
        }

        private void OnLayoutUpdated(object sender, EventArgs eventArgs)
        {
            UpdatePathGeometry();
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(Connection));

        

        public Guid Id { get; set; }

        private PathGeometry _pathGeometry;
        public PathGeometry PathGeometry
        {
            get { return _pathGeometry; }
            set
            {
                if (_pathGeometry != value)
                {
                    _pathGeometry = value;
                    OnLayoutChanged("PathGeometry");
                }
            }
        }

        private Point _buttonPosition;

        public Point ButtonPosition
        {
            get { return _buttonPosition; }
            set
            {
                _buttonPosition = value; 
                OnLayoutChanged("ButtonPosition");
            }
        }
        

        public Node SourceNode { get; set; }
        public Node SinkNode { get; set; }

        private void UpdatePathGeometry()
        {
            if (SourceNode != null && SinkNode != null)
            {
                ButtonPosition = 
                    new Point(SourceNode.Position.X + 100,
                    SourceNode.Position.Y - (SourceNode.Position.Y - SinkNode.Position.Y) / 2 - 25);
                PathGeometry geometry = new PathGeometry();
                PathFigure figure = new PathFigure();
                Point startPoint = new Point(SourceNode.Position.X + 75, SourceNode.Position.Y);
                figure.StartPoint = startPoint;
                Point firstPoint = new Point(SourceNode.Position.X + 125, SourceNode.Position.Y);
                Point secondPoint = new Point(SinkNode.Position.X - 125, SinkNode.Position.Y);
                Point thirdPoint = new Point(SinkNode.Position.X - 75, SinkNode.Position.Y);
                figure.Segments.Add(new BezierSegment(firstPoint,secondPoint,thirdPoint,true));
                geometry.Figures.Add(figure);
                PathGeometry = geometry;
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            GraphCanvas canvas = VisualTreeHelper.GetParent(this) as GraphCanvas;
            if (canvas != null)
            {
                canvas.NodeService.ClearSelection();
                canvas.NodeService.SelectConnection(this);

            }
            e.Handled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnLayoutChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
