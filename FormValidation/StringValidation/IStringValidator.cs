using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicUploaderGUI
{
    public interface IStringValidator
    {
        /// <summary>
        /// Validates a string.
        /// </summary>
        /// <param name="value">The string to validate</param>
        /// <returns>True if valid, false if invalid</returns>
        bool Validate(string value);
        
    }
}
