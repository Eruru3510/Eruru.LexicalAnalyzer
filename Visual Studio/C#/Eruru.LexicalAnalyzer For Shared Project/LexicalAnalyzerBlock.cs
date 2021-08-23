namespace Eruru.LexicalAnalyzer {

	public class LexicalAnalyzerBlock<T> {

		public T Type { get; set; }
		public string Start { get; set; }
		public string End { get; set; }

		public LexicalAnalyzerBlock (T type, string start, string end) {
			Type = type;
			Start = start;
			End = end;
		}

	}

}