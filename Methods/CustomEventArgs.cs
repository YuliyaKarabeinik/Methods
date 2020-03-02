using System;

namespace Methods
{
    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(bool onlyFiltered, bool finishAfterFirstMatch)
        {
            OnlyFiltered = onlyFiltered;
            FinishAfterFirstMatch = finishAfterFirstMatch;
        }

        public bool OnlyFiltered { get; set; }

        public bool FinishAfterFirstMatch { get; set; }
    }
}
