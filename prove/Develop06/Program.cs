using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// Abstract base class for all goals
public abstract class Goal
{
    public string Title { get; private set; }
    protected int _points; // Private member variable for points
    public int Points => _points; // Public property to access points

    protected Goal(string title)
    {
        Title = title;
    }

    public abstract void RecordEvent(); // Abstract method for recording an event
    public abstract bool IsComplete { get; } // Abstract property to check completion status
    public abstract string DisplayStatus(); // Abstract method to display goal status
}

// Simple goal class: A one-time goal
public class SimpleGoal : Goal
{
    private bool _isCompleted; // Tracks if the goal is completed

    public SimpleGoal(string title, int points) : base(title)
    {
        _points = points;
        _isCompleted = false; // Initially not completed
    }

    public override void RecordEvent()
    {
        _isCompleted = true; // Mark the goal as completed
    }

    public override bool IsComplete => _isCompleted; // Completion check

    public override string DisplayStatus()
    {
        return IsComplete ? $"[X] {Title} (Earned {_points} points)" : $"[ ] {Title}"; // Display completion status
    }
}

// Eternal goal class: A goal that never completes
public class EternalGoal : Goal
{
    public EternalGoal(string title, int points) : base(title)
    {
        _points = points; // Points earned each time recorded
    }

    public override void RecordEvent()
    {
        _points += 100; // Increase points each time recorded
    }

    public override bool IsComplete => false; // Eternal goals are never complete.

    public override string DisplayStatus()
    {
        return $"[ ] {Title} (Earned {_points} points each time)"; // Show that it’s ongoing
    }
}

// Checklist goal class: Requires multiple completions
public class ChecklistGoal : Goal
{
    private int _currentCount; // Current count of completions
    private int _targetCount; // Target number of completions
    private int _bonusPoints; // Bonus points for completing the target

    internal int TargetCount => _targetCount; // Internal property for serialization
    internal int BonusPoints => _bonusPoints; // Internal property for serialization

    public ChecklistGoal(string title, int targetCount, int points, int bonusPoints) : base(title)
    {
        _targetCount = targetCount; // Initialize target count
        _points = points;
        _bonusPoints = bonusPoints; // Bonus awarded on target completion
        _currentCount = 0; // Initialize current count
    }

    public override void RecordEvent()
    {
        if (_currentCount < _targetCount)
        {
            _currentCount++; // Increment completion count
            if (_currentCount == _targetCount)
            {
                _points += _bonusPoints; // Add bonus points when target is reached
            }
        }
    }

    public override bool IsComplete => _currentCount >= _targetCount; // Check if goal is complete

    public override string DisplayStatus()
    {
        return IsComplete 
            ? $"[X] {Title} (Completed {_currentCount}/{_targetCount}, Earned {_points} points)" 
            : $"[ ] {Title} (Completed {_currentCount}/{_targetCount})"; // Display status
    }
}

// User class: Manages user goals and points
public class User
{
    public string Name { get; private set; }
    public int TotalPoints { get; private set; } // Total points accumulated
    private List<Goal> _goals; // List of user goals

    public User(string name)
    {
        Name = name;
        _goals = new List<Goal>();
        TotalPoints = 0; // Initialize points
    }

    public void AddGoal(Goal goal)
    {
        _goals.Add(goal); // Add new goal
    }

    public void RecordGoalEvent(Goal goal)
    {
        goal.RecordEvent(); // Record the goal event
        TotalPoints += goal.Points; // Update total points
    }

    public void DisplayGoals()
    {
        Console.WriteLine($"{Name}'s Goals:");
        foreach (var goal in _goals)
        {
            Console.WriteLine(goal.DisplayStatus()); // Display all goals
        }
        Console.WriteLine($"Total Points: {TotalPoints}"); // Show total points
    }

    // Save goals and points to a JSON file
    public void SaveProgress(string filename)
    {
        var json = JsonSerializer.Serialize(_goals); // Serialize goals to JSON
        File.WriteAllText(filename, json); // Write to file
    }

    // Load goals and points from a JSON file
    public void LoadProgress(string filename)
    {
        if (File.Exists(filename))
        {
            var json = File.ReadAllText(filename); // Read from file
            _goals = JsonSerializer.Deserialize<List<Goal>>(json, new JsonSerializerOptions { Converters = { new GoalConverter() } })!; // Deserialize goals
        }
    }

    // Method to access goals
    public List<Goal> GetGoals()
    {
        return _goals; // Allow access to the goals
    }
}

// Custom converter for Goal type for JSON serialization/deserialization
public class GoalConverter : System.Text.Json.Serialization.JsonConverter<Goal>
{
    public override Goal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var goal = JsonSerializer.Deserialize<Dictionary<string, object>>(ref reader, options);
        var type = goal["GoalType"].ToString();
        
        return type switch
        {
            "SimpleGoal" => new SimpleGoal(goal["Title"].ToString(), Convert.ToInt32(goal["Points"])),
            "EternalGoal" => new EternalGoal(goal["Title"].ToString(), Convert.ToInt32(goal["Points"])),
            "ChecklistGoal" => new ChecklistGoal(goal["Title"].ToString(),
                                                  Convert.ToInt32(goal["TargetCount"]),
                                                  Convert.ToInt32(goal["Points"]),
                                                  Convert.ToInt32(goal["BonusPoints"])),
            _ => throw new NotImplementedException()
        };
    }

    public override void Write(Utf8JsonWriter writer, Goal value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("GoalType", value.GetType().Name);
        writer.WriteString("Title", value.Title);
        writer.WriteNumber("Points", value.Points);

        // Include additional properties for checklist goals
        if (value is ChecklistGoal checklistGoal)
        {
            writer.WriteNumber("TargetCount", checklistGoal.TargetCount); // Accessing internal property
            writer.WriteNumber("BonusPoints", checklistGoal.BonusPoints); // Accessing internal property
        }

        writer.WriteEndObject();
    }
}

class Program
{
    static void Main()
    {
        var user = new User("Hero");

        while (true) // Loop for user interaction
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Add Simple Goal");
            Console.WriteLine("2. Add Eternal Goal");
            Console.WriteLine("3. Add Checklist Goal");
            Console.WriteLine("4. Record Goal Event");
            Console.WriteLine("5. Display Goals");
            Console.WriteLine("6. Save Progress");
            Console.WriteLine("7. Load Progress");
            Console.WriteLine("8. Exit");
            Console.Write("Enter your choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter title for Simple Goal: ");
                    var simpleTitle = Console.ReadLine();
                    user.AddGoal(new SimpleGoal(simpleTitle, 1000));
                    break;

                case "2":
                    Console.Write("Enter title for Eternal Goal: ");
                    var eternalTitle = Console.ReadLine();
                    user.AddGoal(new EternalGoal(eternalTitle, 100));
                    break;

                case "3":
                    Console.Write("Enter title for Checklist Goal: ");
                    var checklistTitle = Console.ReadLine();
                    Console.Write("Enter target count: ");
                    var targetCount = int.Parse(Console.ReadLine());
                    user.AddGoal(new ChecklistGoal(checklistTitle, targetCount, 50, 500));
                    break;

                case "4":
                    Console.WriteLine("Select a goal to record an event:");
                    for (int i = 0; i < user.GetGoals().Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {user.GetGoals()[i].DisplayStatus()}");
                    }
                    var eventChoice = int.Parse(Console.ReadLine()) - 1;
                    if (eventChoice >= 0 && eventChoice < user.GetGoals().Count)
                    {
                        user.RecordGoalEvent(user.GetGoals()[eventChoice]);
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice.");
                    }
                    break;

                case "5":
                    user.DisplayGoals();
                    break;

                case "6":
                    user.SaveProgress("user_goals.json");
                    Console.WriteLine("Progress saved.");
                    break;

                case "7":
                    user.LoadProgress("user_goals.json");
                    Console.WriteLine("Progress loaded.");
                    break;

                case "8":
                    return; // Exit the program

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine(); // Blank line for better readability
        }
    }
}
