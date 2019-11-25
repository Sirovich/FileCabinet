namespace FileCabinetApp.Writers
{
    /// <summary>
    /// Provides method for writing record to file.
    /// </summary>
    public interface IWriter
    {
        /// <summary>
        /// Writes record to file.
        /// </summary>
        /// <param name="record">Source record.</param>
        void Write(FileCabinetRecord record);
    }
}
