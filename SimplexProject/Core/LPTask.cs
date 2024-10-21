using SimplexProject.Enums;

namespace SimplexProject.Core
{
    internal class LPTask
    {
        public double[] ObjectiveFuction;
        public double[,] ConstraintsMatrix;
        public double[] ConstraintsRHS;

        public RelationType[] RelationTypes;
        public ObjectiveType Optimization;

        public LPTask(
            double[] objectiveFunction,
            double[, ] constraintsMatrix,
            double[] constraintsRHS,
            RelationType[] relationTypes,
            ObjectiveType optimizationType
            )
        {
            ObjectiveFuction = objectiveFunction;
            ConstraintsMatrix = constraintsMatrix;
            ConstraintsRHS = constraintsRHS;
            RelationTypes = relationTypes;
            Optimization = optimizationType;

        }
    }
}
