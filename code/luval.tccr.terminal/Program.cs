using luval.tccr.indicadores;
using luval.tccr.storage;
using System;

namespace luval.tccr.terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            var extractor = new Extractor();
            extractor.StatusMessage += Extractor_StatusMessage;
            try
            {
                extractor.BatchInsert(DateTime.Today.Date.AddYears(-10), DateTime.Today, 3);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("**** ERROR ****");
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.White;
                
            }
            Console.WriteLine();
            Console.WriteLine("Press Enter to finish");
            Console.ReadLine();
        }

        private static void Extractor_StatusMessage(object sender, string e)
        {
            WriteMessage(e);
        }

        private static void WriteMessage(string message)
        {
            Console.WriteLine(string.Format("[{0}] - {1}", DateTime.Now.TimeOfDay, message));
        }
    }
}
