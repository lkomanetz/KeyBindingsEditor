using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.Settings.Models {

    public interface ISetting<T> {

        T Value { get; set; }
    }

}