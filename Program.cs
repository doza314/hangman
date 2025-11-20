//Selecting between single-player and multiplayer
//Singleplayer - pulls word form random word API
//Multiplayer - you literally pass the computer around between players
int option = 0;
bool valid_option = false;
while(!valid_option)
{
      Console.Clear();
      Console.WriteLine("Select an option ('1' or '2'): \n 1. Single Player \n 2. Local Multiplayer (pass device between players)\n");
      Console.Write("> ");
      string? user_int = Console.ReadLine();
      if (user_int == null){ return; }
      
      //Logic for making sure user input is valid:
      if (user_int == "1" | user_int == "2")
      {   
         option = int.Parse(user_int);  
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

string word = "";
//retrieving/inputing word to be guessed:
switch(option)
{
  case 1: 
    APIHandler handler = new APIHandler();
    word = await handler.getRandomWord();
  break;

  case 2: //Manually typing word
   Console.Write("Enter a word: ");
   string? user_input = Console.ReadLine();

   Console.Clear();

   if (user_input == null) { return; }
   word = user_input;


  break;
}

//Running Game Loop:
Game game = new Game(word);
game.Run();
