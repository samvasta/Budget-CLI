using System;
using System.Collections.Generic;
using System.Linq;
using BudgetCli.Parser.Models;

namespace BudgetCli.Parser.Parsing
{
    public class Recognizer<T> : IRecognizer<T>
    {
        public delegate float ConfidenceFunc(float percentOfInputMatched, float percentOfValueMatched);

        private readonly List<ValueName> _valueNames;
        public Recognizer(IEnumerable<T> values, Func<T, IEnumerable<string>> valueToStringsFunc)
        {
            _valueNames = new List<ValueName>();

            foreach(var value in values)
            {
                foreach(var alternate in valueToStringsFunc(value))
                {
                    _valueNames.Add(new ValueName(value, alternate));
                }
            }
        }

        public Recognizer(Dictionary<T, Name> valueNames)
        {
            _valueNames = new List<ValueName>();

            foreach(var kvp in valueNames)
            {
                    _valueNames.Add(new ValueName(kvp.Key, kvp.Value.Preferred));
                foreach(var alternate in kvp.Value.Alternates)
                {
                    _valueNames.Add(new ValueName(kvp.Key, alternate));
                }
            }
        }

        public IOrderedEnumerable<MatchResult> Recognize(string input)
        {
            return Recognize(input, GetConfidence);
        }

        public IOrderedEnumerable<MatchResult> Recognize(string input, ConfidenceFunc confidenceFunc)
        {
            List<MatchResult> results = new List<MatchResult>();

            if(input == null)
            {
                return results.OrderBy(x => x);
            }

            List<ValueName> valueNamesCopy = new List<ValueName>(_valueNames);
            valueNamesCopy.ForEach(x => x.ResetEnumerator());

            for(int charIdx = 0; charIdx < input.Length; charIdx++)
            {
                char c = input[charIdx];
                int valueIdx = 0;
                while(valueIdx < valueNamesCopy.Count)
                {
                    var valueName = valueNamesCopy[valueIdx];
                    if(valueName.enumerator.MoveNext())
                    {
                        if(c.Equals(valueName.enumerator.Current))
                        {
                            //Matched character. Continue loop
                            valueIdx++;
                        }
                        else
                        {
                            //Unmatched character! Add to results list and remove from enumerators list
                            results.Add(new MatchResult(valueName.value, valueName.name, charIdx, (float)charIdx / (float)input.Length, (float)charIdx / (float)valueName.name.Length, confidenceFunc));
                            valueNamesCopy.Remove(valueName);
                        }
                    }
                    else
                    {
                        //End of the command name. 100% reached. Add to results list and remove from enumerators list
                        results.Add(new MatchResult(valueName.value, valueName.name, charIdx, (float)charIdx / (float)input.Length, 1.0f, confidenceFunc));
                        valueNamesCopy.Remove(valueName);
                    }
                }
            }

            //All that are left should get credit for matching the whole string
            foreach(var valueName in valueNamesCopy)
            {
                results.Add(new MatchResult(valueName.value, valueName.name, input.Length, 1.0f, (float)input.Length/(float)valueName.name.Length, confidenceFunc));
            }

            return results.OrderByDescending(x => x.Confidence).ThenBy(x => Math.Abs(input.Length - x.Text.Length)).ThenBy(x => x.Text);
        }

        public static float GetConfidence(float percentOfInputMatched, float percentOfValueMatched)
        {
            return (3.0f*percentOfInputMatched + percentOfValueMatched) / 4.0f;
        }

        class ValueName
        {
            internal readonly T value;
            internal readonly string name;
            internal readonly CharEnumerator enumerator;
            internal ValueName(T value, string name)
            {
                this.value = value;
                this.name = name;
                enumerator = name.GetEnumerator();
            }
            internal void ResetEnumerator()
            {
                enumerator.Reset();
            }
        }

        public struct MatchResult
        {
            public T Value { get; }
            public string Text { get; }
            public int NumMatchedChars { get; }
            public float PercentOfInput { get; }
            public float PercentOfText { get; }
            public float Confidence { get; }

            public MatchResult(T value, string text, int numChars, float percentOfInput, float percentOfText, ConfidenceFunc confidenceFunc)
            {
                Value = value;
                Text = text;
                NumMatchedChars = numChars;
                PercentOfInput = percentOfInput;
                PercentOfText = percentOfText;
                Confidence = confidenceFunc(percentOfInput, percentOfText);
            }
        }
    }
}