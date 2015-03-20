using System;
namespace AssemblyCSharp
{
	public class CsvWriter
	{

		private string direction = "TestResults/";
		System.IO.StreamWriter file;
		public CsvWriter (string filename, string header)
		{
			file = new System.IO.StreamWriter (direction + filename);
			file.WriteLine (header);
		}

		~CsvWriter()
		{
			file.Close ();
		}

		public void writeLineToFile(string line)
		{
			file.WriteLine (line);
		}
	}
}

