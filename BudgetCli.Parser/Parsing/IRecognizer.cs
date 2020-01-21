using System;
using System.Linq;

namespace BudgetCli.Parser.Parsing
{
    public interface IRecognizer<T>
    {

        IOrderedEnumerable<Recognizer<T>.MatchResult> Recognize(string text);

        IOrderedEnumerable<Recognizer<T>.MatchResult> Recognize(string text, Recognizer<T>.ConfidenceFunc confidenceFunc);
         
    }
}