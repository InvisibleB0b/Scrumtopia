using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Scrumtopia.Annotations;
using Scrumtopia.Common;
using Scrumtopia.Persistency;
using Scrumtopia_classes;

namespace Scrumtopia.ViewModel
{
    public class LoginVM:INotifyPropertyChanged
    {
        private string _userNameVm;
        private string _passwordVm;

        public string UserNameVM
        {
            get { return _userNameVm; }
            set { _userNameVm = value; OnPropertyChanged();}
        }

        public string PasswordVM
        {
            get { return _passwordVm; }
            set { _passwordVm = value; OnPropertyChanged();}
        }

        public Singleton LeSingleton { get; set; }

        public LoginVM()
        {
            LeSingleton = Singleton.Instance;
            LeSingleton.LoggedUser = null;
            UserNameVM = "Admin";
            PasswordVM = "admin";

        }

        public async Task<bool> InitLogin()
        {
            if (UserNameVM == "" || UserNameVM == "")
            {
                return false;
            }
            else
            {
                ScrumUser user = new ScrumUser() { User_Name = UserNameVM, User_Password = PasswordVM };

                ScrumUser loggedUser = await UsersPer.Login(user);

                if (loggedUser != null)
                {
                    if (loggedUser.User_Id != 0)
                    {
                          LeSingleton.LoggedUser = loggedUser;
                          return true;
                    }
                    else
                    {
                        return false;
                    }
                  
                }
                else
                {
                    return false;
                }
            }

           

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
