namespace ListSentence.Modols;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string password { get; set; }
    public List<int> SetenceIds { get; set; }
}