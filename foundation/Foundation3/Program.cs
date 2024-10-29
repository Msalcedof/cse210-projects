using System;
using System.Collections.Generic;

// Base class representing a general activity
public abstract class Activity
{
    private DateTime _date;
    private int _duration; // Duration in minutes

    protected Activity(DateTime date, int duration)
    {
        _date = date;
        _duration = duration;
    }

    // Protected method to access duration
    protected int GetDuration()
    {
        return _duration; 
    }

    // Abstract method to calculate distance
    protected abstract double CalculateDistance(); 

    // Abstract methods for speed and pace
    public abstract double GetSpeed(); 
    public abstract double GetPace(); 

    // Virtual method to summarize activity details
    public virtual string GetSummary()
    {
        return $"{_date:dd MMM yyyy} - Duration: {_duration} min, ";
    }

    // Public method to get distance
    public double GetDistance()
    {
        return CalculateDistance();
    }
}

// Derived class for Running activity
public class Running : Activity
{
    private double _distance; // Distance in miles

    public Running(DateTime date, int duration, double distance) 
        : base(date, duration)
    {
        _distance = distance;
    }

    protected override double CalculateDistance()
    {
        return _distance; // Return the distance directly
    }

    public override double GetSpeed()
    {
        return (GetDistance() > 0) ? (GetDistance() / GetDuration()) * 60 : 0; // Speed in mph
    }

    public override double GetPace()
    {
        return (GetDistance() > 0) ? GetDuration() / GetDistance() : 0; // Pace in min per mile
    }

    public override string GetSummary()
    {
        return base.GetSummary() +
               $"Running - Distance: {GetDistance():F1} miles, Speed: {GetSpeed():F1} mph, Pace: {GetPace():F2} min/mile";
    }
}

// Derived class for Cycling activity
public class Cycling : Activity
{
    private double _speed; // Speed in mph

    public Cycling(DateTime date, int duration, double speed)
        : base(date, duration)
    {
        _speed = speed;
    }

    protected override double CalculateDistance()
    {
        return (_speed / 60) * GetDuration(); // Distance in miles
    }

    public override double GetSpeed()
    {
        return _speed; // Speed in mph
    }

    public override double GetPace()
    {
        return (_speed > 0) ? 60 / _speed : 0; // Pace in min per mile
    }

    public override string GetSummary()
    {
        return base.GetSummary() +
               $"Cycling - Distance: {CalculateDistance():F1} miles, Speed: {GetSpeed()} mph, Pace: {GetPace():F2} min/mile";
    }
}

// Derived class for Swimming activity
public class Swimming : Activity
{
    private int _laps; // Number of laps

    public Swimming(DateTime date, int duration, int laps)
        : base(date, duration)
    {
        _laps = laps;
    }

    protected override double CalculateDistance()
    {
        return (_laps * 50 / 1000.0) * 0.62; // Distance in miles
    }

    public override double GetSpeed()
    {
        double distance = CalculateDistance();
        return (distance > 0) ? (distance / GetDuration()) * 60 : 0; // Speed in mph
    }

    public override double GetPace()
    {
        double distance = CalculateDistance();
        return (distance > 0) ? GetDuration() / distance : 0; // Pace in min per mile
    }

    public override string GetSummary()
    {
        return base.GetSummary() +
               $"Swimming - Distance: {CalculateDistance():F1} miles, Speed: {GetSpeed():F1} mph, Pace: {GetPace():F2} min/mile";
    }
}

// Main program
public class Program
{
    public static void Main()
    {
        List<Activity> activities = new List<Activity>
        {
            new Running(new DateTime(2024, 10, 29), 30, 3.0),
            new Cycling(new DateTime(2024, 11, 2), 45, 12.0),
            new Swimming(new DateTime(2024, 11, 15), 30, 20)
        };

        foreach (var activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}
