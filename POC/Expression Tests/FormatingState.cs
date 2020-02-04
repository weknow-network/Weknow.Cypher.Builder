using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static Weknow.Cypher.Builder.Pattern;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    public class FormatingState
    {
        public static readonly FormatingState Default = new FormatingState(string.Empty);

        public FormatingState(string format)
        {
            Format = format;
        }

        public int Index { get; private set; }
        public string Format { get; }
        public char Current => Format[Index];
        public bool Ended => Index >= Format.Length;


        public static implicit operator int(FormatingState item) => item.Index;


        public static FormatingState operator ++(FormatingState instance) 
        {
            instance.Index += 1;
            return instance;
        }

        public static FormatingState operator --(FormatingState instance) 
        {
            instance.Index -= 1;
            return instance;
        }
    }

}
