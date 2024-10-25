using UnityEngine;

namespace MyTimer
{
	public class Timer
	{
		#region ///フィールド用
		private float m_start;
		private float m_now;
		private float m_pass;
		private float m_interval;
		private float m_difference = 0.0f;
		private bool m_bLoop = true;
		private bool m_bStop = false; 
		#endregion

		#region ///外部アクセス用
		/// <summary>ループするかどうか</summary>
		public bool GetLoop => m_bLoop;

		/// <summary>タイマーが動いているかどうか</summary>
		public bool GetPlayOrStop => m_bStop;

		/// <summary>経過時間</summary>
		public float TimeElapsed
		{
			get
			{
				if (m_interval > 0.0f)
				{
					if (!m_bStop)
					{
						m_now = Time.realtimeSinceStartup;

						m_pass = m_now - m_start;

						return (m_pass <= m_interval)
							? m_pass
							: m_interval;
					}
					else
					{
						m_start = Time.realtimeSinceStartup - m_difference;
						m_now = Time.realtimeSinceStartup;

						m_pass = m_now - m_start;

						return (m_pass <= m_interval)
							? m_pass
							: m_interval;
					}
				}
				return 0.0f;
			}
		}

		/// <summary>残り時間</summary>
		public float TimeRemaining
		{
			get
			{
				if (m_interval > 0.0f)
				{
					if (!m_bStop)
					{
						m_now = Time.realtimeSinceStartup;

						m_pass = m_now - m_start;

						return (m_interval - m_pass >= 0.0f)
							? m_interval - m_pass
							: 0.0f;
					}
					else
					{
						m_start = Time.realtimeSinceStartup - m_difference;
						m_now = Time.realtimeSinceStartup;

						m_pass = m_now - m_start;

						return (m_interval - m_pass >= 0.0f)
							? m_interval - m_pass
							: 0.0f;
					}
				}
				return 0.0f;
			}
		}
		#endregion

		/**
		<summary>
		==========================		<br/>
		計測間隔時間の設定				<br/>
		引　数：						<br/>
		float：計測間隔時間				<br/>
		bool：ループ(初期："true")[省略可]<br/>
		==========================		<br/>
		</summary>
		<param name="interval">float：計測間隔時間</param>
		<param name="bLoop">bool：ループ(初期：true)</param>
		<remarks>SetLoop関数で切り替え可能また、ResetInterval関数で再度再生可能！</remarks>
		**/
		public void SetInterval(float interval,bool bLoop = true)
		{
			m_interval = interval;

			m_start = Time.realtimeSinceStartup;

			m_bLoop = bLoop;
			m_difference = 0.0f;
		}

		/**
		<summary>
		=============================================<br/>
		設定した計測間隔時間に達したかどうか調べる	 <br/>	
		=============================================<br/>
		</summary>
		<returns>bool：true 経過した、false 経過していない</returns>
		**/
		public bool GetTiming()
		{
			if (m_interval > 0.0f)
			{
				m_now = Time.realtimeSinceStartup;

				m_pass = m_now - m_start;

				if (m_pass >= m_interval)
				{
					if (!m_bLoop)
					{
						if (!m_bStop) PlayOrStop(false);
						return true;
					}

					m_start = m_now;
					return true;
				}
			}
			return false;
		}

		/**
		<summary>
		========================================		<br/>
		計測間隔時間のリセット							<br/>
		引　数：										<br/>
		bool：カウントのリセット(初期は"true")[省略可]	<br/>
		========================================		<br/>
		</summary>
		<param name="bTimeOnly">bool：カウントのリセット(初期は"true")[省略可]</param>
		<remarks>true：カウントのみリセット｜false：タイマー自体をリセット</remarks>
		**/
		public void ResetInterval(bool bTimeOnly = true)
		{
			if (bTimeOnly)
			{
				m_now = Time.realtimeSinceStartup;
				m_start = m_now;
			}
			else
			{
				m_interval = 0.0f;
			}
		}

		/**
		<summary>
		====================================<br/>
		計測間隔時間の追加					<br/>
		引　数：							<br/>
		float：時間の追加					<br/>
		====================================<br/>
		</summary>
		<param name="add">float：時間の追加</param>
		**/
		public void AddInterval(float add) => m_interval += add;
		
		/**
		<summary>
		==============================	<br/>
		計測間隔時間の削減				<br/>
		引　数：						<br/>
		float：時間の削減(0以下は無効)	<br/>
		==============================	<br/>
		</summary>
		<param name="sub">float：時間の削減</param>
		<remarks>！削減結果が0以下の場合は無効！</remarks>
		**/
		public void SubInterval(float sub)
		{
			if (m_interval - sub <= 0.0f) return;
			m_interval -= sub;
		}

		/**
		<summary>
		===================================		<br/>
		計測間隔時間の 再生 または 停止			<br/>
		引　数：								<br/>
		bool：再生・停止[true：再生｜false：停止]<br/>
		===================================		<br/>
		</summary>
		<param name="choice">bool：再生・停止[true：再生｜false：停止]</param>
		**/
		public void PlayOrStop(bool choice)
		{
			m_bStop = choice;

			if (!choice)
			{
				m_now = Time.realtimeSinceStartup;
				m_difference = m_now - m_start;
			}
		}

		/**
		<summary>
		================================<br/>
		計測間隔時間のループ			<br/>
		引　数：								<br/>
		bool：ループ(初期："true")[省略可]<br/>
		===================================		<br/>
		</summary>
		<param name="bLoop">bool：ループ(初期："true")[省略可]</param>
		**/
		public void SetLoop(bool bLoop = true) => m_bLoop = bLoop;
	}

#if false //何かに使えるかも..？
	public class Timer
	{
		private float m_timer;
		private float m_pass;
		private float m_interval;
		private bool m_bLoop;
		private float m_intervalTime = 0.1f;
		private IntervalManage intervalManage = new IntervalManage();
		/// <summary>経過時間</summary>
		public float TimeElapsed => m_timer;
		/// <summary>残り時間</summary>
		public float TimeRemaining { get; private set; }

		/**
		<summary>
		==========================	<br/>
		計測間隔時間の設定			<br/>
		引　数：					<br/>
		float：計測間隔時間			<br/>
		bool：ループ(初期は"false")	<br/>
		==========================	<br/>
		</summary>
		<param name="interval">float：計測間隔時間</param>
		<param name="bLoop">bool：ループするかどうか(初期は"false")</param>
		**/
		public void SetInterval(float interval, bool bLoop = false)
		{
			m_bLoop = bLoop;
			m_interval = interval;
			TimeRemaining = interval;

			intervalManage.SetInterval(m_intervalTime);
		}

		/**
		<summary>
		=======================================	<br/>
		特定の条件でのみタイマーの更新をする	<br/>
		引数：									<br/>
		bool：条件式（初期値 : true）			<br/>
		=======================================	<br/>
		</summary>
		<param name="bFlag">bool：条件式(初期値：true)</param>
		<remarks>引数を書かない OR 条件式が true の場合動作する！</remarks>
		**/
		public void UpdateTimer(bool bFlag = true)
		{
			if (bFlag)
			{
				if (m_interval > 0.0f)
				{
					m_timer += (intervalManage.GetTiming() && m_interval - m_timer > 0.0f)
						? m_intervalTime
						: 0.0f;
					m_pass = m_interval - m_timer;
					TimeRemaining = m_pass;
				}
			}
		}

		/**
		<summary>
		=============================================<br/>
		設定した計測間隔時間に達したかどうか調べる	 <br/>
		=============================================<br/>
		</summary>
		<returns>bool：true 経過した、false 経過していない</returns>
		**/
		public bool GetTiming()
		{
			if (m_interval > 0.0f)
			{
				m_pass = m_interval - m_timer;

				if (m_pass <= 0.0f)
				{
					if (m_bLoop)
					{
						m_timer = m_interval;
						TimeRemaining = m_interval;
						return true;
					}
					else
					{
						m_timer = 0.0f;
						TimeRemaining = 0.0f;
						return true;
					}
				}
			}

			return false;
		}

		/**
		<summary>
		==============================	<br/>
		タイマーのカウントをリセット	<br/>
		==============================	<br/>
		</summary>
		**/
		public void RestartTimer()
		{
			m_timer = 0.0f;
			TimeRemaining = m_interval;
		}

		/**
		<summary>
		===============================	<br/>
		タイマーをリセット				<br/>
		引　数：						<br/>
		bool：設定の維持(初期は"true")	<br/>
		===============================	<br/>
		</summary>
		<param name="">bool：設定の維持(初期は"true")</param>
		**/
		public void ResetTimer(bool bMaintain = true)
		{
			m_timer = 0.0f;
			m_pass = 0.0f;
			if (bMaintain)
			{
				TimeRemaining = m_interval;
			}
			else
			{
				m_bLoop = false;
				m_interval = 0.0f;
				TimeRemaining = 0.0f;
			}
		}
	} 
#endif
}
