using System.Net;
using System.Net.Sockets;
using System.Text;

class Host
{
  private const int Port = 5001;
  private string? word;

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
          while(game.hung())
          {
            game.printState();

           // Determine status
           //string status = "PLAY";
           // if (game.IsWon) status = "WIN";
           // else if (game.IsLost) status = "LOSE";


            // 3a. Build and send STATE message to client
          string guessesString = string.Join("", game.Guesses());
          string stateMessage =
               $"STATE|{game.HiddenWord()}|{game.StageString()}|{game.Guesses()}";
               writer.WriteLine(stateMessage);  // <-- SEND to client

          // If game over, stop
          //  if (status == "WIN")
          //  {
          //      Console.WriteLine("[HOST] Client won!");
          //      break;
          //  }
          //  if (status == "LOSE")
          //  {
          //      Console.WriteLine("[HOST] Client lost. Word was: " + word);
          //      break;
          //  }

            // 3b. Wait for GUESS message from client
            char guess = ' '; 
            string? msg = reader.ReadLine();  // <-- RECEIVE from client
            if (msg == null)
            {
                Console.WriteLine("[HOST] Client disconnected.");
                break;
            }
            // Expect "GUESS|x"
            var parts = msg.Split('|');
            if (parts.Length == 2 && parts[0] == "GUESS")
            {
                guess = parts[1][0];
                
            }
           game.ReceiveGuess(guess);
        }

        listener.Stop();
    }
   }
  } 
