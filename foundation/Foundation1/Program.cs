using System;
using System.Collections.Generic;

class Comment
{
    public string CommenterName { get; }
    public string Text { get; }

    public Comment(string commenterName, string text)
    {
        CommenterName = commenterName;
        Text = text;
    }

    public override string ToString()
    {
        return $"{CommenterName}: {Text}";
    }
}

class Video
{
    public string Title { get; }
    public string Author { get; }
    public int Length { get; } // Length in seconds
    private List<Comment> _comments; // Using underscore for member variable

    public Video(string title, string author, int length)
    {
        Title = title;
        Author = author;
        Length = length;
        _comments = new List<Comment>(); // Initialize the comments list
    }

    public void AddComment(string commenterName, string text)
    {
        var newComment = new Comment(commenterName, text);
        _comments.Add(newComment);
    }

    public int GetNumberOfComments() // Method to return the number of comments
    {
        return _comments.Count;
    }

    public override string ToString()
    {
        string commentsList = string.Join("\n", _comments);
        return $"Title: {Title}\n" +
               $"Author: {Author}\n" +
               $"Length: {Length} seconds\n" +
               $"Number of Comments: {GetNumberOfComments()}\n" +
               $"Comments:\n{commentsList}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create a list of videos
        List<Video> videos = new List<Video>();

        // Create Video instances and add comments
        Video video1 = new Video("Understanding Abstraction", "Alice", 300);
        video1.AddComment("Bob", "Great explanation!");
        video1.AddComment("Charlie", "Very helpful, thanks!");
        video1.AddComment("David", "I learned a lot.");

        Video video2 = new Video("Python OOP Basics", "Eve", 450);
        video2.AddComment("Frank", "I love this tutorial!");
        video2.AddComment("Grace", "Can you make more videos like this?");
        video2.AddComment("Heidi", "Clear and concise.");

        Video video3 = new Video("Advanced Python Features", "Ivan", 600);
        video3.AddComment("Judy", "This is a game changer!");
        video3.AddComment("Mallory", "I had no idea about these features.");
        video3.AddComment("Niaj", "Excellent content!");

        // Add videos to the list
        videos.Add(video1);
        videos.Add(video2);
        videos.Add(video3);

        // Display information about each video
        foreach (var video in videos)
        {
            Console.WriteLine(video);
            Console.WriteLine(); // Print a blank line for better readability
        }
    }
}


