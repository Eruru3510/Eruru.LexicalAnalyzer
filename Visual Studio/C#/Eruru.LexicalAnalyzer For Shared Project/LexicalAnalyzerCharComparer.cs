using System.Collections.Generic;

namespace Eruru.LexicalAnalyzer {

	class LexicalAnalyzerCharComparer : IEqualityComparer<char> {

		public static LexicalAnalyzerCharComparer IgnoreCase { get; } = new LexicalAnalyzerCharComparer ();

		public bool Equals (char x, char y) {
			return char.ToUpperInvariant (x) == char.ToUpperInvariant (y);
		}

		public int GetHashCode (char obj) {
			return char.ToUpperInvariant (obj).GetHashCode ();
		}

	}

}