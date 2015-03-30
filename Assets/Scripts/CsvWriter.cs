using System;
using System.IO;
namespace AssemblyCSharp
{
	public class CsvWriter
	{

		private string directory = "TestResults/";
		System.IO.StreamWriter file;
		public CsvWriter (string filename, string header)
		{
			// Find filename in directory and add number to distinguish between already existing files.
			DirectoryInfo logDirectory = new DirectoryInfo(directory);
			int i = 1;
			while (true) {
				FileInfo[] taskFiles = logDirectory.GetFiles (filename + i.ToString() + ".csv");
				if(taskFiles.Length == 0)
				{
					break;
				}
				i++;	
			}
			file = new System.IO.StreamWriter (directory + filename + i.ToString() + ".csv");
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

		public void Close()
		{
			file.Close();
		}
	
	}
}

