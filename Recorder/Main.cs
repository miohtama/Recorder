
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Recorder
{
	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args);
		}
		

	}

	// The name AppDelegate is referenced in the MainWindow.xib file.
	public partial class AppDelegate : UIApplicationDelegate
	{
		bool recording;
		
		RecordHistory history;
		
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			recording = false;
			
			// Get existing data from the disk
			// NOTE: must not pass 20 second start-up limit here
			history = new RecordHistory();
			history.Scan();
			
			// If you have defined a view, add it here:
			window.AddSubview (tabBarController.View);
			
			window.MakeKeyAndVisible ();
			
			recordButton.TouchDown += delegate { HitRecord(); };
    				
			return true;
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
		}
		
		
		public void HitRecord() {
			
			if(recording) {
				StopRecord();
			} else {
				StartRecord();
			}
		}
		
		public void StartRecord() {
			recordButton.SetTitle("Stop Recording", UIControlState.Normal);
			recording = true;
			
			recordIndicator.Hidden = false;
		}
		
		public void StopRecord() {
			recordButton.SetTitle("Record", UIControlState.Normal);
			recording = false;
			
			recordIndicator.Hidden = true;
		}
		
	}
}
