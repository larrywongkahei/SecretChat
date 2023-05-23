using System.Text.Json;
using CommunityToolkit.Maui.Views;
using SignalRServer.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Threading;

namespace OtOSecretChat;

public partial class JoinPopup : Popup
{
	public string RoomNum { get; set; }

    private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

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
                if(theRoom == null)
                {
                        string text = "This room is not exist";
                        ToastDuration duration = ToastDuration.Short;
                        double fontSize = 14;

                        var toast = Toast.Make(text, duration, fontSize);

                        await toast.Show(cancellationTokenSource.Token);
                    
                }
                else if (theRoom.UserTwo != null)
                {
                    string text = "This room is Full";
                    ToastDuration duration = ToastDuration.Short;
                    double fontSize = 14;

                    var toast = Toast.Make(text, duration, fontSize);

                    await toast.Show(cancellationTokenSource.Token);

                }
                else
                {
                    Close(Convert.ToInt32(RoomNum));
                }

            }
        }
                
	}
}
