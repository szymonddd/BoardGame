// See https://aka.ms/new-console-template for more information
public class Program
{
    public static void Main(string[] args)
    {

        Board board = new Board(30);
        
        IPlayer warrior = new Warrior("Wojownik");
        IPlayer mage = new Mage("Mag");
        IPlayer healer = new Healer("Uzdrawiacz");
        
        Game game = new Game(board);
        game.Players.Add(warrior);
        game.Players.Add(mage);
        game.Players.Add(healer);


        game.OnSpecialEvent += (message) => Console.WriteLine(message);


        Console.WriteLine("Gra rozpoczyna się!");
        game.StartGame();


        Console.WriteLine("\nWyniki:");
        game.DisplayResults();
    }
}
public class Player
{
    public string Name { get; set; }
    public int Position { get; set; }
    public int Score { get; set; }

    public Player(string name)
    {
        Name = name;
        Position = 0; 
        Score = 0;    
    }


    public void Move(int steps)
    {
        Position += steps;
    }
    
    public void UpdateScore(int points)
    {
        Score += points;
    }
}
public class Board
{
    public int Size { get; set; }
    public Dictionary<int, int> Rewards { get; set; }

    public Board(int size)
    {
        Size = size;
        Rewards = new Dictionary<int, int>();
        GenerateRewards();
    }


    private void GenerateRewards()
    {
        Random rand = new Random();
        for (int i = 1; i < Size; i++)
        {

            if (rand.NextDouble() < 0.3)
            {
                Rewards[i] = rand.Next(1, 11);
            }
        }
    }

    public int GetReward(int position)
    {
        return Rewards.ContainsKey(position) ? Rewards[position] : 0;
    }
}
public interface IPlayer
{
    string Name { get; set; }
    int Position { get; set; }
    int Score { get; set; }

    void Move(int steps);
    void UpdateScore(int points);
}
public class Warrior : IPlayer
{
    public string Name { get; set; }
    public int Position { get; set; }
    public int Score { get; set; }

    public Warrior(string name)
    {
        Name = name;
        Position = 0;
        Score = 0;
    }

    public void Move(int steps)
    {
        Position += steps;
    }

    public void UpdateScore(int points)
    {
        Score += points * 2;
    }
}
public class Mage : IPlayer
{
    public string Name { get; set; }
    public int Position { get; set; }
    public int Score { get; set; }

    public Mage(string name)
    {
        Name = name;
        Position = 0;
        Score = 0;
    }

    public void Move(int steps)
    {
        Position += steps;
    }

    public void UpdateScore(int points)
    {
        Score += points;
    }

    public void CastSpell(Board board, Player target)
    {

        target.Position += 3;
    }
}
public class Healer : IPlayer
{
    public string Name { get; set; }
    public int Position { get; set; }
    public int Score { get; set; }

    public Healer(string name)
    {
        Name = name;
        Position = 0;
        Score = 0;
    }

    public void Move(int steps)
    {
        Position += steps;
    }

    public void UpdateScore(int points)
    {
        Score += points;
    }

    public void Heal(Player target)
    {
        
        target.UpdateScore(5);
    }
}
public class Game
{
    public List<IPlayer> Players { get; set; }
    public Board GameBoard { get; set; }
    public int CurrentTurn { get; set; }

    public delegate void SpecialEventHandler(string message);
    public event SpecialEventHandler OnSpecialEvent;

    public Game(Board board)
    {
        GameBoard = board;
        Players = new List<IPlayer>();
        CurrentTurn = 0;
    }
    
    public void StartGame()
    {
        while (CurrentTurn < 10) 
        {
            PlayTurn();
            CurrentTurn++;
        }
    }


    public void PlayTurn()
    {
        var player = Players[CurrentTurn % Players.Count];
        Random rand = new Random();
        int steps = rand.Next(1, 7);
        player.Move(steps);
        
        int reward = GameBoard.GetReward(player.Position);
        if (reward > 0)
        {
            player.UpdateScore(reward);
            OnSpecialEvent?.Invoke($"{player.Name} zdobył {reward} punktów!");
        }
    
        if (player.Position % 5 == 0) 
        {
            OnSpecialEvent?.Invoke($"{player.Name} trafił na specjalne pole!");
        }
    }
    
    public void DisplayResults()
    {
        foreach (var player in Players)
        {
            Console.WriteLine($"{player.Name} - Pozycja: {player.Position}, Wynik: {player.Score}");
        }
    }
}
