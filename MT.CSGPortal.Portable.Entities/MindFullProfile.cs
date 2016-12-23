using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.CSGPortal.Portable.Entities
{
    public class MindFullProfile
    {
        public MindFullProfile()
        {
            MindDetails = new Mind();
            MindContacts = new List<MindContact>();
        }

        public Mind MindDetails { get; set; }
        public IEnumerable<MindContact> MindContacts { get; set; }
    }
}
