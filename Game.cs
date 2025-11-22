class Game
{
  //Relevant game variables:
  private string _word;
  // wprivate bool receiving;
  private string _hidden_word = "";
  private HashSet<char> set = new HashSet<char>();

  private int current_stage = 0;
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

  //Game functions:
  //Constructor:
  public Game(string word)
  {
    _word = word;
   _hidden_word = new string('_', word.Length);

   for(int i = 0; i < _hidden_word.Length; i++) 
   {
     if (word[i] == ' ')
     {
       _hidden_word = _hidden_word[..i] + " " + _hidden_word[(i + 1)..];
     }
   }
  }


  //Print the current state of the game:
  public void printState()
  {
      Console.Clear();
      Console.WriteLine(stages[current_stage]);

      if(set.Count() > 0)
      {
      Console.Write("Guesses: ");

      foreach(char c in set)
      {
            Console.Write(c + ", ");
      }
      }

      Console.WriteLine();
      Console.WriteLine(_hidden_word);

  }

  //For when player locally inputs a guess
  public void Guess()
  {
      string? guess_console = Console.ReadLine();
      if (guess_console == null) {return;}
      char guess = guess_console[0];
      set.Add(guess);

      checkGuess(guess);
  }

  //For when host receives guess from client
  public void ReceiveGuess(string guess) 
  {
      char g = guess[0];
      set.Add(g);
      checkGuess(g);
  }
  
    //Checks character against each element in the word
  public void checkGuess(char guess)
  {
     bool found_match = false;
      for(int i = 0; i < _hidden_word.Length; i++)
      {
        if (_word[i] == guess)
        {
          _hidden_word = _hidden_word[..i] + guess + _hidden_word[(i + 1)..];
          found_match = true;
        }
      }

      if(!found_match)
      { 
        current_stage += 1;
      }
  }


  //Main game loop:
  public void Run()
  {
    while (current_stage < stages.Length)
    {
      if(_hidden_word == _word)
      {
        Console.WriteLine("you win!!!");
        break;
      }

      if(current_stage == stages.Length - 1)
      {
        Console.WriteLine("you lost lol !!!!");
        Console.Write("The word was: ");
        Console.WriteLine(_word);
        break;
      }

      printState();

      Console.Write("Guess a Letter: ");
      
      Guess();
    }
  }

  //HOST MESSAGE COMPONENTS:

    public string StageNumberString() //1
  {
    return current_stage.ToString();
  }

    //returns string of guesses
  public string Guesses() //2
  {
    string guessChars = "";

    if(set.Count > 0)
    {
      guessChars = string.Join(", ", set);      
      return guessChars;
    }
    else
    {
      return " ";
    }
  }

  public string HiddenWord() //3 
  {
    return _hidden_word;
  }
  
  public string StateNum()
  {
    if (current_stage < stages.Length)
    {
      if(_hidden_word == _word)
      {
        return "2"; //Client wins
      }
      else
      {
        return "0"; //Still playing
      }
    }
    else
    {
      return "1"; //Host wins
    }
  }
  
  public string word()
  {
    return _word;
  }

  public bool hung()
  {
    return (current_stage < stages.Length);
  }
}
