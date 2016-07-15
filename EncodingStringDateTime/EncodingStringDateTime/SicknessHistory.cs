using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EncodingStringDateTime
{
    class SicknessHistory
    {
        private string SicknessName { get; }
        public string PatientName { get; }
        private string DoctorName { get; }
        public DateTimeOffset DateStartOpenSicknessHistory;
        public DateTimeOffset ? DateFinishCloseSicknessHistory;

        static readonly CultureInfo Invariant = CultureInfo.CurrentCulture;
        public SicknessHistory(string sicknessName, string patientName, string doctorName,
            DateTime? dateStartOpenSicknessHistory = null, DateTime? dateFinishCloseSicknessHistory = null)
        {
            if (String.IsNullOrEmpty(sicknessName)) throw new ArgumentNullException($"Name of sickness must be indicate");
            if (String.IsNullOrEmpty(patientName)) throw new ArgumentNullException($"Name of patient must be indicate");

            SicknessName = sicknessName;
            PatientName = patientName;
            DoctorName = doctorName;
            DateStartOpenSicknessHistory = dateStartOpenSicknessHistory ?? DateTime.Now;
            DateFinishCloseSicknessHistory = dateFinishCloseSicknessHistory;
        }
       

        public override string ToString()
        {
            string tempFinishDate;
            if (DateFinishCloseSicknessHistory != null)
                tempFinishDate = "Sickness history a fost inchisa la " + DateFinishCloseSicknessHistory;
            else tempFinishDate = "boala este in procesul de tratare";
            return
                 $"{PatientName,20} Boala {SicknessName,10} Doctor responsabil {DoctorName} \n Start sickness la data {DateStartOpenSicknessHistory.ToString(Invariant)} \n {tempFinishDate}";
                //$"{PatientName,20}  {SicknessName,20} \n Start {DateStartOpenSicknessHistory}         Finish {DateFinishCloseSicknessHistory}";
        }

        public void SicknessHistoryToTxtFile()
        {
           
            
            List<string> infoForFile = new List<string>();
            infoForFile.Add(DateTime.Now.ToString());
            infoForFile.Add(Encoding.Unicode.EncodingName);

            infoForFile .Add(SicknessName);
            infoForFile .Add(PatientName );
            infoForFile.Add(DoctorName);
            infoForFile.Add(DateStartOpenSicknessHistory.ToString());
            infoForFile.Add("Universal Time"+DateStartOpenSicknessHistory.ToUniversalTime());
            
            infoForFile.Add(DateFinishCloseSicknessHistory .ToString());

            File.WriteAllLines(@"D:\" + PatientName + " " + SicknessName + ".txt",infoForFile,Encoding.Unicode);


        }
        public void SicknessHistoryToTxtFileEncodingGB18030()
        {
            List<string> infoForFile = new List<string>();
            infoForFile.Add(DateTime.Now.ToString());
            infoForFile.Add("GB18030");

            infoForFile.Add(SicknessName);
            infoForFile.Add(PatientName);
            infoForFile.Add(DoctorName);
            infoForFile.Add(DateStartOpenSicknessHistory.ToString());
            infoForFile.Add("Universal Time" + DateStartOpenSicknessHistory.ToUniversalTime());

            infoForFile.Add(DateFinishCloseSicknessHistory.ToString());

            File.WriteAllLines(@"D:\" + PatientName + " " + SicknessName + "GB18030.txt", infoForFile, Encoding.GetEncoding( "GB18030"));

            
        }

       
    }
}
