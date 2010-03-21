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

namespace Recorder
{


	/**
	 * Create gradient buttons in MonoTouch
	 * 
	 * This creates faux "UIGlass" buttons. 
	 * 
	 * http://www.shrinkrays.net/code-snippets/csharp/monotouch-tips-and-snippets.aspx
	 * 
	 */
	public class ButtonHelper
	{
		static UIImage image; // image template

		public ButtonHelper ()
		{
			
		}
		
		/**
		 * Convert button to use a gradient
		 */
		public static void MakeGradientButton(UIButton button)
		{
			
			UIImage image = ButtonHelper.getImage();
			button.SetBackgroundImage(image,UIControlState.Normal);
			button.BackgroundColor = UIColor.Clear;
			button.Font = UIFont.BoldSystemFontOfSize(20);
			button.SetTitleColor(UIColor.White, UIControlState.Normal);
		}
		
		
		/** Lazy image loading */
		static UIImage getImage()
		{
			if(ButtonHelper.image == null) 
			{
				ButtonHelper.image = UIImage.FromFile("resources/uiglassbutton-template.png");
				ButtonHelper.image = ButtonHelper.image.StretchableImage(9,0);
			}
			
			return ButtonHelper.image;
				
		}
		                      
	}
}
