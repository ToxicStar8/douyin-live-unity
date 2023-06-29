/*********************************************
 * BFramework
 * 状态机管理器
 * 创建时间：2023/04/06 15:00:23
 *********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework
{
	/// <summary>
	/// 状态机管理器
	/// </summary>
	public class FsmManager :  ManagerBase
	{
		/// <summary>
		/// 状态机字典
		/// </summary>
		private Dictionary<int, FsmBase> _fsmDic;

		/// <summary>
		/// 状态机的临时编号
		/// </summary>
		private int _tempFsmIndex = 0;

		public override void OnStart()
		{
			_fsmDic = new Dictionary<int, FsmBase>();
		}

		/// <summary>
		/// 创建状态机
		/// </summary>
		/// <typeparam name="T">拥有者类型</typeparam>
		/// <param name="owner">拥有者</param>
		/// <param name="states">状态数组</param>
		/// <returns></returns>
		public Fsm<T> CreateFsm<T>(T owner, FsmState<T>[] states) where T : class
		{
			var fsm = new Fsm<T>(_tempFsmIndex, owner, states);
			_fsmDic[_tempFsmIndex] = fsm;
			_tempFsmIndex++;
			return fsm;
		}

		/// <summary>
		/// 销毁状态机
		/// </summary>
		/// <param name="fsmId">状态机编号</param>
		public void RelaseFsm(int fsmId)
		{
			if (_fsmDic.TryGetValue(fsmId, out var fsm))
			{
				fsm.OnClose();
				_fsmDic.Remove(fsmId);
			}
		}

		public override void OnUpdate() 
		{
			foreach (var item in _fsmDic)
			{
				item.Value.OnUpdate();
			}
		}

		public override void OnDispose()
		{
            foreach (var item in _fsmDic)
            {
				item.Value.OnClose();
			}
			_fsmDic.Clear();
			_fsmDic = null;
		}
	}
}