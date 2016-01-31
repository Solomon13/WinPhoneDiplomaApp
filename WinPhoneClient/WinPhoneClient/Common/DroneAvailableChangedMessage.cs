using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinPhoneClient.ViewModel;

namespace WinPhoneClient.Common
{
    public class DroneAvailableChangedMessage
    {
        public DroneInfo Drone { get; set; }

        public DroneAvailableChangedMessage(DroneInfo d)
        {
            if(d == null)
                throw new ArgumentNullException();
            Drone = d;
        }
    }
}
