using Microsoft.EntityFrameworkCore;

public class DB : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }

    public string DbPath { get; }

    public DB()
    {
        var folder = @"C:\Users\pauli\source\repos\EFBlogging";

        DbPath = Path.Combine(folder, "efblogging.db");

        Directory.CreateDirectory(folder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class Blog
{
    public int BlogId { get; set; }
    public string? Url { get; set; }
    public string? Name { get; set; }
    public List<Post> Posts { get; } = new List<Post>();
}

public class Post
{
    public int PostId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime PublishedOn { get; set; }
    public int BlogId { get; set; }
    public Blog? Blog { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
}

public class User
{
    public int UserId { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public List<Post> Posts { get; } = new List<Post>();
}