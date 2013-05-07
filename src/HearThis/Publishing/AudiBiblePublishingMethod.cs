using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HearThis.Publishing
{
	public class AudiBiblePublishingMethod : BunchOfFilesPublishingMethod
	{
		private string _ethnologueCode;
		public AudiBiblePublishingMethod(IAudioEncoder encoder, string ethnologueCode) : base(encoder)
		{
			_ethnologueCode = ethnologueCode.ToUpperInvariant();
		}

		public override string GetFilePathWithoutExtension(string rootFolderPath, string bookName, int chapterNumber)
		{
			var bookNumber = _statistics.GetBookNumber(bookName);
			string bookIndex = bookNumber.ToString("00");
			var bookAbbr = _statistics.ThreeLetterAbreviations[bookNumber];
			bookAbbr = bookAbbr.Substring(0, 1).ToUpperInvariant() + bookAbbr.Substring(1);
			string chapFormat = "00";
			if (bookName.ToLowerInvariant() == "psalms")
				chapFormat = "000";
			string chapterIndex = chapterNumber.ToString(chapFormat);
			var folderName = string.Format("{0}_{1}_{2}", bookIndex, _ethnologueCode, bookAbbr);
			string folderPath = Path.Combine(rootFolderPath, folderName);
			string fileName = folderName + "_" + chapterIndex;
			EnsureDirectory(folderPath);

			return Path.Combine(folderPath, fileName);

		}
	}
}