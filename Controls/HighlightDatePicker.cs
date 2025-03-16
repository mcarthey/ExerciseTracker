namespace ExerciseTracker.Controls;

public class HighlightDatePicker : DatePicker
{
    public static readonly BindableProperty HighlightedDatesProperty =
        BindableProperty.Create(nameof(HighlightedDates), typeof(IList<DateTime>), typeof(HighlightDatePicker), new List<DateTime>());

    public IList<DateTime> HighlightedDates
    {
        get => (IList<DateTime>)GetValue(HighlightedDatesProperty);
        set => SetValue(HighlightedDatesProperty, value);
    }
}