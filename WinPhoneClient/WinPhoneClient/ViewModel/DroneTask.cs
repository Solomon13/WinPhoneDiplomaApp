using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using WinPhoneClient.JSON;
using WinPhoneClient.Model;

namespace WinPhoneClient.ViewModel
{
    public class DroneTask : ObservableObject, IJsonDependent
    {
        public void ApplyJson(IBaseJsonValue json)
        {
            throw new NotImplementedException();
        }

        public IBaseJsonValue GetJsonValue()
        {
            throw new NotImplementedException();
        }
    }
}
