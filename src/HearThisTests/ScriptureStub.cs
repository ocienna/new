using System;
using System.Collections.Generic;
using System.IO;
using HearThis;
using HearThis.Script;
using Paratext.Data;
using SIL.Scripture;

namespace HearThisTests
{
	internal class ScriptureStub : IScripture, IDisposable
	{
		private ScrStylesheet _stylesheet;
		public List<UsfmToken> UsfmTokens;
		private StyleLookup _stylesheetWrapper;

		public void SetDefaultFont(string fontName)
		{
			DefaultFont = fontName;
		}

		public ScriptureStub()
		{
			DeleteSkippedLineInfoFile();
		}

		public void Dispose()
		{
			DeleteSkippedLineInfoFile();
		}

		private void  DeleteSkippedLineInfoFile()
		{
			var filename = Path.Combine(Program.GetApplicationDataFolder(Name), ScriptProviderBase.kSkippedLineInfoFilename);
			if (File.Exists(filename))
				File.Delete(filename);
		}

		#region IScripture Members

		public ScrVers Versification => ScrVers.English;

		private ScrStylesheet Stylesheet => _stylesheet ??
			(_stylesheet = new ScrStylesheet(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "usfm.sty")));

		public IStyleInfoProvider StyleInfo =>
			_stylesheetWrapper ?? (_stylesheetWrapper = new StyleLookup(Stylesheet));

		public List<UsfmToken> GetUsfmTokens(VerseRef verseRef)
		{
			if (UsfmTokens != null && UsfmTokens.Count > 0 && UsfmTokens[0].HasData && UsfmTokens[0].Data[0] == verseRef.Book)
				return UsfmTokens;
			return new List<UsfmToken>();
		}

		public IScrParserState CreateScrParserState(VerseRef verseRef) =>
			new ParserState(new ScrParserState(Stylesheet, verseRef));

		public bool RightToLeft => false;
		public string DefaultFont { get; private set; }
		public string EthnologueCode => "KAL";
		public string Name => "Stub";

		public string FirstLevelStartQuotationMark { get { return "“"; } }
		public string FirstLevelEndQuotationMark { get { return "”"; } }
		public string SecondLevelStartQuotationMark { get { return "‘"; } }
		public string SecondLevelEndQuotationMark { get { return "’"; } }
		public string ThirdLevelStartQuotationMark { get { return "“"; } }
		public string ThirdLevelEndQuotationMark { get { return "”"; } }
		public bool FirstLevelQuotesAreUnique { get { return false; } }
		#endregion
	}
}
