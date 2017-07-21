using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyPad.KeyBindingsEditor.Converters {

	public class KeyboardButtonToStringConverter {
		private static int ESCAPE_KEY_CODE = 27;

		public static string Convert(int keyCode) {
			if (keyCode == -1 || keyCode == ESCAPE_KEY_CODE)
				return "<NOT MAPPED>";

			string returnVal = String.Empty;

			returnVal = ConvertSpecialKey(keyCode);
			if (!String.IsNullOrEmpty(returnVal))
				return returnVal;

			return ((char)keyCode).ToString();
		}

		private static string ConvertSpecialKey(int kbBtn) {
			if (IsAlphaNumeric(kbBtn))
				return String.Empty;

			switch ((Keys)kbBtn) {
				case Keys.Left: return @"LEFT ARROW";
				case Keys.Up: return @"UP ARROW";
				case Keys.Right: return @"RIGHT ARROW";
				case Keys.Down: return @"DOWN ARROW";
				case Keys.Enter: return @"ENTER KEY";
				case Keys.CapsLock: return @"CAPS LOCK";
				case Keys.Tab: return @"TAB KEY";
				case Keys.LShiftKey: return @"LEFT SHIFT KEY";
				case Keys.RShiftKey: return @"RIGHT SHIFT KEY";
				case Keys.LControlKey: return @"LEFT CONTROL KEY";
				case Keys.RControlKey: return @"RIGHT CONTROL KEY";
				case Keys.Space: return @"SPACEBAR";
				default: return String.Empty;
			}
		}

		private static bool IsAlphaNumeric(int kbBtn) {
			Match match = Regex.Match(((char)kbBtn).ToString(), @"[0-9a-zA-Z]");
			return match.Success;
		}

	}

}
