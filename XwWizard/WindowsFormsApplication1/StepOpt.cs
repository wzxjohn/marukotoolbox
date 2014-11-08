using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XwWizard
{
    public class StepOpt
    {
        public StepOpt(bool cancel, bool prev, bool next)
        {
            this.Cancel = cancel;
            this.Prev = prev;
            this.Next = next;
        }

        public bool Cancel { get; set; }
        public bool Prev { get; set; }
        public bool Next { get; set; }
    }
}