using SimplexProject.Enums;
using SimplexProject.Models;

namespace SimplexProject.Utilities
{
    internal class ConsoleInput
    {
        private TextParser _textParser;

        public ConsoleInput()
        {
            _textParser = new TextParser();
        }

        public LPTask GetInput()
        {
            int numVariables = GetPositiveInteger("Введіть кількість змінних:");
            int numConstraints = GetPositiveInteger("Введіть кількість обмежень:");

            double[,] constraintsCoefficients = new double[numConstraints, numVariables];
            RelationType[] relations = new RelationType[numConstraints];
            double[] rhsValues = new double[numConstraints];

            Console.WriteLine("Введіть обмеження у форматі: коефіцієнти_обмеження, оператор_відношення та права_частина (наприклад: 1.5 2.0 3.0 <= 10)");

            for (int i = 0; i < numConstraints; i++)
            {
                while (true)
                {
                    Console.WriteLine($"Обмеження {i + 1}:");
                    string inputLine = Console.ReadLine() ?? string.Empty;

                    var (coefficients, relation, rhs, isValid) = _textParser.ParseConstraint(inputLine, numVariables);

                    if (isValid && coefficients != null)
                    {
                        for (int j = 0; j < numVariables; j++)
                        {
                            constraintsCoefficients[i, j] = coefficients[j];
                        }
                        relations[i] = relation;
                        rhsValues[i] = rhs;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Невірне введення обмеження. Спробуйте ще раз.");
                    }
                }
            }

            double[] objectiveCoefficients;
            ObjectiveType objectiveType;

            Console.WriteLine("Введіть цільову функцію у форматі: коефіцієнти цільової функції та тип задачі (наприклад: 1.0 2.0 3.0 max)");

            while (true)
            {
                string inputLine = Console.ReadLine() ?? string.Empty;

                var (coefficients, objType, isValid) = _textParser.ParseObjectiveFunction(inputLine, numVariables);

                if (isValid && coefficients != null)
                {
                    objectiveCoefficients = coefficients;
                    objectiveType = objType;
                    break;
                }
                else
                {
                    Console.WriteLine("Невірне введення цільової функції. Спробуйте ще раз.");
                }
            }

            return new LPTask(objectiveCoefficients, constraintsCoefficients, rhsValues, relations, objectiveType);
        }

        private int GetPositiveInteger(string prompt)
        {
            int result;
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine() ?? string.Empty;
                if (int.TryParse(input, out result) && result > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Будь ласка, введіть додатне ціле число.");
                }
            }
            return result;
        }
    }
}
