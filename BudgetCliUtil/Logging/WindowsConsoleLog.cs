using System;
using System.Collections.Generic;
using System.Drawing;
using cc = Colorful.Console;

namespace BudgetCliUtil.Logging
{
    public class ConsoleLog : ILog
    {
        private Dictionary<LogLevel, Color> _logColors;

        public ConsoleLog()
        {
            _logColors = new Dictionary<LogLevel, Color>();
            SetLevelColor(LogLevel.Normal, Color.White);
            SetLevelColor(LogLevel.Important, Color.CadetBlue);
            SetLevelColor(LogLevel.Warning, Color.Goldenrod);
            SetLevelColor(LogLevel.Error, Color.Firebrick);
        }

        public void SetLevelColor(LogLevel level, Color color)
        {
            _logColors[level] = color;
        }

         public void Write(string text)
         {
             cc.Write(text);
         }

         public void Write(string text, Color color)
         {
             cc.Write(text, color);
         }

         public void Write(string text, LogLevel level)
         {
             Write(text, _logColors[level]);
         }

         public void WriteLine()
         {
             cc.WriteLine();
         }

         public void WriteLine(string text)
         {
             cc.WriteLine(text);
         }

         public void WriteLine(string text, Color color)
         {
             cc.WriteLine(text, color);
         }

         public void WriteLine(string text, LogLevel level)
         {
             WriteLine(text, _logColors[level]);
         }
    }
}