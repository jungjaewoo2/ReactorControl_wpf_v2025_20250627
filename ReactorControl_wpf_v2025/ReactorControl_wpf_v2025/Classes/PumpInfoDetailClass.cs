using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactorControl.Classes
{
	/// <summary>
	/// 장치 상세 정보 (펌프)
	/// </summary>
	public class PumpInfoDetailClass
	{
		/// <summary>
		/// 장치이름 (pp-001...)
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 상태(RUN, STOP, FAIL)
		/// </summary>
		public string Status { get; set; }
		/// <summary>
		/// 수신일시
		/// </summary>
		public DateTime ChangeDatetime { get; set; }
	}
}
