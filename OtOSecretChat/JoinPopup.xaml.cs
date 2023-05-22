using CommunityToolkit.Maui.Views;

namespace OtOSecretChat;

public partial class JoinPopup : Popup
{
	public string RoomNum { get; set; }
	public JoinPopup()
	{
		InitializeComponent();
	}

	void JoinRoom(Object sender, EventArgs e)
	{
		RoomNum = Input.Text;
		Close(Convert.ToInt32(RoomNum));
	}
}
