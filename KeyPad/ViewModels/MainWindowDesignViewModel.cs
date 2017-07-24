using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.ViewModels {

	public class MainWindowDesignViewModel {

		public IList<IMenuItem> TopMenu => new List<IMenuItem>() {
				new TopBarMenuItem() {
					Title = "File",
					Children = new List<IMenuItem>() {
						new TopBarMenuItem() {
							Title = "New",
							Action = new DelegateCommand<object>((param) => { })
						},
						new TopBarMenuItem() {
							Title = "Open",
							Action = new DelegateCommand<object>((param) => { })
						},
						new TopBarMenuItem() {
							Title = "Settings",
							Children = new List<IMenuItem>() {
								new TopBarMenuItem() { Title = "Service settings" },
								new TopBarMenuItem() { Title = "KeyPad settings" }
							},
							Action = new DelegateCommand<object>((param) => { })
						},
						new TopBarMenuItem() {
							Title = "Exit",
							Action = new DelegateCommand<object>((param) => { })
						}
					}
				}
			};

	}

}
