using UnityEngine;

namespace MyTimer
{
	public class Timer
	{
		#region ///�t�B�[���h�p
		private float m_start;
		private float m_now;
		private float m_pass;
		private float m_interval;
		private float m_difference = 0.0f;
		private bool m_bLoop = true;
		private bool m_bStop = false; 
		#endregion

		#region ///�O���A�N�Z�X�p
		/// <summary>���[�v���邩�ǂ���</summary>
		public bool GetLoop => m_bLoop;

		/// <summary>�^�C�}�[�������Ă��邩�ǂ���</summary>
		public bool GetPlayOrStop => m_bStop;

		/// <summary>�o�ߎ���</summary>
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

		/// <summary>�c�莞��</summary>
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
		�v���Ԋu���Ԃ̐ݒ�				<br/>
		���@���F						<br/>
		float�F�v���Ԋu����				<br/>
		bool�F���[�v(�����F"true")[�ȗ���]<br/>
		==========================		<br/>
		</summary>
		<param name="interval">float�F�v���Ԋu����</param>
		<param name="bLoop">bool�F���[�v(�����Ftrue)</param>
		<remarks>SetLoop�֐��Ő؂�ւ��\�܂��AResetInterval�֐��ōēx�Đ��\�I</remarks>
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
		�ݒ肵���v���Ԋu���ԂɒB�������ǂ������ׂ�	 <br/>	
		=============================================<br/>
		</summary>
		<returns>bool�Ftrue �o�߂����Afalse �o�߂��Ă��Ȃ�</returns>
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
		�v���Ԋu���Ԃ̃��Z�b�g							<br/>
		���@���F										<br/>
		bool�F�J�E���g�̃��Z�b�g(������"true")[�ȗ���]	<br/>
		========================================		<br/>
		</summary>
		<param name="bTimeOnly">bool�F�J�E���g�̃��Z�b�g(������"true")[�ȗ���]</param>
		<remarks>true�F�J�E���g�̂݃��Z�b�g�bfalse�F�^�C�}�[���̂����Z�b�g</remarks>
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
		�v���Ԋu���Ԃ̒ǉ�					<br/>
		���@���F							<br/>
		float�F���Ԃ̒ǉ�					<br/>
		====================================<br/>
		</summary>
		<param name="add">float�F���Ԃ̒ǉ�</param>
		**/
		public void AddInterval(float add) => m_interval += add;
		
		/**
		<summary>
		==============================	<br/>
		�v���Ԋu���Ԃ̍팸				<br/>
		���@���F						<br/>
		float�F���Ԃ̍팸(0�ȉ��͖���)	<br/>
		==============================	<br/>
		</summary>
		<param name="sub">float�F���Ԃ̍팸</param>
		<remarks>�I�팸���ʂ�0�ȉ��̏ꍇ�͖����I</remarks>
		**/
		public void SubInterval(float sub)
		{
			if (m_interval - sub <= 0.0f) return;
			m_interval -= sub;
		}

		/**
		<summary>
		===================================		<br/>
		�v���Ԋu���Ԃ� �Đ� �܂��� ��~			<br/>
		���@���F								<br/>
		bool�F�Đ��E��~[true�F�Đ��bfalse�F��~]<br/>
		===================================		<br/>
		</summary>
		<param name="choice">bool�F�Đ��E��~[true�F�Đ��bfalse�F��~]</param>
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
		�v���Ԋu���Ԃ̃��[�v			<br/>
		���@���F								<br/>
		bool�F���[�v(�����F"true")[�ȗ���]<br/>
		===================================		<br/>
		</summary>
		<param name="bLoop">bool�F���[�v(�����F"true")[�ȗ���]</param>
		**/
		public void SetLoop(bool bLoop = true) => m_bLoop = bLoop;
	}

#if false //�����Ɏg���邩��..�H
	public class Timer
	{
		private float m_timer;
		private float m_pass;
		private float m_interval;
		private bool m_bLoop;
		private float m_intervalTime = 0.1f;
		private IntervalManage intervalManage = new IntervalManage();
		/// <summary>�o�ߎ���</summary>
		public float TimeElapsed => m_timer;
		/// <summary>�c�莞��</summary>
		public float TimeRemaining { get; private set; }

		/**
		<summary>
		==========================	<br/>
		�v���Ԋu���Ԃ̐ݒ�			<br/>
		���@���F					<br/>
		float�F�v���Ԋu����			<br/>
		bool�F���[�v(������"false")	<br/>
		==========================	<br/>
		</summary>
		<param name="interval">float�F�v���Ԋu����</param>
		<param name="bLoop">bool�F���[�v���邩�ǂ���(������"false")</param>
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
		����̏����ł̂݃^�C�}�[�̍X�V������	<br/>
		�����F									<br/>
		bool�F�������i�����l : true�j			<br/>
		=======================================	<br/>
		</summary>
		<param name="bFlag">bool�F������(�����l�Ftrue)</param>
		<remarks>�����������Ȃ� OR �������� true �̏ꍇ���삷��I</remarks>
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
		�ݒ肵���v���Ԋu���ԂɒB�������ǂ������ׂ�	 <br/>
		=============================================<br/>
		</summary>
		<returns>bool�Ftrue �o�߂����Afalse �o�߂��Ă��Ȃ�</returns>
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
		�^�C�}�[�̃J�E���g�����Z�b�g	<br/>
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
		�^�C�}�[�����Z�b�g				<br/>
		���@���F						<br/>
		bool�F�ݒ�̈ێ�(������"true")	<br/>
		===============================	<br/>
		</summary>
		<param name="">bool�F�ݒ�̈ێ�(������"true")</param>
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
