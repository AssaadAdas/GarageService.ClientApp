
using GarageService.ClientLib.Models;
using GarageService.ClientLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GarageService.ClientApp.ViewModels
{
    [QueryProperty(nameof(NoteId), "noteId")]
    public class ReadNotificationViewModel:BaseViewModel
    {
        private readonly ApiService _ApiService;
        private ClientNotification _clientNotification;
        public string Notes { get; set; }
        private int _Id;
        public int NoteId
        {
            get => _Id;
            set
            {
                _Id = value;
                LoadCommand.Execute(null);
            }
        }

        public ClientNotification ClientNotification
        {
            get => _clientNotification;
            set
            {
                if (_clientNotification != value)
                {
                    _clientNotification = value;
                    OnPropertyChanged(nameof(ClientNotification));
                }
            }
        }
        public ICommand LoadCommand { get; }
        public ReadNotificationViewModel(ApiService apiService)
        {
            _ApiService = apiService;
            LoadCommand = new Command(async () => await LoadNotification());
            //LoadCommand.Execute(null);
        }

        private async Task LoadNotification()
        {
            var response = await _ApiService.GetClientNotification(_Id);
            if (response.IsSuccess)
            {
                ClientNotification = response.Data;
            }

            if(ClientNotification != null)
            {
                Notes = ClientNotification.Notes;
                //IsRead = ClientNotification.IsRead;
                UpdateNotification();
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Notification not found", "OK");
            }
        }

        private async Task UpdateNotification()
        {
            try
            {
                ClientNotification.IsRead = true;
                bool success = await _ApiService.UpdateClientNotificationAsync(_Id, ClientNotification);
                if (!success)
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to update Notification", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }

        }
    }
}
