namespace TodoListApp.Data.Entities;

public class TagEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<TodoTaskEntity> Tasks { get; } = new List<TodoTaskEntity>();
}
