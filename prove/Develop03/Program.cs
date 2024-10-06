using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScriptureMemorization
{
    public class Word
    {
        public string Text { get; private set; }
        public bool IsHidden { get; private set; }

        public Word(string text)
        {
            Text = text;
            IsHidden = false;
        }

        public void Hide()
        {
            IsHidden = true;
        }

        public override string ToString()
        {
            return IsHidden ? "_____" : Text;
        }
    }

    public class ScriptureReference
    {
        public string Reference { get; private set; }

        public ScriptureReference(string reference)
        {
            Reference = reference;
        }
    }

    public class Scripture
    {
        private List<Word> words;
        public ScriptureReference Reference { get; private set; }

        public Scripture(ScriptureReference reference, string text)
        {
            Reference = reference;
            words = text.Split(' ').Select(w => new Word(w)).ToList();
        }

        public void HideRandomWord()
        {
            var unhiddenWords = words.Where(w => !w.IsHidden).ToList();
            if (unhiddenWords.Count > 0)
            {
                var random = new Random();
                var wordToHide = unhiddenWords[random.Next(unhiddenWords.Count)];
                wordToHide.Hide();
            }
        }

        public bool AllWordsHidden()
        {
            return words.All(w => w.IsHidden);
        }

        public override string ToString()
        {
            return $"{Reference.Reference}: " + string.Join(" ", words);
        }
    }

    public class ScriptureLibrary
    {
        private List<Scripture> scriptures;
        private Random random;

        public ScriptureLibrary()
        {
            scriptures = new List<Scripture>();
            random = new Random();
        }

        public void LoadFromFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 2)
                {
                    var reference = new ScriptureReference(parts[0].Trim());
                    var text = parts[1].Trim();
                    scriptures.Add(new Scripture(reference, text));
                }
            }
        }

        public Scripture GetRandomScripture()
        {
            if (scriptures.Count == 0) return null;
            return scriptures[random.Next(scriptures.Count)];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var scriptureLibrary = new ScriptureLibrary();
            scriptureLibrary.LoadFromFile("scriptures.txt");

            var scripture = scriptureLibrary.GetRandomScripture();
            if (scripture == null)
            {
                Console.WriteLine("No scriptures available.");
                return;
            }

            Console.Clear();
            Console.WriteLine(scripture);
            Console.WriteLine("Press Enter to hide words or type 'quit' to exit.");

            while (true)
            {
                var input = Console.ReadLine();
                if (input?.ToLower() == "quit")
                {
                    break;
                }

                if (!scripture.AllWordsHidden())
                {
                    scripture.HideRandomWord();
                    Console.Clear();
                    Console.WriteLine(scripture);
                    Console.WriteLine("Press Enter to hide more words or type 'quit' to exit.");
                }
                else
                {
                    Console.WriteLine("All words are now hidden!");
                    break;
                }
            }
        }
    }
}

/* 
Creative Additions:
1. **ScriptureLibrary Class**: This class manages a collection of scriptures, allowing for more than one scripture to be used in the program. This adds versatility to the memorization process.

2. **File Loading**: The program now reads scriptures from a file (`scriptures.txt`), making it easy to add or modify scriptures without changing the code. This enhances user experience and allows for a larger library of scriptures.

3. **Random Scripture Selection**: The program selects a random scripture each time it runs, providing variety and keeping the memorization process engaging.

These enhancements not only meet the core requirements but also introduce elements that make the program more interactive and user-friendly. 
*/
