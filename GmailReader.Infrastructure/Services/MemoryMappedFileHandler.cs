using GmailReader.Domain.Interfaces;
using System.IO.MemoryMappedFiles;
using System.Security.AccessControl;
using System.Security.Principal;
using GmailReader.Domain.Utilities;

namespace GmailReader.Infrastructure.Services
{
    public class MemoryMappedFileHandler : IIPCHandler
    {
        public Mutex _mutex;
        private MemoryMappedFile _file;

        public void ConfigCommunication()
        {
            bool mutexCreated;
            MutexSecurity mutexSecurity = new MutexSecurity();

            mutexSecurity.AddAccessRule(new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null),
            MutexRights.Synchronize | MutexRights.Modify, AccessControlType.Allow));

            _mutex = new Mutex(false, @"Global\MyMutex", out mutexCreated);
            _mutex.SetAccessControl(mutexSecurity);
            if (mutexCreated == false) throw new Exception("There has been an error creating the mutex");
            _file = MemoryMappedFile.CreateOrOpen(@"Global\MyMemoryMappedFile", 4096,
                MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.DelayAllocatePages,
                HandleInheritability.Inheritable);
        }

        public void ReadValue()
        {

            try
            {
                using (MemoryMappedFile file = MemoryMappedFile.OpenExisting(
                @"Global\MyMemoryMappedFile", MemoryMappedFileRights.Read))
                {

                    using (MemoryMappedViewAccessor accessor =
                        file.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read))
                    {
                        Mutex mutex = Mutex.OpenExisting(@"Global\MyMutex");
                        byte[] buffer = new byte[accessor.Capacity];

                        mutex.WaitOne();
                        accessor.ReadArray<byte>(0, buffer, 0, buffer.Length);
                        mutex.ReleaseMutex();

                        string xmlData = buffer.ConvertByteArrayToString();
                    }
                }
            }
            catch (Exception)
            {
                var a = 1;
            }
            
        }

        public void SendValue()
        {
            using (MemoryMappedViewAccessor accessor = _file.CreateViewAccessor())
            {
                byte[] buffer = "Msn prueba".ConvertStringToByteArray();
                accessor.WriteArray<byte>(0, buffer, 0, buffer.Length);

                _mutex.ReleaseMutex();
            }
        }
    }
}
