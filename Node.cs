using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALE1_Yongshi_Liang
{
    public class Node
    {
        //the value of the node
        private string Value { get; set; }
        //the left child of the node
        private Node Left { get; set; }
        //the right child of the node
        private Node Right { get; set; }
        //the parent node of the node
        private Node Parent { get; set; }
        //the location of the node
        private Point Location { get; set; }

        //the Infix notation of the input formula
        private string InfixString { get; set; }
        //the Prefix notation of the input formula
        private string PrefixString { get; set; }

        //the true/false value of a node. The root will store the value of the whole tree
        private int TfVaule { get; set; }

        //the NAND value of a node. The root will store the value of the whole tree
        private string NandValue { get; set; }

        public Node(string value)
        {
            this.SetValue(value);
            this.SetLeft(null);
            this.SetRight(null);
            this.SetParent(null);

            this.SetPrefixString(string.Empty);
            this.SetInfixString(string.Empty);

            this.SetTfValue(-1);
        }
        //set the value of the node
        public void SetValue(string value)
        {
            this.Value = value;
        }
        //get the value of the node
        public string GetValue()
        {
            return this.Value;
        }
        //set the left child node
        public void SetLeft(Node left)
        {
            this.Left = left;
        }
        //get the left child node
        public Node GetLeft()
        {
            return this.Left;
        }
        //set the right child node
        public void SetRight(Node right)
        {
            this.Right = right;
        }
        //get the right child node
        public Node GetRight()
        {
            return this.Right;
        }
        //set the parent node
        public void SetParent(Node parent)
        {
            this.Parent = parent;
        }
        //get the parent node
        public Node GetParent()
        {
            return this.Parent;
        }
        //set the location of the node
        public void SetLocation(int x, int y)
        {
            this.Location = new Point(x,y);
        }
        //get the location of the node
        public Point GetLocation()
        {
            return this.Location;
        }
        //the root node will store the value of the prefix string
        public void SetPrefixString(string s)
        {
            this.PrefixString = s;
        }
        //get the prefix string of the node
        public string GetPrefixString()
        {
            return this.PrefixString;
        }
        //the root node will store the value of the infix string
        public void SetInfixString(string s)
        {
            this.InfixString = s;
        }
        //get the infix string of the node
        public string GetInfixString()
        {
            return this.InfixString;
        }
        //the root node will store the true/false
        public void SetTfValue(int value)
        {
            this.TfVaule = value;
        }
        //get the true/false value of the node
        public int GetTfValue()
        {
            return this.TfVaule;
        }
        //set the NAND value of the node
        public void SetNANDValue(string value)
        {
            this.NandValue = value;
        }
        //get the NAND value of the node
        public string GetNANDValue()
        {
            return this.NandValue;
        }
    }
}
