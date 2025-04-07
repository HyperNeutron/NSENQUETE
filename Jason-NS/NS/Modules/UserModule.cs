using NS.Helpers;

namespace NS.Modules;

public class UserModule
{
    private Boolean HasConsent;
    private string? UserName;
    private string? SmallStory;
    private string? FeedBack;
    
    public void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }
    
    public async Task MainAsync()
    {
        AskForConsent();
        Console.Clear();

        if (!HasConsent)
        {
            TextHelper.WriteStationLine();
            Console.WriteLine("Helaas kunnen wij niet verder zonder uw toestemming.");
            
            await Task.Delay(3000); // 3 secondes
            return;
        }

        AskForUsername();
        AskForSmallStory();
        AskForFeedback();
        
        Console.Clear();
        TextHelper.WriteStationLine();
        Console.WriteLine("Bedankt voor uw input " + UserName + "!");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Uw verhaal: " + SmallStory);
        Console.WriteLine("Uw feedback: " + FeedBack);
        Console.ResetColor();
        await Task.Delay(3000); // 3 secondes
    }

    private void AskForConsent()
    {
        Console.Clear();
        TextHelper.WriteStationLine();
        Console.WriteLine("Welkom bij het NS programma!\n" +
                          "Hebben wij uw toestemming om deze gegevens te gebruiken? (ja/nee)");
        HasConsent = GetIfHasConsent(Console.ReadLine());
    }

    private void AskForUsername()
    {
        Console.Clear();
        TextHelper.WriteStationLine();
        Console.WriteLine("Wat is uw naam?");
        UserName = GetUserName(Console.ReadLine());
    }
    
    private void AskForSmallStory()
    {
        while (true)
        {
            Console.Clear();
            TextHelper.WriteStationLine();
            if (!string.IsNullOrEmpty(SmallStory))
            {
                Console.WriteLine("Je verhaal is te lang!");
                Console.WriteLine("Je huidige verhaal:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(SmallStory);
                Console.ResetColor();
                Console.WriteLine("Pas je verhaal aan (maximaal 100 tekens):");
            }
            else
            {
                Console.WriteLine("Schrijf een kort verhaal over dit station (maximaal 100 tekens):");
            }
            SmallStory = Console.ReadLine();

            if (!CheckLenght(SmallStory, 100))
            {
                break;
            }
        }
        
    }

    private void AskForFeedback()
    {
        while (true)
        {
            Console.Clear();
            TextHelper.WriteStationLine();
            if (!string.IsNullOrEmpty(FeedBack))
            {
                Console.WriteLine("Je feedback is te lang!");
                Console.WriteLine("Je huidige feedback:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(FeedBack);
                Console.ResetColor();
                Console.WriteLine("Pas je feedback aan (maximaal 500 tekens):");
            }
            else
            {
                Console.WriteLine("Heeft u feedback voor ons? (maximaal 500 tekens):");
            }
            FeedBack = Console.ReadLine();

            if (!CheckLenght(FeedBack, 100))
            {
                break;
            }
        }
    }

    /// <summary>
    /// Check if the given input is 'ja'.
    /// </summary>
    /// <param name="input">The input of the user</param>
    /// <returns>true if the input is 'ja'</returns>
    private bool GetIfHasConsent(string? input)
    {
        return string.IsNullOrEmpty(input) ? false : string.Equals(input.ToLower(), "ja");
    }

    /// <summary>
    /// Check if the given username is null or empty.
    /// </summary>
    /// <param name="input">The input of the user</param>
    /// <returns>The input or 'Anoniem'</returns>
    private string GetUserName(string? input)
    {
        return string.IsNullOrEmpty(input) ? "Anoniem" : input;
    }
    
    ///<summary>
    /// Check if the input is longer than the given amount of characters.
    /// Returns true if the input is longer than the given amount of characters.
    /// </summary>
    /// <param name="input">The input of the user</param>
    ///<param name="characters">The maximum amount of characters</param>
    /// <returns>true if the input lenght is over the character limit</returns>
    private bool CheckLenght(string? input, int characters)
    {
        return string.IsNullOrEmpty(input) ? true : input.Length > characters;
    }
}