using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmailReader.Domain.Interfaces;

public interface IRemoteMailOperations
{
    public Task Authenticate();
    public Task RetrieveDataByMailAddress(string emailAddress, DateTime initDate, DateTime endDate);
    public Task CleanMailbox(string emailAddress);
}

