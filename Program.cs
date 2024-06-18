using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Program
{
    public void Run()
    {

        using (var db = new DB())
        {
            Console.WriteLine($"SQLite DB Located at: {db.DbPath}");
            ResetDatabase(db);
            ImportCsv(db, "users.csv", "posts.csv", "blogs.csv");
            DisplayTree(db);
        }
    }
    public void ResetDatabase(DB db)
    {
        db.Database.ExecuteSqlRaw("DELETE FROM Posts");
        db.Database.ExecuteSqlRaw("DELETE FROM Blogs");
        db.Database.ExecuteSqlRaw("DELETE FROM Users");
    }

    public void ImportCsv(DB db, params string[] csvFiles)
    {
        foreach (var csvFile in csvFiles)
        {
            string[] lines = File.ReadAllLines(csvFile);

            if (csvFile.Contains("user"))
            {
                foreach (var line in lines.Skip(1))
                {
                    var data = line.Split(',');
                    var user = new User
                    {
                        UserId = int.Parse(data[0]),
                        Username = data[1],
                        Password = data[2],
                    };

                    db.Users.Add(user);
                }
            }
            else if (csvFile.Contains("post"))
            {
                foreach (var line in lines.Skip(1))
                {
                    var data = line.Split(',');
                    var post = new Post
                    {
                        PostId = int.Parse(data[0]),
                        Title = data[1],
                        Content = data[2],
                        PublishedOn = DateTime.Parse(data[3]),
                        BlogId = int.Parse(data[4]),
                        UserId = int.Parse(data[5])
                    };

                    db.Posts.Add(post);
                }
            }
            else if (csvFile.Contains("blog"))
            {
                foreach (var line in lines.Skip(1))
                {
                    var data = line.Split(',');
                    var blog = new Blog
                    {
                        BlogId = int.Parse(data[0]),
                        Url = data[1],
                        Name = data[2],
                    };

                    db.Blogs.Add(blog);
                }
            }
        }

        db.SaveChanges();
    }

    public void DisplayTree(DB db)
    {
        var users = db.Users
            .Include(u => u.Posts)
                .ThenInclude(p => p.Blog)
            .ToList();

        foreach (var user in users)
        {
            Console.WriteLine($"User: {user.Username}");

            foreach (var post in user.Posts)
            {
                Console.WriteLine($"  Post: {post.Title}");

                if (post.Blog != null)
                    Console.WriteLine($"    Blog: {post.Blog.Name}");
            }
        }
    }

    public static void Main()
    {
        new Program().Run();
    }
}