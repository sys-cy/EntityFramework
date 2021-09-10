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
namespace Consultations_NLH
{
    public partial class FormAccueil : Window
    {
        // j'utilise un singleton, mais je peux deja voir quelques problemes
        // il fautdrait soit une methode qui reset le singleton soit trouver
        // d'autres moyens eviter de sauvegarder les infos d'une page
        // fermee non sauvegardee quand on en sauvegarde une autre
        public static NorthernLightsEntities maBdd;
        public FormAccueil()
        {
            maBdd = NlhEntity.getInstance();
            InitializeComponent();
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (txtUser.Text.ToLower() == "admin" && txtPass.Password.ToLower() == "admin")
            {
                FormAdmin formAdmin = new FormAdmin();
                formAdmin.Show();
                this.Hide();
            }
            else if (txtUser.Text.ToLower() == "préposé" && txtPass.Password.ToLower() == "préposé")
            {
                FormPrepose formPrepose = new FormPrepose();
                formPrepose.Show();
                this.Hide();
            }
            else if (txtUser.Text.ToLower() == "medecin" && txtPass.Password.ToLower() == "medecin")
            {
                FormMedecin formMedecin = new FormMedecin();
                formMedecin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usager et/ou Passe manquants ou invalides");
                txtUser.Focus();
            }
        }
    }
}