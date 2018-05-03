using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Communication
{
    class SIComm
    {
        private static SIComm instance;

        private SIComm()
        {

        }

        public static SIComm Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SIComm();
                }
                return instance;
            }
        }
    }
}
