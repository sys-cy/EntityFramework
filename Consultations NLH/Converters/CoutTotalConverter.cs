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
    public class CoutTotalConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double coutTotal = 0.00;
            TypeLit typeLit = (TypeLit)values?[0];
            if (values[0] != null)
            {
                Assurance ass = values[3] as Assurance;
                if (ass?.NomCompanie != "RAMQ")
                {
                    switch (typeLit.Description)
                    {
                        case"Privé": coutTotal += 571;
                            break;
                        case "Semi privé": coutTotal += 267;
                            break;
                    }
                }
                else
                {
                    int compteStandard = NlhEntity.getInstance().Lits.Where(l => l.TypeLit.Description == "Standard" && l.Occupe == false).Count();
                    int compteSemiPrive = NlhEntity.getInstance().Lits.Where(l => l.TypeLit.Description == "Semi Privé" && l.Occupe == false).Count();
                    if (typeLit.Description == "Semi privé")
                        if (compteStandard >= 1)
                            coutTotal += 267;
                    if (typeLit.Description == "Privé")
                        if (compteSemiPrive >= 1)
                            coutTotal += 571;
                }
            }
            if (values[1] != null)
            {
                if ((bool)values[1]) coutTotal += 42.50;
            }
            if (values[2] != null)
            {
                if ((bool)values[2]) coutTotal += 7.50;
            }

            return coutTotal.ToString("0.00");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
