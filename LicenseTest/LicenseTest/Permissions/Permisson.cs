using LicenseTest.Interfaces;
using System;
using System.Text;

namespace LicenseTest.Permissions
{
    public class Permisson<T> where T : ILicense, new()
    {
        ILicense license;

        public Permisson(String name)
        {
            license = new T();
        }

        internal void CallMethod<T>(Action<T, object> param)
        {
            try
            {
                param.Invoke((T)license, param);
            }
            catch (Exception e)
            {
                StringBuilder builder = new StringBuilder("You have no permission to execute: ");
                builder.Append(e.Message);
                Console.WriteLine(builder.ToString());
            }
        }
    }
}