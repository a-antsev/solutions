using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace Solutions
{
    public class OptimisationService
    {

        public OptimisationService()
        {
            _values = new List<List<KeyValuePair<string,int>>>();
        }

        public List<KeyValuePair<string,int>> GetResult()
        {
            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
            int min = 0;
            foreach (var val in _values)
            {
                int sum = 0;
                foreach (var v in val)
                {
                    sum += v.Value;
                }
                if (sum > min)
                {
                    min = sum;
                    list = val;
                }
            }
            return list;
        }

        private Dictionary<string, int>[] _dictionaries;

        private int _count;

        public void BruteForce(Dictionary<string,int>[] dictionaries)
        {
            _dictionaries = dictionaries;
            _count = _dictionaries.Length;
            SumElems(new List<KeyValuePair<string, int>>());

        }

        private List<List<KeyValuePair<string,int>>> _values; 

        private void SumElems(List<KeyValuePair<string,int>> list,int num = 0)
        { 
            foreach (var i in _dictionaries[num]) 
            {
                list.Add(i);
                if (num != _count - 1)
                {                  
                    SumElems(list, num + 1);
                    list.Remove(list.Last());
                }
                else
                {
                    _values.Add(new List<KeyValuePair<string, int>>(list));
                    list.Remove(list.Last());
                }
                
            }
            return;
        }
    }
}
