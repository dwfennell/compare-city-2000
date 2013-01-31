using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// A thread safe random number generator.
/// </summary>
/// <remarks>
/// Thanks to stackoverflow user blueraja-danny-pflughoeft for pointing to Stephen Toub's msdn blog containing this code.
/// http://stackoverflow.com/questions/3049467/is-c-sharp-random-number-generator-thread-safe
/// 
/// Thanks also, of course, to Stephen Toub.
/// http://blogs.msdn.com/b/pfxteam/archive/2009/02/19/9434171.aspx
/// </remarks> 
namespace CompareCity.Util
{
    public class ThreadSafeRandom
    {
        private static readonly Random _global = new Random();
        [ThreadStatic]
        private static Random _local;

        public ThreadSafeRandom()
        {
            if (_local == null)
            {
                initLocal();
            }
        }
        public int Next()
        {
            if (_local == null)
            {
                // Another check since we are still getting nullpointers. -dustin
                initLocal();
            }
            return _local.Next();
        }

        private void initLocal()
        {
            int seed;
            lock (_global)
            {
                seed = _global.Next();
            }
            _local = new Random(seed);
        }
    }
}