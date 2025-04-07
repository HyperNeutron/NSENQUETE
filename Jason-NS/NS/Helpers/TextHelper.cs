namespace NS.Helpers;

public class TextHelper
{
    public static void WriteStationLine()
    {
        int consoleWidth = Console.WindowWidth;
        string stationName = Program.StationName;
        string fixedEquals = "===";
        string textToDisplay = fixedEquals + " " + stationName + " ";
        int remainingEqualsCount = consoleWidth - textToDisplay.Length;
        
        remainingEqualsCount = Math.Max(remainingEqualsCount, 0);
        string fullLine = textToDisplay + new string('=', remainingEqualsCount);
        Console.WriteLine(fullLine);
    }

    public static void WriteLine()
    {
        int consoleWidth = Console.WindowWidth;
        string fullLine = new string('=', consoleWidth);
        Console.WriteLine(fullLine);
    }
}