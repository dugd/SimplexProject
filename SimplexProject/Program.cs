

namespace SimplexProject
{
    public class InputParser
    {
        public LinearProgramingProblem ParseFromConsole()
        {
            throw new NotImplementedException();
        }
    }

    public class LinearProgramingProblem
    {

    }

    public class ProblemConverter
    {
        public LinearProgramingProblem ToStandartForm(LinearProgramingProblem problem)
        {
            throw new NotImplementedException();
        }
        
        public LinearProgramingProblem ToDualProblem(LinearProgramingProblem problem)
        {
            throw new NotImplementedException();
        }
    }

    public class SimplexSolver
    {
        public SimplexSolver(LinearProgramingProblem problem)
        {

        }

        public OptimizationResult Solve()
        {
            throw new NotImplementedException();
        }
    }

    public class OptimizationResult
    {

    }

    public class OutputGenerator
    {
        public void DisplayResult(OptimizationResult result)
        {
            throw new NotImplementedException();
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            InputParser parser = new InputParser();

            LinearProgramingProblem problem = parser.ParseFromConsole();

            ProblemConverter converter = new ProblemConverter();
            LinearProgramingProblem standartProblem = converter.ToStandartForm(problem);

            SimplexSolver solver = new SimplexSolver(standartProblem);

            OptimizationResult solverResult = solver.Solve();

            OutputGenerator outputGenerator = new OutputGenerator();
            outputGenerator.DisplayResult(solverResult);
        }
    }
}
