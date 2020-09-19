using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SimpleDX11Renderer
{
    public class Debug
    {
        /// <summary>
        /// Logged messages
        /// </summary>
        private static List<DebugStructure> Logs = new List<DebugStructure>();

        /// <summary>
        /// Logged Warnings
        /// </summary>
        private static List<DebugStructure> Warnings = new List<DebugStructure>();

        /// <summary>
        /// Logged Errors
        /// </summary>
        private static List<DebugStructure> Erros = new List<DebugStructure>();

        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="Message"> Debug message </param>
        public static void Log(string Message)
        {
            Logs.Add(new DebugStructure(Message));
        }

        /// <summary>
        /// Send a warning message
        /// </summary>
        /// <param name="Message"> Debug message </param>
        public static void Warning(string Message)
        {
            Warnings.Add(new DebugStructure(Message));
        }

        /// <summary>
        /// Send a error message
        /// </summary>
        /// <param name="Message"> Debug message </param>
        public static void Error(string Message)
        {
            Erros.Add(new DebugStructure(Message));
        }

        /// <summary>
        /// Clear all messages
        /// </summary>
        public static void Clear()
        {
            Logs.Clear();
            Warnings.Clear();
            Erros.Clear();
        }

        /// <summary>
        /// Get logged messages
        /// </summary>
        public static List<DebugStructure> GetLogs()
        {
            return Logs;
        }

        /// <summary>
        /// Get warning messages
        /// </summary>
        public static List<DebugStructure> GetWarnings()
        {
            return Warnings;
        }

        /// <summary>
        /// Get errors messages
        /// </summary>
        public static List<DebugStructure> GetErrors()
        {
            return Erros;
        }
    }

    /// <summary>
    /// Debug Log/Warning/Error message structure
    /// </summary>
    public struct DebugStructure
    {
        public StackFrame[] StackFrames;
        public string Message;

        public DebugStructure(string Message)
        {
            this.Message = Message;
            StackFrames = new StackTrace(true).GetFrames();
        }
    }
}