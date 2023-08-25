using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Globomantics.Domain;
using Globomantics.Windows.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Globomantics.Windows.ViewModels;

public class MainViewModel : ObservableObject,
	IViewModel
{
	private string statusText = "Everything is OK!";
	private bool isLoading;
	private bool isInitialized;

	public string StatusText
	{
		get => statusText;
		set
		{
			statusText = value;

			OnPropertyChanged(nameof(StatusText));
		}
	}
	public bool IsLoading
	{
		get => isLoading;
		set
		{
			isLoading = value;

			OnPropertyChanged(nameof(IsLoading));
		}
	}

	public ICommand ExportCommand { get; set; }
	public ICommand ImportCommand { get; set; }

	public Action<string>? ShowAlert { get; set; }
	public Action<string>? ShowError { get; set; }
	public Func<IEnumerable<string>>? ShowOpenFileDialog { get; set; }
	public Func<string>? ShowSaveFileDialog { get; set; }
	public Func<string, bool>? AskForConfirmation { get; set; }
	public ObservableCollection<Todo> Completed { get; set; } = new();
	public ObservableCollection<Todo> Unfinished { get; set; } = new();

	public MainViewModel()
	{
		WeakReferenceMessenger.Default.Register<TodoSavedMessage>(this,
			(sender, message) =>
			{
				var item = message.Value;

				if (item.IsCompleted)
				{
					var existing = Unfinished.FirstOrDefault(i => i.Id == item.Id);
				}
			});

	}

	public async Task InitializeAsync()
	{
		if (isInitialized) return;

		isInitialized = true;
	}
}