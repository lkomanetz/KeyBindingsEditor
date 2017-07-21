using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace KeyPad.KeyBindingsEditor.Controls {

	public class DisabledArrowsListView : ListView {

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			switch (e.Key) {
				case Key.Left:
				case Key.Up:
				case Key.Right:
				case Key.Down:
					e.Handled = true;
					return;
			}

			base.OnPreviewKeyDown(e);
		}
	}

}
