
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Recorder
{
	public partial class DetailsViewController : UIViewController
	{
		public Record currentRecord;
		
		public RecordHistory history;
		
		#region Constructors

		// The IntPtr and initWithCoder constructors are required for controllers that need 
		// to be able to be created from a xib rather than from managed code

		public DetailsViewController (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		[Export("initWithCoder:")]
		public DetailsViewController (NSCoder coder) : base(coder)
		{
			Initialize ();
		}

		public DetailsViewController () : base("DetailsViewController", null)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}
		
		/**
		 * Customize looks
		 */
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Title = "---"; // Will be set later	
			this.View.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
			
			this.deleteButton.TouchUpInside += delegate { DoDelete(); };
		}
		
		public void DoDelete()
		{
			UIActionSheet sheet = new UIActionSheet();
			sheet.Title = "Confirm delete";
			sheet.AddButton("Delete");
			sheet.AddButton("Cancel");
			sheet.DestructiveButtonIndex = 0;
			sheet.CancelButtonIndex = 1;
			
			sheet.Clicked += delegate(object s, UIButtonEventArgs e) {
				PerformDelete();
				
				this.NavigationController.PopViewControllerAnimated(true);
			};
			
			sheet.Canceled += delegate(object sender, EventArgs e) {
				Console.WriteLine("Cancelled");
			};
			
			UIViewController parent = this.ParentViewController.ParentViewController;
			UITabBarController tabBarController  = parent as UITabBarController;
			sheet.ShowFromTabBar((UITabBar) tabBarController.View);
			
			// BUGGY http://stackoverflow.com/questions/1197746/uiactionsheet-cancel-button-strange-behaviour
			// sheet.ShowInView(this.View);
			
		}
		
		public void PerformDelete()
		{
			history.Delete(this.currentRecord);
		}
		
		public override void ViewWillAppear(bool animated)
		{
    			base.ViewWillAppear(animated);
    			BindItem();
		}
		
		/**
		 * Populate UI with data from the currently selected item.
		 * 
		 * Item is set externally before opening the view,
		 * by setting  a public member variable.
		 */
		private void BindItem()
		{
			if(currentRecord != null)
			{
				Title = currentRecord.GetName();
				
				double length = currentRecord.GetLength();
				lengthLabel.Text = "Length:" + String.Format("{0}", length);
				
				long size = currentRecord.GetSize();
				
				sizeLabel.Text = "Size:" + String.Format("{0:0.00}", (double) size / 1024.0) + " kB";
			}
		}
		
		#endregion
		
		
		
	}
}
