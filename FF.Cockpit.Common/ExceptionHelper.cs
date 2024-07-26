using System;

namespace FF.Cockpit.Common
{
    public class ExceptionHelper : Exception
    {
        #region Constructors
        /// <summary>
        /// Non parameterised Constructor
        /// </summary>
        public ExceptionHelper() : base()
        {; }
        /// <summary>
        /// Single Parameter Constructor
        /// </summary>
        /// <param name="message">Message string describe exception</param>
        public ExceptionHelper(string message) : base(message)
        {; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">message string describe exception</param>
        /// <param name="innerException">innerException decsribe the exception details .</param>
        public ExceptionHelper(string message, Exception innerException)
           : base(message, innerException)
        {; }
        #endregion
    }
}
