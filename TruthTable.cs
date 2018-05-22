using System;
using System.Collections.Generic;

namespace ALE1_Yongshi_Liang
{
    public class TruthTable
    {
        //the column names of the truth table
        public string ColumnNames;
        //the rows of the truth table
        public List<string> Rows;
        //the true false values of the truth table
        public List<int> TfValues;
        //the result of each row of the truth table
        private string _binaryNumber = "";
        //the hash code of the truth table
        private string HashCodeOfTfTable { get; set; }
        //get the hash code of the truth table
        public string GetHashCodeOfTheTfTable()
        {
            return this.HashCodeOfTfTable;
        }
        //the binary tree which need to create a truth table
        private BinaryTree InputTree { get; }
        //get the inputTree
        public BinaryTree GetInputTree()
        {
            return this.InputTree;
        }

        public TruthTable(BinaryTree tree)
        {
            this.InputTree = tree;
            this.ColumnNames = string.Empty;
            this.TfValues = new List<int>();
            this.Rows = new List<string>();
        }
        //calculate the true/fales of each connective
        private static int CalculateEachConnective(string connective, int leftValue, int rightValue)
        {
            switch (connective)
            {
                //OR 
                case "|":
                    if (leftValue == 0 && rightValue == 0)
                        return 0;
                    else
                        return 1;
                //AND
                case "&":
                    if (leftValue == 1 && rightValue == 1)
                        return 1;
                    else
                        return 0;
                //IFF
                case "=":
                    return leftValue == rightValue ? 1 : 0;
                //IMPLY
                case ">":
                    if (leftValue == 1 && rightValue == 0)
                        return 0;
                    else
                        return 1;
                //NAND
                case "%":
                    if (leftValue == 1 && rightValue == 1)
                        return 0;
                    else
                        return 1;
                default:
                    break;
            }

            //return the defalut tf value
            return -1;
        }

        //calculate the true/false of each node in postorder
        public void CalculateTfForNodes(Node node)
        {
            if (node == null) return;

            CalculateTfForNodes(node.GetLeft());
            CalculateTfForNodes(node.GetRight());

            //don't need to calculate the true/false for the variables
            if (System.Text.RegularExpressions.Regex.IsMatch(node.GetValue(), "^[a-zA-Z]") && node.GetValue() != "~")
                return;

            //if the node is "not", the true/false value will be the oppsite value 
            if (node.GetValue() == "~")
                node.SetTfValue(node.GetRight().GetTfValue() == 1 ? 0 : 1);

            //if the node is a connective except "not"
            if (!System.Text.RegularExpressions.Regex.IsMatch(node.GetValue(), "^[a-zA-Z]") && node.GetValue() != "~")
            {
                var value = CalculateEachConnective(node.GetValue(), node.GetLeft().GetTfValue(), node.GetRight().GetTfValue());
                node.SetTfValue(value);
            }
        }

        //create a truth table
        public void CreateTruthTable()
        {
            /*the idea of showing truth table in a listbox 
              ~A will be show like
              A~A; 01; 10
             */
            //set the columns of the truth table
            foreach (var v in InputTree.Variables)
            {
                //one variable is a column name 
                ColumnNames += v.PadLeft(10, ' ');
            }
            //the Truth Value is also a column name
            ColumnNames += "Values".PadLeft(10, ' ');

            //get the root of the input binary tree
            var root = InputTree.GetRoot();
            //get all the variables by getting leaves
            InputTree.GetAllLeaves(root);

            //get the number of the variables
            var count = InputTree.Variables.Count;
            //calculate all the possibilities
            var p = Math.Pow(2, count);

            //go through all the possibilities
            for (var i = 0; i < p; i++)
            {
                //create a binary string as the true/false to show in a line
                var binaryCode = Convert.ToString(i, 2).PadLeft(count, '0');

                //giving true or false to every variables
                for (var j = 0; j < count; j++)
                {
                    foreach (var l in InputTree.Leaves)
                    {
                        //if the leave is a variable, set every tf value of the variable to 0(false) or 1(true)
                        if (l.GetValue() == InputTree.Variables[j])
                        {
                            l.SetTfValue(int.Parse(binaryCode[j].ToString()));
                        }
                    }

                    //add all the values to the truth table list
                    this.TfValues.Add(Convert.ToInt32(binaryCode[j].ToString()));
                }

                //do the tf calculation of the tree from the root
                this.CalculateTfForNodes(root);
                //add the result of the calculation to tf values of the truth table
                this.TfValues.Add(root.GetTfValue());
            }

            //set the rows of the truth table
            var row = "";
            for (var i = 0; i < TfValues.Count; i++)
            {
                //each row is each element in the Tftable list
                row += TfValues[i].ToString().PadLeft(10, ' ');
                //when the calculation of each possiblity is done
                if ((i + 1) % (InputTree.Variables.Count + 1) != 0) continue;
                //add the row to the truth table in the listbox
                Rows.Add(row);
                //reset the row string
                row = "";
                //get value of the result cell in each row
                _binaryNumber += TfValues[i];
            }

            CalculateHashCode();
        }

        //calculate the hash code
        public void CalculateHashCode()
        {
            //convert the binaryNum to a char array for reversing 
            var array = _binaryNumber.ToCharArray();

            //convert to the hashcode should get the string after the reversing
            for (var i = array.Length - 1; i > -1; i--)
            {
                HashCodeOfTfTable += array[i];
            }

            //set the hashcode
            HashCodeOfTfTable = Convert.ToString(Convert.ToInt32(HashCodeOfTfTable, 2), 16);
        }
    }
}