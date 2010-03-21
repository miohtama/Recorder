
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

		// Set this externally to received status updates
		public UpdateStatus updateStatus;
		
		public static double UPDATE_RATE = 0.25;
		
   		//public void start Recording
        public RecordHelper(RecordHistory history) 
		{
			this.history = history;
		}
		
		
		public void PrepareRecording ()
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
            
            recordError = new NSError ();
            
            //Set recorder parameters
            recorder = AVAudioRecorder.ToUrl(url, settings, out recordError);
           
			
			if(recorder == null) 
			{
				HandleError(recordError);
			}
			
			
            //Set Metering Enabled so you can get the time of the wav file
            recorder.MeteringEnabled = true;
            
			Console.WriteLine("Starting recording");
			
            recorder.PrepareToRecord();            
            
			recorder.FinishedRecording += delegate {
                recorder.Dispose();
                Console.WriteLine("Done Recording");
            };                      
        }
		
		
		public void StartRecording()
		{
		
			timer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(UPDATE_RATE), TickTock);
			
			if(!recorder.Record()) 
			{
				HandleError(recordError);
			}
		}
		
		
		/**
		 * 
		 * 
		 */
        public void StopRecording (bool save)
        {
			timer.Dispose();
			
            recorder.Stop();
			
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
			Console.WriteLine("Tick " + recorder.currentTime);
			
			recorder.UpdateMeters();
			
			if(this.updateStatus != null) 
			{
				updateStatus(recorder.currentTime, recorder.AveragePower(0));
			}
	 	}
	}
}
