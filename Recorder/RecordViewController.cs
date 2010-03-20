using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;


namespace Recorder
{

	[MonoTouch.Foundation.Register("RecordViewController")]
	public class RecordViewController : UIViewController
	{

		public RecordViewController (IntPtr p) : base (p) {}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Title = "Record";
			
			this.View.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
		
		}
	}
}
	

