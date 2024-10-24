using SimplexProject.Core;

namespace SimplexProject.Utils
{
    internal class ConsoleInput
    {
        private TextParser _textParser;

        public ConsoleInput()
        {
            _textParser = new TextParser();
        }

        public LPTask GetInput()
        {

            throw new NotImplementedException();

            // Num of constraints/variables
            // (for (for Constraints) + Relation + RHS) input
            // (for Objective Function) input
            // ObjectibeType input
        }
    }
}
