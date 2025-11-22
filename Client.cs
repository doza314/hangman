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
  
  private string currentStage = " ";

  private int stageNumber = 0;
 
  public void printState(string gallow, string guesses, string hiddenWord)
  {
    Console.Clear();
    Console.WriteLine(gallow);
    Console.WriteLine(guesses);
    Console.WriteLine(hiddenWord);
  }

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
          //stateMessage =
          //     $"STATE/{game.StageNumberString()}/{game.Guesses()}/{game.HiddenWord()}/{game.StateNum()}/{game.word()}";
          //         0             1                     2                  3                 4                 5


          string? message = reader.ReadLine();  // <-- RECEIVE from host
          if (message == null)
          {
              Console.WriteLine("[CLIENT] Disconnected from host.");
              break;
          }

          //Split message
          var msg = message.Split('/');
          if (msg[0] != "STATE") {continue;} // malformed, ignore
          stageNumber = int.Parse(msg[1]);
          currentStage = stages[stageNumber];
          
          //WIN/LOSS check
          if(msg[4] == "2") //client wins
          {
            printState(currentStage, msg[2], msg[3]);

            Console.WriteLine("You Win!!!");
            Console.WriteLine("You guessed the word(s)!");
            break;
          }
          else if(msg[4] == "1")//host wins
          {

            printState(currentStage, msg[2], msg[3]);

            Console.WriteLine("You Lose!!!");
            Console.WriteLine("You were unable to guess the word(s)!");
            Console.WriteLine("The word was '" + msg[5] + "'!");
            break;
          }


          //Print game state
          printState(currentStage, msg[2], msg[3]);
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
