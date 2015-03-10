using System;
using System.Text;

namespace Mojio.Gamification.Core
{
	public class Logger
	{
		public enum Level
		{
			INFO,
			WARNING,
			ERROR,
			DEBUG
		}

		private static Logger _instance;

		public static Logger GetInstance()
		{
			if (_instance == null) {
				_instance = new Logger ();
			}
			return _instance;
		}

		private Logger()
		{
		}
			
		private void write (Level level, string message)
		{
			StringBuilder sb = new StringBuilder ();
			sb.Append ("[");
			sb.Append (DateTime.Now.ToString ("HH:mm:ss tt zzz"));
			sb.Append ("] ");
			sb.Append ("[");
			sb.Append (level.ToString ());
			sb.Append ("] ");
			sb.Append (message);
			Console.WriteLine (sb.ToString ());
		}

		public void Debug (string message)
		{
			write (Level.DEBUG, message); 
		}

		public void Info (string message)
		{
			write (Level.INFO, message); 
		}

		public void Warning (string message)
		{
			write (Level.WARNING, message); 
		}

		public void Error (string message)
		{
			write (Level.ERROR, message); 
		}


	}
}

