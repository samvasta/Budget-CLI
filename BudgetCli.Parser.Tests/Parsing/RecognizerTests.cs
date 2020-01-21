using System;
using System.Reflection;
using Xunit;
using Moq;
using BudgetCli.Parser.Parsing;
using System.Collections.Generic;
using System.Linq;

namespace BudgetCli.Parser.Tests.Parsing
{
    public class RecognizerTests
    {
        private IEnumerable<string> IntToStrings(int i)
        {
            if(i == 1) { return new []{"one", "1", "uno", "item 1", "a"}; }
            if(i == 2) { return new []{"two", "2", "dos", "item 2", "aa"}; }
            if(i == 3) { return new []{"three", "3", "tres", "item 3", "aaa"}; }
            if(i == 4) { return new []{"four", "4", "quatro", "item 4", "aaaa"}; }
            if(i == 5) { return new []{"five", "5", "cinco", "item 5", "aaaaa"}; }
            if(i == 11) { return new []{"eleven", "11", "once", "item 11", "aaaaaaaaaaa"}; }
            return new [] { String.Empty };
        }
        private IEnumerable<string> IntToAs(int i)
        {
            if(i == 1) { return new []{"a"}; }
            if(i == 2) { return new []{"aa"}; }
            if(i == 3) { return new []{"aaa"}; }
            if(i == 4) { return new []{"aaaa"}; }
            if(i == 5) { return new []{"aaaaa"}; }
            if(i == 11) { return new []{"aaaaaaaaaaa"}; }
            return new [] { String.Empty };
        }

        [Fact]
        public void TestRecognizer_Four()
        {
            Recognizer<int> recognizer = new Recognizer<int>(new [] {1, 2, 3, 4, 5, 11}, IntToStrings);

            var results = recognizer.Recognize("four", Recognizer<int>.GetConfidence);

            var oneResult = results.Where(x => x.Value == 1);
            var twoResult = results.Where(x => x.Value == 2);
            var threeResult = results.Where(x => x.Value == 3);
            var fourResult = results.Where(x => x.Value == 4);
            var fiveResult = results.Where(x => x.Value == 5);
            var elevenResult = results.Where(x => x.Value == 11);

            Assert.Equal(4, results.First().Value);         //first suggestion should be 4
            Assert.Equal(5, results.Skip(1).First().Value); //second suggestion should be 5

            Assert.Equal(0, oneResult.Max(x => x.NumMatchedChars));
            Assert.Equal(0, twoResult.Max(x => x.NumMatchedChars));
            Assert.Equal(0, threeResult.Max(x => x.NumMatchedChars));
            Assert.Equal(4, fourResult.Max(x => x.NumMatchedChars));    //_four_
            Assert.Equal(1, fiveResult.Max(x => x.NumMatchedChars));    //_f_ive
            Assert.Equal(0, elevenResult.Max(x => x.NumMatchedChars));
        }

        [Fact]
        public void TestRecognizer_One()
        {
            Recognizer<int> recognizer = new Recognizer<int>(new [] {1, 2, 3, 4, 5, 11}, IntToStrings);

            var results = recognizer.Recognize("one", Recognizer<int>.GetConfidence);

            var oneResult = results.Where(x => x.Value == 1);
            var twoResult = results.Where(x => x.Value == 2);
            var threeResult = results.Where(x => x.Value == 3);
            var fourResult = results.Where(x => x.Value == 4);
            var fiveResult = results.Where(x => x.Value == 5);
            var elevenResult = results.Where(x => x.Value == 11);

            Assert.Equal(1, results.First().Value);
            Assert.Equal(11, results.Skip(1).First().Value);

            Assert.Equal(3, oneResult.Max(x => x.NumMatchedChars));     //_one_
            Assert.Equal(0, twoResult.Max(x => x.NumMatchedChars));
            Assert.Equal(0, threeResult.Max(x => x.NumMatchedChars));
            Assert.Equal(0, fourResult.Max(x => x.NumMatchedChars));
            Assert.Equal(0, fiveResult.Max(x => x.NumMatchedChars));
            Assert.Equal(2, elevenResult.Max(x => x.NumMatchedChars));  //_on_ce
        }

        [Fact]
        public void TestRecognizer_Item1()
        {
            Recognizer<int> recognizer = new Recognizer<int>(new [] {1, 2, 3, 4, 5, 11}, IntToStrings);

            var results = recognizer.Recognize("item 1", Recognizer<int>.GetConfidence);

            var oneResult = results.Where(x => x.Value == 1);
            var twoResult = results.Where(x => x.Value == 2);
            var threeResult = results.Where(x => x.Value == 3);
            var fourResult = results.Where(x => x.Value == 4);
            var fiveResult = results.Where(x => x.Value == 5);
            var elevenResult = results.Where(x => x.Value == 11);

            Assert.Equal(1, results.First().Value);
            Assert.Equal(11, results.Skip(1).First().Value);

            Assert.Equal(6, oneResult.Max(x => x.NumMatchedChars));     //_item 1_
            Assert.Equal(5, twoResult.Max(x => x.NumMatchedChars));     //_item_2
            Assert.Equal(5, threeResult.Max(x => x.NumMatchedChars));   //_item_3
            Assert.Equal(5, fourResult.Max(x => x.NumMatchedChars));    //_item_4
            Assert.Equal(5, fiveResult.Max(x => x.NumMatchedChars));    //_item_5
            Assert.Equal(6, elevenResult.Max(x => x.NumMatchedChars));  //_item 1_1
        }

        [Fact]
        public void TestRecognizer_ElevenLong()
        {
            Recognizer<int> recognizer = new Recognizer<int>(new [] {1, 2, 3, 4, 5, 11}, IntToStrings);

            var results = recognizer.Recognize("11 and then the rest of the string", Recognizer<int>.GetConfidence);

            var oneResult = results.Where(x => x.Value == 1);
            var twoResult = results.Where(x => x.Value == 2);
            var threeResult = results.Where(x => x.Value == 3);
            var fourResult = results.Where(x => x.Value == 4);
            var fiveResult = results.Where(x => x.Value == 5);
            var elevenResult = results.Where(x => x.Value == 11);

            Assert.Equal(11, results.First().Value);
            Assert.Equal(1, results.Skip(1).First().Value);

            Assert.Equal(1, oneResult.Max(x => x.NumMatchedChars));     //_1_
            Assert.Equal(0, twoResult.Max(x => x.NumMatchedChars));
            Assert.Equal(0, threeResult.Max(x => x.NumMatchedChars));
            Assert.Equal(0, fourResult.Max(x => x.NumMatchedChars));
            Assert.Equal(0, fiveResult.Max(x => x.NumMatchedChars));
            Assert.Equal(2, elevenResult.Max(x => x.NumMatchedChars));  //_11_
        }

        

        [Fact]
        public void TestRecognizer_1A()
        {
            Recognizer<int> recognizer = new Recognizer<int>(new [] {1, 2, 3, 4, 5, 11}, IntToAs);

            var results = recognizer.Recognize("a", Recognizer<int>.GetConfidence);

            var oneResult = results.Where(x => x.Value == 1);
            var twoResult = results.Where(x => x.Value == 2);
            var threeResult = results.Where(x => x.Value == 3);
            var fourResult = results.Where(x => x.Value == 4);
            var fiveResult = results.Where(x => x.Value == 5);
            var elevenResult = results.Where(x => x.Value == 11);

            Assert.Equal(1, results.First().Value);
            Assert.Equal(2, results.Skip(1).First().Value);
            Assert.Equal(3, results.Skip(2).First().Value);
            Assert.Equal(4, results.Skip(3).First().Value);
            Assert.Equal(5, results.Skip(4).First().Value);
            Assert.Equal(11, results.Skip(5).First().Value);

            Assert.Equal(1, oneResult.Max(x => x.NumMatchedChars));     //_a_
            Assert.Equal(1, twoResult.Max(x => x.NumMatchedChars));     //_a_a
            Assert.Equal(1, threeResult.Max(x => x.NumMatchedChars));   //_a_aa
            Assert.Equal(1, fourResult.Max(x => x.NumMatchedChars));    //_a_aaa
            Assert.Equal(1, fiveResult.Max(x => x.NumMatchedChars));    //_a_aaaa
            Assert.Equal(1, elevenResult.Max(x => x.NumMatchedChars));  //_a_aaaaaaaaaa
        }

        
        [Fact]
        public void TestRecognizer_2A()
        {
            Recognizer<int> recognizer = new Recognizer<int>(new [] {1, 2, 3, 4, 5, 11}, IntToAs);

            var results = recognizer.Recognize("aa", Recognizer<int>.GetConfidence);

            var oneResult = results.Where(x => x.Value == 1);
            var twoResult = results.Where(x => x.Value == 2);
            var threeResult = results.Where(x => x.Value == 3);
            var fourResult = results.Where(x => x.Value == 4);
            var fiveResult = results.Where(x => x.Value == 5);
            var elevenResult = results.Where(x => x.Value == 11);

            Assert.Equal(2, results.First().Value);
            Assert.Equal(3, results.Skip(1).First().Value);
            Assert.Equal(4, results.Skip(2).First().Value);
            Assert.Equal(5, results.Skip(3).First().Value);
            Assert.Equal(11, results.Skip(4).First().Value);
            Assert.Equal(1, results.Skip(5).First().Value);

            Assert.Equal(1, oneResult.Max(x => x.NumMatchedChars));     //_a_
            Assert.Equal(2, twoResult.Max(x => x.NumMatchedChars));     //_aa_
            Assert.Equal(2, threeResult.Max(x => x.NumMatchedChars));   //_aa_a
            Assert.Equal(2, fourResult.Max(x => x.NumMatchedChars));    //_aa_aa
            Assert.Equal(2, fiveResult.Max(x => x.NumMatchedChars));    //_aa_aaa
            Assert.Equal(2, elevenResult.Max(x => x.NumMatchedChars));  //_aa_aaaaaaaaa
        }

        
        [Fact]
        public void TestRecognizer_4A()
        {
            Recognizer<int> recognizer = new Recognizer<int>(new [] {1, 2, 3, 4, 5, 11}, IntToAs);

            var results = recognizer.Recognize("aaaa", Recognizer<int>.GetConfidence);

            var oneResult = results.Where(x => x.Value == 1);
            var twoResult = results.Where(x => x.Value == 2);
            var threeResult = results.Where(x => x.Value == 3);
            var fourResult = results.Where(x => x.Value == 4);
            var fiveResult = results.Where(x => x.Value == 5);
            var elevenResult = results.Where(x => x.Value == 11);

            Assert.Equal(4, results.First().Value);
            Assert.Equal(5, results.Skip(1).First().Value);
            Assert.Equal(11, results.Skip(2).First().Value);
            Assert.Equal(3, results.Skip(3).First().Value);
            Assert.Equal(2, results.Skip(4).First().Value);
            Assert.Equal(1, results.Skip(5).First().Value);

            Assert.Equal(1, oneResult.Max(x => x.NumMatchedChars));     //_a_
            Assert.Equal(2, twoResult.Max(x => x.NumMatchedChars));     //_aa_
            Assert.Equal(3, threeResult.Max(x => x.NumMatchedChars));   //_aaa_
            Assert.Equal(4, fourResult.Max(x => x.NumMatchedChars));    //_aaaa_
            Assert.Equal(4, fiveResult.Max(x => x.NumMatchedChars));    //_aaaa_a
            Assert.Equal(4, elevenResult.Max(x => x.NumMatchedChars));  //_aaaa_aaaaaaa
        }

        
        [Fact]
        public void TestRecognizer_Empty()
        {
            Recognizer<int> recognizer = new Recognizer<int>(new [] {1, 2, 3, 4, 5, 11}, IntToAs);

            var results = recognizer.Recognize("", Recognizer<int>.GetConfidence);

            var oneResult = results.Where(x => x.Value == 1);
            var twoResult = results.Where(x => x.Value == 2);
            var threeResult = results.Where(x => x.Value == 3);
            var fourResult = results.Where(x => x.Value == 4);
            var fiveResult = results.Where(x => x.Value == 5);
            var elevenResult = results.Where(x => x.Value == 11);

            Assert.Equal(1, results.First().Value);
            Assert.Equal(2, results.Skip(1).First().Value);
            Assert.Equal(3, results.Skip(2).First().Value);
            Assert.Equal(4, results.Skip(3).First().Value);
            Assert.Equal(5, results.Skip(4).First().Value);
            Assert.Equal(11, results.Skip(5).First().Value);

            Assert.Equal(0, oneResult.Max(x => x.NumMatchedChars));
            Assert.Equal(0, twoResult.Max(x => x.NumMatchedChars));
            Assert.Equal(0, threeResult.Max(x => x.NumMatchedChars));
            Assert.Equal(0, fourResult.Max(x => x.NumMatchedChars));
            Assert.Equal(0, fiveResult.Max(x => x.NumMatchedChars));
            Assert.Equal(0, elevenResult.Max(x => x.NumMatchedChars));
        }
        
        [Fact]
        public void TestRecognizer_Null()
        {
            Recognizer<int> recognizer = new Recognizer<int>(new [] {1, 2, 3, 4, 5, 11}, IntToAs);

            var results = recognizer.Recognize(null, Recognizer<int>.GetConfidence);

            Assert.Empty(results);
        }
    }
}