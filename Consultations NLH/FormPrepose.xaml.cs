using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class FormPrepose : Window, INotifyPropertyChanged
    {
        private bool isPatientChanged;
        public bool IsPatientChanged
        {
            get => isPatientChanged;
            set
            {
                isPatientChanged = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPatientChanged)));
            }
        }
        public static readonly DependencyProperty IsModifiedProperty =
            DependencyProperty.RegisterAttached("IsModified", typeof(bool), typeof(FormPrepose), new PropertyMetadata(false, OnIsModified));
        public FormPrepose()
        {
            InitializeComponent();
            IsPatientChanged = false;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbNas.DataContext = NlhEntity.getInstance().Patients.ToList();
            cbAssurance.DataContext = NlhEntity.getInstance().Assurances.ToList();
            cbTypeLit.DataContext = NlhEntity.getInstance().TypeLits.ToList();
            cbAdmissions.DataContext = NlhEntity.getInstance().Admissions.ToList();
            cbMedecin.DataContext = NlhEntity.getInstance().Medecins.ToList();
            cbNumeroLit.DataContext = NlhEntity.getInstance().Lits.ToList();
            cbDepartement.DataContext = NlhEntity.getInstance().Departements.ToList();
        }
        private void btnModifierPatient_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sur de vouloir enregistrer les modificaions?", "Attention!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                NlhEntity.getInstance().SaveChanges();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private static void OnIsModified(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MessageBox.Show("Patient change");
        }
        private void txtPrenom_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsPatientChanged = true;
        }
        private void btnAjouterPatient_Click(object sender, RoutedEventArgs e)
        {
            Regex rgxNas = new Regex(@"^\d{9}$");

            if (!rgxNas.IsMatch(cbNas.Text))
            {
                MessageBox.Show("Nas: nombre à 9 chiffres svp !", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                cbNas.Focus();
            }
            else if (string.IsNullOrEmpty(txtPrenom.Text))
            {
                MessageBox.Show("Prenom obligatoire svp !", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtPrenom.Focus();
            }
            else if (string.IsNullOrEmpty(txtNom.Text))
            {
                MessageBox.Show("Nom obligatoire svp !", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtNom.Focus();
            }
            else if (dpNaissance.SelectedDate is null)
            {
                MessageBox.Show("Date obligatoire svp !", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                dpNaissance.Focus();
            }
            else if (string.IsNullOrEmpty(txtAdresse.Text))
            {
                MessageBox.Show("Adresse obligatoire svp !", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtAdresse.Focus();
            }
            else if (string.IsNullOrEmpty(txtVille.Text))
            {
                MessageBox.Show("Ville obligatoire svp !", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtVille.Focus();
            }
            else if (string.IsNullOrEmpty(txtProvince.Text))
            {
                MessageBox.Show("Province obligatoire svp !", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtProvince.Focus();
            }
            else if (string.IsNullOrEmpty(txtCodePostal.Text))
            {
                MessageBox.Show("Code postal obligatoire svp !", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtCodePostal.Focus();
            }
            else if (string.IsNullOrEmpty(txtTelephone.Text))
            {
                MessageBox.Show("Telephone obligatoire svp !", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtTelephone.Focus();
            }
            else
            {
                try
                {
                    if (cbAssurance.SelectedIndex == -1)
                    {
                        cbAssurance.SelectedIndex = 0;
                    }

                    Patient nouveauPatient = new Patient();
                    nouveauPatient.Nas = cbNas.Text;
                    nouveauPatient.PrenomPatient = txtPrenom.Text;
                    nouveauPatient.NomPatient = txtNom.Text;
                    nouveauPatient.Adresse = txtAdresse.Text;
                    nouveauPatient.Ville = txtVille.Text;
                    nouveauPatient.Province = txtProvince.Text;
                    nouveauPatient.CodePostal = txtCodePostal.Text;
                    nouveauPatient.Telephone = txtTelephone.Text;
                    nouveauPatient.Assurance = (Assurance)cbAssurance.SelectedItem;
                    NlhEntity.getInstance().Patients.Add(nouveauPatient);
                    NlhEntity.getInstance().SaveChanges();
                    cbNas.DataContext = NlhEntity.getInstance().Patients.ToList();
                    MessageBox.Show("Patient ajouté");
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

        private void btnAdmettrePatient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Random generator = new Random();
                String r = generator.Next(0, 100000).ToString("D5");

                Admission adm = new Admission();
                adm.IdAdmission = r;
                adm.Chirurgie = dpChirurgie.SelectedDate != null;
                adm.DateChirurgie = dpChirurgie.SelectedDate;
                adm.DateAdmission = DateTime.Now;
                adm.DateConge = null;
                adm.Nas = cbNas.Text;
                adm.Televiseur = (bool)ckTeleviseur.IsChecked;
                adm.Telephone = (bool)ckTelephone.IsChecked;
                adm.NumeroLit = cbNumeroLit.Text;
                adm.IdMedecin = ((Medecin)cbMedecin.SelectedItem).IdMedecin;
                adm.CoutTotal = decimal.Parse(lblCoutTotal.Content.ToString());
                ((Lit)cbNumeroLit.SelectedItem).Occupe = true;
                NlhEntity.getInstance().Admissions.Add(adm);
                NlhEntity.getInstance().SaveChanges();
                cbAssurance.DataContext = NlhEntity.getInstance().Admissions.ToList();
                cbNas.DataContext = NlhEntity.getInstance().Patients.ToList();
                MessageBox.Show("Patient admis");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}