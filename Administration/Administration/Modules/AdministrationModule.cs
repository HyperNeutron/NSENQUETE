using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using Administration.Helpers;
using NS.Helpers;

namespace NS.Modules
{
    internal class AdministrationModule
    {
        public static void Launch()
        {
            (int id, string? name, string? smallStory, string? feedback) review = (0, null, null, null);

            while (true)
            {
                review = FeedbackDatabase.GetReviewRow();
                break;
            }

            if (string.IsNullOrEmpty(review.name))
            {
                Console.Clear();
                TextHelper.WriteStationLine();
                Console.WriteLine("Er zijn geen berichten om te beoordelen");
                Thread.Sleep(10000); // Elke 10 seconde wordt er gekeken of er nieuwe feedback binnen is gekomen
            }

            Console.Clear();

            TextHelper.WriteStationLine();
            Console.WriteLine("Naam: " + review.name);
            Console.WriteLine("Verhaal: " + review.smallStory);
            Console.WriteLine("Feedback (privé): " + review.feedback);
            TextHelper.WriteLine();


            while (true)
            {
                Console.WriteLine("\nGoedkeuren? ja/nee");
                string? choice = Console.ReadLine();

                if (choice == "ja")
                {
                    FeedbackDatabase.SendProcessedFeedback(review.id, review.name, review.smallStory, review.feedback, true);
                    Console.WriteLine("\nFeedback beoordeeld!");
                    Console.WriteLine("Nieuwe feedback laden ...");
                    Thread.Sleep(2000);
                    break;
                }
                if (choice == "nee")
                {
                    FeedbackDatabase.SendProcessedFeedback(review.id, review.name, review.smallStory, review.feedback, false);
                    Console.WriteLine("\nFeedback beoordeeld!");
                    Console.WriteLine("Nieuwe feedback laden ...");
                    Thread.Sleep(2000);
                    break;
                }

                Console.WriteLine("\nVoer ja of nee in!");
            }

        }
    }
}
