using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ALE1_Yongshi_Liang
{
    public class Graph
    {
        //the string used to make graph by "Graphv"
        private string _graphContent = "";
        //the binary tree which need to create a graph
        private BinaryTree InputTree { get; }
        //get the inputTree
        public BinaryTree GetInputTree()
        {
            return this.InputTree;
        }
        
        public Graph(BinaryTree tree)
        {
            this.InputTree = tree;
        }

        //convert the location to the graph logic
        public void ConvertToGraph(Node node)
        {
            if (node == null) return;

            //check if its the root
            if (node.GetParent() == null)
            {
                //the number of the root should be 1
                this._graphContent += "node1 [ label = \"" + node.GetValue() + "\" ]";
            }
            else
            {
                //previous node number
                var pvnum = Math.Pow(2, node.GetParent().GetLocation().X) + node.GetParent().GetLocation().Y - 1;
                //current node number
                var crnum = Math.Pow(2, node.GetLocation().X) + node.GetLocation().Y - 1;
                //set the connection of those node
                var connect = "\r\n" + "node" + pvnum + " -- node" + crnum + "\r\n";
                //write the graph logic line
                var line = connect + "node" + crnum + " [label =\"" + node.GetValue() + "\" ]";
                //build graph content with lines
                this._graphContent += line;
            }

            ConvertToGraph(node.GetLeft());
            ConvertToGraph(node.GetRight());
        }

        //generate the graph
        public void GenerateGraph(Node root)
        {
            try
            {
                this._graphContent = "";
                //start to convert from the root
                this.ConvertToGraph(root);

                //create new .dot file with the full graph logic
                var fs = new FileStream("logic.dot", FileMode.Create, FileAccess.Write);
                var sw = new StreamWriter(fs);
                sw.WriteLine("graph logic {" + "\r\n" + "node [ fontname = \"Arial\" ]");
                sw.WriteLine(this._graphContent);
                sw.WriteLine("}");

                sw.Dispose();
                fs.Dispose();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        //show graph
        public void ShowGraph(PictureBox pb)
        {
            var pathfound = false;
            var path = "";
            //get all the paths
            var environmentpahts = Environment.GetEnvironmentVariable("Path")?.Split(';');
            if (environmentpahts == null) return;
            foreach (var p in environmentpahts)
            {
                //set the path if it adapted
                if (!p.ToUpper().Contains(@"graphviz2.38\bin".ToUpper())) continue;
                path = p;
                pathfound = true;
            }
            //if the cannot find the correct path
            if (!pathfound)
            {
                MessageBox.Show(@"Please adapt your $PATH environment variable");
            }

            var dot = new Process { StartInfo = { FileName = path + @"\dot.exe" } };
            //set the graph name
            const string name = "new_tree.png"; //+ DateTime.Now.Date + DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                                       //DateTime.Now.Minute.ToString().PadLeft(2, '0');
            dot.StartInfo.Arguments = "-Tpng -o " + name + " logic.dot";
            dot.Start();
            dot.WaitForExit();
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.ImageLocation = name;
        }
    }
}