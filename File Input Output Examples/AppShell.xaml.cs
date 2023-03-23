using Microsoft.Maui.Storage;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using System.Text;
using System.Threading;
using CommunityToolkit.Maui.Alerts;

namespace File_Input_Output_Examples;

public partial class AppShell : Shell
{
	PickOptions pickOptions;
	string openedFilePath;
	public AppShell()
	{
		InitializeComponent();

		//Since we are working with .txt files, we will set the extensions
		//for each platform appropriately.
		var customFileType = new FilePickerFileType(
				new Dictionary<DevicePlatform, IEnumerable<string>>
				{
					{ DevicePlatform.iOS, new[] { "public.my.text.extension" } }, // UTType values
					{ DevicePlatform.Android, new[] { "application/text" } }, // MIME type
					{ DevicePlatform.WinUI, new[] { ".txt" } }, // file extension
					{ DevicePlatform.Tizen, new[] { "*/*" } },
					{ DevicePlatform.macOS, new[] { "txt" } }, // UTType values
				});
		//This sets up the options for the FilePicker used by this app.
		pickOptions = new PickOptions();
		pickOptions.PickerTitle = "Please select a text file.";
		pickOptions.FileTypes = customFileType;
	}

	private void CreateNewFile(object sender, EventArgs e)
	{
		//We get a reference to the main page so we can load the text file
		//for the user.
		MainPage target = (Current.CurrentPage as MainPage);
		
		//Since this is a new, unsaved file, we will just empty
		//MainPage's file path label and the editor box.
		target.FileContents = "";
		target.FileOpenedPath = "";
	}

	private void SaveFile(object sender, EventArgs e)
	{
		//Save the file
		//(See the overloaded definition below)
		SaveFile();
	}

	private void SaveFileAs(object sender, EventArgs e)
	{
		//(See overloaded definition below)
		SaveFileAs();
	}

	private void OpenFile(object sender, EventArgs e)
	{
		//(See the overloaded definition below)
		OpenFile();
	}

	private void ExitProgram(object sender, EventArgs e)
	{
		System.Environment.Exit(0);
	}


	//async is short for asynchronous.  This means that this function
	//will run on a separate thread from the base program.
	//This is required syntactically for the FilePicker box.
	private async void OpenFile()
	{
		//Gets a reference to the MainPage so we can load the contents
		MainPage target = (Current.CurrentPage as MainPage);
		try
		{
			//Show the File Open Dialog box to the user and wait for them
			//to select their file(which is what await keyword does).
			var result = await FilePicker.Default.PickAsync(pickOptions);

			//Make sure the result was successful.
			if (result != null)
			{
				//If so, then set the file opened.
				openedFilePath =  result.FullPath;
				target.FileOpenedPath = openedFilePath;

				//Use the file path to read from the file using standard
				//C# File I/O
				StreamReader s = new StreamReader(openedFilePath);
				target.FileContents = s.ReadToEnd();
				s.Close();
			}
		}
		catch (Exception)
		{
			//If something bad happens in the try above, then forget
			//about it and move on!
			target.FileOpenedPath = "";
			target.FileContents = "";
		}
	}

	//This function first checks to see if a file is already opened.
	//If so, then just save directly to that file using StreamWriter.
	//However, if it is not opened, then call SaveFileAs to open a
	//Save File Dialog Box
	private void SaveFile()
	{
		MainPage targetPage = (Current.CurrentPage as MainPage);

		if(targetPage.FileOpenedPath != "")
		{
			//Save to file already opened.
			StreamWriter file = new StreamWriter(targetPage.FileOpenedPath, false);
			file.Write(targetPage.FileContents);
			file.Close();
			return;
		}
		else
		{
			SaveFileAs();
		}
	}

	//This will prompt the user to save a file to the system.
	//Note: FileSaver is a third party class that is borrowed from
	//      the Community Development Toolkit, and is NOT part of 
	//      the default MAUI framework.  Additionally, at the time 
	//      of writing this code, there is an error where it does 
	//      not erase a file that exits, but merely overwrites part 
	//      of it as needed.
	private async void SaveFileAs()
	{
		MainPage targetPage = (Current.CurrentPage as MainPage);

		try
		{
			//Get the Editor Box's contents.
			string savedata = targetPage.FileContents;

			//Needed for the FileSaver box to function correctly.
			CancellationToken token = new CancellationToken();

			//Convert the File Contents into raw bytes to be used by File Saver
			//and put it in the stream buffer.
			using var stream = new MemoryStream(Encoding.Default.GetBytes(savedata));
			
			//Open the FileSaver dialog box, defauting the name of the saved file to
			//"untitled.txt".   Additionally, pass the stream containing the file
			//contents as well as the token to be saved.
			var result = await FileSaver.SaveAsync("untitled.txt", stream, token);

			//If file save was a success, then update the File Path with the
			//new location.
			if (result.IsSuccessful)
			{
				targetPage.FileOpenedPath = result.FilePath;
			}
		}
		catch (Exception)
		{
			//If something bad happens in the try above, then forget
			//about it and move on!
			targetPage.FileOpenedPath = "";
			targetPage.FileContents = "";
		}
	}

}
