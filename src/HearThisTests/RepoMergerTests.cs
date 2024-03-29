using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using HearThis;
using HearThis.Communication;
using HearThis.Script;
using NUnit.Framework;
using SIL.Progress;

namespace HearThisTests
{
	[TestFixture]
	public class RepoMergerTests
	{
		[Test]
		public void MergeBlock_TheirsLacksData_KeepsOurs()
		{
			var theirLink = new FakeLink();
			var merger = CreateMerger(new FakeProvider(), new FakeLink(), theirLink);
			var result = merger.MergeBlock(0, 2, 3, "wanted", "ours", "", DateTime.Parse("10/10/14"), DateTime.Parse("11/11/14"), ".mp4");
			Assert.That(result, Is.False, "should have chosen our recording.");
			Assert.That(theirLink.FilesCopied, Has.Count.EqualTo(0), "should not have copied any file");
		}

		RepoMerger CreateMerger(ScriptProviderBase provider, IAndroidLink mine, IAndroidLink theirs)
		{
			return new RepoMerger(new Project(provider), mine, theirs);
		}

		/// <summary>
		/// If we don't have a recording at all, and they do, copy it, even if outdated text and somehow appears older.
		/// </summary>
		[Test]
		public void MergeBlock_OursLacksData_RecordingOutdated_CopiesTheirs()
		{
			var theirLink = new FakeLink();
			var ourLink = new FakeLink();
			var merger = CreateMerger(new FakeProvider(), ourLink, theirLink);
			var result = merger.MergeBlock(0, 2, 3, "wanted", "", "theirs", DateTime.Parse("10/10/14"), DateTime.Parse("11/11/14"), ".mp4");
			Assert.That(result, Is.True, "should have chosen their recording.");
			Assert.That(theirLink.FilesCopied, Has.Count.EqualTo(1), "should have copied their file");
			Assert.That(theirLink.FilesCopied[0].DestPath, Is.EqualTo(Path.Combine(Program.GetApplicationDataFolder("myProj"), "Genesis", "2", "2.mp4")));
			Assert.That(theirLink.FilesCopied[0].AndroidPath, Is.EqualTo("myProj/Genesis/2/2.mp4"));
			Assert.That(ourLink.FilesDeleted, Has.Member(Path.Combine(Program.GetApplicationDataFolder("myProj"), "Genesis", "2", "2.wav")));
		}

		/// <summary>
		/// Even if theirs is newer, keep ours if theirs applies to out-of-date text and ours is for the right text.
		/// </summary>
		[Test]
		public void MergeBlock_TheirsOutdated_KeepsOurs()
		{
			var theirLink = new FakeLink();
			var merger = CreateMerger(new FakeProvider(), new FakeLink(), theirLink);
			var result = merger.MergeBlock(0, 2, 3, "wanted", "wanted", "theirs", DateTime.Parse("10/10/14"), DateTime.Parse("11/11/14"), ".mp4");
			Assert.That(result, Is.False, "should have chosen our recording.");
			Assert.That(theirLink.FilesCopied, Has.Count.EqualTo(0), "should not have copied any file");
		}

		/// <summary>
		/// Even if ours is newer, if (somehow) theirs is for the right source and ours is not, copy.
		/// </summary>
		[Test]
		public void MergeBlock_OursOutdated_CopiesTheirs()
		{
			var theirLink = new FakeLink();
			var merger = CreateMerger(new FakeProvider(), new FakeLink(), theirLink);
			var result = merger.MergeBlock(1, 0, 2, "wanted", "ours", "wanted", DateTime.Parse("10/10/14"), DateTime.Parse("9/9/14"), ".mp4");
			Assert.That(result, Is.True, "should have chosen their recording.");
			Assert.That(theirLink.FilesCopied, Has.Count.EqualTo(1), "should have copied their file");
			Assert.That(theirLink.FilesCopied[0].DestPath, Is.EqualTo(Path.Combine(Program.GetApplicationDataFolder("myProj"), "Matthew", "0", "1.mp4")));
			Assert.That(theirLink.FilesCopied[0].AndroidPath, Is.EqualTo("myProj/Matthew/0/1.mp4"));
		}

		[Test]
		public void MergeBlock_BothCurrent_OursNewer_KeepsOurs()
		{
			var theirLink = new FakeLink();
			var merger = CreateMerger(new FakeProvider(), new FakeLink(), theirLink);
			var result = merger.MergeBlock(0, 2, 3, "wanted", "wanted", "wanted", DateTime.Parse("10/10/14"), DateTime.Parse("9/9/14"), ".mp4");
			Assert.That(result, Is.False, "should have chosen our recording.");
			Assert.That(theirLink.FilesCopied, Has.Count.EqualTo(0), "should not have copied any file");
		}

		[Test]
		public void MergeBlock_BothCurrent_TheirsNewer_CopiesTheirs()
		{
			var theirLink = new FakeLink();
			var merger = CreateMerger(new FakeProvider(), new FakeLink(), theirLink);
			var result = merger.MergeBlock(1, 0, 2, "wanted", "wanted", "wanted", DateTime.Parse("10/10/14"),
				DateTime.Parse("11/11/14"), ".mp4");
			Assert.That(result, Is.True, "should have chosen their recording.");
			Assert.That(theirLink.FilesCopied, Has.Count.EqualTo(1), "should have copied their file");
			Assert.That(theirLink.FilesCopied[0].DestPath,
				Is.EqualTo(Path.Combine(Program.GetApplicationDataFolder("myProj"), "Matthew", "0", "1.mp4")));
			Assert.That(theirLink.FilesCopied[0].AndroidPath, Is.EqualTo("myProj/Matthew/0/1.mp4"));
		}

		[Test]
		public void MergeBlock_NeitherCurrent_TheirsNewer_CopiesTheirs()
		{
			var theirLink = new FakeLink();
			var merger = CreateMerger(new FakeProvider(), new FakeLink(), theirLink);
			var result = merger.MergeBlock(1, 0, 2, "wanted", "ours", "theirs", DateTime.Parse("10/10/14"),
				DateTime.Parse("11/11/14"), ".mp4");
			Assert.That(result, Is.True, "should have chosen their recording.");
			Assert.That(theirLink.FilesCopied, Has.Count.EqualTo(1), "should have copied their file");
			Assert.That(theirLink.FilesCopied[0].DestPath,
				Is.EqualTo(Path.Combine(Program.GetApplicationDataFolder("myProj"), "Matthew", "0", "1.mp4")));
			Assert.That(theirLink.FilesCopied[0].AndroidPath, Is.EqualTo("myProj/Matthew/0/1.mp4"));
		}

		[Test]
		public void MergeBlock_NeitherCurrent_OursNewer_KeepsOurs()
		{
			var theirLink = new FakeLink();
			var merger = CreateMerger(new FakeProvider(), new FakeLink(), theirLink);
			var result = merger.MergeBlock(0, 2, 3, "wanted", "ours", "theirs", DateTime.Parse("10/10/14"), DateTime.Parse("9/9/14"), ".mp4");
			Assert.That(result, Is.False, "should have chosen our recording.");
			Assert.That(theirLink.FilesCopied, Has.Count.EqualTo(0), "should not have copied any file");
		}

		MergerWithBlockTracking CreateMergerWithBlockTracking(ScriptProviderBase provider, IAndroidLink mine, IAndroidLink theirs)
		{
			return new MergerWithBlockTracking(new Project(provider), mine, theirs);
		}

		[Test]
		public void MergeChapter_MineLacksInfo_GeneratesIt()
		{
			var theirLink = new FakeLink();
			var ourLink = new FakeLink();
			var fakeProvider = new FakeProvider();
			fakeProvider.Blocks[Tuple.Create(0, 1)] = new[] {"line0", "line1", "line2"};
			var merger = CreateMergerWithBlockTracking(fakeProvider, ourLink, theirLink);
			ourLink.ListFilesData[GetOurChapterPath("myProj", "Genesis", 1)] =
				"1.wav;2014-12-29 13:23:17;f\n2.mp4;2014-12-29 13:23:25;f";
			merger.MergeChapter(0, 1);
			Assert.That(theirLink.FilesCopied, Has.Count.EqualTo(0), "should not have copied any files from theirs");
			Assert.That(merger.MergeBlockCalls, Has.Count.EqualTo(0), "should not have tried any merges");
			var info = GetInfoElement(ourLink, theirLink, "Genesis", 1);
			VerifySourceLine(info, 0, "line0");
			VerifySourceLine(info, 1, "line1");
			VerifySourceLine(info, 2, "line2");
			VerifyRecordingsLine(info, 0, 1, "---");
			VerifyRecordingsLine(info, 1, 2, "---");
		}

		[Test]
		public void MergeChapter_SendDataFalse_SendsNone()
		{
			var theirLink = new FakeLink();
			var ourLink = new FakeLink();
			var fakeProvider = new FakeProvider();
			fakeProvider.Blocks[Tuple.Create(0, 1)] = new[] { "line0", "line1", "line2" };
			var merger = CreateMergerWithBlockTracking(fakeProvider, ourLink, theirLink);
			merger.SendData = false;
			ourLink.ListFilesData[GetOurChapterPath("myProj", "Genesis", 1)] =
				"1.wav;2014-12-29 13:23:17;f\n2.mp4;2014-12-29 13:23:25;f";
			merger.MergeChapter(0, 1);
			Assert.That(theirLink.FilesPut, Has.Count.EqualTo(0));
		}

		private const string ThreeLineSource = @"<?xml version='1.0' encoding='utf-8'?>
<ChapterInfo Number='1'>
  <Recordings>
	{0}
  </Recordings>
  <Source>
	<ScriptLine>
	  <LineNumber>0</LineNumber>
	  <Text>line0</Text>
	</ScriptLine>
	<ScriptLine>
	  <LineNumber>1</LineNumber>
	  <Text>line1</Text>
	</ScriptLine>
	<ScriptLine>
	  <LineNumber>2</LineNumber>
	  <Text>line2</Text>
	</ScriptLine>
  </Source>
</ChapterInfo>";

		[Test]
		public void MergeChapter_TheirsHasRecordings_DoesMerges()
		{
			var theirLink = new FakeLink();
			var ourLink = new FakeLink();
			var fakeProvider = new FakeProvider();
			fakeProvider.Blocks[Tuple.Create(0, 1)] = new[] { "line0", "line1", "line2" };
			var merger = CreateMergerWithBlockTracking(fakeProvider, ourLink, theirLink);
			theirLink.ListFilesData[GetTheirChapterPath("myProj", "Genesis", 1)] =
				"0.wav;2014-12-29 13:23:17;f\n2.wav;2014-12-29 13:23:25;f";
			// Ours has no recordings
			ourLink.Data[Path.Combine(GetOurChapterPath("myProj", "Genesis", 1), ChapterInfo.kChapterInfoFilename)] = Encoding.UTF8.GetBytes(string.Format(ThreeLineSource, ""));
			// Theirs has two recordings, with different text so we can verify the merger calls better.
			theirLink.Data[GetTheirChapterPath("myProj", "Genesis", 1) + "/" + ChapterInfo.kChapterInfoFilename] = Encoding.UTF8.GetBytes(string.Format(ThreeLineSource, @"<ScriptLine>
	  <LineNumber>1</LineNumber>
	  <Text>line0a</Text>
	</ScriptLine>
	<ScriptLine>
	  <LineNumber>3</LineNumber>
	  <Text>line2a</Text>
	</ScriptLine>"));
			merger.MergeChapter(0, 1);
			var info = GetInfoElement(ourLink, theirLink, "Genesis", 1);
			VerifySourceLine(info, 0, "line0");
			VerifySourceLine(info, 1, "line1");
			VerifySourceLine(info, 2, "line2");
			VerifyRecordingsLine(info, 0, 1, "line0a");
			VerifyRecordingsLine(info, 1, 3, "line2a");
			// string source, string myRecording, string theirRecording, DateTime myModTime, DateTime theirModTime
			VerifyMergeBlockCall(merger, 0, 1, 1, "line0", "", "line0a", DateTime.MinValue, DateTime.Parse("2014-12-29 13:23:17"), ".wav");
			VerifyMergeBlockCall(merger, 0, 1, 3, "line2", "", "line2a", DateTime.MinValue, DateTime.Parse("2014-12-29 13:23:25"), ".wav");
		}

		[Test]
		public void MergeChapter_BothHaveRecordings_DoesMerges()
		{
			var theirLink = new FakeLink();
			var ourLink = new FakeLink();
			var fakeProvider = new FakeProvider();
			fakeProvider.Blocks[Tuple.Create(0, 1)] = new[] { "line0", "line1", "line2" };
			var merger = CreateMergerWithBlockTracking(fakeProvider, ourLink, theirLink);
			theirLink.ListFilesData[GetTheirChapterPath("myProj", "Genesis", 1)] =
				"0.mp4;2014-12-29 13:23:17;f\n2.mp4;2014-12-29 13:23:25;f";
			var ourChapterPath = GetOurChapterPath("myProj", "Genesis", 1);
			ourLink.ListFilesData[ourChapterPath] = "0.mp4;2014-12-29 13:23:18;f";
			// Ours has just one recording, which is newer. Text is different from source so we can verify better.
			ourLink.Data[Path.Combine(ourChapterPath, ChapterInfo.kChapterInfoFilename)] = Encoding.UTF8.GetBytes(string.Format(ThreeLineSource, @"<ScriptLine>
	  <LineNumber>1</LineNumber>
	  <Text>line0b</Text>
	</ScriptLine>"));
			merger.MergeBlockReturnOurs.Add(Tuple.Create(0, 1, 1)); // be consistent, have merger prefer ours.
			// Theirs has two recordings, with different text so we can verify the merger calls better.
			theirLink.Data[GetTheirChapterPath("myProj", "Genesis", 1) + "/" + ChapterInfo.kChapterInfoFilename] = Encoding.UTF8.GetBytes(string.Format(ThreeLineSource, @"<ScriptLine>
	  <LineNumber>1</LineNumber>
	  <Text>line0a</Text>
	</ScriptLine>
	<ScriptLine>
	  <LineNumber>3</LineNumber>
	  <Text>line2a</Text>
	</ScriptLine>"));
			merger.MergeChapter(0, 1);
			var info = GetInfoElement(ourLink, theirLink, "Genesis", 1);
			VerifySourceLine(info, 0, "line0");
			VerifySourceLine(info, 1, "line1");
			VerifySourceLine(info, 2, "line2");
			VerifyRecordingsLine(info, 0, 1, "line0b");
			VerifyRecordingsLine(info, 1, 3, "line2a");
			// string source, string myRecording, string theirRecording, DateTime myModTime, DateTime theirModTime, string ext
			VerifyMergeBlockCall(merger, 0, 1, 1, "line0", "line0b", "line0a", DateTime.Parse("2014-12-29 13:23:18"), DateTime.Parse("2014-12-29 13:23:17"), ".mp4");
			VerifyMergeBlockCall(merger, 0, 1, 3, "line2", "", "line2a", DateTime.MinValue, DateTime.Parse("2014-12-29 13:23:25"), ".mp4");
		}

		[Test]
		public void MergeChapter_TheyHaveWavAndMp4_KeepsTheirMostRecent()
		{
			var theirLink = new FakeLink();
			var ourLink = new FakeLink();
			var fakeProvider = new FakeProvider();
			fakeProvider.Blocks[Tuple.Create(0, 1)] = new[] { "line0", "line1", "line2" };
			var merger = CreateMergerWithBlockTracking(fakeProvider, ourLink, theirLink);
			theirLink.ListFilesData[GetTheirChapterPath("myProj", "Genesis", 1)] =
				"0.mp4;2014-12-29 13:23:17;f\n0.wav;2014-12-29 13:23:18;f\n2.wav;2014-12-29 13:23:25;f\n2.mp4;2014-12-29 13:23:26;f";
			var ourChapterPath = GetOurChapterPath("myProj", "Genesis", 1);
			ourLink.ListFilesData[ourChapterPath] =
	"0.mp4;2014-12-29 13:23:10;f";
			// Ours has just one recording, which is older. Text is different from source so we can verify better.
			ourLink.Data[Path.Combine(ourChapterPath, ChapterInfo.kChapterInfoFilename)] = Encoding.UTF8.GetBytes(string.Format(ThreeLineSource, @"<ScriptLine>
	  <LineNumber>1</LineNumber>
	  <Text>line0b</Text>
	</ScriptLine>"));
			merger.MergeBlockReturnOurs.Add(Tuple.Create(0, 1, 1)); // be consistent, have merger prefer ours.
																	// Theirs has two recordings, with different text so we can verify the merger calls better.
			theirLink.Data[GetTheirChapterPath("myProj", "Genesis", 1) + "/" + ChapterInfo.kChapterInfoFilename] = Encoding.UTF8.GetBytes(string.Format(ThreeLineSource, @"<ScriptLine>
	  <LineNumber>1</LineNumber>
	  <Text>line0a</Text>
	</ScriptLine>
	<ScriptLine>
	  <LineNumber>3</LineNumber>
	  <Text>line2a</Text>
	</ScriptLine>"));
			merger.MergeChapter(0, 1);
			var info = GetInfoElement(ourLink, theirLink, "Genesis", 1);
			VerifySourceLine(info, 0, "line0");
			VerifySourceLine(info, 1, "line1");
			VerifySourceLine(info, 2, "line2");
			VerifyRecordingsLine(info, 0, 1, "line0b");
			VerifyRecordingsLine(info, 1, 3, "line2a");
			// string source, string myRecording, string theirRecording, DateTime myModTime, DateTime theirModTime
			VerifyMergeBlockCall(merger, 0, 1, 1, "line0", "line0b", "line0a", DateTime.Parse("2014-12-29 13:23:10"), DateTime.Parse("2014-12-29 13:23:18"), ".wav");
			VerifyMergeBlockCall(merger, 0, 1, 3, "line2", "", "line2a", DateTime.MinValue, DateTime.Parse("2014-12-29 13:23:26"), ".mp4");
		}

		// Note: although the MergeBlocks routine is coded and tested to correctly handle the case where we have data and they don't,
		// there's actually no reason to call it at all in such a case. It doesn't really matter whether we do or not.
		// So I have not written a test case for where ours has data and theirs has none.

		// pathological case where they have a recording for a line that no longer exists
		[Test]
		public void MergeChapter_TheyHaveRecordingForMissingLine_ItIsIgnored()
		{
			var theirLink = new FakeLink();
			var ourLink = new FakeLink();
			var fakeProvider = new FakeProvider();
			fakeProvider.Blocks[Tuple.Create(0, 1)] = new[] { "line0", "line1", "line2" };
			var merger = CreateMergerWithBlockTracking(fakeProvider, ourLink, theirLink);
			theirLink.ListFilesData[GetTheirChapterPath("myProj", "Genesis", 1)] =
				"0.mp4;2014-12-29 13:23:17;f\n3.mp4;2014-12-29 13:23:25;f";
			var ourChapterPath = GetOurChapterPath("myProj", "Genesis", 1);
			// Ours has just no recordings.
			ourLink.Data[Path.Combine(ourChapterPath, ChapterInfo.kChapterInfoFilename)] = Encoding.UTF8.GetBytes(string.Format(ThreeLineSource, ""));
			// Theirs has two recordings, with different text so we can verify the merger calls better.
			theirLink.Data[GetTheirChapterPath("myProj", "Genesis", 1) + "/" + ChapterInfo.kChapterInfoFilename] = Encoding.UTF8.GetBytes(string.Format(ThreeLineSource, @"<ScriptLine>
	  <LineNumber>1</LineNumber>
	  <Text>line0a</Text>
	</ScriptLine>
	<ScriptLine>
	  <LineNumber>4</LineNumber>
	  <Text>line3a</Text>
	</ScriptLine>"));
			merger.MergeChapter(0, 1);
			var info = GetInfoElement(ourLink, theirLink, "Genesis", 1);
			VerifySourceLine(info, 0, "line0");
			VerifySourceLine(info, 1, "line1");
			VerifySourceLine(info, 2, "line2");
			VerifyRecordingsLine(info, 0, 1, "line0a");
			// string source, string myRecording, string theirRecording, DateTime myModTime, DateTime theirModTime
			VerifyMergeBlockCall(merger, 0, 1, 1, "line0", "", "line0a", DateTime.MinValue, DateTime.Parse("2014-12-29 13:23:17"), ".mp4");
			Assert.That(merger.MergeBlockCalls, Has.Count.EqualTo(1), "should not have tried to merge extra recording");
		}

		private void VerifyMergeBlockCall(MergerWithBlockTracking merger, int ibook, int ichap1based, int iblock, params object[] data)
		{
			object[] callData;
			Assert.That(merger.MergeBlockCalls.TryGetValue(Tuple.Create(ibook, ichap1based, iblock), out callData), Is.True);
			Assert.That(data.Length, Is.EqualTo(callData.Length));
			for (int i = 0; i < data.Length; i++)
			{
				Assert.That(callData[i], Is.EqualTo(data[i]));
			}
		}

		void VerifySourceLine(XElement info, int index, string text)
		{
			VerifyLine("Source", info, index, index+1, text);
		}

		void VerifyRecordingsLine(XElement info, int index, int lineNo, string text)
		{
			VerifyLine("Recordings", info, index, lineNo, text);
		}

		private static void VerifyLine(string region, XElement info, int index, int lineNum, string text)
		{
			var line = info.XPathSelectElement("/" + region + "/ScriptLine[" + (index + 1) + "]");
			Assert.That(line, Is.Not.Null);
			var lineNo = line.Element("LineNumber");
			Assert.That(lineNo.Value, Is.EqualTo(lineNum.ToString()));
			var lineText = line.Element("Text");
			Assert.That(lineText.Value, Is.EqualTo(text));
		}

		XElement GetInfoElement(FakeLink ourLink, FakeLink theirLink, string bookName, int chapter)
		{
			Assert.That(ourLink.FilesPut, Has.Count.EqualTo(1), "should have put info.xml to mine");
			Assert.That(theirLink.FilesPut, Has.Count.EqualTo(1), "should have put info.xml to theirs");
			Assert.That(ourLink.FilesPut[0].Path,
				Is.EqualTo(Path.Combine(GetOurChapterPath("myProj", bookName, chapter), "info.xml")));
			Assert.That(theirLink.FilesPut[0].Path, Is.EqualTo("myProj/" + bookName + "/" + chapter.ToString() + "/info.xml"));
			var ourData = Encoding.UTF8.GetString(ourLink.FilesPut[0].Data);
			var theirData = Encoding.UTF8.GetString(theirLink.FilesPut[0].Data);
			Assert.That(ourData, Is.EqualTo(theirData), "should have written same info.xml to both repos");
			return XElement.Parse(ourData);
		}

		string GetOurChapterPath(string projName, string bookName, int chapter)
		{
			return Path.Combine(Program.GetApplicationDataFolder(projName), bookName, chapter.ToString());
		}

		string GetTheirChapterPath(string projName, string bookName, int chapter)
		{
			return projName + "/" + bookName + "/" + chapter;
		}

		[Test]
		public void MergeAll_MergesEachChapter()
		{
			var theirLink = new FakeLink();
			var ourLink = new FakeLink();
			var fakeProvider = new FakeProvider();
			var merger = new MergerWithChapterTracking(new Project(fakeProvider), ourLink, theirLink);
			// Our FakeVersificationScheme declares that there are two books, Genesis and Matthew, having 2 and 3 chapters.
			// Chapter 0 is considered the introduction and should be merged also.
			// However, we don't merge empty chapters (so as not to create a multitude of useless info.xml files).
			fakeProvider.Blocks[Tuple.Create(0, 0)] = new [] {"block00"};
			fakeProvider.Blocks[Tuple.Create(0, 1)] = new[] { "line0", "line1", "line2" };
			fakeProvider.Blocks[Tuple.Create(0, 2)] = new[] { "block02" };
			fakeProvider.Blocks[Tuple.Create(1,1)] = new[] { "block11" };
			merger.Merge(new string[0], new NullProgress());
			Assert.That(merger.ChaptersMerged, Has.Member(new BookChap(0, 0)));
			Assert.That(merger.ChaptersMerged, Has.Member(new BookChap(0, 1)));
			Assert.That(merger.ChaptersMerged, Has.Member(new BookChap(0, 2)));
			Assert.That(merger.ChaptersMerged, Has.Member(new BookChap(1, 1)));
			Assert.That(merger.ChaptersMerged, Has.Count.EqualTo(4), "should not have merged empty chapters");
		}

		[Test]
		public void Merge_MergesSkippedLines()
		{
			var theirData = @"<ScriptLineIdentifier>
      <BookNumber>61</BookNumber>
      <ChapterNumber>1</ChapterNumber>
      <LineNumber>1</LineNumber>
      <Text>Chapter 1</Text>
      <Verse>0</Verse>
    </ScriptLineIdentifier>";
			var ourData = @"<ScriptLineIdentifier>
      <BookNumber>60</BookNumber>
      <ChapterNumber>2</ChapterNumber>
      <LineNumber>3</LineNumber>
      <Text>Some unrecordable text</Text>
      <Verse>2</Verse>
    </ScriptLineIdentifier>";
			var newSkipInfoBlock = RunMergeForSkippedLines(theirData, ourData);
			Assert.That(newSkipInfoBlock, Is.Not.Null);
			var newScriptLines = SkippedScriptLines.Create(newSkipInfoBlock.Data, new string[0]);
			Assert.That(newScriptLines.SkippedLinesList.Count, Is.EqualTo(2));
		}

		[Test]
		public void Merge_WithDuplicates_KeepsTheirs()
		{
			var theirData = @"<ScriptLineIdentifier>
      <BookNumber>61</BookNumber>
      <ChapterNumber>2</ChapterNumber>
      <LineNumber>3</LineNumber>
      <Text>Their61.2.3</Text>
      <Verse>0</Verse>
    </ScriptLineIdentifier>
	<ScriptLineIdentifier>
      <BookNumber>60</BookNumber>
      <ChapterNumber>2</ChapterNumber>
      <LineNumber>3</LineNumber>
      <Text>Their unrecordable text</Text>
      <Verse>3</Verse>
    </ScriptLineIdentifier>";
			var ourData = @"<ScriptLineIdentifier>
      <BookNumber>60</BookNumber>
      <ChapterNumber>2</ChapterNumber>
      <LineNumber>3</LineNumber>
      <Text>Some unrecordable text</Text>
      <Verse>2</Verse>
    </ScriptLineIdentifier>
	<ScriptLineIdentifier>
      <BookNumber>60</BookNumber>
      <ChapterNumber>2</ChapterNumber>
      <LineNumber>4</LineNumber>
      <Text>Our60.2.4</Text>
      <Verse>2</Verse>
    </ScriptLineIdentifier>";
			var newSkipInfoBlock = RunMergeForSkippedLines(theirData, ourData);
			Assert.That(newSkipInfoBlock, Is.Not.Null);
			var newScriptLines = SkippedScriptLines.Create(newSkipInfoBlock.Data, new string[0]);
			Assert.That(newScriptLines.SkippedLinesList.Count, Is.EqualTo(3));
			var dupLine = newScriptLines.GetLine(60, 2, 3);
			Assert.That(dupLine.Text, Is.EqualTo("Their unrecordable text"));
			Assert.That(dupLine.Verse, Is.EqualTo("3"));
			Assert.That(newScriptLines.GetLine(60, 2, 4).Text, Is.EqualTo("Our60.2.4"));
			Assert.That(newScriptLines.GetLine(61, 2, 3).Text, Is.EqualTo("Their61.2.3"));
		}

		private static PathData RunMergeForSkippedLines(string theirData, string ourData)
		{
			var theirLink = new FakeLink();
			var ourLink = new FakeLink();
			var fakeProvider = new FakeProvider();
			var project = new Project(fakeProvider);
			var skippedFilePath = Path.Combine(project.Name, ScriptProviderBase.kSkippedLineInfoFilename);
			theirLink.Data[skippedFilePath] = Encoding.UTF8.GetBytes(
				@"<?xml version=""1.0"" encoding=""utf-8""?>
<SkippedScriptLines>
  <SkippedParagraphStyles />
   <SkippedLinesList>"
				+ theirData +
				@"</SkippedLinesList>
</SkippedScriptLines>");
			ourLink.Data[skippedFilePath] = Encoding.UTF8.GetBytes(
				@"<?xml version=""1.0"" encoding=""utf-8""?>
<SkippedScriptLines>
  <SkippedParagraphStyles />
  <SkippedLinesList>"
				+ ourData +
				@"</SkippedLinesList>
</SkippedScriptLines>");

			var merger = new RepoMerger(project, ourLink, theirLink);
			merger.Merge(new string[0], new NullProgress());
			var newSkipInfoBlock = ourLink.FilesPut.FirstOrDefault(pd => pd.Path == skippedFilePath);
			return newSkipInfoBlock;
		}


		// Arguably, we could have a test where BOTH are missing, but it's not a different code path from where just theirs is missing.
		[Test]
		public void Merge_NoSkippedLinesInTheirs_DoesNotChangeOurSkippedLines()
		{
			var theirLink = new FakeLink();
			var ourLink = new FakeLink();
			var fakeProvider = new FakeProvider();
			var project = new Project(fakeProvider);
			var skippedFilePath = Path.Combine(project.Name, ScriptProviderBase.kSkippedLineInfoFilename);
			ourLink.Data[skippedFilePath] = Encoding.UTF8.GetBytes(@"<?xml version=""1.0"" encoding=""utf-8""?>
<SkippedScriptLines>
  <SkippedParagraphStyles />
  <SkippedLinesList>
    <ScriptLineIdentifier>
      <BookNumber>60</BookNumber>
      <ChapterNumber>2</ChapterNumber>
      <LineNumber>3</LineNumber>
      <Text>Some unrecordable text</Text>
      <Verse>2</Verse>
    </ScriptLineIdentifier>
  </SkippedLinesList>
</SkippedScriptLines>");

			var merger = new RepoMerger(project, ourLink, theirLink);
			merger.Merge(new string[0], new NullProgress());
			var newSkipInfoBlock = ourLink.FilesPut.FirstOrDefault(pd => pd.Path == skippedFilePath);
			Assert.That(newSkipInfoBlock, Is.Null);
		}

		[Test]
		public void Merge_NoSkippedLinesInOurs_CopiesTheirs()
		{
			var theirLink = new FakeLink();
			var ourLink = new FakeLink();
			var fakeProvider = new FakeProvider();
			var project = new Project(fakeProvider);
			var skippedFilePath = Path.Combine(project.Name, ScriptProviderBase.kSkippedLineInfoFilename);
			theirLink.Data[skippedFilePath] = Encoding.UTF8.GetBytes(@"<?xml version=""1.0"" encoding=""utf-8""?>
<SkippedScriptLines>
  <SkippedParagraphStyles />
  <SkippedLinesList>
    <ScriptLineIdentifier>
      <BookNumber>61</BookNumber>
      <ChapterNumber>1</ChapterNumber>
      <LineNumber>1</LineNumber>
      <Text>Chapter 1</Text>
      <Verse>0</Verse>
    </ScriptLineIdentifier>
  </SkippedLinesList>
</SkippedScriptLines>");

			var merger = new RepoMerger(project, ourLink, theirLink);
			merger.Merge(new string[0], new NullProgress());
			var newSkipInfoBlock = ourLink.FilesPut.FirstOrDefault(pd => pd.Path == skippedFilePath);
			Assert.That(newSkipInfoBlock, Is.Not.Null);
			var newScriptLines = SkippedScriptLines.Create(newSkipInfoBlock.Data, new string[0]);
			Assert.That(newScriptLines.SkippedLinesList.Count, Is.EqualTo(1));
			Assert.That(newSkipInfoBlock.Data, Is.EqualTo(theirLink.Data[skippedFilePath]));
		}
	}

	class BookChap : Tuple<int, int>
	{
		public BookChap(int book, int chapter) : base(book, chapter)
		{
		}

		public int Book => Item1;
		public int Chapter => Item2;
	}

	class MergerWithChapterTracking : RepoMerger
	{
		public MergerWithChapterTracking(Project project, IAndroidLink mine, IAndroidLink theirs) : base(project, mine, theirs)
		{
		}

		public readonly HashSet<BookChap> ChaptersMerged = new HashSet<BookChap>();

		public override void MergeChapter(int iBook, int iChap1Based)
		{
			var key = new BookChap(iBook, iChap1Based);
			Assert.That(ChaptersMerged.Contains(key), Is.False, "Should not merge same chapter twice");
			ChaptersMerged.Add(key);
		}
	}

	class MergerWithBlockTracking : RepoMerger
	{
		public MergerWithBlockTracking(Project project, IAndroidLink mine, IAndroidLink theirs) : base(project, mine, theirs)
		{
		}

		public readonly Dictionary<Tuple<int, int, int>, object[]> MergeBlockCalls = new Dictionary<Tuple<int, int, int>, object[]>();
		public readonly HashSet<Tuple<int,int, int>> MergeBlockReturnOurs = new HashSet<Tuple<int, int, int>>();

		public override bool MergeBlock(int iBook, int iChap1Based, int iBlock, string source, string myRecording, string theirRecording,
			DateTime myModTime, DateTime theirModTime, string ext)
		{
			var key = Tuple.Create(iBook, iChap1Based, iBlock);
			MergeBlockCalls.Add(key, new object[]{source, myRecording, theirRecording, myModTime, theirModTime, ext});
			return !MergeBlockReturnOurs.Contains(key);
		}
	}

	class FakeProvider : ScriptProviderBase
	{
		public readonly Dictionary<Tuple<int, int>,string[]> Blocks = new Dictionary<Tuple<int, int>, string[]>();
		public override ScriptLine GetBlock(int bookNumber, int chapterNumber, int lineNumber0Based)
		{
			var text = Blocks[new Tuple<int, int>(bookNumber, chapterNumber)][lineNumber0Based];
			var result = new ScriptLine(text);
			result.Number = lineNumber0Based + 1;
			return result;
		}

		protected override ChapterRecordingInfoBase GetChapterInfo(int book, int chapter)
		{
			throw new NotImplementedException();
		}

		public override void UpdateSkipInfo()
		{
			throw new NotImplementedException();
		}

		public override int GetScriptBlockCount(int bookNumber, int chapter1Based)
		{
			if (!Blocks.TryGetValue(Tuple.Create(bookNumber, chapter1Based), out var blocks))
				return 0;
			return blocks.Length;
		}

		public override int GetSkippedScriptBlockCount(int bookNumber, int chapter1Based)
		{
			throw new NotImplementedException();
		}

		public override int GetUnskippedScriptBlockCount(int bookNumber, int chapter1Based)
		{
			throw new NotImplementedException();
		}

		public override int GetTranslatedVerseCount(int bookNumber0Based, int chapterNumber1Based)
		{
			throw new NotImplementedException();
		}

		public override int GetScriptBlockCount(int bookNumber)
		{
			throw new NotImplementedException();
		}

		public override void LoadBook(int bookNumber0Based)
		{
		}

		public override string EthnologueCode => throw new NotImplementedException();

		public override bool RightToLeft => throw new NotImplementedException();

		public override string FontName => throw new NotImplementedException();

		private string FolderName = "myProj";
		public override string ProjectFolderName => FolderName;

		public override IEnumerable<string> AllEncounteredParagraphStyleNames =>
			throw new NotImplementedException();

		private readonly FakeVerseInfo2 FakeVerseInfo = new FakeVerseInfo2();

		public override IBibleStats VersificationInfo => FakeVerseInfo;
	}

	class FakeVerseInfo2 : IBibleStats
	{
		public int BookCount => 2;

		private readonly string[] Books = { "Genesis", "Matthew" };
		private readonly int[] chapCounts = { 2, 3 };

		public int GetBookNumber(string bookName)
		{
			throw new NotImplementedException();
		}

		private readonly string[] BookCodes = {"Gen", "Mat"};
		public string GetBookCode(int bookNumber0Based)
		{
			return BookCodes[bookNumber0Based];
		}

		public string GetBookName(int bookNumber0Based)
		{
			return Books[bookNumber0Based];
		}

		public int GetChaptersInBook(int bookNumber0Based)
		{
			return chapCounts[bookNumber0Based];
		}
	}

	class PathPair
	{
		public string AndroidPath;
		public string DestPath;
	}

	class PathData
	{
		public string Path;
		public byte[] Data;
	}

	class FakeLink : IAndroidLink
	{
		public string GetDeviceName()
		{
			throw new NotImplementedException();
		}

		public readonly List<PathPair> FilesCopied = new List<PathPair>();

		public bool GetFile(string androidPath, string destPath)
		{
			FilesCopied.Add(new PathPair() {AndroidPath = androidPath, DestPath = destPath});
			return true;
		}

		public readonly Dictionary<string, byte[]> Data = new Dictionary<string, byte[]>();

		public bool TryGetData(string androidPath, out byte[] data)
		{
			return Data.TryGetValue(androidPath, out data);
		}

		public readonly List<PathData> FilesPut = new List<PathData>();

		public bool PutFile(string androidPath, byte[] data)
		{
			FilesPut.Add(new PathData() {Path = androidPath, Data = data});
			return true;
		}

		public readonly Dictionary<string, string> ListFilesData = new Dictionary<string, string>();

		public bool TryListFiles(string androidPath, out string list)
		{
			if (!ListFilesData.TryGetValue(androidPath, out list))
				list = "";
			return true;
		}

		public readonly HashSet<string> FilesDeleted = new HashSet<string>();
		public void DeleteFile(string androidPath)
		{
			FilesDeleted.Add(androidPath);
		}
	}
}
