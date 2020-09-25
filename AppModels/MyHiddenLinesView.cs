using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot;
using Environment = devDept.Eyeshot.Environment;

namespace AppModels
{
    public class MyHiddenLinesView: HiddenLinesView
    {
        public MyHiddenLinesView(Environment environment) : base(environment)
        {
        }

        public MyHiddenLinesView(Environment environment, double fontTolerance) : base(environment, fontTolerance)
        {
        }

        public MyHiddenLinesView(Viewport viewport, Environment environment) : base(viewport, environment)
        {
        }

        public MyHiddenLinesView(Viewport viewport, Environment environment, double fontTolerance) : base(viewport, environment, fontTolerance)
        {
        }

        public MyHiddenLinesView(HiddenLinesViewSettings viewSettings) : base(viewSettings)
        {
        }

        protected MyHiddenLinesView(HiddenLinesViewSettings viewSettings, bool forceTextsAsTriangles) : base(viewSettings, forceTextsAsTriangles)
        {
        }

        protected override void DoWork(BackgroundWorker worker, DoWorkEventArgs doWorkEventArgs)
        {
            base.DoWork(worker, doWorkEventArgs);
        }

        public void DoWork(BackgroundWorker worker, DoWorkEventArgs doWorkEventArgs, bool test)
        {
            this.DoWork(worker, doWorkEventArgs);
        }
    }
}
