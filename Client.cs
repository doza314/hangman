using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
  private const int Port = 5000;

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

      while(true)
      {
          string? msg = reader.ReadLine();  // <-- RECEIVE from host
          if (msg == null)
          {
              Console.WriteLine("[CLIENT] Disconnected from host.");
              break;
          }
         
          var parts = msg.Split('|');
          if (parts.Length < 6 || parts[0] != "STATE")
          continue; // malformed, ignore
      }
  } 
}
