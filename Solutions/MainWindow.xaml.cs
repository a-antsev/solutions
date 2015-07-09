using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Solutions.Controls;

namespace Solutions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Nodes = new List<Node>();
            ChainsList = new List<List<Node>>();
            ResultChains = new List<List<Node>>();
        }

        private void CreateGroupButton_Click(object sender, RoutedEventArgs e)
        {
            GraphCanvas.CreateGroup();
        }

        public List<List<Node>> ChainsList { get; set; }
        public List<List<Node>> ResultChains { get; set; }

        public List<Node> Nodes { get; set; }

        private void GenerateChainButton_Click(object sender, RoutedEventArgs e)
        {
            ChainsList.Clear();
            ResultChains.Clear();
            ChainList.Items.Clear();


            int groupsCount = GraphCanvas.Children.OfType<Group>().Count();
            int i = 0;
            if (!Nodes.Any())
            {
                Node parentNode = new Node(0);
                parentNode.Id = i;
                Nodes.Add(parentNode);
                foreach (var node in GraphCanvas.Children.OfType<Node>())
                {
                    i++;
                    node.Id = i;
                    Nodes.Add(node);
                    if (node.GroupId == 1)
                    {
                        Nodes[0].Children.Add(node);
                    }
                }
            }

            DFS(Nodes[0], new List<Node>());

#if DEBUG
            string str = "";
#endif
            foreach (var list in ChainsList)
            {
                if (list.Count == groupsCount)
                {
                    ResultChains.Add(new List<Node>(list));
#if DEBUG
                    foreach (var node in list)
                    {
                        str += node.Id;
                    }
                    ChainList.Items.Add(str);
                    str = "";
#endif
                }
            }
#if DEBUG
            ChainList.Visibility = Visibility.Visible;
#endif
            ChainList.Items.Clear();
            OptimisationService optimisationService = new OptimisationService();
            Dictionary<string, int>[] dictionaries = new Dictionary<string, int>[GraphCanvas.GroupsCount];
            foreach (var chain in ResultChains)
            {
                for (int j = 0; j < GraphCanvas.GroupsCount; j++)
                {
                    dictionaries[j] = chain[j].OptimisationParams;
                }
                optimisationService.BruteForce(dictionaries);
            }

            List<KeyValuePair<string, int>> result = optimisationService.GetResult();

            foreach (var res in result)
            {
                ChainList.Items.Add(res.Key);
            }
        }

        //private void BruteForce()
        //{

        //}

        public void DFS(Node node, List<Node> list)
        {
            if (!node.Children.Any())
            {
                ChainsList.Add(new List<Node>(list));
                list.Remove(node);
                return;
            }
            foreach (var child in node.Children)
            {
                list.Add(child);
                DFS(child, list);
            }
            list.Remove(node);
        }

    }
}
