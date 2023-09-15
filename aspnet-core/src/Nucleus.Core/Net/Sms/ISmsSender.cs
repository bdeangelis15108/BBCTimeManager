using System.Threading.Tasks;

namespace Nucleus.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}