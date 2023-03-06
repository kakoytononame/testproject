namespace testproject.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        
        public List<Game>? FirstPlayers { get; set; }

        public List<Game>? SecondPlayers { get; set; }
    }
}
