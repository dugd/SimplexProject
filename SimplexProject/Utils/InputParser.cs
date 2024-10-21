using SimplexProject.Core;
using SimplexProject.Enums;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace SimplexProject.Utils
{
    internal class InputParser
    {
        public bool CoefficientsParse(string line, int size, out double[] result)
        {
            result = new double[size];

            string[] splited = line.Split();

            if (splited.Length != size)
            {
                return false;
            }

            for (int i = 0; i < splited.Length; i++)
            {
                if (double.TryParse(splited[i], out double value))
                {
                    result[i] = value;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public bool ConstraintParse(string line, int size, 
            out double[] coefficients, 
            out RelationType relation, 
            out double RHS)
        {
            coefficients = Array.Empty<double>();
            relation = 0;
            RHS = 0;

            
            string[] splited = line.Split();

            if (splited.Length != size + 2)
            {
                return false;
            }

            string coefficientsLine = string.Empty;
            for (int i = 0; i < size; i++)
            {
                coefficientsLine += splited[i];
            }
            string relationLine = splited[size];
            string RHSLine = splited[size + 1];

            if (!CoefficientsParse(coefficientsLine, size, out coefficients))
            {
                return false;
            }

            // TODO: relation and RHS parse

            return true;
        }

        // TODO: Separate console input and string parse
        public LPTask ConsoleParse()
        {
            throw new NotImplementedException();

            // Num of constraints/variables
            // (for (for Constraints) + Relation + RHS) input
            // (for Objective Function) input
            // ObjectibeType input
        }
    }
}
