/**
 * Copyright 2010 mFabrik Research Oy
 * 
 * Licensed under GPL 2.
 */

using System;
using System.IO;

namespace Recorder
{


	public class Record
	{	
		FileInfo fi;

		public Record (FileInfo fi)
		{
			this.fi = fi;
		}
		
		public string GetName()
		{
			return this.fi.Name;
		}
		
		public double GetLength()
		{
			return 0.0;
		}
		
		public long GetSize()
		{
			return this.fi.Length;
		}
		
		public void Delete()
		{
			this.fi.Delete();
		}
	}
}
