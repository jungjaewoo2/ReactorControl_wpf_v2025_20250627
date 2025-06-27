using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactorControl.Classes
{
	public class ServerprocessSteps
	{
		public string Wait { get; set; }
		public string Request { get; set; }
		public string Response { get; set; }
		public string Process { get; set; }
		public string Result { get; set; }

		public ServerprocessSteps()
		{
			// 기본값 설정 가능 (필요 시)
			Wait = "대기 중";
			Request = "요청 중";
			Response = "응답 중";
			Process = "처리 중";
			Result = "결과 대기";
		}
	}
}
