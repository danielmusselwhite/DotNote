using DotNote.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DotNote.ViewModel
{
    public class NotebookVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // model property to hold the Notebook instance
        public Notebook Model { get; }


        #region Properties

        private bool isRenaming;
        public bool IsRenaming
        {
            get { return isRenaming; }
            set
            {
                isRenaming = value;
                OnPropertyChanged(nameof(IsRenaming));
            }
        }

        public string Name
        {
            get => Model.Name;
            set
            {
                Model.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        #endregion

        public NotebookVM(Notebook model)
        {
            Model = model;
        }

        #region Event Handlers
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
