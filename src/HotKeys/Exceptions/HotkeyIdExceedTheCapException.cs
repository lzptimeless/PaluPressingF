using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaluPressingF.HotKeys.Exceptions
{
    public class HotkeyIdExceedTheCapException : Exception
    {
        public HotkeyIdExceedTheCapException(string? msg)
            :base(msg)
        { }
    }
}
