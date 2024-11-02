using StudaProject.Core.Enums;

namespace StudaProject.Core.Utilities.Simplex
{
    public struct ObjectiveData
    {
        public double[] Coefficients;
        public ObjectiveType OptimizationType;

        public ObjectiveData(double[] coefficients, ObjectiveType optimizationType)
        {
            Coefficients = coefficients;
            OptimizationType = optimizationType;
        }
    }

    public struct StandartConstraintData
    {
        public double[,] Matrix;
        public double[] RightHandSide;

        public StandartConstraintData(double[,] matrix, double[] rightHandSide)
        {
            Matrix = matrix;
            RightHandSide = rightHandSide;
        }
    }

    public struct ConstraintData
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
