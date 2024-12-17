namespace MCSM_Utility.Exceptions
{
    public class ReadExcelException : Exception
    {
        public List<string> messages { get; }
        public ReadExcelException(List<string> errors)
        {
            messages = errors ?? new List<string>();
        }
    }
}
