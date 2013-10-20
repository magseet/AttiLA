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
        WaitForUser,
        WaitFirstPrediction,
        WaitNotifications,
        WaitForUser2,
        Tracking,
        Unknown
    }

    public class ProcessState
    {
        public State Name { get; set; }

        public String Expected { get; set; }
    }

    public enum Command
    {
        UserSignal,
        Right,
        Wrong,
        None
    }

    public class Process
    {
        class StateTransition
        {
            readonly ProcessState CurrentState;
            readonly Command Command;

            public StateTransition(ProcessState currentState, Command command)
            {
                CurrentState = new ProcessState() { Name = currentState.Name, Expected = currentState.Expected };
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

        Dictionary<StateTransition, ProcessState> transitions;
        public ProcessState CurrentState { get; private set; }

        public Process()
        {
            CurrentState = new ProcessState { Name = State.Idle, Expected = null };
            transitions = new Dictionary<StateTransition, ProcessState>
            {
                { new StateTransition(new ProcessState { Name = State.WaitForUser, Expected = null  }, Command.None), new ProcessState { Name = State.WaitForUser, Expected = null }},
                { new StateTransition(new ProcessState { Name = State.WaitForUser, Expected = null  }, Command.UserSignal), new ProcessState { Name = State.WaitFirstPrediction, Expected = null }},
                { new StateTransition(new ProcessState { Name = State.WaitFirstPrediction, Expected = null  }, Command.Right), new ProcessState { Name = State.WaitNotifications, Expected = null }},
                { new StateTransition(new ProcessState { Name = State.WaitFirstPrediction, Expected = null  }, Command.None), new ProcessState { Name = State.WaitForUser, Expected = null }},
                { new StateTransition(new ProcessState { Name = State.WaitNotifications, Expected = null  }, Command.Wrong), new ProcessState { Name = State.WaitForUser2, Expected = null }},                
                { new StateTransition(new ProcessState { Name = State.WaitNotifications, Expected = null  }, Command.None), new ProcessState { Name = State.WaitFirstPrediction, Expected = null }},
                { new StateTransition(new ProcessState { Name = State.WaitForUser2, Expected = null  }, Command.Wrong), new ProcessState { Name = State.Tracking, Expected = null }},
                { new StateTransition(new ProcessState { Name = State.WaitForUser2, Expected = null  }, Command.None), new ProcessState { Name = State.WaitFirstPrediction, Expected = null }},
                { new StateTransition(new ProcessState { Name = State.Tracking, Expected = null  }, Command.None), new ProcessState { Name = State.WaitNotifications, Expected = null }}
            };
        }

        public ProcessState GetNext(Command command)
        {
            StateTransition transition = new StateTransition(CurrentState, command);
            ProcessState nextState;
            if (!transitions.TryGetValue(transition, out nextState))
                throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
            return nextState;
        }

        public ProcessState MoveNext(Command command)
        {
            CurrentState = GetNext(command);
            return CurrentState;
        }
    }
}
