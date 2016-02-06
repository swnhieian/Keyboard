using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyboard
{
    static class Probility
    {
        public static double logGaussianDistribution(double x, double mu, double sigma)
        {
            double ret = -0.5 * Math.Log(2 * Math.PI);
            ret = ret - Math.Log(sigma) - (x - mu) * (x - mu) / (2 * sigma * sigma);
            return ret;
        }
    }
}
