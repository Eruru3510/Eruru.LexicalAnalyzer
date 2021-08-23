using System.Collections.Generic;

namespace Eruru.LexicalAnalyzer {

	class LexicalAnalyzerAPI {

		public static bool Equals (char a, char b, bool ignoreCase) {
			if (ignoreCase) {
				return char.ToUpperInvariant (a) == char.ToUpperInvariant (b);
			}
			return a == b;
		}

		public static bool Contains (List<char> characters, char character, bool ignoreCase) {
			return characters.Exists (item => Equals (item, character, ignoreCase));
		}

	}

}