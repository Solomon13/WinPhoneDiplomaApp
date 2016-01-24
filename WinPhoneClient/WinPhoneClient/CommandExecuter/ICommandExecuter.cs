using System.Threading.Tasks;
using Windows.Web.AtomPub;
using WinPhoneClient.JSON;

namespace WinPhoneClient.CommandExecuter
{
    public interface ICommandExecuter
    {
        string UriString { get; }
        Task<IBaseJsonValue> ExecuteAsync();
    }

    public interface IBaseCommandExecuter : ICommandExecuter
    {
        string Token { get; set; }
    }
}
