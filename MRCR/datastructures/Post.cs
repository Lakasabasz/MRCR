namespace MRCR.datastructures;

public enum PostType
{
    Post, Depot, Combined
}
public class Post
{
    public int ID {get; set; }
    public string Name {get; set; }
    public PostType Type {get; set; }
}