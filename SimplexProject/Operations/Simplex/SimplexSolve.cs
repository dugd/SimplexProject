using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexProject.Operations.Simplex
{
    internal class SimplexSolve
    {
        public bool IsOptimal { get; set; }
        public double? ObjectiveValue { get; set; }
        public double[]? VariableValues { get; set; }

        public SimplexSolve(bool isOptimal, double? objectiveValue, double[]? variableValues)
        {
            IsOptimal = isOptimal;
            ObjectiveValue = objectiveValue;
            VariableValues = variableValues;
        }
    }
}
