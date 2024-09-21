using System;

class Program
{
    static void Main(string[] args)
    {
       //getting imputs from users// 
       Console.Write("what is your first name? ");
       string name = Console.ReadLine();

       Console.Write("what is your last name? ");
       string lastName = Console.ReadLine();

       Console.WriteLine($"your name is {lastName}, {name} {lastName}.");



        
    }
}