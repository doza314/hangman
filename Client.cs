using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
  private const int Port = 5001;

  private string[] stages = 
  [
      "  +---+\n  |   |\n      |\n      |\n      |\n      |\n=========",
      "  +---+\n  |   |\n  O   |\n      |\n      |\n      |\n=========",
      "  +---+\n  |   |\n  O   |\n  |   |\n      |\n      |\n=========",
      "  +---+\n  |   |\n  O   |\n /|   |\n      |\n      |\n=========",
      "  +---+\n  |   |\n  O   |\n /|\\  |\n      |\n      |\n=========",
      "  +---+\n  |   |\n  O   |\n /|\\  |\n /    |\n      |\n=========",
      "  +---+\n  |   |\n  O   |\n /|\\  |\n / \\  |\n      |\n=========",
  ];
  
  private int current_stage = 0;
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
      
      //Console.WriteLine("here");
      while (true)
      {  
          //Host sends:
          //string stateMessage =
          //     $"STATE/\n{game.StageString()}/\n{game.Guesses()}/\n{game.HiddenWord()}";
          //     writer.WriteLine(stateMessage); 

          string? msg = reader.ReadLine();  // <-- RECEIVE from host
          if (msg == null)
          {
              Console.WriteLine("[CLIENT] Disconnected from host.");
              break;
          }

          //Split message
          var parts = msg.Split('/');
          if (parts[0] != "STATE") {continue;} // malformed, ignorei
          current_stage = int.Parse(parts[1]);
          
          //WIN/LOSS check
          if(parts[4] == "2") //client wins
          {
            Console.WriteLine("You Win!!!");
            Console.WriteLine("You guessed the word(s)!");
            break;
          }
          else if(parts[4] == "1")//host wins
          {
            Console.WriteLine("You Lose!!!");
            Console.WriteLine("You were unable to guess the word(s)!");
            Console.WriteLine("The word was '" + parts[5] + "'!");
            break;
          }

          //Print game state
          Console.Clear();
          Console.WriteLine(stages[current_stage]);
          Console.WriteLine(parts[2]);
          Console.WriteLine(parts[3]);
          Console.Write("Guess a letter: ");

          //Client inputs guess:
          string guess = "";
          string? client_input = Console.ReadLine();

          if (client_input == null) { return; }
          guess = client_input;
          string guess_msg = 
            $"GUESS/{guess}";

          writer.WriteLine(guess_msg); //<-- Send to Host 

      }
  } 
}
