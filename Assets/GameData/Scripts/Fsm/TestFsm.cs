#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace GameData
{
    public class TestFsm : MonoBehaviour
    {
        public void Start()
        {
            var fsmArr = new FsmState<TestFsm>[2] { new Fsm1(), new Fsm2() };
            var fsm = GameGod.Instance.FsmManager.CreateFsm(this, fsmArr);

            Debug.Log("fsm.CurrStateType=" + fsm.CurrStateType);

            fsm.ChangeState(0);

            Debug.Log("fsm.CurrStateType=" + fsm.CurrStateType);

            fsm.ChangeState(1);

            Debug.Log("fsm.CurrStateType=" + fsm.CurrStateType);

            GameGod.Instance.FsmManager.RelaseFsm(fsm.FsmId);
        }
    }

    public class Fsm1 : FsmState<TestFsm>
    {
        public override void OnInit()
        {
            Debug.Log("初始化Fsm1");
        }
        public override void OnEnter()
        {
            Debug.Log(CurrFsm.Owner.name + "进入Fsm1");
        }
        public override void OnUpdate()
        {
            Debug.Log("Update Fsm1");
        }
        public override void OnLeave()
        {
            Debug.Log("离开Fsm1");
        }
        public override void OnDestroy()
        {
            Debug.Log("销毁Fsm1");
        }
    }

    public class Fsm2 : FsmState<TestFsm>
    {
        public override void OnInit()
        {
            Debug.Log("初始化Fsm2");
        }
        public override void OnEnter()
        {
            Debug.Log("进入Fsm2");
        }
        public override void OnUpdate()
        {
            Debug.Log("Update Fsm2");
        }
        public override void OnLeave()
        {
            Debug.Log("离开Fsm2");
        }
        public override void OnDestroy()
        {
            Debug.Log("销毁Fsm2");
        }
    }
}
#endif