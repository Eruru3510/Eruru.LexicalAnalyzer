using System;
using System.IO;
using Eruru.LexicalAnalyzer;

namespace ConsoleApp1 {

	class Program {

		enum TokenType {

			End,
			Unknown,
			Integer,
			Decimal,
			String,
			NumberSign,
			Include,
			GreaterThan,
			LessThan,
			Int,
			LeftParenthesis,
			RightParenthesis,
			Comma,
			Char,
			LeftBracket,
			RightBracket,
			LeftBrace,
			RightBrace,
			Semicolon,
			Return,
			LineComment,
			BlockComment,
			Assign

		}

		static void Main (string[] args) {
			Console.Title = string.Empty;
			LexicalAnalyzer<TokenType> lexicalAnalyzer = new LexicalAnalyzer<TokenType> (
				new StringReader (@"
					#include<stdio.h>
					
					int main (int argc, char argv[]) {
						printf ('Hello, World!');//打印Hello, World!
						return 0;
					}

					/*
						int Sum (int a, int b) {
							return a + b;
						}
					*/
				"),
				TokenType.End,
				TokenType.Unknown,
				TokenType.Integer,
				TokenType.Decimal,
				TokenType.String
			);
			lexicalAnalyzer.Characters.Add ('#', TokenType.NumberSign);
			lexicalAnalyzer.AddKeyword ("include", TokenType.Include);
			lexicalAnalyzer.Characters.Add ('<', TokenType.LessThan);
			lexicalAnalyzer.Characters.Add ('>', TokenType.GreaterThan);
			lexicalAnalyzer.AddKeyword ("int", TokenType.Int);
			lexicalAnalyzer.Characters.Add ('(', TokenType.LeftParenthesis);
			lexicalAnalyzer.Characters.Add (')', TokenType.RightParenthesis);
			lexicalAnalyzer.Characters.Add (',', TokenType.Comma);
			lexicalAnalyzer.AddKeyword ("char", TokenType.Char);
			lexicalAnalyzer.Characters.Add ('[', TokenType.LeftBracket);
			lexicalAnalyzer.Characters.Add (']', TokenType.RightBracket);
			lexicalAnalyzer.Characters.Add ('{', TokenType.LeftBrace);
			lexicalAnalyzer.Characters.Add ('}', TokenType.RightBrace);
			lexicalAnalyzer.Characters.Add (';', TokenType.Semicolon);
			lexicalAnalyzer.AddKeyword ("return", TokenType.Return);
			lexicalAnalyzer.AddBlock (TokenType.LineComment, "//", "\r\n");
			lexicalAnalyzer.AddBlock (TokenType.BlockComment, "/*", "*/");
			lexicalAnalyzer.TailCharacters.Add ('+', TokenType.Assign);
			while (true) {
				bool hasNext = lexicalAnalyzer.MoveNext ();
				Console.WriteLine ($"{lexicalAnalyzer.Current}");
				if (!hasNext) {
					break;
				}
			}
			Console.ReadLine ();
		}

	}

}