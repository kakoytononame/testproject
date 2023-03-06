namespace testproject.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public int FirstPlayerid {  get; set; }
        public User FirstPlayer {  get; set; }
        public int? SecondPlayerid { get; set;}
        public User? SecondPlayer { get; set; }

        public string Gameboard {get; set;} = string.Empty;

        public string GameStatus { get; set;}
        
    }
}
