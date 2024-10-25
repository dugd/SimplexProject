using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexProject.Core.Simplex
{
    internal interface ISimplexSolver
    {
        SimplexSolve Solve();
    }
}
