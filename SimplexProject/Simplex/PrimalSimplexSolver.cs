using SimplexProject.Enums;

namespace SimplexProject.Simplex
{
    internal class PrimalSimplexSolver
    {
        private LPTask task;
        private List<int> basicVariables;
        private double[,] tableau;

        public PrimalSimplexSolver(LPTask task)
        {
            this.task = task;
            basicVariables = new List<int>();
            tableau = new double[0,0];
        }

        private static LPTask ConvertToStandartForm(LPTask task)
        {
            int constraintsCount = task.ConstraintsMatrix.GetLength(0);
            int oldLength = task.ObjectiveFuction.Length;
            int newVariables = task.RelationTypes.Count(r => r != RelationType.Equal);
            int newLength = oldLength + newVariables;

            var newConstraintsRHS = new double[constraintsCount];
            var newConstraintsMatrix = new double[constraintsCount, newLength];
            RelationType[] newRelationTypes = Enumerable.Repeat(RelationType.Equal, newLength).ToArray();

            int k = oldLength;
            for (int i = 0; i < constraintsCount; i++)
            {
                int sign = task.ConstraintsRHS[i] >= 0 ? 1 : -1;
                for (int j = 0; j < oldLength; j++)
                {
                    newConstraintsMatrix[i, j] = sign * task.ConstraintsMatrix[i, j];
                }

                if (task.RelationTypes[i] != RelationType.Equal)
                {
                    newConstraintsMatrix[i, k] = sign * (task.RelationTypes[i] == RelationType.LessEqual ? 1 : -1);
                    k++;
                }
                newConstraintsRHS[i] = sign * task.ConstraintsRHS[i];
            }

            var newObjectiveFunction = new double[newLength];
            Array.Copy(task.ObjectiveFuction, newObjectiveFunction, oldLength);

            return new LPTask(
                newObjectiveFunction,
                newConstraintsMatrix,
                newConstraintsRHS,
                newRelationTypes,
                task.Optimization);
        }

        private static List<int> FindBasicVariables(double[,] constraintsMatrix)
        {
            int constraintsCount = constraintsMatrix.GetLength(0);
            int variablesCount = constraintsMatrix.GetLength(1);

            var result = new List<int>();

            for (int j = 0; j < variablesCount; j++)
            {
                bool isBasic = true;
                int c = 0;
                for (int i = 0; i < constraintsCount; i++)
                {
                    double val = constraintsMatrix[i, j];
                    if (val == 1) c++;
                    else if (val != 0)
                    {
                        isBasic = false;
                        break;
                    }
                }
                if (isBasic && c == 1)
                {
                    result.Add(j);
                    if (result.Count == constraintsCount) break;
                }

            }
            return result;
        }

        public int FindPivotColumn()
        {
            int result = -1;
            double minValue = 0;
            for (int j = 0; j < tableau.GetLength(1) - 1; j++)
            {
                double value = tableau[tableau.GetLength(0) - 1, j];
                if (value < minValue)
                {
                    minValue = value;
                    result = j;
                }
            }
            return result;
        }

        public int FindPivotRow(int pivotColumn)
        {
            double minRatio = double.PositiveInfinity;
            int pivotRow = -1;

            for (int i = 0; i < tableau.GetLength(0) - 1; i++)
            {
                double rhs = tableau[i, tableau.GetLength(1) - 1];
                double coefficient = tableau[i, pivotColumn];

                if (coefficient > 0)
                {
                    double ratio = rhs / coefficient;
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        pivotRow = i;
                    }
                }
            }
            return pivotRow;
        }

        public void TransformTask()
        {
            task = ConvertToStandartForm(task);
        }

        public void BuildTableau()
        {
            basicVariables = FindBasicVariables(task.ConstraintsMatrix);
            if (basicVariables.Count != task.ConstraintsMatrix.GetLength(0))
            {
                throw new InvalidOperationException("basicVariables.Count != task.ConstraintsMatrix.GetLength(0)");
            }

            int constraintsCount = task.ConstraintsMatrix.GetLength(0);
            int variablesCount = task.ConstraintsMatrix.GetLength(1);

            int height = constraintsCount + 1;
            int width = variablesCount + 1;

            tableau = new double[height, width];

            for (int i = 0; i < constraintsCount; i++)
            {
                for (int j = 0; j < variablesCount; j++)
                {
                    tableau[i, j] = task.ConstraintsMatrix[i, j];
                }
            }

            for (int i = 0; i < constraintsCount; i++)
            {
                tableau[i, width - 1] = task.ConstraintsRHS[i];
            }

            int sign = task.Optimization == ObjectiveType.Minimize ? 1 : -1;
            for (int j = 0; j < variablesCount; j++)
            {
                tableau[height - 1, j] = sign * task.ObjectiveFuction[j];
            }

            tableau[height - 1, width - 1] = basicVariables.Select((val, i) => task.ObjectiveFuction[val] * task.ConstraintsRHS[i]).Sum();
        }

        public void NextIteration()
        {
            int height = tableau.GetLength(0);
            int width = tableau.GetLength(1);

            int pivotColumn = FindPivotColumn();
            int pivotRow = FindPivotRow(pivotColumn);

            if (pivotRow == -1) throw new InvalidOperationException("The problem is unbounded.");

            double pivotValue = tableau[pivotRow, pivotColumn];
            for (int j = 0; j < width; j++)
            {
                tableau[pivotRow, j] /= pivotValue;
            }

            for (int i = 0; i < height; i++)
            {
                if (i == pivotRow) continue;

                double factor = tableau[i, pivotColumn];
                for (int j = 0; j < width; j++)
                {
                    tableau[i, j] -= factor * tableau[pivotRow, j];
                }
            }

            basicVariables[pivotRow] = pivotColumn;
        }

        public bool IsOptimal()
        {
            int variablesCount = tableau.GetLength(1) - 1;
            for (int j = 0; j < variablesCount; j++)
            {
                if (tableau[tableau.GetLength(0) - 1, j] < 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void PrintTask()
        {
            Console.WriteLine(task);
        }

        public void PrintTableau()
        {
            Console.WriteLine("Current Simplex Tableau:");
            int width = tableau.GetLength(1);
            int height = tableau.GetLength(0);
            string[] variableNames = new string[width - 1];
            for (int j = 0; j < width - 1; j++)
            {
                variableNames[j] = "x" + (j + 1);
            }

            Console.Write("Basic \t");
            foreach (string varName in variableNames)
            {
                Console.Write(varName + "\t");
            }
            Console.WriteLine("RHS");

            for (int i = 0; i < height; i++)
            {
                if (i < height - 1)
                {
                    Console.Write(" " + variableNames[basicVariables[i]] + "\t");
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

        public void PrintSolution()
        {
            Console.WriteLine("Solution: ");
            int variablesCount = tableau.GetLength(1) - 1;

            double[] solution = new double[variablesCount];

            for (int i = 0; i < basicVariables.Count; i++)
            {
                solution[basicVariables[i]] = tableau[i, variablesCount];
            }

            for (int j = 0; j < solution.Length; j++)
            {
                Console.WriteLine($"x{j + 1} = {Math.Round(solution[j], 2)}");
            }
            Console.WriteLine($"Optimal Value: {Math.Round(tableau[tableau.GetLength(0) - 1, tableau.GetLength(1) - 1], 2)}");
        }

        public void Solve()
        {
            TransformTask();
            PrintTask();

            BuildTableau();
            PrintTableau();

            while (!IsOptimal())
            {
                NextIteration();
                PrintTableau();
            }

            PrintSolution();
        }
    }
}
