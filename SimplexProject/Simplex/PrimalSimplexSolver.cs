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

        public void TransformTask()
        {
            task = SimplexUtilities.ConvertToStandartForm(task);
        }

        public void BuildTableau()
        {
            basicVariables = SimplexUtilities.FindBasicVariables(task.ConstraintsMatrix);
            if (basicVariables.Count != task.ConstraintsMatrix.GetLength(0))
            {
                throw new InvalidOperationException("basicVariables.Count != task.ConstraintsMatrix.GetLength(0)");
            }

            tableau = SimplexUtilities.BuildTableau(task, basicVariables);
        }

        public void NextIteration()
        {
            int pivotColumn = SimplexUtilities.FindPivotColumn(tableau);
            int pivotRow = SimplexUtilities.FindPivotRow(tableau, pivotColumn);

            if (pivotRow == -1) throw new InvalidOperationException("The problem is unbounded.");

            tableau = SimplexUtilities.NextIteration(tableau, pivotColumn, pivotRow);

            basicVariables[pivotRow] = pivotColumn;
        }

        public bool IsOptimal()
        {
            return SimplexUtilities.IsOptimal(tableau);
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
