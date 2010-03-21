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
		
		RecordHelper recordHelper;
		
		RecordHistory history;
		
		// This is the easy way... fk all those who are against global variables 
		// TabBar must be pushed through to UIActionSheet in the code
		// and it would be super cumbersome to pass it around in the code
		public static AppDelegate appDelegateInstance;
		
		
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			AppDelegate.appDelegateInstance = this;
			
			recording = false;
			
			// Get existing data from the disk
			// NOTE: must not pass 20 second start-up limit here
			history = new RecordHistory();
			
			// Regenerate history view each time history data is being manipulated 
			history.contentChanged += delegate(RecordHistory h) { this.recordTableView.ReloadData(); };
				
			history.Scan();
			
			recordHelper = new RecordHelper(history);
		
			// If you have defined a view, add it here:
			window.AddSubview (tabBarController.View);
			
			window.MakeKeyAndVisible ();
			
			// Bind history data with table view
			// TODO: How to pass data from a parent view to child viewl
			RecordTableViewController ctl = (RecordTableViewController) recordTVC;
			ctl.history  = history;
			
			recordButton.TouchUpInside += delegate { HitRecord(); };
    				
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
			
			
			// Do fancy UI stuff
		
			
			if(recordHelper.PrepareRecording()) 
			{
			
				recordHelper.updateStatus = UpdateRecordStatus;
				recordHelper.finish = FinishRecording;
				
				recordButton.SetTitle("Stop Recording", UIControlState.Normal);
				recording = true;
			
				recordIndicator.Hidden = false;
			
				timeLabel.Text = "0 s";
				timeLabel.Hidden = false;
			
				recordHelper.StartRecording();
			}
			
			
			
			
		}
		
		public void StopRecord() {			
			recordHelper.StopRecording(true);
		}
		
		/**
		 * UI callback for the recorder to stop.
		 * 
		 * Can be UI initiated or initiated from recording callbacks
		 * 
		 */
		public void FinishRecording(bool save)
		{
			recordButton.SetTitle("Record", UIControlState.Normal);
			recording = false;
			
			recordIndicator.Hidden = true;
			timeLabel.Hidden = true;
			recordMeter.Progress = 0;
			
			// Add new entries to the list
			if(save)
			{
				history.ScanAndRefresh();
			}
		}
		
		public void UpdateRecordStatus(double time, float power) 
		{
			// http://stackoverflow.com/questions/1240846/avaudiorecorder-peak-and-average-power
			timeLabel.Text = (int) time + " s";
			
			// Assume max. 100 dB
			recordMeter.Progress = (100.0f - power) / 100.0f;
		}
		
		/**
		 * Needed for UIActionSheets 
		 * 
		 * http://stackoverflow.com/questions/1197746/uiactionsheet-cancel-button-strange-behaviour
		 * 
		 */
		public UITabBar GetTabBar()
		{
			return this.tabBar;
		}
		
	}
}
