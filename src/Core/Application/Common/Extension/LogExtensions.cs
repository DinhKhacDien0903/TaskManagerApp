using System.Runtime.CompilerServices;

namespace Application.Common.Extension
{
    public static class LogExtensions
    {
        public static void Log(this object obj, object objLog, [CallerMemberName] string caller = "")
        {
            try
            {
                Console.WriteLine($"[{DateTime.Now:yyMMdd-hh:mm:ss.fff}][{obj?.GetType().Name}][{caller}] {objLog?.ToString()}");
            }
            catch (Exception)
            {
            }
        }

    }
}