using System.Threading.Tasks;
using Godot;

namespace TESTCS.levels.transition_screens;

public abstract partial class Transition : Node2D
{
    public abstract Task TransitionIn();
    public abstract Task TransitionOut();
}