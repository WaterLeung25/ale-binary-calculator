using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALE1_Yongshi_Liang
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private BinaryTree _binaryTree;

        //parse the input
        private void btn_Parse_Click(object sender, EventArgs e)
        {
            //get the input text
            var input = this.tb_Input.Text;
            //check the input text is valid or not
            if (input == "")
            {
                MessageBox.Show("Please enter a propositon");
                return;
            }
            if (input.Any(c => !System.Text.RegularExpressions.Regex.IsMatch(c.ToString(), "^[a-zA-Z~|&>=(),%]") && c != ' '))
            {
                this.lb_input.Text = "Invalid input! Hints: only a-z, A-Z, ~, |, &, >, =, (, ), ,, % are valid";
                this.lb_input.ForeColor = Color.Red;
                this.tb_Input.Focus();
                return;
            }
            //reset the text of the hints when the input is valid
            this.lb_input.Text = "";
            //initialize an empty binary tree
            _binaryTree = new BinaryTree();
            //generate the proposition tree
            _binaryTree.IniBinaryTreeByInputString(input);

            //initialize a graph with the generated binary tree
            var graph = new Graph(_binaryTree);   
            this.ShowGraphOfTheTree(graph);

            //show the infix notation of the proposition tree
            this.ShowInfix(_binaryTree);

            //show the prefix notation of the proposition tree
            this.ShowPrefix(_binaryTree);

            //initialize a truth table with the generated binary tree
            var tfTable = new TruthTable(_binaryTree);
            this.ShowTfTableAndHashCode(tfTable);

            //initialize a simplified truth table with the generated truth table
            var simpTfTable = new SimplifiedTruthTable(tfTable);
            this.ShowSimplifiedTfTable(simpTfTable);

            //initialize a disjunctive normal form with the generated truth table and the generated simplified truth table
            var dnf = new DisjunctiveNormalForm(tfTable, simpTfTable);
            this.ShowDisjunctiveNormalForm(dnf);

            //initialize a nand value with the generated binary tree
            var nand = new NandProposition(_binaryTree);
            this.ShowNand(nand);

        }

        //show the generated tree
        private void ShowGraphOfTheTree(Graph graph)
        {
            //get the root of the input tree
            var root = graph.GetInputTree().GetRoot();
            //convert the proposition tree to graph logic from the root of the tree
            graph.ConvertToGraph(root);
            //generate the graph of the proposition tree
            graph.GenerateGraph(root);
            //display the graph
            graph.ShowGraph(pictureBox1);
        }

        //show the infix notation
        private void ShowInfix(BinaryTree tree)
        {
            //get the root
            var root = tree.GetRoot();
            //the root of the proposition tree is contain the full infix expression
            tree.CalculateInfixString(root);
            //display the infix expression
            this.tb_Infix.Text = root.GetInfixString();
        }

        //show the infix notation
        private void ShowPrefix(BinaryTree tree)
        {
            //get the root
            var root = tree.GetRoot();
            //the root of the proposition tree is contain the full infix expression
            tree.CalculatePrefixString(root);
            //display the infix expression
            this.tb_Input.Text = root.GetPrefixString();
        }

        //show the truth table and the hash code
        private void ShowTfTableAndHashCode(TruthTable table)
        {
            //create the truth table based on the generated tree
            table.CreateTruthTable();
           
            listBox_TruthTb.Items.Clear();

            //add the column names to the list box
            listBox_TruthTb.Items.Add(table.ColumnNames);
            
            //add the rows to the list box
            foreach (var r in table.Rows)
            {
                listBox_TruthTb.Items.Add(r);
            }

            //output the hashcode
            tb_hashCode.Text = table.GetHashCodeOfTheTfTable();

        }

        //show the simplified truth table
        private void ShowSimplifiedTfTable(SimplifiedTruthTable table)
        {
            //create the truth table based on the generated tree
            table.CreateSimplifiedTruthTable();

            listBox_Simplified_TfTable.Items.Clear();

            //add the column names to the list box

            listBox_Simplified_TfTable.Items.Add(table.SimpColumnNames);

            //add the rows to the list box
            foreach (var r in table.SimpRows)
            {
                listBox_Simplified_TfTable.Items.Add(r);
            }
        }

        //show the disjunctive normal form
        private void ShowDisjunctiveNormalForm(DisjunctiveNormalForm dnf)
        {
            //in truth table
            dnf.GetDisjunctiveNormalForm(false);            
            tb_TFDisj_Infix.Text = dnf.DisjunctiveNormalFormInfix;
            tb_TFDisj_Prefix.Text = dnf.DisjunctiveNormalFormPrefix;

            //in simplified truth table
            dnf.GetDisjunctiveNormalForm(true);
            tb_STFDisj_Infix.Text = dnf.SimpDisjunctiveNormalFormInfix;
            tb_STFDisj_Prefix.Text = dnf.SimpDisjunctiveNormalFormPrefix;
        }

        //show the nand value of the proposition tree
        private void ShowNand(NandProposition nandProposition)
        {
            //set the nand value
            nandProposition.SetNandString(nandProposition.GetInputTree());
            //check whether the value is too long to show
            var nandLength = nandProposition.GetNandString().Length;
            tb_NAND.Text = nandLength <= 200 ? nandProposition.GetNandString() : "The length of the NANDs is larger than 200, unable to show in the text box.";
            
        }

        private void tb_Input_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.btn_Parse_Click(sender, e);
        }
    }
}
