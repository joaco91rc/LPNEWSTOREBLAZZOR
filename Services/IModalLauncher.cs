using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    // Services/IModalLauncher.cs
    public interface IModalLauncher
    {
        event Func<Task>? OnOpenAperturaCaja;
        Task OpenAperturaCajaAsync();
    }

    // Services/ModalLauncher.cs
    public class ModalLauncher : IModalLauncher
    {
        public event Func<Task>? OnOpenAperturaCaja;
        public Task OpenAperturaCajaAsync() => OnOpenAperturaCaja?.Invoke() ?? Task.CompletedTask;
    }

}
