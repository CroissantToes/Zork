namespace Zork.Common
{
    public interface IOutputService
    {
        public void Write(object obj);
        
        public void Write(string message);
        
        public void WriteLine(object obj);

        public void WriteLine(string message);
    }
}