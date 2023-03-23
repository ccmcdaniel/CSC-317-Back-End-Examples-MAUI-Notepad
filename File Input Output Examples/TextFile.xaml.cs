namespace File_Input_Output_Examples;

public partial class TextFile : ContentPage
{
	public TextFile(string filePath)
	{
		InitializeComponent();
		LoadTextFile(filePath);

	}

	private void LoadTextFile(string filePath)
	{
		try
		{
			StreamReader streamReader = new StreamReader(filePath);

			lblTextContents.Text = streamReader.ReadToEnd();

			streamReader.Close();
		}
		catch
		{
			lblTextContents.Text = "Error: Could Not Read from Text File.";
			lblTextContents.TextColor = Color.Parse("Red");
		}
	}

	private void Back(object sender, EventArgs e)
	{
		Navigation.PopAsync();
	}
}