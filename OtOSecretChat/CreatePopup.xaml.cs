using CommunityToolkit.Maui.Views;

namespace OtOSecretChat;

public partial class CreatePopup : Popup
{
	public int RoomNumber { get; set; }
	public CreatePopup()
	{
		InitializeComponent();
		BindingContext = this;
		var randomNum = new Random();
		RoomNumber = randomNum.Next(1000, 9999);
	}

    void CreateRoom(Object sender, EventArgs e) => Close(RoomNumber);
}
