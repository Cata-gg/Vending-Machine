using System;

namespace iQuest.VendingMachine.PresentationLayer
{
    internal class LoginView : DisplayBase
    {
        public string AskForPassword()
        {
            Console.WriteLine();
            Display("Type the admin password: ", ConsoleColor.Cyan);
            return Console.ReadLine();
        }
    }
}