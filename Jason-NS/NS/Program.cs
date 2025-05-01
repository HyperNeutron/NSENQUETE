using NS.Helpers;
using NS.Modules;

namespace NS;

class Program
{
    public static String? StationName;
    
    static void Main()
    {
        //new InlogModule().Main(); <- Work in Progress (Moet nog gemaakt worden)

        if (StationName == null)
        {
            ChooseStation();
        }

        while (true)
        {
            Console.Clear();

            TextHelper.WriteStationLine();

            Console.WriteLine("Welk programma wil je opstarten?");
            Console.WriteLine("1. Gebruiker: feedbacksysteem");
            Console.WriteLine("2. Administratief: feedback beoordelen\n");
            Console.WriteLine("Kies een nummer:");

            int programChoice = int.Parse(Console.ReadLine());

            Console.Clear();
            if (programChoice == 1)
            {
                while (true)
                {
                    new UserModule().Main();
                }
            }
            else if (programChoice == 2)
            {
                while (true)
                {
                    AdministrationModule.Launch();
                }
            }
            else
            {
                Console.WriteLine("Voer een geldig nummer in!\n");
            }

        }
    }


    private static void ChooseStation()
    {
        Console.WriteLine("Welkom bij het NS programma!\n" +
                          "In welk station bevinden wij ons?");
        
        StationName = Console.ReadLine();
        Console.WriteLine("\nWelkom in " + StationName + "!\n");
    }
}
