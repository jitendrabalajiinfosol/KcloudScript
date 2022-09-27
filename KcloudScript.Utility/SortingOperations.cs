using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KcloudScript.Utility
{
    public class SortingOperations
    {
        public static int Mid(List<int> lstPhoneNos, int low, int high)
        {
            int pivot = lstPhoneNos[high];
            int i = (low - 1);

            for (int j = low; j <= high - 1; j++)
            {

                if (lstPhoneNos[j] < pivot)
                {
                    i++;
                    SwapNos(lstPhoneNos, i, j);
                }
            }
            SwapNos(lstPhoneNos, i + 1, high);
            return (i + 1);
        }
        private static void SwapNos(List<int> lstPhoneNos, int i, int j)
        {
            int temp = lstPhoneNos[i];
            lstPhoneNos[i] = lstPhoneNos[j];
            lstPhoneNos[j] = temp;
        }
    }
}
