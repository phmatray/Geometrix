namespace Geometrix.Application.Services;

/// <summary>
/// </summary>
public sealed class Notification
{
    /// <summary>
    ///     Error message.
    /// </summary>
    private readonly IDictionary<string, IList<string>> _errorMessages = new Dictionary<string, IList<string>>();

    public IDictionary<string, string[]> ModelState
    {
        get
        {
            var modelState = _errorMessages
                .ToDictionary(item => item.Key, item => item.Value.ToArray());

            return modelState;
        }
    }


    /// <summary>
    ///     Returns true when it does not contains error messages.
    /// </summary>
    public bool IsValid => _errorMessages.Count == 0;

    public bool IsInvalid => _errorMessages.Count > 0;

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="message"></param>
    public void Add(string key, string message)
    {
        if (!_errorMessages.ContainsKey(key))
        {
            _errorMessages[key] = new List<string>();
        }

        _errorMessages[key].Add(message);
    }
}