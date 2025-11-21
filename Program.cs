//Selecting between single-player and multiplayer
//Singleplayer - pulls word form random word API
//Multiplayer - you literally pass the computer around between players
int option = 0;
bool valid_option = false;
while(!valid_option)
{
      Console.Clear();
      Console.WriteLine("Select an option ('1', '2', or '3'):\n");
      Console.WriteLine(" 1. Single Player\n");
      Console.WriteLine(" 2. Pass n' Play\n");
      Console.WriteLine(" 3. LAN Multiplayer\n");
      
      Console.Write("> ");
    string? user_int = Console.ReadLine();
      if (user_int == null){ return; }
      
    //Logic for making sure user input is valid:
      if (user_int == "1" | user_int == "2" | user_int == "3")
      {   
         option = int.Parse(user_int);  
         break;
      }
      else
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid Input! Please enter either '1', '2', or '3'!");
        Console.WriteLine("[Press Enter]");
        
        user_int = Console.ReadLine();
        if(user_int == null) {return;};
        Console.ResetColor();
      }
}

string word = "";
int client_type = 0;

//Selecting Gamemode:
switch(option)
{
  case 1: //Single Player w/ word from random API
    Console.Clear();
    Console.WriteLine("Pulling random word from https://random-word-api.vercel.app/api?words=1");
    word = await APIHandler.getRandomWord();
    option = 0;
    break;

  case 2: //Manually typing word
   Console.Write("Enter a word: ");
   string? user_input = Console.ReadLine();

   Console.Clear();

   if (user_input == null) { return; }
   word = user_input;
   option = 0;
  break;
      
  case 3: //LAN Multiplayer (Host = client_type 1, Client = client_type 2)
    valid_option = false;

    while(!valid_option)
    {
      Console.Clear();
      Console.WriteLine("LAN Multiplayer:");
      Console.WriteLine("\n 1. Host Game (choose word)");
      Console.WriteLine("\n 2. Join Game (guess word)\n");
      Console.Write("> ");

      string? user_int = Console.ReadLine();
      if (user_int == null){ return; }
      
      //Logic for making sure user input is valid:
      if (user_int == "1" | user_int == "2") 
      {   
         option = int.Parse(user_int);  
         client_type = option;
         break;
      }
      else
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid Input! Please enter either '1' or '2'!");
        Console.WriteLine("[Press Enter]");
        
        user_int = Console.ReadLine();
        if(user_int == null) {return;};
        Console.ResetColor();
      }
    }
  break;
}

//Running Game Loop:
switch(option)
{
  case 0: //Single Player / Pass and Play
    Game game = new Game(word);
    game.Run();
  break;
  case 1: //HOST
    Host host = new Host();
    host.Run();
  break;
  case 2: //Client
    Client client = new Client();
    client.Run();
  break;
}
