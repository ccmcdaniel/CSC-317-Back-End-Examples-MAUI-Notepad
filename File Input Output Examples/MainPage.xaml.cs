namespace File_Input_Output_Examples;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
		
		lblFileName.Text = "";
		txtContents.Text = "";
	}

	//Defining this property allows us to interface
	//with the label that stores the file path.

	//This is used in AppShell.xaml.cs when implementing
	//the file save functions.
	public string FileOpenedPath
	{
		get
		{
			return lblFileName.Text;
		}
		set
		{
			lblFileName.Text = value;
		}
	}

	//Same idea as above but for the editor field.
	public string FileContents
	{
		get
		{
			return txtContents.Text;
		}
		set
		{
			txtContents.Text = value;
		}
	}
}

