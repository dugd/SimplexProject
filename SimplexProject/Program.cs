using SimplexProject.Enums;
using SimplexProject.Utils;

namespace SimplexProject
{
    static class Program
    {
        static void Main(string[] args)
        {
            var inputParser = new TextParser();
            Console.WriteLine(inputParser.ParseCoefficients("5 2", 2, out double[] res1));
            Console.WriteLine(inputParser.ParseConstraint("5 1 -3 >= -1", 3, out double[] res2, out RelationType rel, out double RHS));
        }
    }
}
