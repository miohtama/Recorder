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

		public delegate void ContentChanged(RecordHistory history);
		
		// Event handler when history has been edited
		public ContentChanged contentChanged;
		
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
		public List<Record> GetEntries() 
		{
				
			return records;
		
		}
		
		public void AddEntry(Record r)
		{
			
		}
		
		private void FireContentChanged()
		{
			if(contentChanged != null)
			{
				Console.WriteLine("Notifying history changed");
				contentChanged(this);
			}
				
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
		
		public void ScanAndRefresh()
		{
			Scan();
			FireContentChanged();
		}
		
		public void Delete(Record r) 
		{
			r.Delete(); // Remove file
			
			Scan(); // Rebuild list
			
			this.FireContentChanged();
		}
		
		public string GetNextFilename() 
		{
			
            //Declare string for application temp path and tack on the file extension
           
			string audioFilePath;
			
			string date = System.DateTime.Now.ToString("yyyy-MM-dd");
			
			// Iterate until we find a free filename slot
			int i = 0;
			do {
			
				string fillerChar;
				
				if(i < 25)
				{
					char c = (char) ('a' + i);
					fillerChar = c.ToString();
				} else {
					// run out of alphabets
					fillerChar = "" + i;
				}
				
				string fileName = date + " " + fillerChar + ".wav";
				
				string basedir = GetPath();
            		//string tmpdir = Path.Combine(basedir, "tmp");
            
				audioFilePath = Path.Combine(basedir, fileName);
            
				Console.WriteLine("Testing file:" + audioFilePath);
				
				i++;
				
			} while(File.Exists(audioFilePath));
			
			Console.WriteLine("Picked file:" + audioFilePath);
			
			return audioFilePath;
			
		}
		
		
		
	}
	
	
}
