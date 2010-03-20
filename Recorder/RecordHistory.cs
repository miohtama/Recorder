
using System;

using System.Collections.Generic;

namespace Recorder
{


	public class RecordHistory
	{
		List<Record> records;
		

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
		
		
	}
	
	
}
