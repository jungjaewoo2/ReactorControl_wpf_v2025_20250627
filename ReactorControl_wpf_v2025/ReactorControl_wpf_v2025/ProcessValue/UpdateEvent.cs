using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProcessValue
{
    
    public class UpdateEvent
    {

        public static RoutedEvent PDB_UpdatedEvent = null; 
        
        public UpdateEvent(bool isParent)
        {
            if (isParent)
            {
                PDB_UpdatedEvent =  EventManager.RegisterRoutedEvent("PDB_Updated", RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(Window));
            } else
            {
                RoutedEvent[] rea = EventManager.GetRoutedEvents();
                foreach (RoutedEvent re in rea)
                {

                    if (re.Name == "PDB_Updated")
                    {
                        PDB_UpdatedEvent = re;
                        break;
                    }

                
                }

            }
        }

        public void RegisterRoutedEv(Type type, RoutedEventHandler reh)
        {
            if (PDB_UpdatedEvent == null)
            {
                EventManager.RegisterClassHandler(type, PDB_UpdatedEvent, reh);
            }
        }

        public void Send_PDBUpdatedEvent(Window eventSender)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(PDB_UpdatedEvent);
            eventSender.RaiseEvent(newEventArgs);
        }



        public RoutedEvent getUpdateEvent()
        {
            return PDB_UpdatedEvent;
        }

    }

}
