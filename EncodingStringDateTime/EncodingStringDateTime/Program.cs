using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncodingStringDateTime
{

    class Program
    {
        public static bool Contains(string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
        public static void SicnessHistoryFromGB18030intoUnicode(string sicknessHistoryChina)
        {
            FileInfo sicnessChinaFileInfo = new FileInfo(sicknessHistoryChina);
            Encoding chinese = Encoding.GetEncoding("GB18030");
            Encoding unicode = Encoding.Unicode;
            // List<string> infoChineseHistory = new List<string>();

            //     StreamReader streamReader = new StreamReader(sicnessChinaFileInfo.OpenRead(), chinese);
            string infoChineseHistory = File.ReadAllText(sicnessChinaFileInfo.FullName, chinese);

            //Convert from encoding GB18030 into Unicode
            byte[] chinaBytes = chinese.GetBytes(infoChineseHistory);
            byte[] unicodeBytes = Encoding.Convert(chinese, unicode, chinaBytes);
            string infoUnicodeHistory = unicode.GetString(unicodeBytes);

            File.WriteAllText($@"{sicnessChinaFileInfo.DirectoryName}{sicnessChinaFileInfo.Name}_Hospital.txt",
                infoUnicodeHistory);

        }
        public static SicknessHistory ImportSicnessHistoryfromFileUnicode(string fileSicknessHistoryNew)
        {
            FileInfo sicnessFileInfo = new FileInfo(fileSicknessHistoryNew);
            string[] infoNewHistory = File.ReadAllLines(sicnessFileInfo.FullName);

            string sicknessName = infoNewHistory[2];
            string patientName = infoNewHistory[3];
            string doctorName = infoNewHistory[4];
            DateTime dateStartOpenSicknessHistory = DateTime.Parse(infoNewHistory[5]);
            DateTime? dateFinishCloseSicknessHistory;
            if (infoNewHistory.Length == 7)
                dateFinishCloseSicknessHistory = DateTime.Parse(infoNewHistory[5]);
            else
            {
                dateFinishCloseSicknessHistory = null;
            }

            return new SicknessHistory(sicknessName, patientName, doctorName, dateStartOpenSicknessHistory,
                dateFinishCloseSicknessHistory);
        }

        public static List<SicknessHistory> SearchOfName(string stringScope, List<SicknessHistory> listSicknessHistories)
        {
            return listSicknessHistories.Where(x => Contains(x.PatientName, stringScope, StringComparison.CurrentCultureIgnoreCase)).OrderBy(x => x.PatientName).ToList();

        }
        public static List<SicknessHistory> SearchOfDateStartSicknessHistories(DateTimeOffset criteriuDateTimeOffset, List<SicknessHistory> listSicknessHistories)
        {
            var result = listSicknessHistories
                            .Where(x => x.DateStartOpenSicknessHistory.ToUniversalTime().Date == criteriuDateTimeOffset.Date)
                            .OrderBy(x => x.PatientName)
                            .ToList();

            return result;
        }

        static void Main(string[] args)
        {

            List<SicknessHistory> sicknessHistories = new List<SicknessHistory>
            {
                new SicknessHistory("ANGINA", "Bordea Alexandru", "Ciumac Sergiu", new DateTime(2016, 07, 3, 01, 35, 0,DateTimeKind.Local)),
                new SicknessHistory("ANEMIE", "Balta Galina", "Budeanu Igor",      new DateTime(2016, 07, 3, 0,  02,02,DateTimeKind.Utc)),
                new SicknessHistory("APENDICITA", "Bursuc Jorik", "Albu Ana", new DateTime(2016, 07, 3, 01, 35, 0),new DateTime(2016, 05, 10, 17, 12, 56)),
                new SicknessHistory("APENDICITA", "Bursuc Jorik", "Albu Ana", new DateTime(2016, 07, 3, 01, 35, 0,DateTimeKind.Utc),new DateTime(2016, 05, 10, 17, 12, 56,DateTimeKind.Utc))
            };

            sicknessHistories[0].SicknessHistoryToTxtFile();

            Console.WriteLine("\nA venit un pacient din China cu o sicknessHistory codata in GB18030");

            //Emitam o istorie de boala noua.
            //aceasta linie de cod va fi inlocuita cu plasarea fisierurui din China in directoriu special
            SicknessHistory sickChina = new SicknessHistory("LEICOMIE", "Ursu Ludmila", "Lie China",
                new DateTime(2005, 9, 26, 15, 47, 56));
            sickChina.SicknessHistoryToTxtFileEncodingGB18030();

            SicnessHistoryFromGB18030intoUnicode(@"D:\Ursu Ludmila LEICOMIEGB18030.txt");

            sicknessHistories.Add(ImportSicnessHistoryfromFileUnicode(@"D:\Ursu Ludmila LEICOMIEGB18030.txt_Hospital.txt"));

            Console.WriteLine(new string('-', 70));
            Console.WriteLine("Lista istoriilor de boala a spitalului");
            Console.WriteLine(new string('-', 70));
            foreach (SicknessHistory item in sicknessHistories)
                Console.WriteLine(item.ToString());

            //Acum file-ul cu istorie de boala va putea fi tratat in sistemul Hospital corect
            Console.WriteLine(new string('-', 70));

            //Toate istoriile cu NamePatient care contine "urs"

            Console.WriteLine(new string('-', 70));
            Console.WriteLine("Istoriile cu NamePatient care contine \"urs\"\n");
            var selectedSicknessHistories = SearchOfName("urs", sicknessHistories);
            foreach (SicknessHistory item in selectedSicknessHistories)
                Console.WriteLine(item.ToString());

            Console.WriteLine(new string('-', 70));
            Console.WriteLine("Istoriile cu Data deschiderii egala cu 02.07.2016 conform UniversalTime");
            selectedSicknessHistories = SearchOfDateStartSicknessHistories(new DateTime(2016, 07, 02), sicknessHistories);
            foreach (SicknessHistory item in selectedSicknessHistories)
                Console.WriteLine(item.ToString());

            Console.ReadKey();
        }
    }
}
