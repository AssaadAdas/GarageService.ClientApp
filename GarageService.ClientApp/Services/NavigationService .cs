using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientApp.Services
{
    public class NavigationService : INavigationService
    {
        private WeakReference<Page> _currentPage;

        public void SetCurrentPage(Page page)
        {
            _currentPage = new WeakReference<Page>(page);
        }

        public async Task ShowPopupAsync(Popup popup)
        {
            if (_currentPage != null && _currentPage.TryGetTarget(out var page))
            {
                await page.ShowPopupAsync(popup);
            }
            else if (Application.Current?.MainPage is Page mainPage)
            {
                await mainPage.ShowPopupAsync(popup);
            }
        }
    }
}
