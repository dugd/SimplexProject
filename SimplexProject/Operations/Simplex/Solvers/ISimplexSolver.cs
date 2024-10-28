using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexProject.Operations.Simplex.Solvers
{
    internal interface ISimplexSolver
    {
        SimplexSolve Solve();
    }
}
