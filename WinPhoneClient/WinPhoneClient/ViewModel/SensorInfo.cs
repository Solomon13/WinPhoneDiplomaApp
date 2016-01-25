using GalaSoft.MvvmLight;
using WinPhoneClient.Enums;
using WinPhoneClient.JSON;
using WinPhoneClient.Model;

namespace WinPhoneClient.ViewModel
{
    public class SensorInfo : ObservableObject, IJsonDependent
    {
        public SensorInfo()
        {
        }

        public bool IsEnable { get; set; }
        public void ApplyJson(IBaseJsonValue json)
        {
            throw new System.NotImplementedException();
        }

        public IBaseJsonValue GetJsonValue()
        {
            throw new System.NotImplementedException();
        }
    }
}
