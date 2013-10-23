using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleDA
{
    public enum State
    {
        Idle,
        WaitForSelection,
        WaitForCorrectPrediction,   
        WaitForWrongPrediction,
        WaitForConfirmation,
        Tracking,
        Unknown,
        Ignore
    }

    public enum Command
    {
        Selection,
        Confirmation,
        CorrectPrediction,
        WrongPrediction,
        None
    }

    public class Process
    {
        class StateTransition
        {
            readonly State CurrentState;
            readonly Command Command;

            public StateTransition(State currentState, Command command)
            {
                CurrentState = currentState;
                Command = command;
            }

            //ToDO
            public override int GetHashCode()
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
            }

            //ToDO
            public override bool Equals(object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
            }
        }

        Dictionary<StateTransition, State> transitions;
        public State CurrentState { get; private set; }

        public Process()
        {
            //States under timer : WaitForPrediction, Tracking
            CurrentState = State.Idle;
            transitions = new Dictionary<StateTransition, State>
            {
                { new StateTransition(State.Idle, Command.Selection), State.WaitForSelection },
                { new StateTransition(State.Idle, Command.Confirmation), State.WaitForConfirmation },
                { new StateTransition(State.WaitForSelection, Command.Selection), State.WaitForCorrectPrediction },
                { new StateTransition(State.WaitForSelection, Command.CorrectPrediction), State.WaitForWrongPrediction },
                //{ new StateTransition(State.WaitForCorrectPrediction, Command.CorrectPrediction), State.WaitForWrongPrediction },
                //{ new StateTransition(State.WaitForCorrectPrediction, Command.None),State.WaitForSelection }, //Caso sfigato
                //{ new StateTransition(State.WaitForWrongPrediction,Command.WrongPrediction), State.WaitForConfirmation },                
                //{ new StateTransition(State.WaitForWrongPrediction,Command.Selection), State.WaitForCorrectPrediction },
                //{ new StateTransition(State.WaitForConfirmation,  Command.Confirmation), State.Tracking  },
                //{ new StateTransition(State.WaitForConfirmation,  Command.Selection), State.WaitForCorrectPrediction  },
                //{ new StateTransition(State.WaitForConfirmation,  Command.Selection), State.WaitForWrongPrediction  },
                //{ new StateTransition(State.Tracking, Command.None), State.WaitForWrongPrediction },
                //{ new StateTransition(State.Tracking, Command.Selection), State.WaitForCorrectPrediction },
                { new StateTransition(State.Tracking,  Command.CorrectPrediction), State.WaitForWrongPrediction  }

            };
        }

        public State GetNext(Command command)
        {
            StateTransition transition = new StateTransition(CurrentState, command);
            State nextState;
            if (!transitions.TryGetValue(transition, out nextState))
                return State.Ignore;

            return nextState;
        }

        public State MoveNext(Command command)
        {
            State nextState = GetNext(command);
            if(nextState == null)
                return State.Ignore;

            CurrentState = nextState;
            return CurrentState;
        }
    }
}
