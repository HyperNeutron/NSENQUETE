using Administration.Helpers;
using NS.Helpers;
using NS.Modules;

namespace NS;

class Program
{
    public static String? StationName;

    static void Main()
    {
        while (true)
        {
            TextHelper.WriteLine();
            if (StationName == null)
            {
                ChooseStation();
            }

            AdministrationModule.Launch();
            Console.Clear();
        }

    }

    private static void ChooseStation()
    {
        Console.WriteLine("Welkom bij het NS administratie programma!\n" +
                          "In welk station bevinden wij ons?");

        StationName = Console.ReadLine();
    }
}

