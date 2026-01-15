using System.Net;
using System.Net.Sockets;
using System.Text;

class Host
{
  private const int Port = 5001;
  private string? word;
  private string guess = " ";
  private string stateString = " ";
  private string stateMessage = " ";
  private string guessesString = " ";
  public void Run()
  {
    //Host creates secret word
    Console.Clear();
    Console.Write("Enter a word: ");
    string? user_input = Console.ReadLine();

    if (user_input == null) { return; }
    word = user_input;

    Console.WriteLine("here");

    Game game = new Game(word);

    // Start listening for a client
    var listener = new TcpListener(IPAddress.Any, Port);
    listener.Start();
    Console.WriteLine($"[HOST] Waiting for client on port {Port}...");
    
    //TCP Handshake
    using TcpClient client = listener.AcceptTcpClient();
    Console.WriteLine("[HOST] Client connected.");
    
    //Network Stream
    using NetworkStream stream = client.GetStream();
    using var reader = new StreamReader(stream, Encoding.UTF8);
    using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

    // 3. Main host loop
        while (true)
        {
          stateString = game.StateNum();

          //WIN/LOSS check
          if(stateString == "1")
          {
            Console.WriteLine("You Win!!!");
            Console.WriteLine("Your opponent was unable to guess the word(s)!");
            stateMessage = 
              $"STATE/{game.StageNumberString()}/{game.Guesses()}/{game.HiddenWord()}/{stateString}/{game.word()}";
            writer.WriteLine(stateMessage);
            break;
          }
          else if(stateString == "2")
          {
            Console.WriteLine("You Lose!!!");
            Console.WriteLine("Your opponent guessed the word(s)!");
            stateMessage = 
              $"STATE/{game.StageNumberString()}/{game.Guesses()}/{game.HiddenWord()}/{stateString}/{game.word()}";
            writer.WriteLine(stateMessage);
            break;
          }

            game.printState();

          //Build and send STATE message to client
          guessesString = string.Join("", game.Guesses());
          stateMessage =
               $"STATE/{game.StageNumberString()}/{game.Guesses()}/{game.HiddenWord()}/{game.StateNum()}/{game.word()}";
                //0             1                     2                  3                 4                 5
          writer.WriteLine(stateMessage);  // <-- SEND to client


          // Wait for GUESS message from client
          Console.WriteLine("Waiting for guess...");

          string? message = reader.ReadLine(); // <-- RECEIVE from client
          if (message == null)
          {
            Console.WriteLine("[HOST] Client disconnected.");
            break;
          }

          // Expect "GUESS|x"
          var msg = message.Split('/');
          if (msg[0] == "GUESS")
          {
            guess = msg[1];
          }
          game.ReceiveGuess(guess);
        }
        listener.Stop();
    }
   }
