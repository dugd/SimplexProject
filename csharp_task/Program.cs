

namespace SimplexMethod
{
    public class Simplex
    {
        private double[,] tableau;
        private int numConstraints;
        private int numOriginalVariables;
        private List<int> basisVariables;

        public Simplex(double[,] A, double[] b, double[] c)
        {
            numConstraints = b.Length;
            numOriginalVariables = c.Length;
            int width = numOriginalVariables + numConstraints + 1;
            int height = numConstraints + 1;

            tableau = new double[height, width];
            basisVariables = new List<int>();

            for (int i = 0; i < numConstraints; i++)
            {
                for (int j = 0; j < numOriginalVariables; j++)
                {
                    tableau[i, j] = A[i, j];
                }
                tableau[i, numOriginalVariables + i] = 1;
                basisVariables.Add(numOriginalVariables + i);
                tableau[i, width - 1] = b[i];
            }

            for (int j = 0; j < numOriginalVariables; j++)
            {
                tableau[height - 1, j] = -c[j];
            }
        }

        public void Solve()
        {
            PrintTableau();
            while (!IsOptimal())
            {
                int enteringColumn = FindEnteringVariable();
                int leavingRow = FindLeavingVariable(enteringColumn);
                if (leavingRow == -1)
                {
                    Console.WriteLine("The problem is unbounded.");
                    return;
                }
                basisVariables[leavingRow] = enteringColumn;
                Pivot(leavingRow, enteringColumn);
                PrintTableau();
            }
            PrintSolution();
        }

        private bool IsOptimal()
        {
            int lastRow = tableau.GetLength(0) - 1;
            for (int j = 0; j < tableau.GetLength(1) - 1; j++)
            {
                if (tableau[lastRow, j] < 0)
                    return false;
            }
            return true;
        }

        private int FindEnteringVariable()
        {
            int lastRow = tableau.GetLength(0) - 1;
            int enteringColumn = -1;
            double minValue = 0;
            for (int j = 0; j < tableau.GetLength(1) - 1; j++)
            {
                if (tableau[lastRow, j] < minValue)
                {
                    minValue = tableau[lastRow, j];
                    enteringColumn = j;
                }
            }
            return enteringColumn;
        }

        private int FindLeavingVariable(int enteringColumn)
        {
            int leavingRow = -1;
            double minRatio = double.PositiveInfinity;
            for (int i = 0; i < numConstraints; i++)
            {
                if (tableau[i, enteringColumn] > 0)
                {
                    double ratio = tableau[i, tableau.GetLength(1) - 1] / tableau[i, enteringColumn];
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        leavingRow = i;
                    }
                }
            }
            return leavingRow;
        }

        private void Pivot(int pivotRow, int pivotColumn)
        {
            double pivotValue = tableau[pivotRow, pivotColumn];
            int width = tableau.GetLength(1);
            int height = tableau.GetLength(0);

            for (int j = 0; j < width; j++)
            {
                tableau[pivotRow, j] /= pivotValue;
            }

            for (int i = 0; i < height; i++)
            {
                if (i != pivotRow)
                {
                    double factor = tableau[i, pivotColumn];
                    for (int j = 0; j < width; j++)
                    {
                        tableau[i, j] -= factor * tableau[pivotRow, j];
                    }
                }
            }
        }

        private void PrintTableau()
        {
            Console.WriteLine("Current Simplex Tableau:");
            int width = tableau.GetLength(1);
            int height = tableau.GetLength(0);
            string[] variableNames = new string[width - 1];
            for (int j = 0; j < numOriginalVariables; j++)
            {
                variableNames[j] = "x" + (j + 1);
            }
            for (int j = numOriginalVariables; j < width - 1; j++)
            {
                variableNames[j] = "s" + (j - numOriginalVariables + 1);
            }

            Console.Write("Basic \t");
            foreach (string varName in variableNames)
            {
                Console.Write(varName + "\t");
            }
            Console.WriteLine("RHS");

            for (int i = 0; i < height; i++)
            {
                if (i < numConstraints)
                {
                    Console.Write(" " + variableNames[basisVariables[i]] + "\t");
                }
                else
                {
                    Console.Write(" Z\t");
                }

                for (int j = 0; j < width; j++)
                {
                    Console.Write(Math.Round(tableau[i, j], 2) + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void PrintSolution()
        {
            Console.WriteLine("Optimal solution found:");
            double[] solution = new double[numOriginalVariables];
            for (int i = 0; i < numConstraints; i++)
            {
                if (basisVariables[i] < numOriginalVariables)
                {
                    solution[basisVariables[i]] = tableau[i, tableau.GetLength(1) - 1];
                }
            }
            for (int j = 0; j < numOriginalVariables; j++)
            {
                Console.WriteLine("x" + (j + 1) + " = " + Math.Round(solution[j], 2));
            }
            double optimalValue = tableau[tableau.GetLength(0) - 1, tableau.GetLength(1) - 1];
            Console.WriteLine("Optimal value (Z) = " + Math.Round(optimalValue, 2));
        }
    }

    class Program
    {
        static void InputDoubleValue(string line, out double value)
        {
            while (true)
            {
                Console.Write(line);
                if (double.TryParse(Console.ReadLine(), out value))
                    break;
                else
                    Console.WriteLine("Please enter a valid number.");
            }
        }

        static void InputIntValue(string line, out int value)
        {
            while (true)
            {
                Console.Write(line);
                if (int.TryParse(Console.ReadLine(), out value) && value > 0)
                    break;
                else
                    Console.WriteLine("Please enter a valid positive integer.");
            }
        }

        static void InputValues(out double[,] A, out double[] B, out double[] C)
        {
            int numVariables;
            int numConstraints;

            InputIntValue("Enter the number of variables: ", out numVariables);

            InputIntValue("Enter the number of constraints: ", out numConstraints);

            A = new double[numConstraints, numVariables];
            B = new double[numConstraints];
            C = new double[numVariables];

            Console.WriteLine("\nEnter the constraint coefficients for each row (along with the right-hand side):");
            for (int i = 0; i < numConstraints; i++)
            {
                Console.WriteLine($"Constraint {i + 1}:");
                for (int j = 0; j < numVariables; j++)
                {
                    InputDoubleValue($"Enter coefficient for variable x{j + 1}: ", out A[i, j]);
                }

                InputDoubleValue("Enter the right-hand side (RHS): ", out B[i]);
            }

            Console.WriteLine("\nEnter the coefficients for the objective function:");
            for (int j = 0; j < numVariables; j++)
            {
                InputDoubleValue($"Enter coefficient for variable x{j + 1}: ", out C[j]);
            }
        }


        static void Main(string[] args)
        {
            double[,] A = {
                { 1, 0, 1 },
                { 0, 2, 2 },
                { 3, 2, 1 }
            };

            double[] B = { 4, 12, 18 };

            double[] C = { 3, 5, 2 };

            InputValues(out A, out B, out C);

            Simplex simplex = new Simplex( A, B, C);
            simplex.Solve();

            Console.ReadLine();
        }
    }
}
