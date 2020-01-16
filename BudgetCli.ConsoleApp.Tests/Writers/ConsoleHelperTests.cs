using System;
using System.Linq;
using Xunit;
using Moq;
using BudgetCli.ConsoleApp.Interfaces;
using BudgetCli.ConsoleApp.Writers;

namespace BudgetCli.ConsoleApp.Tests.Writers
{
    public class ConsoleHelperTests
    {
        [Fact]
        public void TestIndent()
        {
            string output = "";

            Action<string> writeFunc = (string text) =>
            {
                output += text;
            };

            ConsoleHelper.Indent(4, writeFunc, 0);

            writeFunc("Test");

            Assert.Equal("    Test", output);
        }
        
        [Fact]
        public void TestIndent_CursorPastIndent()
        {
            string output = "";

            Action<string> writeFunc = (string text) =>
            {
                output += text;
            };

            ConsoleHelper.Indent(4, writeFunc, 6);

            writeFunc("Test");

            Assert.Equal("Test", output);
        }

        [Theory]
        [InlineData(new []{"test", "test test", "test"}, 0)]
        [InlineData(new []{"test2", "test2 test2", "test2"}, 2)]
        [InlineData(new []{"test3", "test3 test3", "", "test3"}, 2)]
        public void TestIndentString(string[] inputLines, int indent)
        {
            string output = "";

            Action<string> writeFunc = (string text) =>
            {
                output += text;
            };

            ConsoleHelper.WriteWithIndent(string.Join(Environment.NewLine, inputLines), indent, writeFunc, 80, 0);

            string expected = string.Join(Environment.NewLine, inputLines.Select(x => new string(' ', indent) + x));

            Assert.Equal(expected, output);
        }

        
        [Fact]
        public void TestIndentString_LongString()
        {
            string output = "";

            Action<string> writeFunc = (string text) =>
            {
                output += text;
            };

            ConsoleHelper.WriteWithIndent("1234567890abcdefghijklmnopqrstuvwxy_and_z", 4, writeFunc, 10, 0);

            string expected = $"    123456{Environment.NewLine}" +
                              $"    7890ab{Environment.NewLine}" +
                              $"    cdefgh{Environment.NewLine}" +
                              $"    ijklmn{Environment.NewLine}" +
                              $"    opqrst{Environment.NewLine}" +
                              $"    uvwxy_{Environment.NewLine}" +
                              $"    and_z";

            Assert.Equal(expected, output);
        }

        

        
        [Fact]
        public void TestIndentString_MidLineStart()
        {
            string output = "        ";

            Action<string> writeFunc = (string text) =>
            {
                output += text;
            };

            ConsoleHelper.WriteWithIndent("1234567890abcdefg", 4, writeFunc, 10, 8);

            string expected = $"        12{Environment.NewLine}" +
                              $"    345678{Environment.NewLine}" +
                              $"    90abcd{Environment.NewLine}" +
                              $"    efg";

            Assert.Equal(expected, output);
        }
    }
}