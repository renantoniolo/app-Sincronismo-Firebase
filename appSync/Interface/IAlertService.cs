using System;
using System.Threading.Tasks;

namespace appSync.Interface
{
    public interface IAlertService
    {
        Task ShowAsync(string title, string message, string buttonOk);
    }
}
