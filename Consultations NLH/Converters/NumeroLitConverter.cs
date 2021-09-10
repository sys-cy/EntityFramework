using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Consultations_NLH.Converters
{
    class NumeroLitConverter : IMultiValueConverter
    {
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] != null)
            {
                return NlhEntity.getInstance().Lits.ToList();
            }
            else if (values[1] != null)
            {
                Departement dep = (Departement)values[1];
                if (values[2] != null)
                {
                    TypeLit tl = (TypeLit)values[2];
                    return NlhEntity.getInstance().Lits.Where(l => l.IdDepartement == dep.IdDepartement && l.Occupe != true && l.TypeLit.IdType == tl.IdType).ToList();
                }
                else
                    return NlhEntity.getInstance().Lits.Where(l => l.IdDepartement == dep.IdDepartement && l.Occupe != true).ToList();
            }
            return null;
        }
        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
