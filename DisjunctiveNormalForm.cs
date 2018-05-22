using System;
using System.Collections.Generic;

namespace ALE1_Yongshi_Liang
{
    public class DisjunctiveNormalForm
    {
        //store all the elements of the disjunctive form
        private readonly List<string> _disjunctiveFormFormulas;

        //prefix notaion of the disjunctive normal form in the Truth table
        public string DisjunctiveNormalFormPrefix { get; private set; }

        //infix notation of the disjunctive normal form in the Truth table
        public string DisjunctiveNormalFormInfix { get; private set; }

        //prefix notation of the disjunctive normal form in the Simplified Truth table
        public string SimpDisjunctiveNormalFormPrefix { get; private set; }

        //infix notation of the disjunctive normal form in the Simplified Truth table
        public string SimpDisjunctiveNormalFormInfix { get; private set; }

        //the truth table which need to create a DNF
        private readonly TruthTable _inpuTruthTable;

        //the simplified truth table which need to create a DNF
        private readonly SimplifiedTruthTable _inputSimplifiedTruthTable;

        public DisjunctiveNormalForm(TruthTable truthTable, SimplifiedTruthTable simplifiedTruthTable)
        {
            this._inpuTruthTable = truthTable;
            this._inputSimplifiedTruthTable = simplifiedTruthTable;

            //initialize the disjunctive form formulas list
            _disjunctiveFormFormulas = new List<string>();
            //initialize the prefix notation of the disjunctive form in the Truth table 
            DisjunctiveNormalFormPrefix = string.Empty;
            //initialize the infix notation of the disjuctive form in the Truth table
            DisjunctiveNormalFormInfix = string.Empty;
            //initialize the prefix notation of the disjunctive form in the Simplified Truth table
            SimpDisjunctiveNormalFormPrefix = string.Empty;
            //initialize the infix notation of the disjuctive form in the Simplified Truth table
            SimpDisjunctiveNormalFormInfix = string.Empty;
        }

        //get the disjunctive normal form
        public void GetDisjunctiveNormalForm(bool simplified)
        {
            //in infix notaion
            DisjunctiveNormalFormInfix = string.Empty;
            SimpDisjunctiveNormalFormInfix = string.Empty;
            GroupFormulasAsDisjoints(simplified, "infix", 0);

            //in prefix notaion
            DisjunctiveNormalFormPrefix = string.Empty;
            SimpDisjunctiveNormalFormPrefix = string.Empty;
            GroupFormulasAsDisjoints(simplified, "prefix", 0);
        }

        //grouping all the formulas as disjoints together and gives the disjunctive normal form
        private void GroupFormulasAsDisjoints(bool simplified, string notation, int startIndex)
        {
            //initialize the formulas list
            _disjunctiveFormFormulas.Clear();

            //get the disjunctive normal form in the truth table
            if (!simplified)
            {
                //start from the first row of the truth table
                GetDisjunctiveNormalFormFormulas(notation, startIndex);
                if (notation.Equals("infix"))
                {
                    //Grouping all formulas in infix notation
                    if (_disjunctiveFormFormulas.Count > 0)
                    {
                        DisjunctiveNormalFormInfix = DisjunctiveAllElements("infix", _disjunctiveFormFormulas);
                    }
                    else
                    {
                        DisjunctiveNormalFormInfix = _inpuTruthTable.TfValues[_inpuTruthTable.TfValues.Count - 1] == 1 ? "1" : "0";
                    }
                }
                else if (notation.Equals("prefix"))
                {
                    //Grouping all formulas in prefix notation
                    if (_disjunctiveFormFormulas.Count > 0)
                    {
                        DisjunctiveNormalFormPrefix = DisjunctiveAllElements("prefix", _disjunctiveFormFormulas);
                    }
                    else
                    {
                        DisjunctiveNormalFormPrefix = _inpuTruthTable.TfValues[_inpuTruthTable.TfValues.Count - 1] == 1 ? "1" : "0";
                    }
                }
            }
            else //in Simplified TFtable
            {
                //start from the first row of the simplified truth table
                GetSimplifiedDisjunctiveNormalFormFormulas(notation, startIndex);
                _disjunctiveFormFormulas.Reverse();
                if (notation.Equals("infix"))
                {
                    //Grouping all formulas in infix notation
                    if (_disjunctiveFormFormulas.Count > 0)
                    {
                        SimpDisjunctiveNormalFormInfix = DisjunctiveAllElements("infix", _disjunctiveFormFormulas);
                    }
                    else
                    {
                        SimpDisjunctiveNormalFormInfix = _inpuTruthTable.TfValues[_inputSimplifiedTruthTable.SimpTfValues.Count - 1] == 1 ? "1" : "0";
                    }
                }
                else if (notation.Equals("prefix"))
                {
                    //Grouping all formulas in prefix notation
                    if (_disjunctiveFormFormulas.Count > 0)
                    {
                        SimpDisjunctiveNormalFormPrefix = DisjunctiveAllElements("prefix", _disjunctiveFormFormulas);
                    }
                    else
                    {
                        SimpDisjunctiveNormalFormPrefix = _inpuTruthTable.TfValues[_inputSimplifiedTruthTable.SimpTfValues.Count - 1] == 1 ? "1" : "0";
                    }
                }
            }
        }

        //get the disjunctive normal form from the turth table
        public void GetDisjunctiveNormalFormFormulas(string notation, int startIndex)
        {
            //get the true false values of the truth table
            var tfValues = _inpuTruthTable.TfValues;
            //get the variables from the truth table
            var variables = this._inpuTruthTable.GetInputTree().Variables;
            //check whether the result of this row of the truth table is 1
            var index = tfValues[startIndex + variables.Count];
            if (Convert.ToInt32(index) == 1)
            {
                var elements = new List<string>();

                for (var i = 0; i < variables.Count; i++)
                {
                    //Take for every row with a truth value of 1 the conjunction of the predicates with value 1 
                    if (Convert.ToInt32(tfValues[startIndex + i]) == 1)
                    {
                        elements.Add(variables[i]);
                    }
                    //the negation of the predicates with value 0.
                    else
                    {
                        elements.Add(" ~ " + variables[i]);
                    }

                }

                //get all the formulas
                if (elements.Count > 0)
                {
                    _disjunctiveFormFormulas.Add(ConjunctiveAllElements(notation, elements));
                }
            }

            //go through each row of the truth table
            if (startIndex < tfValues.Count - variables.Count - 1)
            {
                GetDisjunctiveNormalFormFormulas(notation, startIndex + variables.Count + 1);
            }
        }

        //get the disjunctive normal form from the simplified truth table
        public void GetSimplifiedDisjunctiveNormalFormFormulas(string notation, int startIndex)
        {
            //get the true false values of the simplified truth table
            var simpTfValues = _inputSimplifiedTruthTable.SimpTfValues;
            //get the variables from the simplified truth table
            var variables = _inputSimplifiedTruthTable.GetInputTruthTable().GetInputTree().Variables;
            //check whether the result of this row of the simplified truth table is 1
            var index = simpTfValues[startIndex + variables.Count];
            //check whether the result of this row of the simplified truth table is 1
            if (Convert.ToInt32(index) == 1)
            {
                var elements = new List<string>();
                for (var i = 0; i < variables.Count; i++)
                {
                    if (simpTfValues[startIndex + i] == "*")
                    {
                        continue;
                    }

                    switch (Convert.ToInt32(simpTfValues[startIndex + i]))
                    {   //Take for every row with a simplified truth value of 1 the conjunction of the predicates with value 1
                        case 1:
                            elements.Add(variables[i]);
                            break;
                        //the negation of the predicates with value 0.
                        case 0:
                            elements.Add(" ~ " + variables[i]);
                            break;
                    }

                }
                //get all the formulas
                if (elements.Count > 0)
                {
                    _disjunctiveFormFormulas.Add(ConjunctiveAllElements(notation, elements));
                }
            }

            //go through each row of the simplified truth table
            if (startIndex < simpTfValues.Count - variables.Count - 1)
            {
                GetSimplifiedDisjunctiveNormalFormFormulas(notation, startIndex + variables.Count + 1);
            }
        }

        //group all elements to be one conjunctive form in infix notation
        private string ConjunctiveAllElements(string notation, List<string> elements)
        {
            //return the value of the only variable
            if (elements.Count == 1) return elements[0];

            var formula = "";
            if (notation.Equals("infix"))
            {
                //initialize the formula with 2 variables
                formula = "( " + elements[0] + " & " + elements[1] + " ) ";
                //if the formula has more the two variables
                for (var i = 2; i < elements.Count; i++)
                {
                    formula = "( " + formula + " & " + elements[i] + " ) ";
                }
            }
            else if (notation.Equals("prefix"))
            {
                formula = " & ( " + elements[0] + " , " + elements[1] + " ) ";
                for (var i = 2; i < elements.Count; i++)
                {
                    formula = " & ( " + formula + " , " + elements[i] + " ) ";
                }
            }

            return formula;
        }

        //group all formulas to be one conjunctive form in infix notation
        private string DisjunctiveAllElements(string notation, List<string> elements)
        {
            //return the value of the only formula
            if (elements.Count == 1)
            {
                if (notation.Equals("infix"))
                {
                    return elements[0].Replace("(", "").Replace(")", "");
                }
                return elements[0];
            }

            var form = "";
            if (notation.Equals("infix"))
            {
                //initialize the form with 2 formulas
                form = elements[0] + " | " + elements[1] + " ";
                //if the form has more the two formulas
                for (var i = 2; i < elements.Count; i++)
                {
                    form = form + " | " + elements[i] + " ";
                }
            }
            else if (notation.Equals("prefix"))
            {
                form = " | ( " + elements[0] + " , " + elements[1] + " ) ";
                for (var i = 2; i < elements.Count; i++)
                {
                    form = " | ( " + form + " , " + elements[i] + " ) ";
                }
            }

            return form;
        }
    }
}