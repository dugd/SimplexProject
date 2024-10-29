﻿using SimplexProject.Enums;

namespace SimplexProject.Simplex.Utilities
{
    internal static class SimplexUtilities
    {
        public static LPTask ConvertToStandartForm(LPTask task)
        {
            int constraintsCount = task.ConstraintsCount;
            int oldLength = task.VariablesCount;
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

        public static List<int> FindBasicVariables(double[,] constraintMatrix)
        {
            int constraintsCount = constraintMatrix.GetLength(0);
            int variablesCount = constraintMatrix.GetLength(1);

            var result = new List<int>();

            for (int j = 0; j < variablesCount; j++)
            {
                bool isBasic = true;
                int c = 0;
                for (int i = 0; i < constraintsCount; i++)
                {
                    double val = constraintMatrix[i, j];
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

        public static double[,] BuildTableau(ObjectiveData objectiveData, StandartConstraintData constraintData, List<int> basicVariables)
        {
            int constraintsCount = constraintData.Matrix.GetLength(0);
            int variablesCount = constraintData.Matrix.GetLength(1);

            int height = constraintsCount + 1;
            int width = variablesCount + 1;

            var tableau = new double[height, width];

            for (int i = 0; i < constraintsCount; i++)
            {
                for (int j = 0; j < variablesCount; j++)
                {
                    tableau[i, j] = constraintData.Matrix[i, j];
                }
            }

            for (int i = 0; i < constraintsCount; i++)
            {
                tableau[i, width - 1] = constraintData.RightHandSide[i];
            }

            int sign = objectiveData.OptimizationType == ObjectiveType.Minimize ? 1 : -1;
            for (int j = 0; j < variablesCount; j++)
            {
                tableau[height - 1, j] = sign * objectiveData.Coefficients[j];
            }

            tableau[height - 1, width - 1] = basicVariables.Select((val, i) => objectiveData.Coefficients[val] * constraintData.RightHandSide[i]).Sum();

            return tableau;
        }

        public static bool IsOptimal(double[,] tableau)
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

        public static int FindPivotColumn(double[,] tableau)
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

        public static int FindPivotRow(double[,] tableau, int pivotColumn)
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

        public static double[,] NextIteration(double[,] tableau, int pivotColumn, int pivotRow)
        {

            int height = tableau.GetLength(0);
            int width = tableau.GetLength(1);

            var newTableau = new double[height, width];

            double pivotValue = tableau[pivotRow, pivotColumn];
            for (int j = 0; j < width; j++)
            {
                newTableau[pivotRow, j] = tableau[pivotRow, j] / pivotValue;
            }

            for (int i = 0; i < height; i++)
            {
                if (i == pivotRow) continue;

                double factor = tableau[i, pivotColumn];
                for (int j = 0; j < width; j++)
                {
                    newTableau[pivotRow, j] = tableau[i, j] - factor * tableau[pivotRow, j];
                }
            }

            return newTableau;
        }
    }
}
