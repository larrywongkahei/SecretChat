using System.Text.Json;
using CommunityToolkit.Maui.Views;
using SignalRServer.Models;
using CommunityToolkit.Maui.Alerts;

namespace OtOSecretChat;

public partial class JoinPopup : Popup
{
	public string RoomNum { get; set; }
	public JoinPopup()
	{
		InitializeComponent();
	}

	async void JoinRoom(Object sender, EventArgs e)
	{
		RoomNum = Input.Text;
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage response = await client.GetAsync($"http://localhost:5001/api/Rooms/{RoomNum}"))
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var theRoom = JsonSerializer.Deserialize<Room>(content);
                if (theRoom != null)
                {
                    Close(Convert.ToInt32(RoomNum));
                }
            }
        }
                
	}
}
