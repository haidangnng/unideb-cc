namespace QuestionLair.Web.Services;

public class ThemeContextService
{
    private bool _isDarkMode = false;

    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            if (_isDarkMode != value)
            {
                _isDarkMode = value;
                OnThemeChanged?.Invoke();
            }
        }
    }

    public event Action? OnThemeChanged;

    public void ToggleTheme()
    {
        IsDarkMode = !_isDarkMode;
    }

}
