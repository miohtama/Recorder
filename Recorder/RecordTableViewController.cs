/**
 * Copyright 2010 mFabrik Research Oy
 * 
 * Licensed under GPL 2.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Recorder
{
		[MonoTouch.Foundation.Register("RecordTableViewController")]
		public partial class RecordTableViewController : UITableViewController {
		
			static NSString kCellIdentifier = new NSString ("MyIdentifier");
			string[] content;
			//
			// Constructor invoked from the NIB loader
			//
			public RecordTableViewController (IntPtr p) : base (p) {}
		
			
			//
			// The data source for our TableView
			//
			class DataSource : UITableViewDataSource {
				RecordTableViewController tvc;
				
				public DataSource (RecordTableViewController tvc)
				{
					this.tvc = tvc;
				}
				
				public override int RowsInSection (UITableView tableView, int section)
				{
					return tvc.content.Length;
				}
		
				public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
				{
					var cell = tableView.DequeueReusableCell (kCellIdentifier);
					if (cell == null){
						cell = new UITableViewCell (UITableViewCellStyle.Default, kCellIdentifier);
					}
				
					// Customize the cell here...
					cell.TextLabel.Text = tvc.content[indexPath.Row];
					return cell;
				}
			}
		
			//
			// This class receives notifications that happen on the UITableView
			//
			class TableDelegate : UITableViewDelegate {
				RecordTableViewController tvc;
	
				public TableDelegate (RecordTableViewController tvc)
				{
					this.tvc = tvc;
				}
				
				public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
				{
					Console.WriteLine ("Recorder: Row selected {0}", indexPath.Row);
					
				}
			}
			
			public override void ViewDidLoad ()
			{
				base.ViewDidLoad ();
				Title = "History";
			
				content = new string[] { "Foo", "Bar" };
				
				TableView.Delegate = new TableDelegate (this);
				TableView.DataSource = new DataSource (this);
			}
			
	}
}

	