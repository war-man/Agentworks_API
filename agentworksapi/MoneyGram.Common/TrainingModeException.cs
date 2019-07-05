using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyGram.Common
{
    [Serializable]
    public class TrainingModeException : Exception
    {
        public TrainingModeException(string Message)
        {
            ErrorString = Message;
        }
        public string ErrorString { get; set; }
    }
}
