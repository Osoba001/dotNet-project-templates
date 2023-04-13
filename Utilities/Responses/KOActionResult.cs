using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Responses
{
    /// <summary>
    /// An Action result that is used to return any type
    /// </summary>
    public class KOActionResult
    {
        /// <summary>
        /// Set IsSuccess property to be true on initialize.
        /// </summary>
        public KOActionResult()
        {
            ErrorMessagesList = new();
            IsSuccess = true;
        }
        /// <summary>
        /// Add error message to the error message collection and set IsSuccess property to be false
        /// </summary>
        /// <param name="error">Error message</param>
        public void AddError(string error)
        {
            ErrorMessagesList.Add(error);
            IsSuccess = false;
        }
        /// <summary>
        /// Return all the error message in the error message collection as a single tring
        /// </summary>
        public string ReasonPhrase => GetRessErrors();
        public bool IsSuccess { get; private set;}
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
    /// <summary>
    /// A generic action result used when a type is required in the result and the type must be a class
    /// </summary>
    /// <typeparam name="T">Requred type in the action result</typeparam>
    public class KOActionResult<T> : KOActionResult 
    {
        public T? Item { get; set; }

    }
}

