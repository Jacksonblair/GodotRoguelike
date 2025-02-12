using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Godot;

namespace TESTCS.helpers.Commands;

public abstract partial class Command : GodotObject
{
    public abstract Task Execute();
    [Signal] public delegate void CommandFinishedEventHandler();
}

public class Commands
{
    private Queue<Command> _commands = new Queue<Command>();
    private bool _isExecuting = false;
    private bool _isInterrupted = false;

    public void Enqueue(Command command)
    {
        GD.Print("ENQUEUING ABILITY");
        
        _commands.Enqueue(command);
        if (!_isExecuting)
        {
            _isExecuting = true;
            ExecuteNext();
        }
    }

    public void Interrupt()
    {
        GD.Print("COMMANDS DISRUPTED");

        _isInterrupted = true;
        _commands.Clear();
        _isExecuting = false;
    }

    private void ExecuteNext()
    {
        while (_commands.Count > 0)
        {
            if (_isInterrupted)
            {
                _isExecuting = false;
                return;
            }

            var command = _commands.Dequeue();
            command.Execute();
            command.CommandFinished += OnCommandFinished;
        }

        _isExecuting = false; // Done processing
    }

    private void OnCommandFinished()
    {
        ExecuteNext();
    }
}