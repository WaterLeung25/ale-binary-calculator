namespace ALE1_Yongshi_Liang
{
    public class NandProposition
    {
        private readonly BinaryTree _inputTree;
        private string NandString { get; set; }

        public BinaryTree GetInputTree()
        {
            return this._inputTree;
        }

        public NandProposition(BinaryTree tree)
        {
            this._inputTree = tree;
            this.NandString = "test";
        }

        public void SetNandString(BinaryTree tree)
        {
            if (_inputTree.GetRoot() == null) return;
            var root = _inputTree.GetRoot();
            
            CalculateNand(root);
            this.NandString = root.GetNANDValue();
        }

        public string GetNandString()
        {
            return this.NandString;
        }

        //calculate the NAND of the inputted proposition tree
        private void CalculateNand(Node node)
        {
            if (node == null) return;
            CalculateNand(node.GetLeft());
            CalculateNand(node.GetRight());
                
            if (node.GetRight() != null)
            {
                if (node.GetLeft() != null)
                {
                    //check whether every child node has its NANDs value or not. if not set the rest of nodes NANDs value to null
                    //because there should be something wrong while set the NANDs value of the node
                    if (node.GetLeft().GetNANDValue() == null || node.GetRight().GetNANDValue() == null)
                    {
                        node.SetNANDValue(null);
                        return;
                    }

                    if (node.GetRight().GetNANDValue() == null)
                    {
                        node.SetNANDValue(null);
                        return;
                    }
                }
            }
                
            //if the node is a variable, NANDs value is the same with its value
            if (System.Text.RegularExpressions.Regex.IsMatch(node.GetValue(), "^[a-zA-Z]"))
                node.SetNANDValue(node.GetValue());
            //if the node is ~, there is only on right child, calculate the NAND with its right child NANDs value
            if (node.GetValue() == "~")
                node.SetNANDValue(this.CalculateNanDsOfEachConnective("~", node.GetRight().GetNANDValue(), null));
            //calculate the rest connective
            if (!System.Text.RegularExpressions.Regex.IsMatch(node.GetValue(), "^[a-zA-Z]") && node.GetValue() != "~")
                node.SetNANDValue(this.CalculateNanDsOfEachConnective(node.GetValue(), node.GetLeft().GetNANDValue(), node.GetRight().GetNANDValue()));
        }

        //calculate the NANDs of each connective
        private string CalculateNanDsOfEachConnective(string connective, string leftValue, string rightValue)
        {
            switch (connective)
            {
                //~A --> ~(A&A)
                case "~":
                    return "%(" + leftValue + ", " + leftValue + ")";
                //A|B --> ~((~A)&(~B))
                case "|":
                    return "%(%(" + leftValue + ", " + leftValue + "), %(" + rightValue + ", " + rightValue + "))";
                //A&B --> ~((~A)|(~B)) --> ~(~(A&B))
                case "&":
                    return "%(%(" + leftValue + ", " + rightValue + "), %(" + leftValue + ", " + rightValue + "))";
                //A>B --> (~A)|B --> ~(A&(~B)) 
                case ">":
                    return "%(" + leftValue + ", %(" + rightValue + ",  " + rightValue + "))";
                //A=B --> ((~A)&(~B)|(A&B))
                case "=":
                    return CalculateNanDsOfEachConnective("|", CalculateNanDsOfEachConnective("&", CalculateNanDsOfEachConnective("~", leftValue, null), CalculateNanDsOfEachConnective("~", rightValue, null)), CalculateNanDsOfEachConnective("&", leftValue, rightValue));
                case "%":
                    return "%(" + leftValue + ", " + rightValue + ")";
            }
            return null;
        }
    }
}