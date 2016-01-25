using Windows.Data.Json;
using WinPhoneClient.JSON;

namespace WinPhoneClient.Model
{
    interface IJsonDependent
    {
        void ApplyJson(IBaseJsonValue json);
        IBaseJsonValue GetJsonValue();
    }
}
