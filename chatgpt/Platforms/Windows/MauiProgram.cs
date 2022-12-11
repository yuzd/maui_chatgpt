using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.WebUI;
using WinRT;
using Microsoft.UI.Dispatching;
using WinUIApplication = Microsoft.UI.Xaml.Application;
namespace chatgpt.Platforms.Windows
{
	//partial class MauiProgram
	//{
	//	static event Action? Exit;

	//	[DllImport("Microsoft.ui.xaml.dll")]
	//	static extern void XamlCheckProcessRequirements();

	//	[STAThread]
	//	static void Main(string[] args)
	//	{
	//		XamlCheckProcessRequirements();
	//		ComWrappersSupport.InitializeComWrappers();
	//		WinUIApplication.Start(p =>
	//		{
	//			var context = new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread());
	//			SynchronizationContext.SetSynchronizationContext(context);

	//			var app = new App();
	//			Exit += () =>
	//			{
					
	//			};
	//		});

	//		Exit?.Invoke();
	//	}
	//}
}
