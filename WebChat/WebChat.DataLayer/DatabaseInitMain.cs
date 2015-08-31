namespace WebChat.DataLayer
{
    using System;
    using System.Linq;

    class DatabaseInitMain
    {
        static void Main()
        {
            var context = new WebChatContext();
            var count = context.Messages.Count();

            Console.WriteLine(count);
        }
    }
}
