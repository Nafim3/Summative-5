// See https://aka.ms/new-console-template for more information






class Program
{
    static void Main()
    {
        string input;
        do
        {
            int selection = GetMenuSelection("Select an option:\n1. Create a new character.\n2. Load an existing character.", 1, 2);

            string name = string.Empty;
            string creatureType = string.Empty;
            string characterClass = string.Empty;
            int hitPoints = 20;
            int strength = 20;
            int speed = 20;

            if (selection == 1)
            {
                name = GetUserInput("What is your character's name?");
                creatureType = GetCreatureType();
                characterClass = GetCharacterClass();
                AllocateBonusPoints(ref hitPoints, ref strength, ref speed);
            }
            else if (selection == 2)
            {
                LoadCharacterFromFile(ref name, ref creatureType, ref characterClass, ref hitPoints, ref strength, ref speed);
            }

            DisplayCharacterDetails(name, creatureType, characterClass, hitPoints, strength, speed);

            input = GetYesOrNoInput("Would you like to save this character to a file? (Y/N)");

            if (input.ToUpper() == "Y")
            {
                SaveCharacterToFile(name, creatureType, characterClass, hitPoints, strength, speed);
            }

            input = GetYesOrNoInput("Would you like to make another character? (Y/N)");

        } while (input.ToUpper() == "Y");
    }

    static int GetMenuSelection(string menu, int minValue, int maxValue)
    {
        int selection;
        do
        {
            Console.WriteLine(menu);
            selection = GetUserInputAsInt();
        } while (selection < minValue || selection > maxValue);

        return selection;
    }

    static string GetUserInput(string prompt)
    {
        Console.WriteLine(prompt);
        return Console.ReadLine();
    }

    static int GetUserInputAsInt()
    {
        int input;
        while (!int.TryParse(Console.ReadLine(), out input))
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
        return input;
    }

    static string GetCreatureType()
    {
        string[] creatureTypes = { "Human", "Monkey", "Lion" };
        int selection = GetMenuSelection("What is your character's creature type?\n1. Human\n2. Monkey\n3. Lion\n4", 1, 3);
        return creatureTypes[selection - 1];
    }

    static string GetCharacterClass()
    {
        string[] characterClasses = { "Warrior", "Archer", "Rogue", "Bomber" };
        int selection = GetMenuSelection("What is your character's class type?\n1. Warrior\n2. Archer\n3. Rogue\n4. Bomber", 1, 4);
        return characterClasses[selection - 1];
    }

    static void AllocateBonusPoints(ref int hitPoints, ref int strength, ref int speed)
    {
        int bonusPointsRemaining = 30;

        hitPoints += AllocateBonusPoints("Hit Points", ref bonusPointsRemaining);
        strength += AllocateBonusPoints("Strength", ref bonusPointsRemaining);
        speed += bonusPointsRemaining; // All remaining points go to speed
    }

    static int AllocateBonusPoints(string stat, ref int bonusPointsRemaining)
    {
        int bonusPointsToAdd;
        do
        {
            Console.WriteLine($"You have {bonusPointsRemaining} bonus points to allocate to your character.");
            Console.WriteLine($"How many points would you like to add to {stat}?");
            bonusPointsToAdd = GetUserInputAsInt();
        } while (bonusPointsToAdd < 0 || bonusPointsToAdd > bonusPointsRemaining);

        bonusPointsRemaining -= bonusPointsToAdd;
        return bonusPointsToAdd;
    }

    static void LoadCharacterFromFile(ref string name, ref string creatureType, ref string characterClass, ref int hitPoints, ref int strength, ref int speed)
    {
        string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.character");

        if (files.Length == 0)
        {
            Console.WriteLine("There are no character files to load.");
            return;
        }

        int selection = GetMenuSelection("Select a character to load:", 1, files.Length);
        string[] lines = File.ReadAllLines(files[selection - 1]);

        name = GetSubstringAfterLastSpace(lines[0]);
        creatureType = GetSubstringAfterLastSpace(lines[1]);
        characterClass = GetSubstringAfterLastSpace(lines[2]);
        string[] stats = lines[3].Split(' ');
        hitPoints = int.Parse(stats[1]);
        strength = int.Parse(stats[2]);
        speed = int.Parse(stats[3]);
    }

    static string GetSubstringAfterLastSpace(string input)
    {
        return input.Substring(input.LastIndexOf(' ') + 1);
    }

    static void DisplayCharacterDetails(string name, string creatureType, string characterClass, int hitPoints, int strength, int speed)
    {
        Console.WriteLine($"You created {name}, the {creatureType} {characterClass}");
        Console.WriteLine($"Hit Points: {hitPoints}");
        Console.WriteLine($"Strength: {strength}");
        Console.WriteLine($"Speed: {speed}");
    }

    static void SaveCharacterToFile(string name, string creatureType, string characterClass, int hitPoints, int strength, int speed)
    {
        using (StreamWriter writer = new StreamWriter($"{name}_{creatureType}_{characterClass}.character"))
        {
            writer.WriteLine($"Name - {name}");
            writer.WriteLine($"Type - {creatureType}");
            writer.WriteLine($"Class - {characterClass}");
            writer.WriteLine($"Stats - {hitPoints} {strength} {speed}");
        }
    }

    static string GetYesOrNoInput(string prompt)
    {
        string input;
        do
        {
            Console.WriteLine(prompt);
            input = Console.ReadLine().ToUpper();
        } while (input != "Y" && input != "N");

        return input;
    }
}
