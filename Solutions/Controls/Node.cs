using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Office.Interop.Excel;
using Solutions.Annotations;
using Application = Microsoft.Office.Interop.Excel.Application;
using Point = System.Windows.Point;

namespace Solutions.Controls
{
    public class Node : Control, INotifyPropertyChanged
    {
        static Node()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Node), new FrameworkPropertyMetadata(typeof(Node)));
        }

        public Node()
        {

        }

        public Node(int groupId)
        {
            GroupId = groupId;
            Children = new List<Node>();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            UpdatePosition();
        }

        public void UpdatePosition()
        {
            var canvas = VisualTreeHelper.GetParent(this) as GraphCanvas;

            if (canvas != null)
            {
                Position = new Point(Canvas.GetLeft(this) + 75, Canvas.GetTop(this) + 25);
            }
        }


        private Point _position;

        public Point Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }

        public int Id { get; set; }

        public Guid Guid { get; set; }

        public double Param { get; set; }

        public List<Node> Children { get; set; }

        private string _excelPath;
        public string ExcelPath
        {
            get { return _excelPath; }
            set
            {
                _excelPath = value;
                OptimisationParams = SelectData(value);
                GC.Collect();
            }
        }

        public Dictionary<string,int> OptimisationParams { get; set; }

        public bool IsTableAdded
        {
            get { return (bool)GetValue(IsTableAddedProperty); }
            set { SetValue(IsTableAddedProperty, value); }
        }

        public static readonly DependencyProperty IsTableAddedProperty =
            DependencyProperty.Register("IsTableAdded", typeof(bool), typeof(Node));

        
        
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(Node));

        public int GroupId
        {
            get { return (int)GetValue(GroupIdProperty); }
            set { SetValue(GroupIdProperty, value); }
        }

        public static readonly DependencyProperty GroupIdProperty =
            DependencyProperty.Register("GroupId", typeof(int), typeof(Node));


        public string NodeName
        {
            get { return (string)GetValue(NodeNameProperty); }
            set { SetValue(NodeNameProperty, value); }
        }

        public static readonly DependencyProperty NodeNameProperty =
            DependencyProperty.Register("NodeName", typeof(string), typeof(Node));


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            var canvas = VisualTreeHelper.GetParent(this) as GraphCanvas;

            if (canvas != null)
            {
                canvas.NodeService.Connect(this);
            }

            e.Handled = true;
        }


        private static Dictionary<string, int> SelectData(string filename)
        {
            Application excelApplication = new Application();

            Workbooks workbooks = excelApplication.Workbooks;
            Workbook workbook = workbooks.Open(filename);
            Sheets sheets = workbook.Worksheets;
            Worksheet worksheet = (Worksheet)sheets.Item[1];

            Range firstColumn = worksheet.UsedRange.Columns[1];
            Range secondColumn = worksheet.UsedRange.Columns[2];
            var keys = (Array)firstColumn.Cells.Value;
            var values = (Array)secondColumn.Cells.Value;
            var keysArray = keys.OfType<object>().Select(o => o.ToString()).ToArray();
            var valuesArray = values.OfType<object>().Select(o => int.Parse(o.ToString())).ToArray();
            var result = new Dictionary<string, int>();
            for (int i = 0; i < keysArray.Count(); i++)
            {
                result.Add(keysArray[i], valuesArray[i]);
            }
            workbook.Close(filename);
            excelApplication.Quit();
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excelApplication);
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
