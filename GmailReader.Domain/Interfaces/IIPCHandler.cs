using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmailReader.Domain.Interfaces
{
    public interface IIPCHandler
    {
        public void ConfigCommunication();
        public void SendValue();
        public void ReadValue();
    }
}
