using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Consultations_NLH
{
    public static class NlhEntity
    {
        private static NorthernLightsEntities nlhEntity = new NorthernLightsEntities();
        public static NorthernLightsEntities getInstance()
        {
            return nlhEntity;
        }
    }
}