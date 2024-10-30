using SimplexProject.Enums;
using System.Globalization;

namespace SimplexProject.Utilities
{
    internal class TextParser
    {
        private readonly char[] defaultSeparators = new[] { ' ', '\t' };

        public (double[] coefficients, bool isValid) ParseCoefficients(string line, int size)
        {
            double[] result = new double[size];
            string[] splited = line.Split(defaultSeparators, StringSplitOptions.RemoveEmptyEntries);

            if (splited.Length != size)
            {
                return (Array.Empty<double>(), false);
            }

            for (int i = 0; i < splited.Length; i++)
            {
                if (double.TryParse(splited[i], NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {
                    result[i] = value;
                }
                else
                {
                    return (Array.Empty<double>(), false);
                }
            }

            return (result, true);
        }

        public (RelationType relation, bool isValid) ParseRelation(string line)
        {
            try
            {
                return (RelationTypeExtensions.FromDescriptionString(line), true);
            }
            catch (ArgumentException)
            {
                return (default, false);
            }
        }

        public (double[] coefficients, RelationType relation, double RHS, bool isValid) ParseConstraint(string line, int size)
        {
            string[] splited = line.Split(defaultSeparators, StringSplitOptions.RemoveEmptyEntries);

            if (splited.Length != size + 2)
            {
                return (Array.Empty<double>(), default, 0, false);
            }

            string coefficientsLine = string.Join(" ", splited, 0, size);
            string relationLine = splited[size];
            string RHSLine = splited[size + 1];

            var (coefficients, coeffIsValid) = ParseCoefficients(coefficientsLine, size);
            if (!coeffIsValid)
            {
                return (Array.Empty<double>(), default, 0, false);
            }

            var (relation, relationIsValid) = ParseRelation(relationLine);
            if (!relationIsValid)
            {
                return (Array.Empty<double>(), default, 0, false);
            }

            if (!double.TryParse(RHSLine, NumberStyles.Float, CultureInfo.InvariantCulture, out double RHS))
            {
                return (Array.Empty<double>(), default, 0, false);
            }

            return (coefficients, relation, RHS, true);
        }

        public (ObjectiveType objectiveType, bool isValid) ParseObjectiveType(string line)
        {
            try
            {
                return (ObjectiveTypeExtensions.FromDescriptionString(line), true);
            }
            catch (ArgumentException)
            {
                return (default, false);
            }
        }

        public (double[] coefficients, ObjectiveType objectiveType, bool isValid) ParseObjectiveFunction(string line, int size)
        {
            string[] splited = line.Split(defaultSeparators, StringSplitOptions.RemoveEmptyEntries);

            if (splited.Length != size + 1)
            {
                return (Array.Empty<double>(), default, false);
            }

            string coefficientsLine = string.Join(" ", splited, 0, size);
            string objectiveLine = splited[size];

            var (coefficients, coeffIsValid) = ParseCoefficients(coefficientsLine, size);
            if (!coeffIsValid)
            {
                return (Array.Empty<double>(), default, false);
            }

            var (objectiveType, objIsValid) = ParseObjectiveType(objectiveLine);
            if (!objIsValid)
            {
                return (Array.Empty<double>(), default, false);
            }

            return (coefficients, objectiveType, true);
        }
    }
}
