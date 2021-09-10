using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Consultations_NLH
{
    // cette fenetre affiche les items de la table Patients dans notre bdd
    // dans deux datagrid, dépendant si ils ont une date de congé ou non
    // pour réadmettre un patient on n'a qua nuller la date de congé
    // du patient sélectionné
    public partial class FormMedecin : Window
    {
        static NorthernLightsEntities maBdd = NlhEntity.getInstance();
        public FormMedecin()
        {
            InitializeComponent();
        }
        public ObservableCollection<Admission> Admissions { get; private set; }
        public ICollectionView AdminActives
        {
            get
            {
                var admissions = new CollectionViewSource { Source = Admissions }.View;
                admissions.Filter = a => (a as Admission).DateConge is null;
                admissions.Refresh();
                return admissions;
            }
        }
        public ICollectionView AdminInactives
        {
            get
            {
                var admissions = new CollectionViewSource { Source = Admissions }.View;
                admissions.Filter = a => (a as Admission).DateConge != null;
                admissions.Refresh();
                return admissions;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Admissions = new ObservableCollection<Admission>(maBdd.Admissions.ToList());
            dgActives.ItemsSource = AdminActives;
            dgInactives.ItemsSource = AdminInactives;
        }
        private void ctxSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sur de vouloir donner congé au patient sélectionné?", "Attention!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                (dgActives.SelectedItem as Admission).DateConge = DateTime.Now;
                ((Admission)dgActives.SelectedItem).Lit.Occupe = false;
                dgActives.ItemsSource = AdminActives;
                dgInactives.ItemsSource = AdminInactives;
                maBdd.SaveChanges();
                MessageBox.Show("La facture totale du patient est de: $" + ((Admission)dgActives.SelectedItem).CoutTotal.ToString());
                }
                catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            FormAccueil formAccueil = new FormAccueil();
            formAccueil.Show();
        }
    }
}