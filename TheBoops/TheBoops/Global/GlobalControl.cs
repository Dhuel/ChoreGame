using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBoops.Global
{
    public static class GlobalControl
    {
        private static string HashCode { get; set; }    
       
        public static void setHash(string _hashcode)
        {
            HashCode = _hashcode;
        }
        public static string getHash()
        {
            return HashCode;
        }
    }
}
