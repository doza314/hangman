using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
  private const int Port = 5001;

  public void Run()
  {
      //Enter Host's IP
      Console.Write("Enter host IP address (e.g. 192.168.1.10): ");
      string? ip = Console.ReadLine();
      if (string.IsNullOrWhiteSpace(ip)) {return;}
      
      //TCP Handshake
      using TcpClient client = new TcpClient();
      Console.WriteLine("[CLIENT] Connecting...");
      client.Connect(ip, Port);
      Console.WriteLine("[CLIENT] Connected!");
      
      //Stream
      using NetworkStream stream = client.GetStream();
      using var reader = new StreamReader(stream, Encoding.UTF8);
      using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };   
      
      Console.WriteLine("here");
      while (true)
      {
          
          string? msg = reader.ReadLine();  // <-- RECEIVE from host
          if (msg == null)
          {
              Console.WriteLine("[CLIENT] Disconnected from host.");
              break;
          }
          
          Console.WriteLine(msg);

          var parts = msg.Split('|');
          if (parts.Length < 3 || parts[0] != "STATE") {continue;} // malformed, ignore

          
           
      }
  } 
}
