
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.AVFoundation;

namespace Recorder
{
	public partial class DetailsViewController : UIViewController
	{
		public Record currentRecord;
		
		public RecordHistory history;
		
		AVAudioPlayer player;
		
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
			
			this.playButton.TouchUpInside += delegate { DoPlay(); };
			this.deleteButton.TouchUpInside += delegate { DoDelete(); };
			
			ButtonHelper.MakeGradientButton(this.playButton);
			ButtonHelper.MakeGradientButton(this.sendButton);
			ButtonHelper.MakeGradientButton(this.deleteButton);
		}
		
		public void HandlePlayError()
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Error";
			alert.Message = "Could not start sound player";
			alert.AddButton("Ok");
			alert.Show();
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
				if(e.ButtonIndex == 0)
				{
					PerformDelete();
					this.NavigationController.PopViewControllerAnimated(true);
				}
			};
			
			sheet.Canceled += delegate(object sender, EventArgs e) {
				// NOTE: Never called...
				// Event handler is not working?
				Console.WriteLine("Cancelled");
			};
			
			
			ShowSheet(sheet);
		}
		
		/**
		 * 
		 * 
		 */
		private void ShowSheet(UIActionSheet sheet)
		{
			// Need to make assumption about underlying views
			// Bad design by Apple...
			UIViewController parent = this.ParentViewController.ParentViewController;
			UITabBarController tabBarController  = (UITabBarController) parent;
			sheet.ShowFromTabBar(tabBarController.TabBar);
			
			// BUGGY http://stackoverflow.com/questions/1197746/uiactionsheet-cancel-button-strange-behaviour
			// sheet.ShowInView(this.View);
		}
		
		/**
		 * Open an action sheet for the duration of the sound clip play with the option 
		 * to stop the playing
		 * 
		 */
		public void DoPlay()
		{
			
			var mediaFile = NSUrl.FromFilename(this.currentRecord.GetPath());
			
			Console.WriteLine("Playing file " + mediaFile.AbsoluteUrl);
			
			this.player = AVAudioPlayer.FromUrl(mediaFile);
			
			// This can happen if URL is screwed up
			if(player == null)
			{
				HandlePlayError();
				return;
			}
			
			UIActionSheet sheet = new UIActionSheet();
			sheet.Title = "Playing";
			sheet.AddButton("Stop");
			
			sheet.Clicked += delegate(object s, UIButtonEventArgs e) {
				Console.WriteLine("Stopping");
				player.Stop();
			};
			
			ShowSheet(sheet);
			
			if(!player.PrepareToPlay())
			{
				Console.WriteLine("Player prearing failed");
				HandlePlayError();
				return;
			}
			
			// Destroy player and close dialog when the sound clip ends
			player.FinishedPlaying += delegate { 
				sheet.DismissWithClickedButtonIndex(0, true);
				this.player.Dispose(); 
			};	
			
			if(!player.Play())
			{
				HandlePlayError();
			}
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
