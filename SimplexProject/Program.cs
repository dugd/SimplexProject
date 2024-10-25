using SimplexProject.Core;
using SimplexProject.Core.Simplex;
using SimplexProject.Core.Simplex.Solvers;
using SimplexProject.Utils;

namespace SimplexProject
{
    static class Program
    {
        static void Main(string[] args)
        {
            var consoleInput = new ConsoleInput();
            LPTask task = consoleInput.GetInput();

            PrimalSimplexSolver primalSimplexSolver = new PrimalSimplexSolver(task);
            SimplexSolve solve = primalSimplexSolver.Solve();
        }
    }
}
