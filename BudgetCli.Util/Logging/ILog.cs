using System.Drawing;

namespace BudgetCli.Util.Logging
{
    public interface ILog
    {
        void Write(string text);
        void Write(string text, Color color);
        void Write(string text, LogLevel level);
        
        void WriteLine();
        void WriteLine(string text);
         void WriteLine(string text, Color color);
         void WriteLine(string text, LogLevel level);

    }
}