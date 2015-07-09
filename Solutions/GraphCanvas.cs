using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Solutions.Controls;

namespace Solutions
{
    class GraphCanvas : Canvas
    {
        public GraphCanvas()
        {
            GroupsCount = 0;
        }

        public int GroupsCount { get; set; }

        //Сервис создающий соединения между верщинами графа  
        private NodeService _nodeService;

        public NodeService NodeService
        {
            get { return _nodeService ?? (_nodeService = new NodeService(this)); }
        }

        //Сервис распологающий элементы на канве
        private PositionService _positionService;

        public PositionService PositionService
        {
            get { return _positionService ?? (_positionService = new PositionService(this)); }
        }

        //protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    base.OnMouseLeftButtonDown(e);
        //    NodeService.ClearSelection();
        //}

        //Создание группы
        public void CreateGroup()
        {
            Group group = new Group();
            group.Id = GroupsCount + 1;
            group.Height = 110;
            SetTop(group,20);
            SetLeft(group, (group.Id - 1) * 200 + 50 * group.Id);
            Children.Add(group);
            GroupsCount++;
        }
    }
}
