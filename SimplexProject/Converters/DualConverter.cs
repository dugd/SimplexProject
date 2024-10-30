using SimplexProject.Enums;
using SimplexProject.Models;

namespace SimplexProject.Converters
{
    internal static class DualConverter
    {
        public static LPTask PrepareConvertToDualForm(LPTask task)
        {
            int variablesCount = task.VariablesCount;
            int constraintsCount = task.ConstraintsCount;

            RelationType newRelation = task.Optimization == ObjectiveType.Maximize ? RelationType.LessEqual : RelationType.GreaterEqual;

            var newConstraintsRHS = new double[constraintsCount];
            var newConstraintsMatrix = new double[constraintsCount, variablesCount];
            RelationType[] newRelationTypes = Enumerable.Repeat(newRelation, constraintsCount).ToArray();

            var newObjectiveFunction = new double[variablesCount];
            Array.Copy(task.ObjectiveFuction, newObjectiveFunction, variablesCount);

            for (int i = 0; i < constraintsCount; i++)
            {
                if (task.RelationTypes[i] == RelationType.Equal)
                {
                    throw new InvalidOperationException();
                }
                int sign = (newRelation == task.RelationTypes[i]) ? 1 : -1;
                for (int j = 0; j < variablesCount; j++)
                {
                    newConstraintsMatrix[i, j] = sign * task.ConstraintsMatrix[i, j];
                }
                newConstraintsRHS[i] = sign * task.ConstraintsRHS[i];
            }

            return new LPTask(
                newObjectiveFunction,
                newConstraintsMatrix,
                newConstraintsRHS,
                newRelationTypes,
                task.Optimization);
        }

        public static LPTask ConvertToDualForm(LPTask task)
        {
            int variablesCount = task.ConstraintsCount;
            int constraintsCount = task.VariablesCount;

            RelationType newRelation = task.Optimization == ObjectiveType.Maximize ? RelationType.GreaterEqual : RelationType.LessEqual;

            var newConstraintsRHS = new double[constraintsCount];
            Array.Copy(task.ObjectiveFuction, newConstraintsRHS, constraintsCount);

            var newConstraintsMatrix = new double[constraintsCount, variablesCount];
            for (int i = 0; i < constraintsCount; i++)
            {
                for (int j = 0; j < variablesCount; j++)
                {
                    newConstraintsMatrix[i, j] = task.ConstraintsMatrix[j, i];
                }
            }

            RelationType[] newRelationTypes = Enumerable.Repeat(newRelation, constraintsCount).ToArray();

            var newObjectiveFunction = new double[variablesCount];
            Array.Copy(task.ConstraintsRHS, newObjectiveFunction, variablesCount);

            ObjectiveType newOptimization = task.Optimization == ObjectiveType.Maximize ? ObjectiveType.Minimize : ObjectiveType.Maximize;

            return new LPTask(
                newObjectiveFunction,
                newConstraintsMatrix,
                newConstraintsRHS,
                newRelationTypes,
                newOptimization);
        }
    }
}
