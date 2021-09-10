using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Consultations_NLH.Converters
{
    class DepartementConverter : IMultiValueConverter
    {
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {


            if (values[0] != null)
            {
                return ((Admission)values[0]).Lit.Departement;
            }
            else if (values[1] != null)
            {
                return (Departement)NlhEntity.getInstance().Departements.Where(d => d.NomDepartement == "Chirurgie").FirstOrDefault();
            }
            else if (values[2] != null)
            {
                DateTime dt = new DateTime();
                dt = (DateTime)values[2];
                if (DateTime.Now.Year - dt.Year <= 16)
                    return (Departement)NlhEntity.getInstance().Departements.Where(d => d.NomDepartement == "Pediatrie").FirstOrDefault();
                else return null;
            }
            else
                return null;
        }
        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
