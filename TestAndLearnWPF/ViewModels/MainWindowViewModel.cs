using BaseClassesLibrary.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAndLearnWPF.ViewModels
{
    public class MainWindowViewModel: BaseViewModel
    {
        #region Propertis
        public BaseMetroDialog MetroDialog { get; set; }
        #endregion
        public MainWindowViewModel()
        {

        }
    }
}
