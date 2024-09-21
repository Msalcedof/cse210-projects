using System;

class Program
{
    static void Main(string[] args)
    {
       Console.Write("what is your first name? ");
       string name = Console.ReadLine();
       
       Console.Write("what is your last name? ");
       string last_name = Console.ReadLine();

       Console.WriteLine($"your name is {last_name}, {name} {last_name}.");



        
    }
}