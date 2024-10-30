using SimplexProject.Enums;
using SimplexProject.Models;

namespace SimplexProject.Converters
{
    internal static class StandartConverter
    {
        public static LPTask ConvertToStandartForm(LPTask task)
        {
            int constraintsCount = task.ConstraintsCount;
            int oldLength = task.VariablesCount;
            int newVariables = task.RelationTypes.Count(r => r != RelationType.Equal);
            int newLength = oldLength + newVariables;

            var newConstraintsRHS = new double[constraintsCount];
            var newConstraintsMatrix = new double[constraintsCount, newLength];
            RelationType[] newRelationTypes = Enumerable.Repeat(RelationType.Equal, constraintsCount).ToArray();

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

        public static LPTask ConvertToDualStandartForm(LPTask task)
        {
            int constraintsCount = task.ConstraintsCount;
            int oldLength = task.VariablesCount;
            int newVariables = task.RelationTypes.Count(r => r != RelationType.Equal);
            int newLength = oldLength + newVariables;

            var newConstraintsRHS = new double[constraintsCount];
            var newConstraintsMatrix = new double[constraintsCount, newLength];
            RelationType[] newRelationTypes = Enumerable.Repeat(RelationType.Equal, constraintsCount).ToArray();

            int k = oldLength;
            for (int i = 0; i < constraintsCount; i++)
            {
                int sign = task.RelationTypes[i] == RelationType.LessEqual ? 1 : -1;
                for (int j = 0; j < oldLength; j++)
                {
                    newConstraintsMatrix[i, j] = sign * task.ConstraintsMatrix[i, j];
                }

                if (task.RelationTypes[i] != RelationType.Equal)
                {
                    newConstraintsMatrix[i, k] = 1;
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
    }
}
