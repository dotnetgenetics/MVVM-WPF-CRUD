using MVVMWPFCRUD.DataAccess;
using MVVMWPFCRUD.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MVVMWPFCRUD.ViewModel
{
    public class StudentViewModel
    {
        private ICommand _saveCommand;
        private ICommand _resetCommand;
        private ICommand _editCommand;
        private ICommand _deleteCommand;
        private StudentRepository _repository;
        private Student _studentEntity = null;
        public StudentRecord StudentRecord { get; set; }
        public StudentEntities StudentEntities { get; set; }

        public ICommand ResetCommand
        {
            get
            {
                if (_resetCommand == null)
                    _resetCommand = new RelayCommand(param => ResetData(), null);

                return _resetCommand;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(param => SaveData(), null);

                return _saveCommand;
            }
        }

        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new RelayCommand(param => EditData((int)param), null);

                return _editCommand;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(param => DeleteStudent((int)param), null);

                return _deleteCommand;
            }
        }

        public StudentViewModel()
        {
            _studentEntity = new Student();
            _repository = new StudentRepository();
            StudentRecord = new StudentRecord();
            GetAll();
        }

        public void ResetData()
        {
            StudentRecord.Name = string.Empty;
            StudentRecord.Id = 0;
            StudentRecord.Address = string.Empty;
            StudentRecord.Contact = string.Empty;
            StudentRecord.Age = 0;
        }

        public void DeleteStudent(int id)
        {
            if (MessageBox.Show("Confirm delete of this record?", "Student", MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.RemoveStudent(id);
                    MessageBox.Show("Record successfully deleted.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured while saving. " + ex.InnerException);
                }
                finally
                {
                    GetAll();
                }
            }
        }

        public void SaveData()
        {
            if (StudentRecord != null)
            {
                _studentEntity.Name = StudentRecord.Name;
                _studentEntity.Age = StudentRecord.Age;
                _studentEntity.Address = StudentRecord.Address;
                _studentEntity.Contact = StudentRecord.Contact;

                try
                {
                    if (StudentRecord.Id <= 0)
                    {
                        _repository.AddStudent(_studentEntity);
                        MessageBox.Show("New record successfully saved.");
                    }
                    else
                    {
                        _studentEntity.ID = StudentRecord.Id;
                        _repository.UpdateStudent(_studentEntity);
                        MessageBox.Show("Record successfully updated.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured while saving. " + ex.InnerException);
                }
                finally
                {
                    GetAll();
                    ResetData();
                }
            }
        }

        public void EditData(int id)
        {
            var model = _repository.Get(id);
            StudentRecord.Id = model.ID;
            StudentRecord.Name = model.Name;
            StudentRecord.Age = (int)model.Age;
            StudentRecord.Address = model.Address;
            StudentRecord.Contact = model.Contact;
        }

        public void GetAll()
        {
            StudentRecord.StudentRecords = new ObservableCollection<StudentRecord>();
            _repository.GetAll().ForEach(data => StudentRecord.StudentRecords.Add(new StudentRecord()
            {
                Id = data.ID,
                Name = data.Name,
                Address = data.Address,
                Age = Convert.ToInt32(data.Age),
                Contact = data.Contact
            }));
        }
    }
}
