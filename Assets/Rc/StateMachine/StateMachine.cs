using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FSM {

    /// <summary>
    /// 状態
    /// </summary>
    public class State : ScriptableObject {
        string stateName = "";

        public UnityAction onEnter      { private get; set; }
        public UnityAction onExecute    { private get; set; }
        public UnityAction onExit       { private get; set; }

        public string GetName() { return stateName; }

        public void OnEnter()   { onEnter?.Invoke(); }
        public void OnExecute() { onExecute?.Invoke(); }
        public void OnExit()    { onExit?.Invoke(); }

#if false
        public State() {
        }

        public State(string _name, UnityAction _onEnter, UnityAction _onExecute, UnityAction _onExit) {
            stateName = _name;
            onEnter = _onEnter;
            onExecute = _onExecute;
            onExit = _onExit;
        }
#endif
    }

    /// <summary>
    /// 遷移
    /// </summary>
    public struct Transition {
        public string transitionName;
        public State stateFrom;
        public State stateTo;

        public string GetName() { return transitionName; }

#if false
        public Transition(string _name, State _nameFrom, State _nameTo) {
            transitionName = _name;
            stateFrom = _nameFrom;
            stateTo = _nameTo;
        }
#endif
    }

    /// <summary>
    /// 超簡易状態マシン
    /// </summary>
    public class StateMachine : ScriptableObject {

        /// ===========================================================================
        /// 
        /// 型
        /// 
        /// ===========================================================================

        /// <summary>
        /// 状態マシンの処理状態
        /// </summary>
        enum MachineState {
            Entry,
            Transit,
            Execute,
        }

        /// ===========================================================================
        /// 
        /// 変数
        /// 
        /// ===========================================================================

        MachineState m_machineState = MachineState.Entry;
        State m_currentState = null;
        State m_nextState = null;

        Dictionary<string, State> m_states = new Dictionary<string, State>();
        Dictionary<string, Transition> m_transitions = new Dictionary<string, Transition>();

        /// ===========================================================================
        /// 
        /// 関数
        /// 
        /// ===========================================================================

        public void OnEnable() {
            
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update() {
            switch (m_machineState) {
                case MachineState.Entry:
                    break;

                case MachineState.Transit:
                    m_currentState?.OnExit();
                    m_nextState?.OnEnter();

                    m_currentState = m_nextState;
                    m_nextState = new State();
                    m_machineState = MachineState.Execute;
                    break;

                case MachineState.Execute:
                    m_currentState?.OnExecute();
                    break;
            }
        }

        /// <summary>
        /// 状態の追加
        /// </summary>
        /// <param name="_state"></param>
        public void AddState(State _state) {
            if (m_states.ContainsKey(_state.GetName())) {
                return;
            }
            m_states.Add(_state.GetName(), _state);
        }

        /// <summary>
        /// 状態をセット
        /// 遷移に関係なく状態を変更できる
        /// </summary>
        /// <param name="_name"></param>
        public void SetState(string _name) {
            if (!m_states.ContainsKey(_name)) {
                return;
            }
            if (m_machineState == MachineState.Transit) {
                return;
            }
            if (m_currentState?.GetName() == _name) {
                return;
            }
            m_machineState = MachineState.Transit;
            m_nextState = m_states[_name];
        }

        /// <summary>
        /// 遷移の追加
        /// </summary>
        /// <param name="_transition"></param>
        public void AddTransition(Transition _transition) {
            if (m_transitions.ContainsKey(_transition.GetName())) {
                return;
            }
            m_transitions.Add(_transition.GetName(), _transition);
        }

        /// <summary>
        /// 遷移の実行
        /// </summary>
        /// <param name="_transitName"></param>
        public void Transit(string _transitName) {
            if (!m_transitions.ContainsKey(_transitName)) {
                return;
            }
            Transition transition = m_transitions[_transitName];
            if (!m_states.ContainsKey(transition.stateFrom.GetName())) {
                return;
            }
            if (!m_states.ContainsKey(transition.stateTo.GetName())) {
                return;
            }
            if (m_currentState?.GetName() != transition.stateFrom.GetName()) {
                return;
            }
            SetState(transition.stateTo.GetName());
        }
    }

    public class Selection {
        public struct SelectObject {
            public uint id;
            public object userData;
        }

        public int m_selectMax = 1;
        public List<SelectObject> m_objects = new List<SelectObject>();
        public List<SelectObject> m_selectOjbects = new List<SelectObject>();

        public void Select(uint _id) {
            if (m_selectMax <= m_selectOjbects.Count) {
                return;
            }
            int idx = m_selectOjbects.FindIndex((_obj) => _obj.id == _id);
            if (0 <= idx) {
                return;
            }
            idx = m_objects.FindIndex((_obj) => _obj.id == _id);
            m_selectOjbects.Add(m_objects[idx]);
        }

        public void Unselect(uint _id) {
            int idx = m_selectOjbects.FindIndex((_obj) => _obj.id == _id);
            if (0 <= idx) {
                m_selectOjbects.RemoveAt(idx);
            }
        }

        public List<SelectObject> GetSelects() {
            return m_selectOjbects;
        }
    }


#if UNITY_EDITOR

    public class StateMachineEditor : EditorWindow {

        const string MENU_PATH = "RC/StateMachine";

        StateMachine m_fsm = null;

        [MenuItem(MENU_PATH)]
        static void Open() {
            StateMachineEditor editorWindow = EditorWindow.CreateInstance<StateMachineEditor> (); 
            editorWindow.Show ();
        }

        void OnGUI ()
        {

            if (m_fsm == null) {
                if (GUILayout.Button("StateMachine作成")) {
                    m_fsm = ScriptableObject.CreateInstance<StateMachine>();

                    string path = AssetDatabase.GenerateUniqueAssetPath("Assets/" + typeof(StateMachine) + ".asset");

                    AssetDatabase.CreateAsset(m_fsm, path);
                    AssetDatabase.SaveAssets();
                }
            }
            else {
                //var so = new SerializedObject(this);
                //EditorGUILayout.PropertyField(so.FindProperty("m_fsm"));
                
            }


            //EscでWindowを閉じる
            //if (Event.current.keyCode == KeyCode.Escape 
            //        && Event.current.type == EventType.KeyDown) {
            //    hoge.Close ();
            //}
        }
    }

#endif // UNITY_EDITOR
}
