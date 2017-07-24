using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.Settings.ViewModels {

	public class KeyPadSettingsDesignViewModel {
		public IList<TreeMenuItem> SettingsMenu =>
			new List<TreeMenuItem>() {
				new TreeMenuItem() {
					Title = "Service Settings",
					Children = new List<TreeMenuItem>() {
						new TreeMenuItem() {
							Title = "Bindings File Location",
							Action = new DelegateCommand<object>((param) => { })
						}
					}
				},
				new TreeMenuItem() {
					Title = "KeyPad Settings",
					Action = new DelegateCommand<object>((param) => { })
				}
			};
	}

}
