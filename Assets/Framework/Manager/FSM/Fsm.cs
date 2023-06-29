/*********************************************
 * BFramework
 * 状态机
 * 创建时间：2023/04/06 15:00:23
 *********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Framework
{
	/// <summary>
	/// 状态机
	/// </summary>
	/// <typeparam name="T">FSMManager</typeparam>
	public class Fsm<T> : FsmBase where T : class
	{
		/// <summary>
		/// 状态机拥有者
		/// </summary>
		public T Owner { get; private set; }

		/// <summary>
		/// 状态字典
		/// </summary>
		private Dictionary<sbyte, FsmState<T>> _stateDic;

		/// <summary>
		/// 当前状态
		/// </summary>
		private FsmState<T> _currState;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="fsmId">状态机编号</param>
		/// <param name="owner">拥有者</param>
		/// <param name="states">状态数组</param>
		public Fsm(int fsmId, T owner, FsmState<T>[] states) : base(fsmId)
		{
			_stateDic = new Dictionary<sbyte, FsmState<T>>();
			Owner = owner;

			//把状态加入字典
			int len = states.Length;
			for (int i = 0; i < len; i++)
			{
				FsmState<T> state = states[i];
				state.CurrFsm = this;
				state.OnInit();
				_stateDic[(sbyte)i] = state;
			}

			//设置默认状态
			CurrStateType = -1;
		}

        public override void OnUpdate()
		{
			_currState?.OnUpdate();
		}

		/// <summary>
		/// 获取状态
		/// </summary>
		/// <param name="stateType">状态Type</param>
		/// <returns>状态</returns>
		public FsmState<T> GetState(sbyte stateType)
		{
			FsmState<T> state = null;
			_stateDic.TryGetValue(stateType, out state);
			return state;
		}

		/// <summary>
		/// 切换状态
		/// </summary>
		/// <param name="newState"></param>
		public void ChangeState(sbyte newState)
		{
			if (CurrStateType == newState) return;

			//设置为默认时就已经离开过了 不离开第二次
			if (CurrStateType != -1 && _currState != null)
			{
				_currState.OnLeave();
			}
			CurrStateType = newState;
			//-1就是默认状态
			if(newState != -1)
			{
				_currState = _stateDic[CurrStateType];
				//进入新状态
				_currState.OnEnter();
			}
		}

		/// <summary>
		/// 关闭状态机
		/// </summary>
		public override void OnClose()
		{
			if (_currState != null)
			{
				_currState.OnLeave();
			}

			foreach (KeyValuePair<sbyte, FsmState<T>> state in _stateDic)
			{
				state.Value.OnDestroy();
			}
			_stateDic.Clear();
		}
	}
}