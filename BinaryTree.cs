using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALE1_Yongshi_Liang
{
    public class BinaryTree
    {
        //the input prefix string, with/without brackets
        private string _inputString = string.Empty;

        //the current index of InputString
        private int _index = 1;

        //the root of the tree
        private Node Root { get; set; }
        //get the root of the tree
        public Node GetRoot()
        {
            return this.Root;
        }

        //store all the leaves of the tree
        public List<Node> Leaves { get; private set; }

        //store all the variables
        public List<string> Variables { get; }

        public BinaryTree()
        {
            //initialize the leaves
            Leaves = new List<Node>();
            //initialize the variables
            Variables = new List<string>();                        
        }

        //calculate the infix expression by tree (with brackets)
        public void CalculateInfixString(Node node)
        {
            //using the postfix evaluation algorithm to go through every node from the leaves to the root

            //do the recusion to start from leaves
            if (node == null) return;
            CalculateInfixString(node.GetLeft());
            CalculateInfixString(node.GetRight());

            //the variable's infix expression should be the same as itself
            if (System.Text.RegularExpressions.Regex.IsMatch(node.GetValue(), "^[a-zA-Z]"))
                node.SetInfixString(node.GetValue());
            //the ~'s infix expression should with its described variable, that variable is its right child node
            if (node.GetValue() == "~")
                node.SetInfixString(node.GetValue() + node.GetRight().GetInfixString());
            //set the root to contain the expression as its subtree value
            if (!System.Text.RegularExpressions.Regex.IsMatch(node.GetValue(), "^[a-zA-Z]") && node.GetValue() != "~")
            {              
                if (node == Root)
                {
                    //the root of the entire tree no need to in a bracket
                    node.SetInfixString(node.GetLeft().GetInfixString() + " " + node.GetValue() + " " + node.GetRight().GetInfixString());
                }
                else
                {   
                    //the root of the subtree should be in a bracket
                    node.SetInfixString(" ( " + node.GetLeft().GetInfixString() + " " + node.GetValue() + " " + node.GetRight().GetInfixString() + " ) ");
                }
            }
        }

        //calculate the prefix notation string (with brackets) based on the tree
        public void CalculatePrefixString(Node node)
        {
            if (node != null)
            {
                CalculatePrefixString(node.GetLeft()); 
                CalculatePrefixString(node.GetRight()); 

                if (System.Text.RegularExpressions.Regex.IsMatch(node.GetValue(), "^[a-zA-Z]"))
                    node.SetPrefixString(node.GetValue());
                if (node.GetValue() == "~")
                    node.SetPrefixString(node.GetValue() + " " + node.GetRight().GetPrefixString());
                if (!System.Text.RegularExpressions.Regex.IsMatch(node.GetValue(), "^[a-zA-Z]") && node.GetValue() != "~")
                    node.SetPrefixString(" " + node.GetValue() + " ( " + node.GetLeft().GetPrefixString() + " , " + node.GetRight().GetPrefixString() + " ) ");
            }
        }

        //generate a tree by input string
        public void IniBinaryTreeByInputString(string inputstr)
        {
            _inputString = "";

            //remove brackets and commas
            foreach (var c in inputstr)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(c.ToString(), "^[a-zA-Z~|&>=%]"))
                    _inputString += c.ToString();
            }

            //the root of the tree is the first letter of the input, since the input string is in prefix notation
            var rootValue = _inputString.Substring(0, 1);
            //set the root of the tree with its value
            Root = new Node(rootValue);
            //set the root location
            Root.SetLocation(0, 1);

            //generate the binary tree
            this.GenerateTree(Root);

            //initialize all the leaves
            this.Leaves = new List<Node>();
            //gather all the leaves
            this.GetAllLeaves(Root);

        }

        public void GenerateTree(Node currNode)
        {
            //special case when the next symbol is ~, it should combined with the next variable 
            if (currNode.GetValue() == "~")
            {
                var newNode = new Node(_inputString[_index].ToString());
                currNode.SetRight(newNode);
                newNode.SetParent(currNode);
                newNode.SetLocation(currNode.GetLocation().X + 1, currNode.GetLocation().Y *2);

                _index++;

                GenerateTree(currNode.GetRight());
            }
            //if the node is a variable means it has no child node in a proposition tree
            if (System.Text.RegularExpressions.Regex.IsMatch(currNode.GetValue(), "^[a-zA-Z]"))
            {
                return;
            }
            //if the node is not a variable except ~
            if (!System.Text.RegularExpressions.Regex.IsMatch(currNode.GetValue(), "^[a-zA-Z]") && currNode.GetValue() != "~")
            {
                //add the left subtree first
                var newNode = new Node(_inputString[_index].ToString());
                newNode.SetParent(currNode);
                newNode.SetLocation(currNode.GetLocation().X + 1, currNode.GetLocation().Y * 2 -1);
                currNode.SetLeft(newNode);
                _index++;
                //the left child of the current node is the root of the left subtree
                GenerateTree(currNode.GetLeft());

                //then add the right subtree
                newNode = new Node(_inputString[_index].ToString());
                newNode.SetParent(currNode);
                newNode.SetLocation(currNode.GetLocation().X + 1, currNode.GetLocation().Y * 2);
                currNode.SetRight(newNode);
                _index++;
                //the right child of the current node is the root of the right subtree
                GenerateTree(currNode.GetRight());
            }
        }

        //get all the leaves of the tree
        public void GetAllLeaves(Node node)
        {
            if (node == null) return;

            //the node have no child node is a leave
            if (node.GetLeft() == null && node.GetRight() == null)
            {
                this.Leaves.Add(node);

                //get the new occur variable
                if (!Variables.Contains(node.GetValue()))
                {
                    Variables.Add(node.GetValue());
                }
            }
            GetAllLeaves(node.GetLeft());
            GetAllLeaves(node.GetRight());
        }
                             
    }
}
