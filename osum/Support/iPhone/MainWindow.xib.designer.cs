// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace osum.Support.iPhone {
	
	
	// Base type probably should be MonoTouch.Foundation.NSObject or subclass
	[MonoTouch.Foundation.Register("AppDelegate")]
	public partial class AppDelegate {
		
		private MonoTouch.UIKit.UIWindow __mt_window;
		
		private GameWindowIphone __mt_glView;
		
		private GameViewController __mt_viewController;
		
		#pragma warning disable 0169
		[MonoTouch.Foundation.Connect("window")]
		private MonoTouch.UIKit.UIWindow window {
			get {
				this.__mt_window = ((MonoTouch.UIKit.UIWindow)(this.GetNativeField("window")));
				return this.__mt_window;
			}
			set {
				this.__mt_window = value;
				this.SetNativeField("window", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("glView")]
		private GameWindowIphone glView {
			get {
				this.__mt_glView = ((GameWindowIphone)(this.GetNativeField("glView")));
				return this.__mt_glView;
			}
			set {
				this.__mt_glView = value;
				this.SetNativeField("glView", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("viewController")]
		private GameViewController viewController {
			get {
				this.__mt_viewController = ((GameViewController)(this.GetNativeField("viewController")));
				return this.__mt_viewController;
			}
			set {
				this.__mt_viewController = value;
				this.SetNativeField("viewController", value);
			}
		}
	}
	
	// Base type probably should be MonoTouch.UIKit.UIViewController or subclass
	[MonoTouch.Foundation.Register("GameViewController")]
	public partial class GameViewController {
	}
}
