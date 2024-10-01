using System;

class Program
{
    static void Main(string[] args)
    {
        Job job1 =  new Job();
        {

            job1. _jobTittle = "software Engeneer";
            job1. _company = "Microsoft";
            job1. _startYear = 2019;
            job1. _endYear = 2022;
        }

        Job job2 = new Job();
        {

            job2. _jobTittle = "Manager";
            job2. _company = "Apple";
            job2. _startYear = 2022;
            job2. _endYear = 2023;
        
        }

        //job1.DisplayJobDetails();
        //job2.DisplayJobDetails();
        
        Resume myResume = new Resume();
        {

            myResume._name = "Alisson Rose";

            myResume._jobs.Add(job1);
            myResume._jobs.Add(job2);

            myResume.Display();

        }

    
    }
    
}
public class Job
{
    public string _jobTittle;
    public string _company;
    public int _startYear;
    public int _endYear;

    public void DisplayJobDetails()
    {
        Console.WriteLine($"{_jobTittle} ({_company}) {_startYear}-{_endYear}");
    }
    
}

public class Resume
{
    public string _name;
    public List<Job> _jobs = new List<Job>();

    public void Display()
    {
        Console.WriteLine($"Name: {_name}");
        Console.WriteLine("Jobs:");

        foreach (Job job in _jobs)
        {
            job.DisplayJobDetails();
        }
    }

}

