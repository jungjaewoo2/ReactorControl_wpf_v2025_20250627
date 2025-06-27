using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PDBLib;

namespace ProcessValue
{
    /// <summary>
    /// PDB CONTROL은 PDBAccess 를 데이터 소스로 하여 
    /// PID 와 값을 가지는 WPF Control을 위한 상위 클래스 
    /// </summary>

    public class PDBControl : Control
    {

        public string PID
        {
            get;
            set;
        }
        public string Description;
        public string DigitalString;
        public string RealString;
        public string ValueStr;
        
        
        float _pValue = 0.0f;
        public float EngHigh;
        public float EngLow;
        public float PValue;
        public string Unit = string.Empty;
       
        long pidIndex = 0;

        public PDBManager _PDBManager = null;


        public PDBControl()
        {
            MouseEnter += Window_MouseEnter;
            MouseLeave += Window_MouseLeave;
        }
        PIDInfo _pidInfo = new PIDInfo();
        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            if ((PID == null) || (PID == "")) return;
            try
            {
                _pidInfo.updateValue(PID + "(" + PValue.ToString() + ")", Description);
                _pidInfo.Left = e.GetPosition(null).X + 20;
                _pidInfo.Top = e.GetPosition(null).Y + 120;
                _pidInfo.Show();
            } catch
            {
                int a = 5;
            }
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            _pidInfo.Hide();
        }



        public void update()
        {
            if ((_PDBManager == null) || (PID==null)) return;

            pidIndex = _PDBManager.getIndex(PID);
            if (pidIndex >= 0)
            {
                PID pid = _PDBManager.getPIDfromPDB(pidIndex);

                PValue = pid.ConvFvalue;
                Unit = pid.Unit;
                DigitalString = pid.DigitalString;
                RealString = pid.RealString;
                EngHigh = pid.HighEng;
                EngLow = pid.LowEng;

                if (pid.value.varType == (byte)VarType.BOOL)
                {
                    ValueStr = DigitalString;
                } else
                {
                    ValueStr = RealString;
                }

                OnUpdate();
            }
        }

        public void updateDescription()
        {
            if ((_PDBManager == null) || (PID == null)) return;

            pidIndex = _PDBManager.getIndex(PID);
            if (pidIndex >= 0)
            {
                Description = _PDBManager.getDescStringfromPDB(pidIndex);
            } else
            {
                Description = "PID Not Found";
            }
        }

        public virtual void OnUpdate()
        {
            // 값이 업데이트 될때 실행되는 함수. 상속받은 클래스에서 정의해야 함.
        }
    }
}

