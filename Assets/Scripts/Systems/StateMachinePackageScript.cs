using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                                  TUTORIAL!!!
  
  >> To Add a State Machine to your class. Follow these steps.
  
  >>* 1 * Create a new sub-class inhereting from StateMachine, as well as its necessary Constructor. 
  >>      An instance of this StateMachine will have to be created at runtime using the Constructor.
  >>      If there's anything you want to happen immediately after the StateMachine is created, add it to the empty curly brackets in the constructor. (The same goes for States.)
  >>      You'll also want to call the StateMachine's Cleanup() method in Unity's OnDestroy() method so the State Objects aren't left floating around.
  >>      Example Below.
  
  public MyStateMachine stateMachine;
  public class MyStateMachine : StateMachine
  {
      public MyStateMachine(MonoBehaviour owner) : base(owner){}
  
  }
  
  void Awake()
  {
      stateMachine = new MyStateMachine(this);
  }
  void OnDestroy()
  {
      stateMachine.Cleanup();
  }
  
  
  >>* 2 * Create a new sub-class inside the StateMachine inhereting from StateBase. All states will also need similar constructors.

      public class FirstState : StateBase
      {
          public FirstState(MonoBehaviour owner) : base(owner){}
      }
      public class SecondState : StateBase
      {
          public SecondState(MonoBehaviour owner) : base(owner){}
      }
  
  >> Each of these States you create can have unique Update(), OnEnterState(), and OnExitState() methods, as well as any other fields you wish to make use of.
  >> You could even have a StateMachine inside of a specific state. ;)
  
  >>* 3 * Create an Enum to cleanly identify every State you make.
  
      public new enum State { FirstState, SecondState };
  
  >>* 4 * Create the StateMachine's InitializeStates() method Within this method, you must, for every State ID in to The State ID Enum, in order,
  >>      Run RegisterState() and feed in a constructor call for the appropriate state class, as shown below.
  
      protected override void InitializeStates()
      {
           RegisterState(new FirstState(owner));
           RegisterState(new SecondState(owner));
      }
  
  >> That should be it. Happy State Machining!
  
  
  
  
*/


namespace StateMachineSLS
{
    /// <summary>
    /// Custom State Machine system designed by StarLightShadows.
    /// </summary>
    public abstract class StateMachine
    {
        public MonoBehaviour owner;
        public StateMachine(MonoBehaviour owner)
        {
            this.owner = owner;
            InitializeStates();
        }

        /// <summary>
        /// The ID Enum collection for this state machine. Make sure to setup InitializeStates() identically to this.
        /// </summary>
        public enum State { ExampleState }
        /// <summary>
        /// Initializer for the possible States of this State Machine. Necessary to set up correctly. See Tutorial for Information. (In Script File above Implementation.)
        /// </summary>
        protected abstract void InitializeStates();
        /// <summary>
        /// Make sure to call this when the MonoBehavior that ultimately owns the State Machine is destroyed, or otherwise you'll have State Objects floating around in memory that can accumulate and cause lag. See Tutorial for Information. (In Script File above Implementation.)
        /// </summary>
        public void Cleanup() => stateObjectRefs.Clear();

        /// <summary>
        /// The current state (ID Enum) of this state machine. Use this for comparisons.
        /// </summary>
        public State currentStateID;

        /// <summary>
        /// Returns the current State (Object) of this state machine. Cast this to the different State Types and use for finer control over the States' sub-conditions. 
        /// </summary> 
        /// <returns> NULL if the States have been set up incorrectly. </returns>
        public StateBase CurrentStateOB() => stateObjectRefs[(int)currentStateID];
        /// <summary>
        /// Returns the current State (Object) of this state machine. Cast this to the different State Types and use for finer control over the States' sub-conditions. 
        /// </summary> 
        /// <typeparam name="T">The specific State Type. (Returns null if State is not Active.)</typeparam>
        /// <returns> NULL if the States have been set up incorrectly OR the State specified in Parameters is not active. </returns>
        public StateBase CurrentStateOB<T>()
        {
            StateBase _currentStateObject = stateObjectRefs[(int)currentStateID];
            if (_currentStateObject.GetType() != typeof(T)) return null;
            return _currentStateObject;
        }
        /// <summary>
        /// Returns the current State (Object) of this state machine. Cast this to the different State Types and use for finer control over the States' sub-conditions. 
        /// </summary> 
        /// <param name="state"> The specific State Object you are trying to recieve. (ID Enum.) (Returns null if State is not Active.) </param>
        /// <returns> NULL if the States have been set up incorrectly OR the State specified in Parameters is not active. </returns>
        public StateBase CurrentStateOB(State state)
        {
            if (state != currentStateID) return null;
            return stateObjectRefs[(int)currentStateID];
        }
        /// <summary>
        /// Returns the current State (Object) of this state machine. Cast this to the different State Types and use for finer control over the States' sub-conditions. 
        /// </summary> 
        /// <typeparam name="T">The specific State Type. (Returns null if State is not Active.)</typeparam>
        /// <param name="state"> The specific State Object you are trying to recieve. (ID Enum.) (Returns null if State is not Active.) </param>
        /// <returns> NULL if the States have been set up incorrectly OR the State specified in Parameters is not active. </returns>
        public StateBase CurrentStateOB<T>(State state)
        {
            StateBase _currentStateObject = stateObjectRefs[(int)currentStateID];
            if (state != currentStateID || state.GetType() != typeof(T)) return null;
            return _currentStateObject;
        }

        /// <summary>
        /// Gets a specific State (Object). Use for finer control over the States' sub-conditions. Does NOT ensure this state is the currently running one, so use wisely.
        /// </summary>
        /// <typeparam name="T">The State Type you are asking for.</typeparam>
        /// <returns>The State (Object) of Type T. (Assuming you set up InitializeStates() correctly, of course.)</returns>
        public StateBase GetState<T>()
        {
            for (int i = 0; i < stateObjectRefs.Count; i++)
            {
                if (stateObjectRefs[i].GetType() == typeof(T)) return stateObjectRefs[i];
            }
            return null;
        }
        /// <summary>
        /// Gets a specific State (Object). Use for finer control over the States' sub-conditions. Does NOT ensure this state is the currently running one, so use wisely.
        /// </summary>
        /// <param name="state">The specific State Object you are trying to recieve. (ID Enum.)</param>
        /// <returns>The State (Object) based on the State (ID Enum) given. (Assuming you set up InitializeStates() correctly, of course.)</returns>
        public StateBase GetState(State state)
        {
            return stateObjectRefs[(int)state];
        }
        


        private List<StateBase> stateObjectRefs = new List<StateBase>();

        /// <summary>
        /// Registers a newly created state with the List of States. Make sure to call in the same order as the State IDs are listed in the State ID Enum.
        /// </summary>
        /// <param name="newState"> The newly created State. Has to be created using a constructor like so "new STATECLASSNAME(owner)"</param>
        protected void RegisterState(StateBase newState)=> stateObjectRefs.Add(newState);

        /// <summary>
        /// This changes the state from one state to another. Running OnExitState() on the previous State and OnEnterState() on the new State.
        /// </summary>
        /// <param name="state">The State (ID Enum) that you wish to switch to.</param>
        public void ChangeState(State state)
        {
            CurrentStateOB().OnExitState();
            currentStateID = state;
            CurrentStateOB().OnEnterState();
        }
        /// <summary>
        /// This changes the state from one state to another. Running OnExitState() on the previous State and OnEnterState() on the new State.
        /// </summary>
        /// <param name="state">The State (ID Enum) that you wish to switch to.</param>
        /// <returns> the State Object of the new state activated. </returns>
        public StateBase ChangeStateReturn(State state)
        {
            CurrentStateOB().OnExitState();
            currentStateID = state;
            CurrentStateOB().OnEnterState();
            return CurrentStateOB();
        }

        /// <summary>
        /// Runs the Update function of whichever State is currently running. I can't really imagine where else you'd use this besides Unity's Update function.
        /// </summary>
        public void Update() => CurrentStateOB().Update();
        /// <summary>
        /// Runs the FixedUpdate function of whichever State is currently running. I can't really imagine where else you'd use this besides Unity's FixedUpdate function.
        /// </summary>
        public void FixedUpdate() => CurrentStateOB().FixedUpdate();





        

        /// <summary>
        /// The base State for a State Machine. If you use this as an actual state in your state machine, it's equivilent to having a dummy state that does literally nothing.
        /// </summary>
        public class StateBase
        {
            public MonoBehaviour owner;

            public StateBase(MonoBehaviour owner) => this.owner = owner;

            public virtual void OnEnterState() { }
            public virtual void Update() { }
            public virtual void FixedUpdate() { }
            public virtual void OnExitState() { }
        }

        ExampleState _exampleState;
        public class ExampleState : StateBase
        {
            public ExampleState(MonoBehaviour owner) : base(owner) { }
        }




        // Generic Attempt
        // This would've made everything infinitely easier.

        // /// <summary>
        // /// This creates a State object and adds it to the stateObjectRefs. Ensure this Method is called once for every state named in the State enum, in order.
        // /// </summary>
        // /// <param name="state"> This is the State Object slot you need to pass in to create the new state.</param>
        // protected void CreateState<StateT>(ref StateT state)
        // {
        //     StateBase<StateT> NEWSTATE = new StateBase<StateT>(this.owner);
        //     state = (StateT)Convert.ChangeType(NEWSTATE, typeof(StateT));
        //     stateObjectRefs.Add(state);
        // }
        //private class StateList<StateT> where StateT : StateBase<StateT>
        //{
        //    List<StateT> list = new List<StateT>();
        //
        //    public void Add(StateT state) => list.Add(state);
        //}
        // /// <summary>
        // /// The base State for a State Machine. If you use this as an actual state in your state machine, it's equivilent to having a dummy state that does literally nothing.
        // /// </summary>
        // public class StateBase<StateType>
        // {
        //     public MonoBehaviour owner;
        // 
        //     public StateBase(MonoBehaviour owner) => this.owner = owner;
        // 
        //     public virtual void OnEnterState() { }
        //     public virtual void Update() { }
        //     public virtual void OnExitState() { }
        // }

    }
}
