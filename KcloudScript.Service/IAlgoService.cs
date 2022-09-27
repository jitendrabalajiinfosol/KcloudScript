using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KcloudScript.Utility;

namespace KcloudScript.Service
{
    public interface IAlgoService
    {
        void SortNo(List<int> lstNos, int low, int high);
    }

    public class AloService : IAlgoService
    {
        public AloService()
        {

        }
        public void SortNo(List<int> lstNos, int low, int high)
        {
            if (low < high)
            {
                int pi = SortingOperations.Mid(lstNos, low, high);
                SortNo(lstNos, low, pi - 1);
                SortNo(lstNos, pi + 1, high);
            }
        }
    }
}
