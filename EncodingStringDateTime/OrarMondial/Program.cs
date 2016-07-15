using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrarMondial
{
    class Race
    {
        
        DateTimeOffset startRace;
        private double _durationMinutes;
        private DateTimeOffset finishRace => startRace + TimeSpan.FromMinutes(_durationMinutes);

        public Race(DateTimeOffset start,double durationMin)
        {
            startRace = start;
            _durationMinutes = durationMin;

        }

        public override string ToString()
        {
            return $"Data {startRace.Date:d}    {startRace.TimeOfDay} - {finishRace.TimeOfDay}. Duration {TimeSpan.FromMinutes(_durationMinutes) }";
        }

        public Race ConvertTo(string idTimeZone)
        {
            TimeZoneInfo newTimeZone = TimeZoneInfo.FindSystemTimeZoneById(idTimeZone);
            DateTimeOffset startRaceNewTimeZone = TimeZoneInfo.ConvertTime(this.startRace, newTimeZone);
            return new Race( startRaceNewTimeZone,_durationMinutes);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // In Germania se petrece un eveniment la care participa reprezentatii diferitor tari: Moldova,Spania,Russia,UK
            CultureInfo cultureInfo = new CultureInfo("de-DE");
            
            Console.WriteLine(cultureInfo);
            
            var eveniment = new Race(new DateTimeOffset(2016, 07, 20, 10, 30, 00,TimeSpan.FromHours(1)), 90);
            Console.WriteLine(eveniment);

            Console.WriteLine("\nTranslarea evenimentului in diferite orashe");
            var evenimentVladivostok = eveniment.ConvertTo("Vladivostok Standard Time");
            Console.Write("Vladivostok".PadRight(15));
            Console.WriteLine(evenimentVladivostok);

            var evenimentKrasnoiarsk = eveniment.ConvertTo("North Asia Standard Time");

            Console.Write("Krasnoiarsk".PadRight(15));
            Console.WriteLine(evenimentKrasnoiarsk);

            var evenimentChishinau = eveniment.ConvertTo("E. Europe Standard Time");
            Console.Write("Chishinau".PadRight(15));
            Console.WriteLine(evenimentChishinau);

            Console.ReadKey();

        }
    }
}
