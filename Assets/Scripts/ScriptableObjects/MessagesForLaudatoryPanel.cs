using UnityEngine;

[CreateAssetMenu(fileName = "new Message", menuName = "SO/Message")]
public class MessagesForLaudatoryPanel : ScriptableObject
{
    [SerializeField] private string[] _messages;

    private int _index = -1;

    public string GetNextMessage()
    {
         _index++;
        CheckIndex();
        return _messages[_index];
    }

    public void Reset()
    {
        _index = -1;
    }

    public string GetCurrentMessage()
    {
        CheckIndex();
        return _messages[_index];
    }

    public void CheckIndex()
    {
        if (_index >= _messages.Length)
        {
            _index = _messages.Length - 1;
        }
        else if (_index < 0)
        {
            _index = 0;
        }
    }
}

