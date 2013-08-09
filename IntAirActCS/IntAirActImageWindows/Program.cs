using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using IntAirAct;

namespace IntAirActImageWindows
{
    static class Program
    {
        private static TraceSource logger = new TraceSource("IntAirActImageWindows");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1();

            IAIntAirAct intAirAct = IAIntAirAct.New();

            intAirAct.Route(new IARoute("PUT", "/image"), delegate(IARequest req, IAResponse res)
            {
                logger.TraceEvent(TraceEventType.Information, 0, "Received request on {0}", req.Route);
                
                String url = req.BodyAsString();
                form.BeginInvoke((Action) delegate ()
                {
                    form.LoadImageFromURL(url);
                });
            });
            
            intAirAct.Start();
            
            Application.Run(form);

            intAirAct.Stop();
        }
    }
}
