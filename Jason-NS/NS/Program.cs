using NS.Helpers;
using NS.Modules;

namespace NS;

class Program
{
    public static String? StationName;
    
    static void Main()
    {
        new InlogModule().Main();
        
        while (true)
        {
            TextHelper.WriteLine();
            if (StationName == null)
            {
                ChooseStation();
            }
        
            new UserModule().Main();
            Console.Clear();
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
