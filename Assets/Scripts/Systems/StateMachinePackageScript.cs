using System;
using System.Collections.Generic;
using UnityEngine;

/*
                                  TUTORIAL!!!
  
  >> To Add a State Machine to your class. Follow these steps.
  
  >>* 1 * Create a new sub-class inhereting from StateMachine. Its Type parameters have to be equivilent to the MonoBehavior holding it and its own type.
  >>      An instance of this StateMachine will have to be created at runtime the static Create<>() method.
  >>      If there's anything you want to happen immediately after the StateMachine is created, create an override for OnInitialize(). (The same goes for States.)
  >>      You'll also want to call the StateMachine's Cleanup() method in Unity's OnDestroy() method so the State Objects aren't left floating around.
  >>      Example Below.
  
  public MyStateMachine stateMachine;
  public class MyStateMachine : StateMachine<OwnerClass, MyStateMachine>
  {
      
  }
  
  void Awake()
  {
      stateMachine = MyStateMachine.Create(this);
  }
  void OnDestroy()
  {
      stateMachine.Cleanup();
  }
  
  
  >>* 2 * Create a new sub-class inside the StateMachine inhereting from StateBase. 

      public class FirstState : StateBase
      {
          
      }
      public class SecondState : StateBase
      {
          
      }
  
  >> Each of these States you create can have unique Update(), OnEnterState(), and OnExitState() methods, as well as any other fields you wish to make use of.
  >> You could even have a StateMachine inside of a specific state. ;)
  
  >>* 3 * Create an Enum to cleanly identify every State you make. This can be defined in the owner MonoBehavior or in the machine (the former is cleaner.)
  
      public enum States { FirstState, SecondState };
  
  >>* 4 * Create the StateMachine's InitializeStates() method Within this method, you must, for every State ID in to The State ID Enum, in order,
  >>      Run RegisterState<>() using the type of the States defined in the StateMachine, as shown below.
  
      protected override void InitializeStates()
      {
           RegisterState<FirstState>();
           RegisterState<SecondState>();
      }
  
  >> That should be it. Happy State Machining!
  
  
  
  
*/


namespace StateMachineSLS
{
    /// <summary>
    /// Custom State Machine system designed by StarLightShadows.
    /// </summary>
    public abstract class StateMachine<O, M> where O : MonoBehaviour where M : StateMachine<O, M>
    {
        public O owner;

        public static M Create(O owner)
        {
            M machine = Activator.CreateInstance<M>();
            machine.owner = owner;
            machine.InitializeStates();
            machine.OnInitialize();
            return machine;
        }


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
        public int currentStateID;

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
        public T CurrentStateOB<T>() where T : StateBase
        {
            T _currentStateObject = (T)stateObjectRefs[(int)currentStateID];
            if (_currentStateObject.GetType() != typeof(T)) return null;
            return _currentStateObject;
        }
        /// <summary>
        /// Returns the current State (Object) of this state machine. Cast this to the different State Types and use for finer control over the States' sub-conditions. 
        /// </summary> 
        /// <param name="state"> The specific State Object you are trying to recieve. (ID Enum.) (Returns null if State is not Active.) </param>
        /// <returns> NULL if the States have been set up incorrectly OR the State specified in Parameters is not active. </returns>
        public StateBase CurrentStateOB(int state)
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
        public T CurrentStateOB<T>(int state) where T : StateBase
        {
            T _currentStateObject = (T)stateObjectRefs[(int)currentStateID];
            if (state != currentStateID || state.GetType() != typeof(T)) return null;
            return _currentStateObject;
        }

        /// <summary>
        /// Gets a specific State (Object). Use for finer control over the States' sub-conditions. Does NOT ensure this state is the currently running one, so use wisely.
        /// </summary>
        /// <typeparam name="T">The State Type you are asking for.</typeparam>
        /// <returns>The State (Object) of Type T. (Assuming you set up InitializeStates() correctly, of course.)</returns>
        public T GetState<T>() where T : StateBase
        {
            for (int i = 0; i < stateObjectRefs.Count; i++)
            {
                if (stateObjectRefs[i].GetType() == typeof(T)) return (T)stateObjectRefs[i];
            }
            return null;
        }
        /// <summary>
        /// Gets a specific State (Object). Use for finer control over the States' sub-conditions. Does NOT ensure this state is the currently running one, so use wisely.
        /// </summary>
        /// <param name="state">The specific State Object you are trying to recieve. (ID Enum.)</param>
        /// <returns>The State (Object) based on the State (ID Enum) given. (Assuming you set up InitializeStates() correctly, of course.)</returns>
        public StateBase GetState(int state) => stateObjectRefs[(int)state];



        private List<StateBase> stateObjectRefs = new();

        /// <summary>
        /// Registers a newly created state with the List of States. Make sure to call in the same order as the State IDs are listed in the State ID Enum.
        /// </summary>
        /// <param name="newState"> The newly created State. Has to be created using a constructor like so "new STATECLASSNAME(owner)"</param>
        protected void RegisterState<T>() where T : StateBase
        {
            T newState = Activator.CreateInstance<T>();
            newState.owner = owner;
            newState.machine = (M)this;
            newState.OnInitialize();
            stateObjectRefs.Add(newState);
        }

        /// <summary>
        /// This changes the state from one state to another. Running OnExitState() on the previous State and OnEnterState() on the new State.
        /// </summary>
        /// <param name="state">The State (ID Enum) that you wish to switch to.</param>
        /// <returns> the State Object of the new state activated. </returns>
        public StateBase ChangeState(int state)
        {
            CurrentStateOB().OnExitState();
            currentStateID = state;
            CurrentStateOB().OnEnterState();
            return CurrentStateOB();
        }
        /// <summary>
        /// This changes the state from one state to another. Running OnExitState() on the previous State and OnEnterState() on the new State.
        /// </summary>
        /// <typeparam name="T">The State Type you wish to switch to.</param>
        /// <returns> the State Object of the new state activated. </returns>
        public StateBase ChangeState<T>()
        {
            CurrentStateOB().OnExitState();
            for (int i = 0; i < stateObjectRefs.Count; i++)
                if (stateObjectRefs[i].GetType() == typeof(T))
                {
                    currentStateID = i;
                    break;
                }
            CurrentStateOB().OnEnterState();
            return CurrentStateOB();
        }

        /// <summary>
        /// Runs upon creation of the StateMachine after InitializeStates.
        /// </summary>
        public virtual void OnInitialize() { }
        /// <summary>
        /// The Update for the StateMachine. Runs the Update function of whichever State is currently running.<para />
        /// IMPORTANT: If Overridden, call base.Update(); at some point during, or else the equivilent method on the current State won't run.
        /// </summary>
        public virtual void Update() => CurrentStateOB().Update();
        /// <summary>
        /// The FixedUpdate for the StateMachine. Runs the FixedUpdate function of whichever State is currently running.<para />
        /// IMPORTANT: If Overridden, call base.FixedUpdate(); at some point during, or else the equivilent method on the current State won't run.
        /// </summary>
        public virtual void FixedUpdate() => CurrentStateOB().FixedUpdate();







        /// <summary>
        /// The base State for a State Machine. If you use this as an actual state in your state machine, it's equivilent to having a dummy state that does literally nothing.
        /// </summary>
        public class StateBase
        {
            public O owner;
            public M machine;

            public virtual void OnInitialize() { }
            public virtual void OnEnterState() { }
            public virtual void Update() { }
            public virtual void FixedUpdate() { }
            public virtual void OnExitState() { }
        }

        /*
        ExampleState _exampleState;
        public class ExampleState : StateBase
        {

        }
         */


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
