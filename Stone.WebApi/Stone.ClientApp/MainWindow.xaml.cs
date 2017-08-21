using Stone.Common;
using Stone.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Configuration;
using Stone.ClientApp.Model;
using Stone.Cipher;

namespace Stone.ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string wepApiUrl = ConfigurationManager.AppSettings.Get("WepApiUrl");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tabItem = ((sender as TabControl).SelectedItem as TabItem).Header as string;

            switch (tabItem)
            {
                case "Listar Transações":
                    GetTransactions();
                    break;
                case "Transações":
                    GetTransactionTypes();
                    break;
                default:
                    return;
            }
        }

        private async void GetTransactions()
        {
            try
            {
                ShowHideWait(this, true);

                WebApiWrapper<ResultData<TransactionEntity>> api = new WebApiWrapper<ResultData<TransactionEntity>>(wepApiUrl);
                ResultData<TransactionEntity> result = await api.Get("/api/transaction/GetByClientCode/" + ((App)Application.Current).ClientCode.ToString());

                if (result == null)
                    return;

                List<TransactionVM> vms = new List<TransactionVM>(); 
                foreach (TransactionEntity t in result.Data)
                {
                    vms.Add(new TransactionVM
                    {
                        Amount = t.Amount,
                        Number = t.Number,
                        CardNumber = t.Card.Number,
                        TransactionDate = t.TransactionDate,
                        TransactionReturn = t.TransactionReturn != 0 ? t.TransactionReturn.GetStringValue() : "???",
                        Type = ((TransactionTypeEntity.Types)t.TransactionType.Id).GetStringValue()
                    });
                }

                transactionGrid.ItemsSource = vms;
                
            }
            catch
            {
                ShowError(this, "Falha ao recuperar tipos de transação. Verifique a conexão com a API.");
            }
            finally
            {
                ShowHideWait(this, false);
            }          
        }

        private async void GetTransactionTypes()
        {
            try
            {
                ShowHideWait(this, true);

                WebApiWrapper<ResultData<TransactionTypeEntity>> api = new WebApiWrapper<ResultData<TransactionTypeEntity>>(wepApiUrl);
                ResultData<TransactionTypeEntity> result = await api.Get("/api/TransactionType");

                if (result != null)
                    dropdownTransactionType.ItemsSource = result.Data;
                else
                    dropdownTransactionType.ItemsSource = null;

            }
            catch (Exception)
            {
                ShowError(this, "Falha ao recuperar tipos de transação. Verifique a conexão com a API.");
            }
            finally
            {
                ShowHideWait(this, false);
            } 
            
        }

        private void ShowHideWait(MainWindow context, bool show)
        {
            Dispatcher.Invoke(() =>
            {
                //waitCover
                var waitCover = (Grid)context.FindName("waitCover");
                if(show)
                    waitCover.Visibility = System.Windows.Visibility.Visible;
                else
                    waitCover.Visibility = System.Windows.Visibility.Hidden;
            });
        }

        private void ShowError(MainWindow context, string msg)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show(msg, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }

        private void ShowSuccess(MainWindow context, string msg)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show(msg, "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private void ShowMsg(MainWindow context, string msg)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show(msg, "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private string GetPassword(MainWindow context)
        {
            string pass = "";
            Dispatcher.Invoke(() =>
            {
                pass = Microsoft.VisualBasic.Interaction.InputBox("Password", "Password", "");
            });

            return pass;
        }

        private async void confirmarBtn_Click(object sender, RoutedEventArgs e)
        {
            TransactionEntity t = null;
            try
            {
                var pass = GetPassword(this);
                t = new TransactionEntity
                {
                    ClientCode = ((App)Application.Current).ClientCode,
                    Amount = Convert.ToDecimal(InputAmount.Text),
                    Number = Convert.ToInt32(InputNumber.Text),
                    Card = new CardEntity
                    {
                        Number = InputCardNumber.Text,
                        Password = StringCipher.Encrypt(pass)
                    },
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = ((TransactionTypeEntity)dropdownTransactionType.SelectedItem).Id
                    }
                };
                
            }
            catch (Exception)
            {
                ShowError(this, "Preencha corretamente os valores");
                return;
            }
            
            if(t == null)
                return;

            WebApiWrapper<ResultData<Stone.BusinessEntities.TransactionEntity.TransactionReturnEnum>> api = new WebApiWrapper<ResultData<Stone.BusinessEntities.TransactionEntity.TransactionReturnEnum>>(wepApiUrl);

            ResultData<Stone.BusinessEntities.TransactionEntity.TransactionReturnEnum> result = await api.CreateAsync("/api/transaction", t);

            if (!result.Success)
            {
                ShowError(this, result.Msg);
                return;
            }

            TransactionEntity.TransactionReturnEnum rtn = (TransactionEntity.TransactionReturnEnum)result.Data.First();

            ShowMsg(this, rtn.GetStringValue());
        }

        private void dropdownTransactionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //evitar o bug do TapControl com um DropDown interno
            e.Handled = true;
        }
    }
}
