using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace KeyPad.KeyBindingsEditor.Converters {

	public class WidthConverter : IValueConverter {

		public object Convert(object obj, Type type, object parameter, CultureInfo culture) {
			double totalWidth = 0D;

			ListView lv = obj as ListView;
			GridView gv = lv.View as GridView;
			
			for (int i = 0; i < gv.Columns.Count; ++i) {
				totalWidth += gv.Columns[i].ActualWidth;
			}

			return lv.ActualWidth - totalWidth;
		}

		public object ConvertBack(object obj, Type type, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}

	}

}
