/**
 * Copyright 2010 mFabrik Research Oy
 * 
 * Licensed under GPL 2.
 * 
 * http://mfabrik.com
 * 
 */

using System;

using MonoTouch.UIKit;

using MonoTouch.AudioToolbox;
using MonoTouch.AVFoundation;
using MonoTouch.Foundation;

namespace Recorder
{

	/**
	 * Audio recorder which regularly updates its status about passed time and peak power. 
	 * 
	 * Please see http://wiki.monotouch.net/HowTo/Sound/How_to_record_sound_using_the_iPhone_microphone
	 */
	public class RecordHelper
	{
		
		// Manage recorder files
		RecordHistory history;
		
		AVAudioRecorder recorder;
		
		NSTimer timer;
		
		NSError recordError;
		
		public delegate void UpdateStatus(double seconds, float peakPower);

		public delegate void Finish(bool save);
		
		// Set this externally to received status updates
		public UpdateStatus updateStatus;
		
		public Finish finish;
		
		public static double UPDATE_RATE = 0.25;
		
   		//public void start Recording
        public RecordHelper(RecordHistory history) 
		{
			this.history = history;
		}
		
		
		/**
		 * @return true on success
		 * 
		 */
		public bool PrepareRecording ()
        {
            NSObject[] values = new NSObject[]
            {    
                NSNumber.FromFloat(44100.0f),
                NSNumber.FromInt32((int)AudioFileType.WAVE),
                NSNumber.FromInt32(1),
                NSNumber.FromInt32((int)AVAudioQuality.Max)
            };

            NSObject[] keys = new NSObject[]
            {
                AVAudioSettings.AVSampleRateKey,
                AVAudioSettings.AVFormatKey,
                AVAudioSettings.AVNumberOfChannelsKey,
                AVAudioSettings.AVEncoderAudioQualityKey
            };

            NSDictionary settings = NSDictionary.FromObjectsAndKeys (values, keys);
            
			string audioFilePath = history.GetNextFilename();
            
            NSUrl url = NSUrl.FromFilename(audioFilePath);
            
            // recordError = new NSError ();
            
            //Set recorder parameters
            recorder = AVAudioRecorder.ToUrl(url, settings, out recordError);
           
			
			if(recorder == null) 
			{
				HandleError(recordError);
				return false;
			}
			
			
            //Set Metering Enabled so you can get the time of the wav file
            recorder.MeteringEnabled = true;
            
			Console.WriteLine("Preparing recording");
			
            if(!recorder.PrepareToRecord())
			{
				HandleError(recordError);
				return false;
			}
            
			recorder.FinishedRecording += delegate {
                recorder.Dispose();
                Console.WriteLine("Recorder finished and disposed");
            };                      
			
			return true;
        }
		
		
		public void StartRecording()
		{
			Console.WriteLine("Starting recording");
		
			timer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(UPDATE_RATE), TickTock);
			
			if(!recorder.Record()) 
			{
				HandleError(recordError);
				StopRecording(false);
			}
		}
		
		
		/**
		 * 
		 * 
		 */
        public void StopRecording (bool save)
        {
			if(timer != null)
			{
				timer.Dispose();
				timer = null;
			}
			
            recorder.Stop();
			
			if(!save)
			{
				recorder.DeleteRecording();
			}
			
			if(finish != null)
			{
				finish(save);
			}
			Console.WriteLine("Recording ended");
        }
		
		/**
		 * TODO: This class user should handle errors
		 * 
		 * Quick and dirty...
		 */
		public void HandleError(NSError error)
		{
			string msg;
			
			if(error == null)
			{
				msg = "Unknown error";	
			} else {
				msg = error.LocalizedDescription;
			}
				
			Console.WriteLine("Could not start recording:" + msg);
			UIAlertView alert = new UIAlertView();
			alert.Title = "Could not start recording";
			alert.Message = msg;
			alert.AddButton("Ok");
			alert.Show();
		}
	
		[Export("TickTock")] 
	 	void TickTock() 
	 	{ 
			
			recorder.UpdateMeters();
			
			float power = recorder.PeakPower(0);
			Console.WriteLine("Tick " + recorder.currentTime + " power:" + power);
			
			
			if(this.updateStatus != null) 
			{
				updateStatus(recorder.currentTime, power);
			}
	 	}
	}
}
