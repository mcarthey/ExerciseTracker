using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using ExerciseTracker.Models;
using System.Linq;

namespace ExerciseTracker;

public partial class MainPage : ContentPage
{
    private DateTime _selectedDate = DateTime.Today;
    public ObservableCollection<ExerciseProgram> ExercisePrograms { get; set; }
    public ObservableCollection<ExerciseProgram> FilteredPrograms { get; set; } = new ObservableCollection<ExerciseProgram>();

    public ICommand OpenDatePickerCommand { get; }
    public ICommand OpenFlyoutCommand { get; }

    public ObservableCollection<ExerciseSet> TodayExerciseSets { get; set; }
    public ExerciseProgram TodayProgram { get; set; }

    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (_selectedDate != value)
            {
                _selectedDate = value;
                OnPropertyChanged();
                UpdateWorkoutsForDate(value); // Refresh your list, etc.
            }
        }
    }

    private void UpdateWorkoutsForDate(DateTime value)
    {
        // Implement as needed.
    }

    public MainPage()
    {
        InitializeComponent();
        LoadData();
        AssignDefaultIcons();

        // 1) Define commands
        OpenDatePickerCommand = new Command(() =>
        {
            HiddenDatePicker.IsVisible = true;
            HiddenDatePicker.InputTransparent = false; // so we can tap/focus
            Dispatcher.Dispatch(() => HiddenDatePicker.Focus());
        });

        OpenFlyoutCommand = new Command(() =>
        {
            Shell.Current.FlyoutIsPresented = true;
        });

        // 2) Now set the BindingContext once everything is ready
        BindingContext = this;

        // 3) Attach any gesture recognizers after
        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnHeaderPanUpdated;
        BlueHeader.GestureRecognizers.Clear();
        BlueHeader.GestureRecognizers.Add(panGesture);
    }

    // Variable to track the total vertical pan on the header
    double headerPanTotalY = 0;
    private void OnHeaderPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                headerPanTotalY = 0;
                break;
            case GestureStatus.Running:
                // Accumulate the vertical gesture distance
                headerPanTotalY = e.TotalY;
                break;
            case GestureStatus.Completed:
                // If the user swiped down more than 100 pixels, show the search overlay
                if (headerPanTotalY > 100)
                {
                    ShowSearchOverlay();
                }
                break;
        }
    }

    private void ShowSearchOverlay()
    {
        SearchOverlay.IsVisible = true;
        ProgramSearchBar.Focus();
    }

    private void CancelSearch_Clicked(object sender, EventArgs e)
    {
        // Hide the search overlay and clear the search text when canceled
        SearchOverlay.IsVisible = false;
        ProgramSearchBar.Text = string.Empty;
    }

    private void ProgramSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filter = e.NewTextValue?.ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(filter))
        {
            FilteredPrograms.Clear();
        }
        else
        {
            var filtered = ExercisePrograms.Where(p =>
                (!string.IsNullOrEmpty(p.Split) && p.Split.ToLowerInvariant().Contains(filter)) ||
                (!string.IsNullOrEmpty(p.Notes) && p.Notes.ToLowerInvariant().Contains(filter)));
            FilteredPrograms.Clear();
            foreach (var prog in filtered)
            {
                FilteredPrograms.Add(prog);
            }
        }
    }

    private async void ProgramSearchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        // When search is submitted, take an action (e.g., navigate to details or update UI)
        if (FilteredPrograms.Any())
        {
            await DisplayAlert("Search Submitted", $"{FilteredPrograms.Count} program(s) found.", "OK");
            // TODO: Implement navigation or display of selected program details.
        }
        else
        {
            SearchOverlay.IsVisible = false;
        }
    }

    private void HiddenDatePicker_Unfocused(object sender, FocusEventArgs e)
    {
        // Hide the DatePicker after selection or dismissal
        HiddenDatePicker.IsVisible = false;
        HiddenDatePicker.InputTransparent = true; // block taps again
    }

    public void AssignDefaultIcons()
    {
        foreach (var set in TodayExerciseSets)
        {
            set.Exercise.Icon = "default_icon";
        }
        OnPropertyChanged(nameof(TodayExerciseSets));
    }

    private void LoadData()
    {
        // Sample Exercises
        var pushUps = new Exercise { Id = Guid.NewGuid(), Name = "Pushups", MuscleGroup = "Chest" };
        var squats = new Exercise { Id = Guid.NewGuid(), Name = "Squats", MuscleGroup = "Legs" };

        // Sample Exercise Program
        var upperBodyProgram = new ExerciseProgram
        {
            Week = 1,
            Day = 1,
            Split = "Upper",
            Notes = "Focus on good form"
        };

        // Sample Exercise Sets within the Program
        var benchSet = new ExerciseSet
        {
            ExerciseProgram = upperBodyProgram,
            Exercise = pushUps,
            SetType = "Strength",
            Sets = 4,
            RepsMin = 8,
            RepsMax = 10,
            RpeRange = "7-9",
            RestInterval = "~3 min"
        };

        var squatSet = new ExerciseSet
        {
            ExerciseProgram = upperBodyProgram,
            Exercise = squats,
            SetType = "Hypertrophy",
            Sets = 3,
            RepsMin = 10,
            RepsMax = 12,
            RpeRange = "6-8",
            RestInterval = "~2 min"
        };

        // Link sets to program
        upperBodyProgram.ExerciseSets.Add(benchSet);
        upperBodyProgram.ExerciseSets.Add(squatSet);

        // Store the program and sets
        ExercisePrograms = new ObservableCollection<ExerciseProgram> { upperBodyProgram };
        TodayProgram = upperBodyProgram;
        TodayExerciseSets = new ObservableCollection<ExerciseSet>(upperBodyProgram.ExerciseSets);
    }
}
