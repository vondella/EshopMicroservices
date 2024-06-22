namespace postcmd.Exceptions
{
    public class ConcurrencyExecption: NotFoundException
    {
        public ConcurrencyExecption(string message):base(message)
        {
            
        }
    }
}
