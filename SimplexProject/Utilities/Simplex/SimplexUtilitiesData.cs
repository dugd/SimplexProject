using SimplexProject.Enums;

namespace SimplexProject.Utilities.Simplex
{
    internal struct ObjectiveData
    {
        public double[] Coefficients;
        public ObjectiveType OptimizationType;

        public ObjectiveData(double[] coefficients, ObjectiveType optimizationType)
        {
            Coefficients = coefficients;
            OptimizationType = optimizationType;
        }
    }

    internal struct StandartConstraintData
    {
        public double[,] Matrix;
        public double[] RightHandSide;

        public StandartConstraintData(double[,] matrix, double[] rightHandSide)
        {
            Matrix = matrix;
            RightHandSide = rightHandSide;
        }
    }

    internal struct ConstraintData
    {
        public double[,] Matrix;
        public double[] RightHandSide;
        public RelationType[] RelationTypes;

        public ConstraintData(double[,] matrix, double[] rightHandSide, RelationType[] relationTypes)
        {
            Matrix = matrix;
            RightHandSide = rightHandSide;
            RelationTypes = relationTypes;
        }
    }
}
