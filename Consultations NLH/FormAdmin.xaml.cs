using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Entity.Validation;
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
    public partial class FormAdmin : Window
    {
        static NorthernLightsEntities maBdd = NlhEntity.getInstance();
        ObservableCollection<Medecin> ocMedecins = new ObservableCollection<Medecin>(maBdd.Medecins);
        bool changementsEffectues = false;
        public FormAdmin()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ocMedecins.CollectionChanged += OnNouveauMedAjoute;
            dgMedecins.ItemsSource = ocMedecins;
            dgAdmissions.ItemsSource = maBdd.Admissions.ToList();
            dgPatients.ItemsSource = maBdd.Patients.ToList();
            lstDepartements.DataContext = maBdd.Departements.ToList();
            lstAssurances.ItemsSource = maBdd.Assurances.ToList();

            var query =
                from l in maBdd.Lits
                select new {l.NumeroLit, l.Occupe, l.TypeLit.Description, l.Departement.NomDepartement};
            dgLits.DataContext = query.ToList();
            lstTypeLit.ItemsSource = maBdd.TypeLits.ToList();
        }
        private void OnNouveauMedAjoute(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newMedecin = e.NewItems[0] as Medecin;
                maBdd.Medecins.Add(newMedecin);
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            FormAccueil formAccueil = new FormAccueil();
            formAccueil.Show();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                maBdd.SaveChanges();
                MessageBox.Show("Vos modifications ont été enregistrées", "Northern Lights Hospital vous assure que", MessageBoxButton.OK, MessageBoxImage.Information);
                changementsEffectues = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Êtes vous sur de vouloir supprimer ce Médecin?", "Attention!", MessageBoxButton.YesNo, MessageBoxImage.Warning)==MessageBoxResult.Yes)
            {
                maBdd.Medecins.Remove((Medecin)dgMedecins.SelectedItem);
                ocMedecins.Remove(dgMedecins.SelectedItem as Medecin);
                changementsEffectues = true;
            }
                    }
        private void dgMedecins_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            changementsEffectues = true;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (changementsEffectues)
            {
                switch (MessageBox.Show("Voulez-vous enregistrer les modifications éffectués?", "Des modifications ont eues lieu", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation))
                {
                    case MessageBoxResult.Yes:
                        maBdd.SaveChanges();
                        break;
                    case MessageBoxResult.No:
                        // Aucune action
                        break;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }
        private void dgPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void lstDepartements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblIdDepartement.Content = ((Departement)lstDepartements.SelectedItem).IdDepartement;
        }

        private void lstAssurances_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblIdIdAssurance.Content = ((Assurance)lstAssurances.SelectedItem).IdAssurance;
        }

        private void lstTypeLit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblIdType.Content = ((TypeLit)lstTypeLit.SelectedItem).IdType;
        }
    }
}
