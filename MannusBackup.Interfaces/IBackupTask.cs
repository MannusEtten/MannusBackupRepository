using System;

namespace MannusBackup.Interfaces
{
    /// <summary>
    /// definitie van een 'container' voor het uitvoeren van een taak
    /// </summary>
    public interface IBackupTask
    {
        /// <summary>
        /// Treedt op als taak is uitgevoerd, kan een collectie zijn van meerdere specifieke taken
        /// </summary>
        event EventHandler<TaskFinishedEventArgs> TaskIsFinished;

        /// <summary>
        /// Naam van de taak zodat die opgenomen kan worden in de logging en console
        /// </summary>
        string TaskName { get; }

        /// <summary>
        /// Uitvoeren van de taak
        /// </summary>
        void ExecuteBackupTask();
    }
}