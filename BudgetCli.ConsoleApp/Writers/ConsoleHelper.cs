using System;
using System.Drawing;
using BudgetCli.ConsoleApp.Interfaces;
using Console = Colorful.Console;

namespace BudgetCli.ConsoleApp.Writers
{
    public static class ConsoleHelper
    {
        public static void WriteWithIndent(string text, int charIndentNum, ConsoleColor color)
        {
            int cursorLeft = Console.CursorLeft;
            WriteWithIndent(text, charIndentNum, x => Console.Write(x, color), Console.WindowWidth, ref cursorLeft);
        }
        public static void WriteWithIndent(string text, int charIndentNum, Color color)
        {
            int cursorLeft = Console.CursorLeft;
            WriteWithIndent(text, charIndentNum, x => Console.Write(x, color), Console.WindowWidth, ref cursorLeft);
        }
        public static void WriteWithIndent(string text, int charIndentNum)
        {
            int cursorLeft = Console.CursorLeft;
            WriteWithIndent(text, charIndentNum, Console.Write, Console.WindowWidth, ref cursorLeft);
        }

        public static void Indent(int charIndentNum)
        {
            Indent(charIndentNum, Console.Write, Console.CursorLeft);
        }

        public static void WriteWithIndent(string text, int charIndentNum, Action<string> writeFunc, int consoleWidth, int cursorLeft)
        {
            int cursorLeftRef = cursorLeft;
            WriteWithIndent(text, charIndentNum, writeFunc, consoleWidth, ref cursorLeftRef);
        }
        
        private static void WriteWithIndent(string text, int charIndentNum, Action<string> writeFunc, int consoleWidth, ref int cursorLeft)
        {
            //Treat the console like 2 columns. 1st column is the indent, 2nd column is the content            
            int secondColumnWidth = consoleWidth - charIndentNum;

            string[] lines = text.Split(Environment.NewLine);
            for(int lineIdx = 0; lineIdx < lines.Length; lineIdx++)
            {
                string line = lines[lineIdx];
                
                Indent(charIndentNum, writeFunc, cursorLeft);
                cursorLeft = Math.Max(charIndentNum, cursorLeft);

                //Tokens may NOT be split onto multiple lines. Line feed should exist only between tokens
                string[] tokens = line.Split(" ");
                
                for(int tokenIdx = 0; tokenIdx < tokens.Length; tokenIdx++)
                {
                    string token = tokens[tokenIdx];

                    if(token.Length > secondColumnWidth)
                    {
                        WriteWrapWithIndent(token, charIndentNum, writeFunc, consoleWidth, ref cursorLeft);
                    }
                    else
                    {
                        if(cursorLeft + token.Length > consoleWidth)
                        {
                            //Not done with the entire string yet so go to new line & indent
                            writeFunc(Environment.NewLine);
                            Indent(charIndentNum, writeFunc, 0);
                            cursorLeft = charIndentNum;
                        }

                        writeFunc(token);
                    }
                    //Write space if there is another token
                    if(tokenIdx < tokens.Length-1 && cursorLeft < consoleWidth-2)
                    {
                        writeFunc(" ");
                        cursorLeft++;
                    }
                    cursorLeft += token.Length;
                }

                if(lineIdx < lines.Length-1)
                {
                    writeFunc(Environment.NewLine);
                    cursorLeft = 0;
                }
            }
        }

        private static void WriteWrapWithIndent(string token,  int charIndentNum, Action<string> writeFunc, int consoleWidth, ref int cursorLeft)
        {
            if(token.Contains(Environment.NewLine))
            {
                throw new ArgumentException("Cannot write wrap with indent when token contains a line break");
            }

            //Treat the console like 2 columns. 1st column is the indent, 2nd column is the content            
            int secondColumnWidth = consoleWidth - charIndentNum;

            Indent(charIndentNum, writeFunc, cursorLeft);
            cursorLeft = Math.Max(charIndentNum, cursorLeft);

            int posInText = 0;
            while(posInText < token.Length)
            {
                int secondColumnCursorPosition = cursorLeft - charIndentNum;
                int subStringLength = Math.Min(secondColumnWidth - secondColumnCursorPosition, token.Length-posInText);

                string substring = token.Substring(posInText, subStringLength);
                
                writeFunc(substring);
                posInText += substring.Length;
                cursorLeft += substring.Length;

                if(posInText < token.Length)
                {
                    //Not done with the entire string yet so go to new line & indent
                    writeFunc(Environment.NewLine);
                    Indent(charIndentNum, writeFunc, 0);
                    cursorLeft = charIndentNum;
                }
            }
        }

        public static void Indent(int charIndentNum, Action<string> writeFunc, int cursorLeft)
        {
            //Pad out to the start of the second column
            if(cursorLeft < charIndentNum)
            {
                writeFunc(new string(' ', charIndentNum - cursorLeft));
            }
        }
    }
}