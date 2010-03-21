/**
 * Copyright 2010 mFabrik Research Oy
 * 
 * Licensed under GPL 2.
 */

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
		
		/***
		 * @return Path where all recorded files are
		 */
		public string GetPath()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		}
		
		
		/**
		 * 
		 * 
		 */
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
		 * http://www.csharpfriends.com/Articles/getArticle.aspx?articleID=356
		 */
		public void Scan() 
		{
			Console.WriteLine("Scanning existing records");
			
			records = new List<Record>();
			
			DirectoryInfo di = new DirectoryInfo(GetPath());
 			FileInfo[] rgFiles = di.GetFiles("*.wav");
 			foreach(FileInfo fi in rgFiles)
 			{
				Console.WriteLine(fi.Name);
				Record r = new Record(fi);
				records.Add(r);
 			}
		}
		
		public string GetNextFilename() 
		{
			
            //Declare string for application temp path and tack on the file extension
           
			string audioFilePath;
			
			string date = System.DateTime.Now.ToString("yyyy-MM-dd");
			
			// Iterate until we find a free filename slot
			int i = 0;
			do {
				
				i++;
				
				
				string fileName = date + " " + i + ".wav";
				
				
				string basedir = GetPath();
            		//string tmpdir = Path.Combine(basedir, "tmp");
            
				audioFilePath = Path.Combine(basedir, fileName);
            
				Console.WriteLine("Testing file:" + audioFilePath);
				
			} while(File.Exists(audioFilePath));
			
			Console.WriteLine("Picked file:" + audioFilePath);
			
			return audioFilePath;
			
		}
		
		
		
	}
	
	
}
