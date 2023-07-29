using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Dynamic;
using System.Text.RegularExpressions;

namespace logic_gate_language_interpreter
{
    internal class Program
    {

        /*-----By Evie :3-----*/
        //an interpreter for my logic gate language, MESSY

        public static Regex regex = new Regex(@"^[a-zA-Z0-9]+$");

        public static Dictionary<string, bool> variables = new Dictionary<string, bool>();
        public static string[] bannedWords = { "true", "false", "!", "print", "var", "//" }; //add more

        public static string path;
        public static string[] lines;

        public static int lineNumber = 0;
        static void Main(string[] args)
        {
            do
            {
                Console.Write("Input file path for the logic gate script: ");
                path = Console.ReadLine();
                if (!File.Exists(path)) Console.WriteLine($"{path} does not exist");

            } while (!File.Exists(path));

            lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                lineNumber++;
                string[] tokens = line.Split(' ');
                //Console.WriteLine(line);

                switch (tokens[0])
                {
                    case "!":
                        Not(line);
                        break;
                    case "//":
                        //
                        break;
                    case "var":
                        VarDeclare(line);
                        break;
                    case "print":
                        Print(line);
                        break;
                    default:
                        if(line != string.Empty) Var(line);
                        break;
                }
            }

            Console.WriteLine("finished executing press a key to continue");
            Console.ReadKey();
        }

        static void Print(string line)
        {

            string[] tokens = line.Split(' ');

            if (tokens.Length < 2)
            {
                string[] extra = { "extra" };
                Error(line, 5, extra);
            }

            string varToPrint = tokens[1];

            if (tokens[1] == "!")
            {
                if (tokens.Length < 3)
                {
                    string[] extra = { "extra" };
                    Error(line, 5, extra);
                }
                varToPrint = tokens[2];
            }

            bool printing = false;
            if (tokens.Length > 2)
            {
                if (tokens[2] == "//") printing = true;
            }
            if (tokens.Length > 3)
            {
                if (tokens[3] == "//") printing = true;
            }

            if (printing || (tokens.Length == 2 || tokens.Length == 3))
            {
                if (variables.ContainsKey(varToPrint))
                {
                    Console.WriteLine(tokens[1] == "!" ? !variables[varToPrint] : variables[varToPrint]);
                }
                else if (varToPrint == "true")
                {
                    Console.WriteLine(tokens[1] != "!");
                }
                else if (varToPrint == "false")
                {
                    Console.WriteLine(tokens[1] == "!");
                }
                else
                {
                    string[] extra = { varToPrint };
                    Error(line, 1, extra);
                }
            }
            else
            {
                if (tokens.Length > 4)
                {
                    if (tokens[4] != "//")
                    {
                        string[] extra = { "extra" };
                        Error(line, 6, extra);
                    }
                }
                string stri = "";
                for (int i = 1; i < 4; i++)
                {
                    stri += tokens[i] + " ";
                }
                Console.WriteLine(Equation(stri));
            }

        }
        static void VarDeclare(string line)
        {

            string[] tokens = line.Split(' ');

            if (tokens.Length < 4)
            {
                string[] extra = { "extra" };
                Error(line, 5, extra);
            }

            string varDeclare = tokens[1];
            bool result = false;

            if (variables.ContainsKey(varDeclare)) 
            {
                string[] extra = { varDeclare };
                Error(line, 2, extra);
            }
            if (bannedWords.Contains(varDeclare))
            {
                string[] extra = { varDeclare };
                Error(line, 3, extra);
            }
            if (!regex.IsMatch(varDeclare))
            {
                string[] extra = { varDeclare };
                Error(line, 7, extra);
            }

            bool setting = false;
            if(tokens.Length > 4)
            {
                if (tokens[4] == "//") setting = true;
            }
            if (tokens.Length > 5)
            {
                if (tokens[5] == "//") setting = true;
            }

            string var = tokens[3];
            if (tokens[3] == "!") var = tokens[4];

            if (setting || tokens.Length == 4 || tokens[3] == "!")
            {
                if (variables.ContainsKey(var))
                {
                    result = tokens[3] == "!" ? !variables[var] : variables[var];
                }
                else if (var == "true")
                {
                    result = tokens[3] != "!";
                }
                else if (var == "false")
                {
                    result = tokens[3] == "!";
                }
                else
                {
                    string[] extra = { var };
                    Error(line, 1, extra);
                }
            }
            else
            {
                if (tokens.Length > 6)
                {
                    if (tokens[6] != "//")
                    {
                        string[] extra = { "extra" };
                        Error(line, 6, extra);
                    }
                }

                string stri = "";
                for (int i = 3; i < 6; i++)
                {
                   stri += tokens[i] + " ";
                }
                result = Equation(stri);
            }

            variables[varDeclare] = result;
        }
        static void Not(string line)
        {

            string[] tokens = line.Split(' ');

            if (tokens.Length < 2)
            {
                string[] extra = { "extra" };
                Error(line, 5, extra);
            }
            else if (tokens.Length > 2)
            {
                if (tokens[2] != "//")
                {
                    string[] extra = { "extra" };
                    Error(line, 6, extra);
                }
            }

            string var = tokens[1];

            if (!variables.ContainsKey(var))
            {
                string[] extra = { var };
                Error(line, 1, extra);
            }
            else
            {
                variables[var] = !variables[var];
            }
            
        }
        static void Var(string line)
        {
            string[] tokens = line.Split(' ');

            if (tokens.Length < 3)
            {
                string[] extra = { "extra" };
                Error(line, 5, extra);
            }

            string varName = tokens[0];
            bool varValue = false;

            bool setting = false;
            if (tokens.Length > 3)
            {
                if (tokens[3] == "//") setting = true;
            }
            if (tokens.Length > 4)
            {
                if (tokens[4] == "//") setting = true;
            }

            string var = tokens[2];
            if (tokens[2] == "!") var = tokens[3];

            if (setting || tokens.Length == 3 || tokens[2] == "!")
            {
                if (variables.ContainsKey(var))
                {
                    varValue = tokens[2] == "!" ? !variables[var] : variables[var];
                }
                else if (var == "true")
                {
                    varValue = tokens[2] != "!";
                }
                else if (var == "false")
                {
                    varValue = tokens[2] == "!";
                }
                else
                {
                    string[] extra = { var };
                    Error(line, 1, extra);
                }
            }
            else
            {
                if (tokens.Length > 5)
                {
                    if (tokens[5] != "//")
                    {
                        string[] extra = { "extra" };
                        Error(line, 6, extra);
                    }
                }

                string stri = "";
                for (int i = 2; i < 5; i++)
                {
                    stri += tokens[i] + " ";
                }
                varValue = Equation(stri);
            }

            variables[varName] = varValue;
        }
        static bool Equation(string line)
        {

            string[] tokens = line.Split(' ');

            string op = tokens[1];
            string srcVar1 = tokens[0];
            string srcVar2 = tokens[2];

            bool val1 = false;
            bool val2 = false;

            if (srcVar1 == "true")
            {
                val1 = true;
            }
            else if (srcVar1 == "false")
            {
                val1 = false;
            }
            else if (!variables.ContainsKey(srcVar1))
            {
                string[] extra = { srcVar1 };
                Error(lines[lineNumber - 1], 1, extra);
            }
            else
            {
                val1 = variables[srcVar1];
            }

            if (srcVar2 == "true")
            {
                val2 = true;
            }
            else if (srcVar2 == "false")
            {
                val2 = false;
            }
            else if (!variables.ContainsKey(srcVar2))
            {
                string[] extra = { srcVar2 };
                Error(lines[lineNumber - 1], 1, extra);
            }
            else
            {
                val2 = variables[srcVar1];
            }

            bool result = false;
            switch (op)
            {
                case "&":
                    result = val1 && val2;
                    break;
                case "/":
                    result = val1 || val2;
                    break;
                case "%":
                    result = val1 == val2;
                    break;
                case "!&":
                    result = !(val1 && val2);
                    break;
                case "!/":
                    result = !(val1 || val2);
                    break;
                case "!%":
                    result = !(val1 == val2);
                    break;
                default:
                    string[] extra = { op };
                    Error(lines[lineNumber-1], 4, extra);
                    break;
            }
            return result;
        }
        static void Error(string line, int errorNum, string[] extra)
        {
            if(errorNum == 1)
            {
                Console.WriteLine($"Error {errorNum} on line {lineNumber}");
                Console.WriteLine($"{line}");
                Console.WriteLine($"undefined variable '{extra[0]}'");
            }
            else if(errorNum == 2)
            {
                Console.WriteLine($"Error {errorNum} on line {lineNumber}");
                Console.WriteLine($"{line}");
                Console.WriteLine($"variable '{extra[0]}' already defined");
            }
            else if (errorNum == 3)
            {
                Console.WriteLine($"Error {errorNum} on line {lineNumber}");
                Console.WriteLine($"{line}");
                Console.WriteLine($"Error: variable cant be named {extra[0]}");
            }
            else if (errorNum == 4)
            {
                Console.WriteLine($"Error {errorNum} on line {lineNumber}");
                Console.WriteLine($"{line}");
                Console.WriteLine($"Error: unrecognised operator {extra[0]}");
            }
            else if (errorNum == 5)
            {
                Console.WriteLine($"Error {errorNum} on line {lineNumber}");
                Console.WriteLine($"{line}");
                Console.WriteLine($"Error: insufficient inputs");
            }
            else if (errorNum == 6)
            {
                Console.WriteLine($"Error {errorNum} on line {lineNumber}");
                Console.WriteLine($"{line}");
                Console.WriteLine($"Error: too many inputs");
            }
            else if (errorNum == 7)
            {
                Console.WriteLine($"Error {errorNum} on line {lineNumber}");
                Console.WriteLine($"{line}");
                Console.WriteLine($"variables must be alphanumeric '{extra[0]}'");
            }


            Console.WriteLine("Press key to continue");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
