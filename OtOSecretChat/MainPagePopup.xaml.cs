using CommunityToolkit.Maui.Views;
namespace OtOSecretChat;


public partial class MainPagePopup : Popup
{
	public MainPagePopup()
	{
		InitializeComponent();

    }

    void OnCreateBtn(Object sender, EventArgs e) => Close(false);

    void OnJoinBtn(Object sender, EventArgs e) => Close(true);

}
