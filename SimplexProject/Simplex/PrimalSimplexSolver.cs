using SimplexProject.Enums;

namespace SimplexProject.Simplex
{
    internal class PrimalSimplexSolver
    {
        private LPTask task;

        public PrimalSimplexSolver(LPTask task)
        {
            this.task = task;
        }

        public static LPTask ConvertToStandartForm(LPTask task)
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

        public void TransformTask()
        {
            task = ConvertToStandartForm(task);

        }

        public void BuildTableau()
        {
            throw new NotImplementedException();
        }

        public void NextIteration()
        {
            throw new NotImplementedException();
        }

        public bool IsOptimal()
        {
            throw new NotImplementedException();
        }

        public void PrintTask()
        {
            throw new NotImplementedException();
        }

        public void PrintTableau()
        {
            throw new NotImplementedException();
        }

        public void PrintSolution()
        {
            throw new NotImplementedException();
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
