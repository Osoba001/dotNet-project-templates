using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Responses
{
    public class KOActionResult
    {
        public KOActionResult()
        {
            ErrorMessagesList = new List<string>();
            IsSuccess = true;
        }
        public void AddError(string error)
        {
            ErrorMessagesList.Add(error);
            IsSuccess = false;
        }
        public string ReasonPhrase => GetRessErrors();
        public bool IsSuccess { get; set; }
        private List<string> ErrorMessagesList { get; set; }

        private string GetRessErrors()
        {
            StringBuilder er = new();
            foreach (var error in ErrorMessagesList)
            {
                er.AppendLine(error);
            }
            return er.ToString();
        }
        public object? data { get; set; }
    }
    public class KOActionResult<T> : KOActionResult where T : class
    {
        public T? Item { get; set; }

    }
}

