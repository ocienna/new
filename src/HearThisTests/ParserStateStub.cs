using System.Collections.Generic;
using HearThis.Script;
using Paratext;

namespace HearThisTests
{
	internal class ParserStateStub : IScrParserState
	{
		public HashSet<string> NoteMarkers = new HashSet<string>(new [] {"nt", "nt1"});
		public HashSet<string> ParaMarkers = new HashSet<string>(new[] { "mt", "mt1", "mt2", "ip", "im", "ms", "imt", "s", "s1", "c", "p" });

		public void UpdateState(List<UsfmToken> tokenList, int tokenIndex)
		{
			NoteTag = null;
			CharTag = null;
			ParaTag = null;
			ParaStart = false;

			if (NoteMarkers.Contains(tokenList[tokenIndex].Marker))
			{
				NoteTag = new ScrTag();
			}

			if (ParaMarkers.Contains(tokenList[tokenIndex].Marker))
			{
				ParaTag = new ScrTag { Marker = tokenList[tokenIndex].Marker };
				ParaStart = true;
			}

		}

		public ScrTag NoteTag { get; private set; }
		public ScrTag CharTag { get; private set; }
		public ScrTag ParaTag { get; private set; }
		public bool ParaStart { get; private set; }
	}
}
