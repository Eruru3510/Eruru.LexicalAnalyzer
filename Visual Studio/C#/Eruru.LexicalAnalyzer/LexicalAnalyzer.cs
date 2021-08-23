using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Eruru.LexicalAnalyzer {

	public class LexicalAnalyzer<T> : IDisposable, IEnumerator<LexicalAnalyzerToken<T>> {

		public T EndType { get; set; }
		public T UnknownType { get; set; }
		public T IntegerType { get; set; }
		public T DecimalType { get; set; }
		public T StringType { get; set; }
		public TextReader TextReader {

			set {
				_TextReader = value;
				Index = 0;
				Line = 1;
				LineIndex = 0;
				_Current = null;
			}

		}
		public bool IgnoreCase { get; }
		public bool AllowString { get; set; } = true;
		public bool AllowNumber { get; set; } = true;
		public bool AllowIgnoreCharactersBreakKeyword { get; set; } = true;
		public bool AllowCharactersBreakKeyword { get; set; } = true;
		public List<char> IgnoreCharacters { get; set; } = new List<char> () { ' ', '\t', '\r', '\n' };
		public List<char> BreakKeywordCharacters { get; set; } = new List<char> () { ' ', '\t', '\r', '\n' };
		public List<LexicalAnalyzerBlock<T>> Blocks { get; set; } = new List<LexicalAnalyzerBlock<T>> ();
		public Dictionary<string, KeyValuePair<T, object>> Symbols { get; set; } = new Dictionary<string, KeyValuePair<T, object>> ();
		public Dictionary<char, T> Characters { get; set; }
		public List<char> StringStartCharacters { get; set; } = new List<char> () { '\'', '"' };
		public Dictionary<char, T> TailCharacters { get; set; }
		public Dictionary<string, KeyValuePair<T, object>> Keywords { get; set; } = new Dictionary<string, KeyValuePair<T, object>> ();
		public int Index { get; private set; }

		readonly List<int> Buffer = new List<int> ();

		TextReader _TextReader;
		int Line, LineIndex;

		public LexicalAnalyzer (T endType, T unknownType, T integerType, T decimalType, T stringType, bool ignoreCase = false) {
			EndType = endType;
			UnknownType = unknownType;
			IntegerType = integerType;
			DecimalType = decimalType;
			StringType = stringType;
			IgnoreCase = ignoreCase;
			Characters = IgnoreCase ? new Dictionary<char, T> (LexicalAnalyzerCharComparer.IgnoreCase) : new Dictionary<char, T> ();
			TailCharacters = IgnoreCase ? new Dictionary<char, T> (LexicalAnalyzerCharComparer.IgnoreCase) : new Dictionary<char, T> ();
			Keywords = IgnoreCase ? new Dictionary<string, KeyValuePair<T, object>> (StringComparer.OrdinalIgnoreCase) : new Dictionary<string, KeyValuePair<T, object>> ();
		}
		public LexicalAnalyzer (TextReader textReader, T endType, T unknownType, T integerType, T decimalType, T stringType, bool ignoreCase = false)
			: this (endType, unknownType, integerType, decimalType, stringType, ignoreCase) {
			TextReader = textReader;
			EndType = endType;
			UnknownType = unknownType;
			IntegerType = integerType;
			DecimalType = decimalType;
			StringType = stringType;
		}

		public void AddBlock (T type, string start, string end) {
			Blocks.Add (new LexicalAnalyzerBlock<T> (type, start, end));
		}

		public void AddSymbol (string text, T type, object value = null) {
			Symbols.Add (text, new KeyValuePair<T, object> (type, value));
		}

		public void AddKeyword (string text, T type, object value = null) {
			Keywords.Add (text, new KeyValuePair<T, object> (type, value));
		}

		public int SkipIgnoreCharacters () {
			while (Peek () > -1) {
				if (LexicalAnalyzerAPI.Contains (IgnoreCharacters, (char)Peek (), IgnoreCase)) {
					Read ();
					continue;
				}
				break;
			}
			return Peek ();
		}

		public string ReadTo (string[] ends, bool ignoreCase, bool eatEnd) {
			StringBuilder stringBuilder = new StringBuilder ();
			while (Peek () > -1) {
				foreach (string end in ends) {
					if (Match (end, ignoreCase, eatEnd)) {
						break;
					}
				}
				stringBuilder.Append ((char)Read ());
			}
			return stringBuilder.ToString ();
		}
		public string ReadTo (string[] ends, bool eatEnd) {
			return ReadTo (ends, IgnoreCase, eatEnd);
		}
		public string ReadTo (string[] ends) {
			return ReadTo (ends, IgnoreCase, true);
		}
		public string ReadTo (string end, bool ignoreCase, bool eatEnd) {
			StringBuilder stringBuilder = new StringBuilder ();
			while (Peek () > -1) {
				if (Match (end, ignoreCase, eatEnd)) {
					break;
				}
				stringBuilder.Append ((char)Read ());
			}
			return stringBuilder.ToString ();
		}
		public string ReadTo (string end, bool eatEnd) {
			return ReadTo (end, IgnoreCase, eatEnd);
		}
		public string ReadTo (string end) {
			return ReadTo (end, IgnoreCase, true);
		}

		public int Peek () {
			return Buffer.Count == 0 ? _TextReader.Peek () : Buffer[0];
		}

		public int Read () {
			int character;
			if (Buffer.Count == 0) {
				character = _TextReader.Read ();
			} else {
				character = Buffer[0];
				Buffer.RemoveAt (0);
			}
			Index++;
			if (character == '\n') {
				Line++;
				LineIndex = 0;
			} else {
				LineIndex++;
			}
			return character;
		}

		public int Pushbuffer () {
			int character = _TextReader.Read ();
			Buffer.Add (character);
			return character;
		}

		public void ClearBuffer () {
			while (Buffer.Count > 0) {
				Read ();
			}
		}

		public bool Match (string end, bool ignoreCase, bool eat) {
			if (Peek () == end[0]) {
				if (end.Length == 1) {
					if (eat) {
						Read ();
					}
					return true;
				}
			} else {
				return false;
			}
			if (Buffer.Count == 0) {
				Pushbuffer ();
			}
			for (int i = 1; i < end.Length; i++) {
				if (Buffer.Count <= i) {
					Pushbuffer ();
				}
				if (!LexicalAnalyzerAPI.Equals ((char)Buffer[i], end[i], ignoreCase)) {
					return false;
				}
			}
			if (eat) {
				ClearBuffer ();
			}
			return true;
		}
		public bool Match (string end, bool eat) {
			return Match (end, IgnoreCase, eat);
		}
		public bool Match (string end) {
			return Match (end, IgnoreCase, true);
		}

		public bool IsNumber (char character) {
			switch (character) {
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					return true;
			}
			return false;
		}

		public string ReadString (char end, out bool isClosed) {
			StringBuilder stringBuilder = new StringBuilder ();
			isClosed = false;
			while (Peek () > -1) {
				char character = (char)Peek ();
				if (character == end) {
					isClosed = true;
					Read ();
					break;
				}
				if (character == '\\') {
					stringBuilder.Append (character);
					Read ();
					if (Peek () == -1) {
						break;
					}
					character = (char)Peek ();
				}
				stringBuilder.Append (character);
				Read ();
			}
			return stringBuilder.ToString ();
		}

		public string ReadNumber (out bool isDecimal, out bool isScientificNotation) {
			StringBuilder stringBuilder = new StringBuilder ();
			int startIndex = 1;
			isDecimal = false;
			isScientificNotation = false;
			int signIndex = 0;
			while (Peek () > -1) {
				char character = (char)Peek ();
				bool needBreak = false;
				switch (character) {
					case '+':
					case '-':
						if (stringBuilder.Length != signIndex) {
							needBreak = true;
						}
						startIndex = stringBuilder.Length + 2;
						break;
					case '.':
						if (isDecimal || isScientificNotation) {
							needBreak = true;
						}
						isDecimal = true;
						startIndex = stringBuilder.Length + 2;
						break;
					case 'e':
					case 'E':
						if (isScientificNotation || stringBuilder.Length < startIndex) {
							needBreak = true;
						}
						isScientificNotation = true;
						signIndex = stringBuilder.Length + 1;
						break;
					default:
						if (!IsNumber (character)) {
							needBreak = true;
						}
						break;
				}
				if (needBreak) {
					break;
				}
				stringBuilder.Append (character);
				Read ();
			}
			return stringBuilder.ToString ();
		}

		public string ReadKeyword () {
			StringBuilder stringBuilder = new StringBuilder ();
			while (Peek () > -1) {
				char character = (char)Peek ();
				if (AllowIgnoreCharactersBreakKeyword && LexicalAnalyzerAPI.Contains (IgnoreCharacters, character, IgnoreCase)) {
					break;
				}
				if (AllowCharactersBreakKeyword && Characters.ContainsKey (character)) {
					break;
				}
				if (LexicalAnalyzerAPI.Contains (BreakKeywordCharacters, character, IgnoreCase)) {
					break;
				}
				stringBuilder.Append (character);
				Read ();
			}
			return stringBuilder.ToString ();
		}

		#region  IEnumerator<LexicalAnalyzerToken<T>>

		public LexicalAnalyzerToken<T> Current {

			get {
				if (_Current == null) {
					MoveNext ();
				}
				return _Current;
			}

		}

		object IEnumerator.Current => Current;
		LexicalAnalyzerToken<T> _Current;

		public bool MoveNext () {
			SkipIgnoreCharacters ();
			if (Peek () == -1) {
				_Current = new LexicalAnalyzerToken<T> (EndType, null, Index, 1, Line, LineIndex);
				return false;
			}
			int startIndex = Index;
			int line = Line;
			int lineStartIndex = LineIndex;
			string text;
			foreach (var block in Blocks) {
				if (Match (block.Start)) {
					text = ReadTo (block.End);
					_Current = new LexicalAnalyzerToken<T> (block.Type, text, startIndex, text.Length, line, lineStartIndex);
					return true;
				}
			}
			foreach (var symbol in Symbols) {
				if (Match (symbol.Key)) {
					_Current = new LexicalAnalyzerToken<T> (symbol.Value.Key, symbol.Value.Value, startIndex, symbol.Key.Length, line, lineStartIndex);
					return true;
				}
			}
			char character = (char)Peek ();
			if (Characters.TryGetValue (character, out T type)) {
				_Current = new LexicalAnalyzerToken<T> (type, character, startIndex, 1, line, lineStartIndex);
				Read ();
				return true;
			}
			if (AllowString && LexicalAnalyzerAPI.Contains (StringStartCharacters, character, IgnoreCase)) {
				Read ();
				text = ReadString (character, out bool isClosed);
				_Current = new LexicalAnalyzerToken<T> (StringType, text, startIndex, text.Length + (isClosed ? 2 : 1), line, lineStartIndex);
				return true;
			}
			if (AllowNumber) {
				bool parse = false;
				switch (character) {
					case '+':
					case '-':
					case '.':
						Pushbuffer ();
						char temp = (char)_TextReader.Peek ();
						if ((character != '.' && temp == '.') || IsNumber (temp)) {
							parse = true;
						}
						break;
					default:
						if (IsNumber (character)) {
							parse = true;
						}
						break;
				}
				if (parse) {
					text = ReadNumber (out bool isDecimal, out bool isScientificNotation);
					if (!isDecimal || isScientificNotation) {
						if (long.TryParse (text, NumberStyles.Any, null, out long longResult)) {
							_Current = new LexicalAnalyzerToken<T> (IntegerType, longResult, startIndex, text.Length, line, lineStartIndex);
							return true;
						}
					}
					if (isDecimal || isScientificNotation) {
						if (decimal.TryParse (text, NumberStyles.Any, null, out decimal decimalResult)) {
							_Current = new LexicalAnalyzerToken<T> (DecimalType, decimalResult, startIndex, text.Length, line, lineStartIndex);
							return true;
						}
					}
					_Current = new LexicalAnalyzerToken<T> (UnknownType, text, startIndex, text.Length, line, lineStartIndex);
					return true;
				}
			}
			if (TailCharacters.TryGetValue (character, out type)) {
				_Current = new LexicalAnalyzerToken<T> (type, character, startIndex, 1, line, lineStartIndex);
				Read ();
				return true;
			}
			text = ReadKeyword ();
			if (Keywords.TryGetValue (text, out KeyValuePair<T, object> keyword)) {
				_Current = new LexicalAnalyzerToken<T> (keyword.Key, keyword.Value, startIndex, text.Length, line, lineStartIndex);
				return true;
			}
			_Current = new LexicalAnalyzerToken<T> (UnknownType, text, startIndex, text.Length, line, lineStartIndex);
			return true;
		}

		public void Reset () {
			throw new Exception ($"无法重置{nameof (TextReader)}，请通过为{nameof (TextReader)}属性赋值来实现");
		}

		#endregion

		#region IDisposable

		public void Dispose () {
			_TextReader.Dispose ();
		}

		#endregion

	}

}