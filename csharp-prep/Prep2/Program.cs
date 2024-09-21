using System;
using System.Formats.Asn1;
using System.Globalization;
using System.Threading.Tasks.Dataflow;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("please type your grade percentage: ");
        string grade = Console.ReadLine();
        int user = int.Parse(grade);

        string letter = "";

        if (user >= 90)
        {
            letter = "A";
        }

        else if (user >= 80)
        {
            letter = "B";

        }

        else if (user >= 70)
        {
            letter = "C";      
        }

        else if (user >= 60)
        {
            letter = "D";
        }
        else
        {
            letter = "f";

        }

        Console.WriteLine($"your grade is: {letter} ");

        if (user > 70)
        {
            Console.WriteLine("Congratulations you passed.!!");
        }

        else
        {
            Console.WriteLine("Maybe Next Time. Keep it going, You can Do it!");
        }


    }
}