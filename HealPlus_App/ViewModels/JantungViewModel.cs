using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using HealPlus_App.Setup;
using HealPlus_App.Models;

namespace HealPlus_App.ViewModels
{
    class UserViewModel : INotifyPropertyChanged
    {
        public UserViewModel()
        {
            collection = new ObservableCollection<Jantung>();
            dbconn = new Db_Connection();
            model = new Jantung();

            InsertCommand = new RelayCommand(async () => await InsertDataAsync());
            DeleteCommand = new RelayCommand(async () => await DeleteDataAsync());
            UpdateCommand = new RelayCommand(async () => await UpdateAsync());
            SelectCommand = new RelayCommand(async () => await ReadDataAsync());
            SelectCommand.Execute(null);
        }
        public RelayCommand InsertCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand UpdateCommand { get; set; }
        public RelayCommand SelectCommand { get; set; }
        public ObservableCollection<Jantung> Collection
        {
            get
            {
                return collection;
            }
            set
            {
                collection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }
        public Jantung Model
        {
            get
            {
                return model;
            }
            set
            {
                model = Model;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }

        public bool IsChecked
        {
            get
            {
                return check;
            }
            set
            {
                check = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action OnCallBack;
        private readonly Db_Connection dbconn;
        private ObservableCollection<Jantung> collection;
        private Jantung model;
        private bool check;

        private async Task ReadDataAsync()
        {
            dbconn.OpenConnection();
            await Task.Delay(0);
            var query = "SELECT * FROM [user]";
            var sqlcmd = new SqlCommand(query, dbconn.SqlConnect);
            var sqlresult = sqlcmd.ExecuteReader();

            if (sqlresult.HasRows)
            {
                collection.Clear();
                while (sqlresult.Read())
                {
                    collection.Add(new Jantung
                    {
                        Uid_j = sqlresult[0].ToString(),
                        Name = sqlresult[1].ToString(),
                        Description = sqlresult[2].ToString(),
                        Obat = sqlresult[3].ToString(),
                    });
                }
            }
            dbconn.CloseConnection();
            OnCallBack?.Invoke();
        }
        private bool IsValidating()
        {
            var flag = true;
            if (model.Uid_j == null)
            {
                MessageBox.Show("uid can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                flag = false;
            }
            else if (model.Name == null)
            {
                MessageBox.Show("Name can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                flag = false;
            }
            else if (model.Description == null)
            {
                MessageBox.Show("Description cannot empty!!!", "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                flag = false;
            }
            else if (model.Obat == null)
            {
                MessageBox.Show("Obatcannot empty!!!", "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                flag = false;
            }
                return flag;
        }
        private async Task InsertDataAsync()
        {
            if (IsValidating())
            {
                try
                {
                    dbconn.OpenConnection();
                    var query = $"INSERT INTO [jantung] VALUES (" +
                                $"'{model.Uid_j}', " +
                                $"'{model.Name}', " +
                                $"'{model.Description}'," +
                                $"'{model.Obat}')";
                    var sqlcmd = new SqlCommand(query, dbconn.SqlConnect);
                    sqlcmd.ExecuteNonQuery();
                    dbconn.CloseConnection();
                    await ReadDataAsync();
                    MessageBox.Show("Sucessfully registerd", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task DeleteDataAsync()
        {
            try
            {
                dbconn.OpenConnection();
                var query = $"DELETE FROM [Jantung] WHERE username=" +
                            $"'{model.Uid_j}'";
                var sqlcmd = new SqlCommand(query, dbconn.SqlConnect);
                sqlcmd.ExecuteNonQuery();
                dbconn.CloseConnection();
                await ReadDataAsync();
                MessageBox.Show("Sucessfully Deleted", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task UpdateAsync()
        {
            try
            {
                dbconn.OpenConnection();
                var query = $"UPDATE [user] set keypass=" +
                            $"'{model.Name}' , " +
                            $"Description = '{model.Description}', " +
                            $"Obat = '{model.Obat}', " +
                            "WHERE uid=" +
                            $"'{model.Uid_j}'";
                var sqlcmd = new SqlCommand(query, dbconn.SqlConnect);
                sqlcmd.ExecuteNonQuery();
                dbconn.CloseConnection();
                await ReadDataAsync();
                MessageBox.Show("Sucessfully Update", "Data Updated", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }




    }
}
