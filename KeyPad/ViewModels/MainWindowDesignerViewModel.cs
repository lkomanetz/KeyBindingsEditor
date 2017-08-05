using KeyPad.UserControls.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPad.ViewModels {

	public class MainWindowDesignerViewModel {

		public IList<CardViewModel> Cards => new List<CardViewModel>() {
			new CardViewModel() {
				Title = "Service",
				TitleActions = new List<TitleAction>() {
					new TitleAction() { ActionImage = $@"{Environment.CurrentDirectory}/IconImages/edit_icon.png" },
					new TitleAction() { ActionImage = $@"{Environment.CurrentDirectory}/IconImages/add_icon.png" }
				}
			},
			new CardViewModel() {
				Title = "Key Bindings",
				TitleActions = new List<TitleAction>() {
					new TitleAction() { ActionImage = $@"{Environment.CurrentDirectory}/IconImages/edit_icon.png" },
					new TitleAction() { ActionImage = $@"{Environment.CurrentDirectory}/IconImages/add_icon.png" }
				}
			}
		};

		public IForm FormViewContent = new KeyBindingsEditor.ViewModels.KeyBindingsEditorViewModel(null);

	}

}
