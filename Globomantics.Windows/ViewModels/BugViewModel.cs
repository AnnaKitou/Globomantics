using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Globomantics.Domain;
using Globomantics.Infrastructure;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Globomantics.Windows.ViewModels
{
	public class BugViewModel : BaseTodoViewModel<Bug>
	{
		private readonly IRepository<Bug> _repository;
		private string? description;
		private string? affectedVersion;
		private int affectedUsers;
		private DateTimeOffset dueDate;
		private Severity severity;

		public string? Description
		{
			get => description;
			set
			{
				description = value;
				OnPropertyChanged(nameof(Description));
			}
		}
		public string? AffectedVersion
		{
			get => affectedVersion;
			set
			{
				affectedVersion = value;
				OnPropertyChanged(nameof(AffectedVersion));
			}
		}
		public int AffectedUsers
		{
			get => affectedUsers;
			set
			{
				affectedUsers = value;
				OnPropertyChanged(nameof(AffectedUsers));
			}
		}
		public DateTimeOffset DueDate
		{
			get => dueDate;
			set
			{
				dueDate = value;

				OnPropertyChanged(nameof(DueDate));
			}
		}
		public Severity Severity
		{
			get => severity;
			set
			{
				severity = value;

				OnPropertyChanged(nameof(Severity));
			}
		}

		public IEnumerable<Severity> SeverityLevels { get; } = new[]
		{
			Severity.Critical,
			Severity.Annoying,
			Severity.Major,
			Severity.Minor
		};

		public ObservableCollection<byte[]> Screenshots { get; set; } = new();
		public ICommand AttachScreenshotCommand { get; set; }

		public BugViewModel(IRepository<Bug> repository) : base()
		{
			_repository = repository;
			SaveCommand = new RelayCommand(async () => await SaveAsync());
			AttachScreenshotCommand = new RelayCommand(() =>
			{
				var filenames = ShowOpenFileDialog?.Invoke();

				if (filenames is null || !filenames.Any())
				{
					return;
				}

				foreach (var filename in filenames)
				{
					Screenshots.Add(File.ReadAllBytes(filename));
				}

			});
		}

		public override async Task SaveAsync()
		{
			if (string.IsNullOrEmpty(Title))
			{
				ShowError?.Invoke($"{nameof(Title)} cannot be empty");
				return;
			}

			if (Model is null)
			{
				Model = new Bug(Title,
					Description ?? "No descriptiopn",
					Severity,
					AffectedVersion,
					AffectedUsers,
					App.CurrentUser,
					App.CurrentUser,
					Screenshots.ToArray())
				{
					DueDate = DueDate,
					Parent = Parent,
					IsCompleted = IsCompleted
				};
			}
			else
			{
				Model = Model with
				{
					Title = Title,
					Description = Description ?? "No descriptiopn",
					Severity = Severity,
					AffectedVersion = AffectedVersion,
					AffectedUsers = AffectedUsers,
					DueDate = DueDate,
					Parent = Parent,
					IsCompleted = IsCompleted,
					Images = Screenshots.ToArray()
				};
			}

			await _repository.AddAsync(Model);

			await _repository.SaveChangesAsync();

		}

		public override void UpdateModel(Todo model)
		{
			if (model is not Bug bug) return;

			base.UpdateModel(bug);

			Description = bug.Description;
			AffectedVersion = bug.AffectedVersion;
			AffectedUsers = bug.AffectedUsers;
			DueDate = bug.DueDate;
			Severity = bug.Severity;
			DueDate=bug.DueDate;
		}
	}
}


