using SimplexProject.Converters;
using SimplexProject.Models;
using SimplexProject.Utilities;
using SimplexProject.Solvers;
using SimplexProject.Enums;

namespace SimplexProject
{
    static class Program
    {
        static void PrintTask(dynamic data)
        {
            LPTask task = data.Task;
            Console.WriteLine("Task: ");
            Console.WriteLine(task);
            Console.WriteLine();
        }

        static void PrintTableau(dynamic data)
        {
            double[,] tableau = data.Tableau;
            List<int> basicVariables = data.BasicVariables;

            Console.WriteLine("Current Simplex Tableau:");
            int width = tableau.GetLength(1);
            int height = tableau.GetLength(0);
            string[] variableNames = new string[width - 1];
            for (int j = 0; j < width - 1; j++)
            {
                variableNames[j] = "x" + (j + 1);
            }

            Console.Write("Basic \t");
            foreach (string varName in variableNames)
            {
                Console.Write(varName + "\t");
            }
            Console.WriteLine("RHS");

            for (int i = 0; i < height; i++)
            {
                if (i < height - 1)
                {
                    Console.Write(" " + variableNames[basicVariables[i]] + "\t");
                }
                else
                {
                    Console.Write(" Z\t");
                }

                for (int j = 0; j < width; j++)
                {
                    Console.Write(Math.Round(tableau[i, j], 2) + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void PrintSolution(dynamic data)
        {
            double[] solution = data.Solution;
            double optimalValue = data.OptimalValue;

            Console.WriteLine("Solution: ");

            for (int j = 0; j < solution.Length; j++)
            {
                Console.WriteLine($"x{j + 1} = {Math.Round(solution[j], 2)}");
            }
            Console.WriteLine($"Optimal Value: {Math.Round(optimalValue, 2)}");
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            var consoleInput = new ConsoleInput();
            LPTask task = consoleInput.GetInput();

            Console.WriteLine("Оберіть метод для вирішення задачі:");
            Console.WriteLine("1 - Двоїстий симплекс метод");
            Console.WriteLine("2 - Перетворення в двоїсту задачу та рішення через PrimalSimplexSolver");
            Console.Write("Ваш вибір: ");
            var choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.WriteLine("\nОбрано: Двоїстий симплекс метод");
                SolveUsingDualSimplex(task);
            }
            else if (choice == "2")
            {
                Console.WriteLine("\nОбрано: Перетворення в двоїсту задачу та рішення через PrimalSimplexSolver");
                SolveUsingPrimalSimplex(task);
            }
            else
            {
                Console.WriteLine("Невірний вибір. Будь ласка, перезапустіть програму та оберіть 1 або 2.");
            }
        }

        static void SolveUsingDualSimplex(LPTask task)
        {
            var solver = new DualSimplexSolver(task);

            while (solver.CurrentStep != SimplexStep.Complete)
            {
                solver.MoveToNextStep();
                dynamic data = solver.GetDisplayData();

                if (solver.CurrentStep == SimplexStep.Transform)
                {
                    PrintTask(data);
                }
                else if (solver.CurrentStep == SimplexStep.BuildTableau || solver.CurrentStep == SimplexStep.Iteration)
                {
                    PrintTableau(data);
                }
                else if (solver.CurrentStep == SimplexStep.Complete)
                {
                    PrintSolution(data);
                }
            }
        }

        static void SolveUsingPrimalSimplex(LPTask task)
        {
            Console.WriteLine("Task: ");
            Console.WriteLine(task);
            Console.WriteLine();

            Console.WriteLine("Prepare before dual form: ");
            LPTask prepare = DualConverter.PrepareConvertToDualForm(task);
            Console.WriteLine(prepare);
            Console.WriteLine();

            Console.WriteLine("Dual form: ");
            LPTask dual = DualConverter.ConvertToDualForm(prepare);
            Console.WriteLine(dual);
            Console.WriteLine();

            var solver = new PrimalSimplexSolver(dual);

            while (solver.CurrentStep != SimplexStep.Complete)
            {
                solver.MoveToNextStep();
                dynamic data = solver.GetDisplayData();

                if (solver.CurrentStep == SimplexStep.Transform)
                {
                    PrintTask(data);
                }
                else if (solver.CurrentStep == SimplexStep.BuildTableau || solver.CurrentStep == SimplexStep.Iteration)
                {
                    PrintTableau(data);
                }
                else if (solver.CurrentStep == SimplexStep.Complete)
                {
                    PrintSolution(data);
                }
            }
        }
    }
}
