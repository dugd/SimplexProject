using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexProject.Core.Simplex.Solvers
{
    internal class PrimalSimplexSolver : ISimplexSolver
    {
        private int numVariables;
        private int numConstraints;
        private double[,] tableau;
        private List<int> basisVariables;

        public PrimalSimplexSolver(LPTask task)
        {
            numConstraints = task.ConstraintsMatrix.GetLength(0);
            numVariables = task.ConstraintsMatrix.GetLength(1);

            int height = numConstraints + 1;
            int width = numVariables + 1;

            tableau = new double[height, width];

            basisVariables = new List<int>();
            if (!InitiateBasisVariables(task.ConstraintsMatrix))
            {
                throw new ArgumentException();
            }

            for (int i = 0; i < numConstraints; i++)
            {
                for (int j = 0; j < numVariables; j++)
                {
                    tableau[i, j] = task.ConstraintsMatrix[i, j];
                }
            }

            for (int i = 0; i < numConstraints; i++)
            {
                tableau[i, width - 1] = task.ConstraintsRHS[i];
            }

            int factor = (task.Optimization == Enums.ObjectiveType.Minimize) ? 1 : -1;
            for (int j = 0; j < numVariables; j++)
            {
                tableau[height - 1, j] = factor * task.ObjectiveFuction[j];
            }
        }

        private bool InitiateBasisVariables(double[,] constraintsMatrix)
        {
            int n = constraintsMatrix.GetLength(0);
            int m = constraintsMatrix.GetLength(1);

            for (int j = 0; j < m; j++)
            {
                int oneCount = 0;
                bool isBasisColumn = true;

                for (int i = 0; i < n; i++)
                {
                    double val = constraintsMatrix[i, j];
                    if (val == 1) oneCount++;
                    else if (val != 0)
                    {
                        isBasisColumn = false;
                        break;
                    }
                }

                if (isBasisColumn && oneCount == 1)
                {
                    basisVariables.Add(j);
                    if (basisVariables.Count == n)
                        return true;
                }
            }

            return false;
        }


        public SimplexSolve Solve()
        {
            PrintTableau();
            while (!IsOptimal())
            {
                int enteringColumn = FindEnteringVariable();
                int leavingRow = FindLeavingVariable(enteringColumn);
                if (leavingRow == -1)
                {
                    Console.WriteLine("The problem is unbounded.");
                    return new SimplexSolve(true, null, null);
                }
                basisVariables[leavingRow] = enteringColumn;
                Pivot(leavingRow, enteringColumn);
                PrintTableau();
            }

            int height = tableau.GetLength(0);
            int width = tableau.GetLength(1);

            double[] variables = new double[numVariables];

            for (int i = 0; i < basisVariables.Count; i++)
            {
                variables[basisVariables[i]] = tableau[i, width - 1];
            }

            return new SimplexSolve(true, tableau[height - 1, width - 1], variables);
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
            for (int j = 0; j < numVariables; j++)
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
    }
}
