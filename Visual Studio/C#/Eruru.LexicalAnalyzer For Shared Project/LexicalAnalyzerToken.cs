using System;

namespace Eruru.LexicalAnalyzer {

	public class LexicalAnalyzerToken<T> {

		public T Type { get; set; }
		public object Value { get; set; }
		public int StartIndex { get; set; }
		public int Length { get; set; }
		public int Line { get; set; }
		public int LineStartIndex { get; set; }
		public byte Byte {

			get => ToByte ();

			set => Value = value;

		}
		public ushort UShort {

			get => ToUShort ();

			set => Value = value;

		}
		public uint UInt {

			get => ToUInt ();

			set => Value = value;

		}
		public ulong ULong {

			get => ToULong ();

			set => Value = value;

		}
		public sbyte SByte {

			get => ToSByte ();

			set => Value = value;

		}
		public short Short {

			get => ToShort ();

			set => Value = value;

		}
		public int Int {

			get => ToInt ();

			set => Value = value;

		}
		public long Long {

			get => ToLong ();

			set => Value = value;

		}
		public float Float {

			get => ToFloat ();

			set => Value = value;

		}
		public double Double {

			get => ToDouble ();

			set => Value = value;

		}
		public decimal Decimal {

			get => ToDecimal ();

			set => Value = value;

		}
		public bool Bool {

			get => ToBool ();

			set => Value = value;

		}
		public char Char {

			get => ToChar ();

			set => Value = value;

		}
		public string String {

			get => ToString ();

			set => Value = value;

		}
		public DateTime DateTime {

			get => ToDateTime ();

			set => Value = value;

		}

		public LexicalAnalyzerToken (T type, object value, int startIndex, int length, int line, int lineStartIndex) {
			Type = type;
			Value = value;
			StartIndex = startIndex;
			Length = length;
			Line = line;
			LineStartIndex = lineStartIndex;
		}

		public byte ToByte (byte defaultValue = default) {
			try {
				return Convert.ToByte (Value);
			} catch {
				return defaultValue;
			}
		}
		public ushort ToUShort (ushort defaultValue = default) {
			try {
				return Convert.ToUInt16 (Value);
			} catch {
				return defaultValue;
			}
		}
		public uint ToUInt (uint defaultValue = default) {
			try {
				return Convert.ToUInt32 (Value);
			} catch {
				return defaultValue;
			}
		}
		public ulong ToULong (ulong defaultValue = default) {
			try {
				return Convert.ToUInt64 (Value);
			} catch {
				return defaultValue;
			}
		}
		public sbyte ToSByte (sbyte defaultValue = default) {
			try {
				return Convert.ToSByte (Value);
			} catch {
				return defaultValue;
			}
		}
		public short ToShort (short defaultValue = default) {
			try {
				return Convert.ToInt16 (Value);
			} catch {
				return defaultValue;
			}
		}
		public int ToInt (int defaultValue = default) {
			try {
				return Convert.ToInt32 (Value);
			} catch {
				return defaultValue;
			}
		}
		public long ToLong (long defaultValue = default) {
			try {
				return Convert.ToInt64 (Value);
			} catch {
				return defaultValue;
			}
		}
		public float ToFloat (float defaultValue = default) {
			try {
				return Convert.ToSingle (Value);
			} catch {
				return defaultValue;
			}
		}
		public double ToDouble (double defaultValue = default) {
			try {
				return Convert.ToDouble (Value);
			} catch {
				return defaultValue;
			}
		}
		public decimal ToDecimal (decimal defaultValue = default) {
			try {
				return Convert.ToDecimal (Value);
			} catch {
				return defaultValue;
			}
		}
		public bool ToBool (bool defaultValue = default) {
			try {
				return Convert.ToBoolean (Value);
			} catch {
				return defaultValue;
			}
		}
		public char ToChar (char defaultValue = default) {
			try {
				return Convert.ToChar (Value);
			} catch {
				return defaultValue;
			}
		}
		public string ToString (string defaultValue = default) {
			try {
				return Convert.ToString (Value);
			} catch {
				return defaultValue;
			}
		}
		public DateTime ToDateTime (DateTime defaultValue = default) {
			try {
				return Convert.ToDateTime (Value);
			} catch {
				return defaultValue;
			}
		}

		public static implicit operator byte (LexicalAnalyzerToken<T> token) {
			return token.ToByte ();
		}
		public static implicit operator ushort (LexicalAnalyzerToken<T> token) {
			return token.ToUShort ();
		}
		public static implicit operator uint (LexicalAnalyzerToken<T> token) {
			return token.ToUInt ();
		}
		public static implicit operator ulong (LexicalAnalyzerToken<T> token) {
			return token.ToULong ();
		}
		public static implicit operator sbyte (LexicalAnalyzerToken<T> token) {
			return token.ToSByte ();
		}
		public static implicit operator short (LexicalAnalyzerToken<T> token) {
			return token.ToShort ();
		}
		public static implicit operator int (LexicalAnalyzerToken<T> token) {
			return token.ToInt ();
		}
		public static implicit operator long (LexicalAnalyzerToken<T> token) {
			return token.ToLong ();
		}
		public static implicit operator float (LexicalAnalyzerToken<T> token) {
			return token.ToFloat ();
		}
		public static implicit operator double (LexicalAnalyzerToken<T> token) {
			return token.ToDouble ();
		}
		public static implicit operator decimal (LexicalAnalyzerToken<T> token) {
			return token.ToDecimal ();
		}
		public static implicit operator bool (LexicalAnalyzerToken<T> token) {
			return token.ToBool ();
		}
		public static implicit operator char (LexicalAnalyzerToken<T> token) {
			return token.ToChar ();
		}
		public static implicit operator string (LexicalAnalyzerToken<T> token) {
			return token.ToString ();
		}
		public static implicit operator DateTime (LexicalAnalyzerToken<T> token) {
			return token.ToDateTime ();
		}

		public override string ToString () {
			return $"{{ Type: {Type}, Value: {Value}, StartIndex: {StartIndex}, Length: {Length}, Line: {Line}, LineStartIndex: {LineStartIndex} }}";
		}

	}

}