using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taipu
{
    public interface Scene
    {
        public void Load() { }
        public void Update() { }
        public void Draw() { }
    }
}
