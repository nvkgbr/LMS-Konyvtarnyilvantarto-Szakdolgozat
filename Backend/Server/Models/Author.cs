namespace Server.Models
{
    public class Author
    {
        int id;
        string name;

        public int Id { get => id; set => id = value; }

        public string Name { get => name; set => name = value; }

        public Author() { }

        public Author(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
