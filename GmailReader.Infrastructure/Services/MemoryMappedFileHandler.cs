using GmailReader.Domain.Interfaces;
using System.IO.MemoryMappedFiles;
using System.Security.AccessControl;
using System.Security.Principal;

namespace GmailReader.Infrastructure.Services
{
    public class MemoryMappedFileHandler : IIPCHandler
    {
        public void ConfigCommunication()
        {
            //bool mutexCreated;
            //Mutex mutex;
            //MutexSecurity mutexSecurity = new MutexSecurity();
            //MemoryMappedFileSecurity mmfSecurity = new MemoryMappedFileSecurity();

            //mutexSecurity.AddAccessRule(new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null),
            //MutexRights.Synchronize | MutexRights.Modify, AccessControlType.Allow));
            //mmfSecurity.AddAccessRule(new AccessRule<MemoryMappedFileRights>("everyone", MemoryMappedFileRights.FullControl,
            //AccessControlType.Allow));

            //mutex = new Mutex(false, @"Global\MyMutex", out mutexCreated, mutexSecurity);
            //if (mutexCreated == false) log.DebugFormat("There has been an error creating the mutex");
            //else log.DebugFormat("mutex created successfully");
        }

        public void ReadValue()
        {
            throw new NotImplementedException();
        }

        public void SendValue()
        {
            throw new NotImplementedException();
        }
    }
}
