using ClassLibrary2.Services;
using Microsoft.Azure.Devices.Client;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpApp
{

    public sealed partial class MainPage : Page
    {
        private static readonly string _conn = "HostName=ec-win20-iothub-mw.azure-devices.net;DeviceId=consoleapp;SharedAccessKey=zHGjUjbxSgfEQnyNW/HuJc5mpBSErc2vvapFN2nmp6Q=";
        private static readonly DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(_conn, TransportType.Mqtt);

        
        public MainPage()
        {
            this.InitializeComponent();
           //Här kör den funktionen som ska fånga meddelanden och skriv ut i UWP applikationen
            ReceiveMessageAsync(deviceClient).GetAwaiter();

            DeviceService.SendMessageAsync(deviceClient).GetAwaiter();



            async Task ReceiveMessageAsync(DeviceClient deviceClient)
            {
                while (true)
                {
                    var payload = await deviceClient.ReceiveAsync();
                    //om inte payload har fått något svar, så med continue kommer den bryta och gå tillbaka och börja om loopen
                    if (payload == null)
                        continue;
                    //Hämta 0 och 1 med getbytes och konvertera till en sträng med getstring som stödjer specialtecken med utf8
                    var message = $"Message received: {Encoding.UTF8.GetString(payload.GetBytes())}";
                    //Skapar en ny listview 
                    ListView messageList = new ListView();
                    dataMessage.Items.Add(message);
                    //lägger in data till parent elementet
                    stackingPanel.Children.Add(messageList);

                    //När meddelanden har mottagits tar den bort den från hubben
                    await deviceClient.CompleteAsync(payload);
                }
            }

            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string textdata = messageBox.Text;
            Message message = new Message(Encoding.UTF8.GetBytes(textdata));
            //kör en arrow async funktion som skickar data till hubben
            await Task.Run(async () => await deviceClient.SendEventAsync(message));
            
        }


    }
}
