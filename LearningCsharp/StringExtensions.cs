using System;
using System.Collections.Generic;
using System.Text;

namespace LearningCsharp
{
    internal static class StringExtensions
    {
        public static bool isBuyukHarfIleBasliyorMu(this string str)
        {
            return char.IsUpper(str[0]);
        }
    }
}
