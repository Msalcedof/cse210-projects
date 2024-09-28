using System;
using System.Collections.Generic;
using System.IO;

namespace JournalApp

{
    // Class to represent a journal entry
    public class JournalEntry
    {
        public string Prompt { get; set; }
        public string Response { get; set; }
        public string Date { get; set; }
        public string Mood { get; set; } // Added mood field

        public JournalEntry(string prompt, string response, string mood)
        {
            Prompt = prompt;
            Response = response;
            Date = DateTime.Now.ToString("yyyy-MM-dd");
            Mood = mood;
        }

        public override string ToString()
        {
            return $"{Date} | {Prompt} | {Response} | {Mood}";
        }

        // Converts the entry to a CSV-friendly format
        public string ToCsv()
        {
            return $"\"{Date}\",\"{Prompt}\",\"{Response}\",\"{Mood}\"";
        }
    }

    // Class to manage journal entries
    public class Journal
    {
        private List<JournalEntry> entries = new List<JournalEntry>();
        private static Random random = new Random();

        // List of prompts
        private List<string> prompts = new List<string>
        {
            "Who was the most interesting person I interacted with today?",
            "What was the best part of my day?",
            "How did I see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?"
        };

        public void AddEntry(string response, string mood)
        {
            string prompt = prompts[random.Next(prompts.Count)];
            entries.Add(new JournalEntry(prompt, response, mood));
        }

        public void DisplayEntries()
        {
            foreach (var entry in entries)
            {
                Console.WriteLine(entry);
            }
        }

        public void SaveToFile(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                // Write header for CSV
                writer.WriteLine("Date,Prompt,Response,Mood");
                foreach (var entry in entries)
                {
                    writer.WriteLine(entry.ToCsv());
                }
            }
        }

        public void LoadFromFile(string filename)
        {
            entries.Clear();
            using (StreamReader reader = new StreamReader(filename))
            {
                // Read header line
                reader.ReadLine();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        entries.Add(new JournalEntry(parts[1].Trim('"'), parts[2].Trim('"'), parts[3].Trim('"')) { Date = parts[0].Trim('"') });
                    }
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Journal journal = new Journal();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nJournal Menu:");
                Console.WriteLine("1. Write a new entry");
                Console.WriteLine("2. Display journal");
                Console.WriteLine("3. Save journal to file (CSV format)");
                Console.WriteLine("4. Load journal from file");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter your response:");
                        string response = Console.ReadLine();
                        Console.WriteLine("Enter your mood (e.g., Happy, Sad, etc.):");
                        string mood = Console.ReadLine(); // Added mood input
                        journal.AddEntry(response, mood);
                        Console.WriteLine("Entry added.");
                        break;

                    case "2":
                        journal.DisplayEntries();
                        break;

                    case "3":
                        Console.Write("Enter filename to save to (include .csv extension): ");
                        string saveFilename = Console.ReadLine();
                        journal.SaveToFile(saveFilename);
                        Console.WriteLine("Journal saved.");
                        break;

                    case "4":
                        Console.Write("Enter filename to load from (include .csv extension): ");
                        string loadFilename = Console.ReadLine();
                        journal.LoadFromFile(loadFilename);
                        Console.WriteLine("Journal loaded.");
                        break;

                    case "5":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}

