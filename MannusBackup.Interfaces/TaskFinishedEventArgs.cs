using System;

namespace MannusBackup.Interfaces
{
    /// <summary>
    /// resultaat van een afgeronde taak
    /// </summary>
    public class TaskFinishedEventArgs : EventArgs
    {
        public TaskFinishedEventArgs()
        {
            Count = true;
        }

        /// <summary>
        /// Telt de taak mee voor be-eindiging van de backup-procedure
        /// </summary>
        public bool Count { get; set; }

        /// <summary>
        /// Eindtijd van de taak
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Omschrijving taak
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Starttijd van de taak
        /// </summary>
        public DateTime StartTime { get; set; }

        public override string ToString()
        {
            TimeSpan verschil = EndTime - StartTime;
            string schilString = string.Format("{0}:{1}:{2}", verschil.Hours, verschil.Minutes, verschil.Seconds);
            string separator = "------------------------------------------------------------------------------------------------------";
            return string.Format("{2} taak gestart op {0} en gereed op {1} ({3})\r\n{4}", StartTime, EndTime, Message, schilString, separator);
        }
    }
}