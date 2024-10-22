using SimplexProject.Core;
using SimplexProject.Enums;

namespace SimplexProject.Utils
{
    internal class InputParser
    {
        public bool ParseCoefficients(string line, int size, out double[] result)
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

        public bool ParseRelation(string line, out RelationType result)
        {
            switch (line)
            {
                case "<=":
                    result = RelationType.LessEqual;
                    break;
                case ">=":
                    result = RelationType.GreaterEqual;
                    break;
                case "=":
                    result = RelationType.Equal;
                    break;
                default:
                    result = default;
                    return false;
            }

            return true;
        }

        public bool ParseConstraint(string line, int size, 
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

            string coefficientsLine = string.Join(" ", splited, 0, size);
            string relationLine = splited[size];
            string RHSLine = splited[size + 1];

            return ParseCoefficients(coefficientsLine, size, out coefficients) &&
                ParseRelation(relationLine, out relation) &&
                double.TryParse(RHSLine, out RHS);
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
