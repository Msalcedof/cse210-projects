using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace MindfulnessApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MindfulnessApp app = new MindfulnessApp();
            app.Run();
        }
    }

    public class MindfulnessApp
    {
        private List<Activity> _activities; // Private list of activities
        private Dictionary<string, int> _activityLog; // Private activity log

        public MindfulnessApp()
        {
            _activities = new List<Activity>
            {
                new BreathingActivity(),
                new ReflectionActivity(),
                new ListingActivity(),
                new GratitudeActivity() // New Gratitude Activity added
            };

            _activityLog = new Dictionary<string, int>();
            foreach (var activity in _activities)
            {
                _activityLog[activity.GetType().Name] = 0; // Initialize log counts
            }
        }

        public void Run()
        {
            LoadLog(); // Load log from file if exists

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Mindfulness App!");
                Console.WriteLine("Select an activity:");
                for (int i = 0; i < _activities.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_activities[i].GetType().Name.Replace("Activity", "")} (Performed: {_activityLog[_activities[i].GetType().Name]})");
                }
                Console.WriteLine("0. Exit");

                string input = Console.ReadLine();
                if (input == "0") break;

                if (int.TryParse(input, out int choice) && choice > 0 && choice <= _activities.Count)
                {
                    _activities[choice - 1].Start();
                    _activityLog[_activities[choice - 1].GetType().Name]++; // Increment log count
                }
            }

            SaveLog(); // Save log to file on exit
        }

        private void LoadLog()
        {
            if (File.Exists("activityLog.txt"))
            {
                var lines = File.ReadAllLines("activityLog.txt");
                foreach (var line in lines)
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int count))
                    {
                        _activityLog[parts[0]] = count; // Load counts from file
                    }
                }
            }
        }

        private void SaveLog()
        {
            using (StreamWriter writer = new StreamWriter("activityLog.txt"))
            {
                foreach (var entry in _activityLog)
                {
                    writer.WriteLine($"{entry.Key}:{entry.Value}"); // Save counts to file
                }
            }
        }
    }

    public abstract class Activity
    {
        protected string _name; // Protected name variable
        protected string _description; // Protected description variable
        protected int _duration; // Protected duration variable

        public void Start()
        {
            Console.Clear();
            Console.WriteLine($"Starting {_name}");
            Console.WriteLine(_description);
            Console.Write("Enter duration in seconds: ");
            _duration = int.Parse(Console.ReadLine());
            PrepareToBegin();
            PerformActivity();
            Finish();
        }

        protected abstract void PerformActivity();

        protected void PrepareToBegin()
        {
            Console.WriteLine("Get ready to begin...");
            Pause(3);
        }

        protected void Finish()
        {
            Console.WriteLine("Good job! You've completed the activity.");
            Pause(3);
        }

        protected void Pause(int seconds)
        {
            for (int i = 0; i < seconds; i++)
            {
                Console.Write(".");
                Thread.Sleep(1000);
            }
            Console.WriteLine();
        }
    }

    public class BreathingActivity : Activity
    {
        public BreathingActivity()
        {
            _name = "Breathing Activity"; // Initialize name
            _description = "This activity will help you relax by guiding you through breathing in and out slowly. Clear your mind and focus on your breathing."; // Initialize description
        }

        protected override void PerformActivity()
        {
            int elapsed = 0;
            while (elapsed < _duration)
            {
                Console.WriteLine("Breathe in...");
                BreathingAnimation(4); // Breathing animation during inhale
                Console.WriteLine("Breathe out...");
                BreathingAnimation(4); // Breathing animation during exhale
                elapsed += 8; // 4 seconds each for breathe in and out
            }
        }

        private void BreathingAnimation(int duration)
        {
            for (int i = 0; i < duration; i++)
            {
                Console.Write(new string(' ', i) + "O" + new string(' ', duration - i - 1) + "\r");
                Thread.Sleep(1000);
            }
            Console.WriteLine(); // Move to the next line after animation
        }
    }

    public class ReflectionActivity : Activity
    {
        private static List<string> _prompts = new List<string>
        {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        };

        private List<string> _usedPrompts = new List<string>(); // Track used prompts

        public ReflectionActivity()
        {
            _name = "Reflection Activity"; // Initialize name
            _description = "This activity will help you reflect on times in your life when you have shown strength and resilience."; // Initialize description
        }

        protected override void PerformActivity()
        {
            if (_usedPrompts.Count == _prompts.Count)
            {
                _usedPrompts.Clear(); // Reset used prompts if all have been used
            }

            Random random = new Random();
            string prompt;

            do
            {
                prompt = _prompts[random.Next(_prompts.Count)];
            } while (_usedPrompts.Contains(prompt));

            _usedPrompts.Add(prompt); // Add prompt to used list
            Console.WriteLine(prompt);
            Pause(5);

            int elapsed = 0;
            while (elapsed < _duration)
            {
                string question = _questions[random.Next(_questions.Count)];
                Console.WriteLine(question);
                Pause(5);
                elapsed += 5;
            }
        }

        private static List<string> _questions = new List<string>
        {
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "How did you feel when it was complete?",
            "What made this time different than other times when you were not as successful?",
            "What is your favorite thing about this experience?",
            "What could you learn from this experience that applies to other situations?",
            "What did you learn about yourself through this experience?",
            "How can you keep this experience in mind in the future?"
        };
    }

    public class ListingActivity : Activity
    {
        private static List<string> _prompts = new List<string>
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "When have you felt the Holy Ghost this month?",
            "Who are some of your personal heroes?"
        };

        public ListingActivity()
        {
            _name = "Listing Activity"; // Initialize name
            _description = "This activity will help you reflect on the good things in your life by having you list as many things as you can."; // Initialize description
        }

        protected override void PerformActivity()
        {
            Random random = new Random();
            string prompt = _prompts[random.Next(_prompts.Count)];
            Console.WriteLine(prompt);
            Pause(5);

            Console.WriteLine("Start listing items. Press Enter after each item. You have " + _duration + " seconds.");
            List<string> items = new List<string>();
            DateTime endTime = DateTime.Now.AddSeconds(_duration);
            while (DateTime.Now < endTime)
            {
                string item = Console.ReadLine();
                if (!string.IsNullOrEmpty(item))
                {
                    items.Add(item);
                }
            }

            Console.WriteLine($"You listed {items.Count} items.");
        }
    }

    public class GratitudeActivity : Activity // New Gratitude Activity class
    {
        private static List<string> _prompts = new List<string>
        {
            "What is something you are grateful for today?",
            "Who in your life do you appreciate?",
            "What small things bring you joy?",
            "What is a recent experience that made you smile?"
        };

        private List<string> _usedPrompts = new List<string>(); // Track used prompts

        public GratitudeActivity()
        {
            _name = "Gratitude Activity"; // Initialize name
            _description = "This activity will help you think about the things you are grateful for."; // Initialize description
        }

        protected override void PerformActivity()
        {
            if (_usedPrompts.Count == _prompts.Count)
            {
                _usedPrompts.Clear(); // Reset used prompts if all have been used
            }

            Random random = new Random();
            string prompt;

            do
            {
                prompt = _prompts[random.Next(_prompts.Count)];
            } while (_usedPrompts.Contains(prompt));

            _usedPrompts.Add(prompt); // Add prompt to used list
            Console.WriteLine(prompt);
            Pause(5);

            Console.WriteLine("Think about your gratitude item. You have " + _duration + " seconds.");
            Pause(_duration);
        }
    }
}
