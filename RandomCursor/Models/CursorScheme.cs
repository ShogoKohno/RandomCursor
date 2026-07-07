namespace RandomCursor.Models;

public class CursorScheme
{
    public string Name { get; }

    public string[] Cursors { get; }

    public CursorScheme(
        string name,
        string[] cursors)
    {
        Name = name;
        Cursors = cursors;
    }
}