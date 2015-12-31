using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_FalloutShelterVaultEditor
{
    class Dweller
    {
        private string _lastname = null;
        private int _tally = 0;

        public string LastName
        {
            get { return _lastname; }
            set { _lastname = value; }
        }

        public int Tally
        {
            get { return _tally; }
            set { _tally = value; }
        }

        public Dweller(string lastname)
        {
            _lastname = lastname;
            ++_tally;
        }
    }
}
