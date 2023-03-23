using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace File_Input_Output_Examples;

class Program : MauiApplication
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	static void Main(string[] args)
	{
		var app = new Program();
		app.Run(args);
	}
}
