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
		
			/* Set by Main when loaded */
			public RecordHistory history;
		
			static NSString kCellIdentifier = new NSString ("MyIdentifier");
			
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
					return tvc.history.GetEntries().Count;
				}
		
				public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
				{
					var cell = tableView.DequeueReusableCell (kCellIdentifier);
					if (cell == null){
						cell = new UITableViewCell (UITableViewCellStyle.Default, kCellIdentifier);
					}
				
					// Customize the cell here...
					cell.TextLabel.Text = tvc.history.GetEntries()[indexPath.Row].GetName();
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
					return cell;
				}
			}
		
			//
			// This class receives notifications that happen on the UITableView
			//
			class TableDelegate : UITableViewDelegate {
				
				RecordTableViewController tvc;
				DetailsViewController detailsViewController;
	
				public TableDelegate (RecordTableViewController tvc)
				{
					this.tvc = tvc;
				}
				
				/**
				 * User has pressed a row
				 * 
				 * Initialize details view based on which row was pressed 
				 * and populate it with the data from the corresponding history entry.
				 * 
				 * Then use navigation controller to push a new view in the stack.
				 * 
				 */
				public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
				{
					Console.WriteLine ("Recorder: Row selected {0}", indexPath.Row);
				
					 if (detailsViewController == null) {
        						detailsViewController = new DetailsViewController();
					}
    			
					detailsViewController.currentRecord = tvc.history.GetEntries()[indexPath.Row];
					detailsViewController.history = tvc.history;
				
    					tvc.	NavigationController.PushViewController(detailsViewController, true);
				}
			}
			
			public override void ViewDidLoad ()
			{
				base.ViewDidLoad ();
				Title = "History";
				
				TableView.Delegate = new TableDelegate (this);
				TableView.DataSource = new DataSource (this);
			
			}
		 
			
	}
}

	 