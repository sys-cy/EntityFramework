using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Consultations_NLH.Converters
{
    public class PatientToAdmissionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Patient patient = (Patient)value;
            return patient?.Admissions.Where(a => a.Patient.Nas == patient.Nas).FirstOrDefault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Admission admission = (Admission)value;
            return admission?.Patient;
        }
    }
}
