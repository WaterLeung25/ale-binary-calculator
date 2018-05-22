using System.Collections.Generic;
using System.Linq;

namespace ALE1_Yongshi_Liang
{
    public class SimplifiedTruthTable
    {
        //the column names of the simplified truth table
        public string SimpColumnNames;
        //the rows of the simplified truth table
        public List<string> SimpRows;
        //the true false values of the simplified truth table
        public List<string> SimpTfValues;
        //the truth table which need to simplified
        private TruthTable InputTable { get; }
        //get the input truth table
        public TruthTable GetInputTruthTable()
        {
            return this.InputTable;
        }
       
        public SimplifiedTruthTable(TruthTable tfTable)
        {
            this.InputTable = tfTable;
            this.SimpColumnNames = string.Empty;
            this.SimpRows = new List<string>();
            this.SimpTfValues = new List<string>();
        }

        //create a simplified truth table
        public void CreateSimplifiedTruthTable()
        {
            //convert the input truth table
            this.ConvertToSimplifiedTruthTable();

            //set the column names. column names are the same
            this.SimpColumnNames = this.InputTable.ColumnNames;

            //set the rows of the simplified truth table
            var row = "";
            for (var i = 0; i < SimpTfValues.Count; i++)
            {
                //each row is each element in the Tftable list
                row += SimpTfValues[i].PadLeft(10, ' ');
                //when the calculation of each possiblity is done
                if ((i + 1) % (this.InputTable.GetInputTree().Variables.Count + 1) != 0) continue;
                //add the row to the truth table in the listbox
                SimpRows.Add(row);
                //reset the row string
                row = "";
            }
        }

        //convert the generated truth table to a string list
        public void ConvertToSimplifiedTruthTable()
        {
            var tfTableValues = this.InputTable.TfValues.Select(i => i.ToString()).ToList();
            SimplifyTruthTable(new List<string>(), tfTableValues);
        }

        //simplify a truth table
        public void SimplifyTruthTable(List<string> simplifyTableValues, List<string> newTfTableValues)
        {
            //get all the strings of the list in one string to compare
            var simplifiedString = simplifyTableValues.Aggregate("", (current, s) => current + s);
            var tfTableString = newTfTableValues.Aggregate("", (current, s) => current + s);

            //if the table cannot be simplified anymore
            if (simplifiedString == tfTableString)
            {
                this.SimpTfValues = newTfTableValues;
                return;
            }
            //if not, the table continue being simplified
            simplifyTableValues = newTfTableValues;

            var oldPossbilities = new List<string>();
            var possibility = "";

            //get every possibility of the table which need to be simplified
            for (var i = 0; i < simplifyTableValues.Count; i++)
            {
                possibility += simplifyTableValues[i]; 
                if ((i + 1) % (this.InputTable.GetInputTree().Variables.Count + 1) != 0) continue;
                oldPossbilities.Add(possibility);
                possibility = "";
            }

            //check for each possibility see if it can be combined with other possibilities
            var newPossibilities = new List<string>();
            var combinedPossibilities = new List<string>();

            foreach (var outter in oldPossbilities)
            {
                foreach (var inner in oldPossbilities)
                {
                    //if these two possbilities cannot simplified, the outter one should be the same with the old possibility
                    if (!CanBeSimplified(inner, outter)) continue;
                    //get the new simplified possibilities
                    var newPossibility = CombineTwoPossibilities(inner, outter);
                    if (!newPossibilities.Contains(newPossibility))
                        newPossibilities.Add(newPossibility);
                    if (!combinedPossibilities.Contains(inner))
                        combinedPossibilities.Add(inner);
                    if (!combinedPossibilities.Contains(outter))
                        combinedPossibilities.Add(outter);
                }

                //keep the old possibilities if it cannot be simplified
                if (!combinedPossibilities.Contains(outter))
                {
                    newPossibilities.Add(outter);
                }
            }

            //go for the next recursion
            var simplifiedTable = new List<string>();

            //get all the strings of the list in one string for the next comparison
            foreach (var outer in newPossibilities)
            {
                foreach (var inner in outer)
                {
                    simplifiedTable.Add(inner.ToString());
                }
            }

            SimplifyTruthTable(simplifyTableValues, simplifiedTable);
        }

        //combine two possibilities if they can be simplified
        public string CombineTwoPossibilities(string p1, string p2)
        {
            //initialize a new possbility
            var newPoss = "";

            //start combine two possibilities
            for (var i = 0; i < p1.Length; i++)
            {
                //using * to instead of the different element
                if (p1[i] != p2[i])
                {
                    newPoss += "*";
                }
                else
                {
                    newPoss += p1[i];
                }
            }

            return newPoss;
        }

        //check whether two possibilities can be simplified
        public bool CanBeSimplified(string p1, string p2)
        {
            //check whether these two possibilities have the same value
            if (p1[p1.Length - 1] != p2[p2.Length - 1])//value is the last index of the string
                return false;

            var differentElement = 0;

            //put every element of a possibility into a new list
            var pos1 = p1.Select(c => c.ToString()).ToList();
            var pos2 = p2.Select(c => c.ToString()).ToList();

            //check whether two list of elements have different elements except the value
            for (var i = 0; i < pos1.Count - 1; i++)
            {
                if (pos1[i] != pos2[i])
                    differentElement++;
            }

            //only if there is only one different element then these two possibilities can be simplified
            return differentElement == 1;
        }
    }
}