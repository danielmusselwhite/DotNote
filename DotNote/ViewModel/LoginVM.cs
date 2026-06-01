using DotNote.Model;
using DotNote.ViewModel.Commands.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNote.ViewModel
{
    public class LoginVM
    {
        #region Properties
        private User user;
        public User User
        {
            get { return user; }
            set { user = value; }
        }
        #endregion

        #region Commands
        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }
        #endregion

        public LoginVM()
        {
            User = new User();
            RegisterCommand = new RegisterCommand(this);
            LoginCommand = new LoginCommand(this);
        }
    }
}
