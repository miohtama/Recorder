
using System;
using System.IO;

using System.Collections.Generic;

namespace Recorder
{


	public class RecordHistory
	{
		List<Record> records;
		
		static string FILENAME_BASE = "record";

		public RecordHistory ()
		{
			records = new List<Record>();
		}
		
		
		public Record[] GetEntries() 
		{
			return records.ToArray();
		
		}
		
		public void AddEntry(Record r)
		{
		}
		
		
		/**
		 * Scan the user folder for existingn files
		 * 
		 */
		public void Scan() 
		{
			
		}
		
		public string GetNextFilename() 
		{
			
            //Declare string for application temp path and tack on the file extension
           
			string audioFilePath;
			
			string date = System.DateTime.Now.ToShortDateString();
			
			// Iterate until we find a free filename slot
			int i = 0;
			do {
				
				i++;
				
				string fileName = date + " " + i + ".wav";
				string basedir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            		//string tmpdir = Path.Combine(basedir, "tmp");
            
				audioFilePath = Path.Combine(basedir, fileName);
            
				Console.WriteLine("Testing file:" + audioFilePath);
				
			} while(File.Exists(audioFilePath));
			
			return audioFilePath;
			
		}
		
		
		
	}
	
	
}
